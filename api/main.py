from fastapi import FastAPI, Body
from pydantic import BaseModel, Field
from typing import List, Optional, Union
from datetime import datetime
import uvicorn
import os
import random
import pandas as pd
from joblib import load
from fastapi import HTTPException
from fastapi.encoders import jsonable_encoder
import logging

logging.basicConfig(level=logging.DEBUG)

#######################
# 1) DEFINIÇÃO DE ESQUEMAS (SCHEMAS)
#######################
class Procedimento(BaseModel):
    # Representa um procedimento odontológico individual, com custo e descrição.
    idProcedimento: int
    tipoProcedimento: str
    descricao: Optional[str]
    custo: str

class Consulta(BaseModel):
    # Cada consulta tem uma data, status (Realizada, Agendada, etc.) e o procedimento realizado.
    idConsulta: int
    dataConsulta: str
    status: str
    procedimento: Procedimento

class PacienteInput(BaseModel):
    # Dados gerais do paciente (nome, cpf, etc.) e a lista de consultas.
    idPaciente: int
    nomeCompleto: str
    cpf: str
    dataNascimento: str
    email: Optional[str]
    telefone: Optional[str]
    endereco: Optional[str]
    planoOdontologico: Optional[str]
    empresa: Optional[str]
    numConsultas: int
    gastoTotal: str
    consultas: List[Consulta]

class AnalisePaciente(BaseModel):
    # Retorno final da análise de risco, incluindo a justificativa e grau de risco (grauRisco).
    idPaciente: int
    nomePaciente: str
    tipoAlerta: str
    grauRisco: str
    justificativa: str
    totalConsultas: int
    gastoTotal: float
    dataAnalise: str
    modelo_utilizado: bool
    confianca: float = Field(..., alias="confiança")

    class Config:
        populate_by_name = True

#######################
# 2) LÓGICA DO MODELO: parse_paciente, generate_justificativa, etc.
#######################

# Mapeamento de procedimentos para categorias – importante pro modelo identificar as contagens de cada tipo.
mapping = {
    "Consulta odontológica geral": "Consultas e Diagnóstico",
    "Avaliação clínica e diagnóstico": "Consultas e Diagnóstico",
    "Consulta para clareamento": "Consultas e Diagnóstico",
    "Consulta para próteses": "Consultas e Diagnóstico",
    "Acompanhamento ortodôntico": "Consultas e Diagnóstico",
    "Limpeza dental (profilaxia)": "Prevenção e Profilaxia",
    "Aplicação de flúor": "Prevenção e Profilaxia",
    "Aplicação de selante": "Prevenção e Profilaxia",
    "Instrução de higiene bucal": "Prevenção e Profilaxia",
    "Atendimento odontológico de urgência": "UrgênciaeEmergência24h",
    "Alívio de dor": "UrgênciaeEmergência24h",
    "Drenagem de abscessos": "UrgênciaeEmergência24h",
    "Controle de hemorragias": "UrgênciaeEmergência24h",
    "Radiografia intraoral": "RadiologiaeExames",
    "Radiografia panorâmica": "RadiologiaeExames",
    "Documentação ortodôntica completa": "RadiologiaeExames",
    "Tomografia computadorizada": "RadiologiaeExames",
    "Restauração em resina composta": "Dentística",
    "Restauração em amálgama": "Dentística",
    "Troca de restaurações antigas": "Dentística",
    "Extração de dente comum": "CirurgiaOraleExtracoes",
    "Extração de dente do siso": "CirurgiaOraleExtracoes",
    "Frenectomia lingual e labial": "CirurgiaOraleExtracoes",
    "Canal em dentes anteriores": "Endodontia",
    "Canal em dentes posteriores": "Endodontia",
    "Retratamento endodôntico": "Endodontia",
    "Tratamento de gengivite": "Periodontia",
    "Raspagem de tártaro": "Periodontia",
    "Cirurgia periodontal": "Periodontia",
    "Atendimento odontológico para crianças": "Odontopediatria",
    "Aplicação de flúor e selante": "Odontopediatria",
    "Tratamento restaurador em dentes de leite": "Odontopediatria",
    "Extração de dentes de leite": "Odontopediatria",
    "Instalação de aparelho fixo metálico": "Ortodontia",
    "Manutenção mensal do aparelho": "Ortodontia",
    "Retirada do aparelho ortodôntico": "Ortodontia",
    "Mantenedores ortodônticos": "Ortodontia",
    "Clareamento dental caseiro": "OdontologiaEstetica",
    "Clareamento estético em consultório": "OdontologiaEstetica",
    "Prótese fixa (coroa unitária)": "PrótesesDentárias",
    "Prótese removível total (dentadura)": "PrótesesDentárias",
    "Prótese removível parcial": "PrótesesDentárias",
    "Prótese sobre cerâmica ou resina": "PrótesesDentárias",
    "Placa de mordida para bruxismo": "PrótesesDentárias"
}

# Estas são as categorias que foram usadas no treinamento do modelo
expected_categories = [
    "Consultas e Diagnóstico",
    "Prevenção e Profilaxia",
    "UrgênciaeEmergência24h",
    "RadiologiaeExames",
    "Dentística",
    "CirurgiaOraleExtracoes",
    "Endodontia",
    "Periodontia",
    "Odontopediatria",
    "Ortodontia",
    "OdontologiaEstetica",
    "PrótesesDentárias"
]

def parse_paciente(paciente_dict, reference_date=datetime.now(), periodo_limite=365):
    # Essa função organiza as features usadas pelo modelo, como qtd de consultas realizadas, gasto total, etc.
    gasto_str = paciente_dict["gastoTotal"].replace("R$", "").replace(".", "").replace(",", ".").strip()
    try:
        gasto_total = float(gasto_str)
    except:
        gasto_total = 0.0

    consultas = paciente_dict.get("consultas", [])
    formato_data = "%d/%m/%Y %H:%M"
    consultas_validas = []
    for c in consultas:
        try:
            dt = datetime.strptime(c["dataConsulta"], formato_data)
            # Aqui checamos se a data está num range de 0..periodo_limite dias até a reference_date
            if 0 <= (reference_date - dt).days <= periodo_limite:
                consultas_validas.append(c)
        except:
            continue

    qtd_realizadas = sum(1 for c in consultas_validas if c["status"] == "Realizada")
    qtd_agendadas = sum(1 for c in consultas_validas if c["status"] == "Agendada")
    qtd_canceladas = sum(1 for c in consultas_validas if c["status"] == "Cancelada")

    datas_realizadas = []
    for c in consultas_validas:
        if c["status"] == "Realizada":
            try:
                dt = datetime.strptime(c["dataConsulta"], formato_data)
                datas_realizadas.append(dt)
            except:
                continue

    if datas_realizadas:
        dataMin_str = min(datas_realizadas).strftime("%d/%m/%Y")
        dataMax_str = max(datas_realizadas).strftime("%d/%m/%Y")
        periodo_total_dias = (max(datas_realizadas) - min(datas_realizadas)).days
        if len(datas_realizadas) > 1:
            intervalos = [(datas_realizadas[i+1] - datas_realizadas[i]).days for i in range(len(datas_realizadas)-1)]
            intervalo_medio = sum(intervalos) / len(intervalos)
        else:
            intervalo_medio = 0
    else:
        dataMin_str = "N/A"
        dataMax_str = "N/A"
        periodo_total_dias = 0
        intervalo_medio = 0

    # Prepara contadores das categorias que foram usadas no modelo
    cat_counts = {cat: 0 for cat in expected_categories}
    for c in consultas_validas:
        if c["status"] == "Realizada":
            tipo_proc = c["procedimento"].get("tipoProcedimento", "")
            categoria = mapping.get(tipo_proc, None)
            if categoria and categoria in cat_counts:
                cat_counts[categoria] += 1

    # Identifica se tem repetições de categorias específicas
    repeticoes_list = [f"{proc} ({count} vezes)" for proc, count in cat_counts.items() if count > 1]
    if repeticoes_list:
        procedimentosRepetidos_str = ", ".join(repeticoes_list)
    else:
        procedimentosRepetidos_str = "Nenhuma repetição relevante"
    numProcedsRepetidos = sum(1 for count in cat_counts.values() if count > 1)

    # Gerar label simples (heurística) de risco só pra termos como fallback
    if qtd_realizadas >= 10:
        label = "UsoExcessivo"
    elif qtd_realizadas >= 5:
        label = "Uso Moderado com Tendência a Excesso"
    elif qtd_realizadas >= 3:
        label = "Uso Moderado"
    else:
        label = "NenhumRisco"

    # Aqui é onde montamos as features que nosso modelo usará
    result = {
        "idPaciente": paciente_dict["idPaciente"],
        "qtd_realizadas": qtd_realizadas,
        "qtd_agendadas": qtd_agendadas,
        "qtd_canceladas": qtd_canceladas,
        "intervaloMedioDias": float(intervalo_medio),
        "periodoTotalDias": periodo_total_dias,
        "gastoTotal": gasto_total,
        "numProcedsRepetidos": numProcedsRepetidos,
        "label": label,
        "dataMin_str": dataMin_str,
        "dataMax_str": dataMax_str,
        "procedimentosRepetidos_str": procedimentosRepetidos_str,
    }
    # Adicionamos as colunas de contagem de cada categoria
    for cat, count in cat_counts.items():
        col_name = "count_" + cat.replace(" ", "").replace("(", "").replace(")", "").replace("/", "")
        result[col_name] = count

    return result

def map_risco(label):
    # Só um jeito simples de gerar um "grau de risco" numérico (ex.: porcentagem) baseado no label
    if label == "NenhumRisco":
        return random.randint(5, 15)
    elif label == "Uso Moderado":
        return random.randint(30, 50)
    elif label == "Uso Moderado com Tendência a Excesso":
        return random.randint(50, 75)
    elif label == "UsoExcessivo":
        return random.randint(80, 100)
    else:
        return 0

def generate_justificativa(features, add_info, prediction):
    # Função que cria uma justificativa textual com base nos dados de consultas, repetição, etc.
    qtd_realizadas = features.get("qtd_realizadas", 0)
    intervalo = features.get("intervaloMedioDias", 0)
    gasto = features.get("gastoTotal", 0)
    dataMin_str = add_info.get("dataMin_str", "N/A")
    dataMax_str = add_info.get("dataMax_str", "N/A")
    rep_text = add_info.get("procedimentosRepetidos_str", "Nenhuma repetição relevante")
    if not rep_text or rep_text.lower() in ["nenhuma repetição relevante", "nenhuma repetição"]:
        rep_text = None

    # Se não tiver repetições, removo do texto
    if qtd_realizadas == 1:
        template_unica = (
            "Após análise dos dados entre {dataMin_str} e {dataMax_str}, constatei que o paciente realizou apenas uma consulta odontológica, "
            "totalizando um gasto de R$ {gasto:.2f}. Como há apenas esse registro, não é possível calcular intervalos nem identificar repetições de procedimentos. "
            "Portanto, com base nos dados disponíveis, concluo que o paciente não apresenta nenhum risco."
        )
        return template_unica.format(dataMin_str=dataMin_str, dataMax_str=dataMax_str, gasto=gasto)

    # Textos pré-definidos pra cada tipo de risco
    templates = {
        "NenhumRisco": [
            ("Após análise dos registros entre {dataMin_str} e {dataMax_str}, observei que o paciente realizou {qtd_realizadas} consultas, "
             "com um gasto total de R$ {gasto:.2f} e intervalos médios de {intervalo:.1f} dias. Como não foram detectadas repetições nos procedimentos, o padrão de uso se mostra totalmente adequado."),
            ("Conforme avaliação dos dados entre {dataMin_str} e {dataMax_str}, constata-se que o paciente teve {qtd_realizadas} consultas, totalizando R$ {gasto:.2f} em gastos, "
             "com intervalos de {intervalo:.1f} dias entre as visitas. A ausência de repetições evidencia um comportamento de uso seguro."),
            ("Examinando os atendimentos realizados entre {dataMin_str} e {dataMax_str}, verifiquei que o paciente efetuou {qtd_realizadas} consultas, "
             "com um gasto acumulado de R$ {gasto:.2f} e uma média de {intervalo:.1f} dias entre elas. Não foram observadas repetições, o que indica um uso normal dos serviços."),
            ("Entre {dataMin_str} e {dataMax_str}, o paciente realizou {qtd_realizadas} consultas, com um gasto total de R$ {gasto:.2f} e intervalos de {intervalo:.1f} dias, "
             "sem que se identifiquem repetições de procedimentos – evidenciando um padrão de uso dentro dos limites esperados.")
        ],
        "Uso Moderado": [
            ("Após avaliação dos dados entre {dataMin_str} e {dataMax_str}, constatei que o paciente realizou {qtd_realizadas} consultas, com um gasto total de R$ {gasto:.2f} "
             "e uma média de {intervalo:.1f} dias entre atendimentos. Foram observadas algumas repetições ({rep_text}), mas elas permanecem dentro dos limites aceitáveis, "
             "configurando um uso moderado dos serviços."),
            ("Conforme os registros entre {dataMin_str} e {dataMax_str}, o paciente realizou {qtd_realizadas} consultas, totalizando um gasto de R$ {gasto:.2f} e intervalos médios de {intervalo:.1f} dias. "
             "{rep_text} foram identificadas de forma isolada, o que é compatível com um padrão moderado de utilização."),
            ("Ao analisar os atendimentos entre {dataMin_str} e {dataMax_str}, verifiquei que o paciente teve {qtd_realizadas} consultas com um gasto acumulado de R$ {gasto:.2f} e intervalos de {intervalo:.1f} dias. "
             "Embora haja repetições, como ({rep_text}), elas não ultrapassam os limites normais, indicando um uso moderado."),
            ("Entre {dataMin_str} e {dataMax_str}, foram registradas {qtd_realizadas} consultas, com um gasto total de R$ {gasto:.2f} e uma média de {intervalo:.1f} dias entre atendimentos. "
             "As repetições observadas ({rep_text}) sugerem um uso moderado, sem que haja sinais de abuso.")
        ],
        "Uso Moderado com Tendência a Excesso": [
            ("Após análise dos registros entre {dataMin_str} e {dataMax_str}, observei que o paciente realizou {qtd_realizadas} consultas, com um gasto total de R$ {gasto:.2f} "
             "e intervalos médios de {intervalo:.1f} dias. Foram identificadas repetições frequentes, tais como ({rep_text}), o que pode indicar uma tendência ao aumento do uso a longo prazo. "
             "Recomendo monitoramento contínuo para evitar futuros desequilíbrios."),
            ("Conforme os dados entre {dataMin_str} e {dataMax_str}, o paciente teve {qtd_realizadas} consultas, acumulando um gasto de R$ {gasto:.2f} e mantendo intervalos de {intervalo:.1f} dias. "
             "A repetição frequente de procedimentos, como ({rep_text}), sugere que o uso do convênio está se intensificando – um sinal de que é prudente acompanhar esse padrão de perto."),
            ("Ao avaliar os registros entre {dataMin_str} e {dataMax_str}, verifiquei que o paciente realizou {qtd_realizadas} consultas, totalizando R$ {gasto:.2f} em gastos, com uma média de {intervalo:.1f} dias entre atendimentos. "
             "A ocorrência de repetições ({rep_text}) evidencia uma tendência que pode se intensificar, sugerindo a necessidade de um monitoramento regular para prevenir abusos futuros."),
            ("Entre as datas {dataMin_str} e {dataMax_str}, o paciente realizou {qtd_realizadas} consultas com um gasto total de R$ {gasto:.2f} e intervalos médios de {intervalo:.1f} dias. "
             "A identificação de repetições, como ({rep_text}), indica que há uma inclinação para um uso maior dos serviços, recomendando uma análise mais atenta do histórico.")
        ],
        "UsoExcessivo": [
            ("Após analisar os registros entre {dataMin_str} e {dataMax_str}, constatei que o paciente realizou {qtd_realizadas} consultas com intervalos extremamente curtos (média de {intervalo:.1f} dias) e um gasto total de R$ {gasto:.2f}. "
             "A presença consistente de repetições, como ({rep_text}), evidencia um uso excessivo dos serviços odontológicos, o que requer atenção imediata para evitar complicações a longo prazo."),
            ("Conforme os dados coletados entre {dataMin_str} e {dataMax_str}, o paciente realizou {qtd_realizadas} consultas, totalizando um gasto de R$ {gasto:.2f} e mantendo intervalos de apenas {intervalo:.1f} dias entre atendimentos. "
             "A alta frequência de repetições ({rep_text}) confirma um padrão de uso excessivo, sugerindo a necessidade de intervenção urgente."),
            ("Ao examinar os registros entre {dataMin_str} e {dataMax_str}, observei que o paciente teve {qtd_realizadas} consultas com um gasto acumulado de R$ {gasto:.2f} e intervalos médios de {intervalo:.1f} dias. "
             "A ocorrência expressiva de repetições, exemplificada por ({rep_text}), caracteriza claramente um uso excessivo dos serviços, indicando um risco elevado que demanda ação imediata."),
            ("Entre {dataMin_str} e {dataMax_str}, o paciente registrou {qtd_realizadas} consultas com intervalos muito curtos (média de {intervalo:.1f} dias) e um gasto total de R$ {gasto:.2f}. "
             "A elevada ocorrência de repetições, tais como ({rep_text}), evidencia um padrão preocupante de uso excessivo, recomendando uma avaliação detalhada para prevenir complicações futuras.")
        ]
    }

    if prediction not in templates:
        return f"Classificação {prediction}, mas sem template definido."

    chosen_list = templates[prediction]
    chosen_template = random.choice(chosen_list)

    if not rep_text:
        final_rep = "nenhuma repetição de procedimentos"
    else:
        final_rep = rep_text

    return chosen_template.format(
        dataMin_str=dataMin_str,
        dataMax_str=dataMax_str,
        qtd_realizadas=qtd_realizadas,
        gasto=gasto,
        intervalo=intervalo,
        rep_text=final_rep
    )

def load_model():
    # Aqui tentamos carregar o modelo de ML treinado. Se não encontrar, damos erro.
    base_dir = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", "model", "artifacts"))
    model_path = os.path.join(base_dir, "model_rf.joblib")
    if os.path.exists(model_path):
        print("Modelo carregado com sucesso.")
        return load(model_path)
    else:
        print("Modelo não encontrado. Usando heurística.")
        return None


def infer_patient(paciente_dict):
    # Gera as features do paciente e tenta obter a predição do modelo
    features = parse_paciente(paciente_dict)

    # Carrega o modelo, se possível
    model = load_model()
    if not model:
        error_msg = "Modelo não carregado. A análise não pode ser realizada sem o modelo treinado."
        logging.error(error_msg)
        raise HTTPException(status_code=500, detail=error_msg)

    # Definir a ordem das colunas exatamente como no treinamento
    feature_cols = [
        "qtd_realizadas", "qtd_agendadas", "qtd_canceladas",
        "intervaloMedioDias", "periodoTotalDias", "gastoTotal", "numProcedsRepetidos",
        "count_ConsultaseDiagnóstico",
        "count_PrevençãoeProfilaxia",
        "count_UrgênciaeEmergência24h",
        "count_RadiologiaeExames",
        "count_Dentística",
        "count_CirurgiaOraleExtrações",
        "count_Endodontia",
        "count_Periodontia",
        "count_Odontopediatria",
        "count_Ortodontia",
        "count_OdontologiaEstética",
        "count_PrótesesDentárias"
    ]

    X_infer = pd.DataFrame([{col: features.get(col, 0) for col in feature_cols}])

    model_pred = model.predict(X_infer)[0]
    proba = model.predict_proba(X_infer)[0]
    classes = model.classes_
    pred_index = list(classes).index(model_pred)
    confidence = float(proba[pred_index])
    logging.debug(f"Modelo: predição = {model_pred}, confiança = {confidence:.2f}")

    final_pred = model_pred
    modelo_utilizado = True
    grau_risco = f"{map_risco(final_pred)}%"

    # Constrói a justificativa textual
    add_info = {
        "dataMin_str": features.get("dataMin_str"),
        "dataMax_str": features.get("dataMax_str"),
        "procedimentosRepetidos_str": features.get("procedimentosRepetidos_str")
    }
    justificativa = generate_justificativa(features, add_info, final_pred)

    result = {
        "idPaciente": features["idPaciente"],
        "nomePaciente": paciente_dict.get("nomeCompleto", "Desconhecido"),
        "tipoAlerta": final_pred,
        "grauRisco": grau_risco,
        "justificativa": justificativa,
        "totalConsultas": paciente_dict.get("numConsultas", 0),
        "gastoTotal": features["gastoTotal"],
        "dataAnalise": datetime.now().strftime("%Y-%m-%d %H:%M:%S"),
        "modelo_utilizado": modelo_utilizado,
        "confianca": confidence
    }

    # Convertendo para o schema com alias, pra que "confianca" seja exibido como "confiança"
    analise = AnalisePaciente(**result)
    return jsonable_encoder(analise, by_alias=True)

#######################
# 3) FASTAPI: CRIAR O ENDPOINT
#######################
app = FastAPI(
    title="SmartDent: IA para Odontologia",
    description="API que integra análises inteligentes com AI para otimizar práticas odontológicas, com toda a lógica implementada de forma direta e acessível.",
    version="1.0.0"
)

@app.post("/analisar-uso", response_model=Union[AnalisePaciente, List[AnalisePaciente]])
def analisar_uso(pacientes: Union[PacienteInput, List[PacienteInput]] = Body(...)):
    # Endpoint principal: podemos enviar um único paciente ou uma lista de pacientes pra análise.
    if not isinstance(pacientes, list):
        pacientes = [pacientes]
    resultados = []
    for paciente in pacientes:
        paciente_dict = paciente.dict()
        resultado = infer_patient(paciente_dict)
        resultados.append(resultado)
    if len(resultados) == 1:
        return resultados[0]
    return resultados

#######################
# 4) RODAR LOCALMENTE
#######################
if __name__ == "__main__":
    uvicorn.run("main:app", host="0.0.0.0", port=8000, reload=True)
