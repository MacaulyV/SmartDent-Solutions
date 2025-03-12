import json
import os
import pandas as pd
from datetime import datetime

def parse_paciente(paciente, reference_date=datetime.now(), periodo_limite=365):
    # Função responsável por analisar os dados de um paciente e gerar algumas métricas básicas.
    # Ela vai olhar as consultas, filtrar as que estão dentro de um período (ex.: 365 dias),
    # depois conta quantas foram realizadas, agendadas, canceladas, etc.
    # No final, retorna uma espécie de "resumo" do uso desse paciente.

    try:
        # Aqui convertemos o custo total que está em string (ex.: "R$ 300,00") para float.
        gasto_str = paciente["gastoTotal"].replace("R$", "").replace(".", "").replace(",", ".").strip()
        gasto_total = float(gasto_str)
    except Exception:
        # Se por acaso der algum problema (formato estranho), definimos como 0.0.
        gasto_total = 0.0

    consultas = paciente.get("consultas", [])
    formato_data = "%d/%m/%Y %H:%M"
    consultas_validas = []

    # Vamos filtrar somente as consultas que estão dentro do período determinado
    for c in consultas:
        try:
            dt = datetime.strptime(c["dataConsulta"], formato_data)
            # Se a diferença entre a data de referência e a data da consulta estiver no range 0..periodo_limite dias
            if 0 <= (reference_date - dt).days <= periodo_limite:
                consultas_validas.append(c)
        except Exception:
            # Se a data estiver inválida, a gente ignora.
            continue

    # Contar a quantidade de consultas por status
    qtd_realizadas = sum(1 for c in consultas_validas if c["status"] == "Realizada")
    qtd_agendadas = sum(1 for c in consultas_validas if c["status"] == "Agendada")
    qtd_canceladas = sum(1 for c in consultas_validas if c["status"] == "Cancelada")

    # Aqui vamos pegar as datas das consultas que foram 'Realizadas' para mais alguns cálculos
    datas_realizadas = [datetime.strptime(c["dataConsulta"], formato_data) for c in consultas_validas if
                        c["status"] == "Realizada"]

    if datas_realizadas:
        dataMin_str = min(datas_realizadas).strftime("%d/%m/%Y")
        dataMax_str = max(datas_realizadas).strftime("%d/%m/%Y")
        periodo_total_dias = (max(datas_realizadas) - min(datas_realizadas)).days
        # Calcular o intervalo médio de tempo entre consultas realizadas
        if len(datas_realizadas) > 1:
            intervalos = [(datas_realizadas[i + 1] - datas_realizadas[i]).days for i in
                          range(len(datas_realizadas) - 1)]
            intervalo_medio = sum(intervalos) / len(intervalos)
        else:
            intervalo_medio = 0
    else:
        # Se não tem consulta realizada, não tem o que calcular aqui
        dataMin_str = "N/A"
        dataMax_str = "N/A"
        periodo_total_dias = 0
        intervalo_medio = 0

    # Categorias que a gente mapeou dos procedimentos
    categories = [
        "Consultas e Diagnóstico",
        "Prevenção e Profilaxia",
        "Urgência e Emergência 24h",
        "Radiologia e Exames",
        "Dentística",
        "Cirurgia Oral e Extrações",
        "Endodontia",
        "Periodontia",
        "Odontopediatria",
        "Ortodontia",
        "Odontologia Estética",
        "Próteses Dentárias"
    ]
    cat_counts = {cat: 0 for cat in categories}

    # Dicionário de mapeamento: pra saber de qual categoria cada procedimento faz parte
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
        "Atendimento odontológico de urgência": "Urgência e Emergência 24h",
        "Alívio de dor": "Urgência e Emergência 24h",
        "Drenagem de abscessos": "Urgência e Emergência 24h",
        "Controle de hemorragias": "Urgência e Emergência 24h",
        "Radiografia intraoral": "Radiologia e Exames",
        "Radiografia panorâmica": "Radiologia e Exames",
        "Documentação ortodôntica completa": "Radiologia e Exames",
        "Tomografia computadorizada": "Radiologia e Exames",
        "Restauração em resina composta": "Dentística",
        "Restauração em amálgama": "Dentística",
        "Troca de restaurações antigas": "Dentística",
        "Extração de dente comum": "Cirurgia Oral e Extrações",
        "Extração de dente do siso": "Cirurgia Oral e Extrações",
        "Frenectomia lingual e labial": "Cirurgia Oral e Extrações",
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
        "Clareamento dental caseiro": "Odontologia Estética",
        "Clareamento estético em consultório": "Odontologia Estética",
        "Prótese fixa (coroa unitária)": "Próteses Dentárias",
        "Prótese removível total (dentadura)": "Próteses Dentárias",
        "Prótese removível parcial": "Próteses Dentárias",
        "Prótese sobre cerâmica ou resina": "Próteses Dentárias",
        "Placa de mordida para bruxismo": "Próteses Dentárias"
    }

    procedimento_counts = {}
    # Contamos quantos procedimentos cada paciente realizou efetivamente
    for c in consultas_validas:
        if c["status"] == "Realizada":
            tipo_proc = c["procedimento"].get("tipoProcedimento", "")
            procedimento_counts[tipo_proc] = procedimento_counts.get(tipo_proc, 0) + 1
            categoria = mapping.get(tipo_proc, None)
            if categoria:
                cat_counts[categoria] += 1

    # Aqui vamos pegar os procedimentos que se repetiram mais de uma vez pra flagar "repetição"
    repeticoes_list = [f"{proc} ({count} vezes)" for proc, count in procedimento_counts.items() if count > 1]
    procedimentosRepetidos_str = ", ".join(repeticoes_list) if repeticoes_list else "Nenhuma repetição relevante"

    numProcedsRepetidos = sum(1 for count in procedimento_counts.values() if count > 1)

    # Atribuímos um rótulo (label) simples, de acordo com o número de consultas realizadas
    if qtd_realizadas >= 10:
        label = "UsoExcessivo"
    elif qtd_realizadas >= 5:
        label = "Uso Moderado com Tendência a Excesso"
    elif qtd_realizadas >= 3:
        label = "Uso Moderado"
    else:
        label = "NenhumRisco"

    result = {
        "idPaciente": paciente["idPaciente"],
        "qtd_realizadas": qtd_realizadas,
        "qtd_agendadas": qtd_agendadas,
        "qtd_canceladas": qtd_canceladas,
        "intervaloMedioDias": intervalo_medio,
        "periodoTotalDias": periodo_total_dias,
        "gastoTotal": gasto_total,
        "numProcedsRepetidos": numProcedsRepetidos,
        "label": label,
        "dataMin_str": dataMin_str,
        "dataMax_str": dataMax_str,
        "procedimentosRepetidos_str": procedimentosRepetidos_str
    }

    # Adicionando colunas específicas de cada categoria (quantidade de procedimentos por tipo)
    for cat in categories:
        col_name = "count_" + cat.replace(" ", "").replace("(", "").replace(")", "").replace("/", "")
        result[col_name] = cat_counts[cat]

    return result


def create_dataset(json_path="data/synthetic_patients.json", output_csv="data/dataset_treino.csv"):
    # Função que carrega o arquivo JSON gerado, processa cada paciente (parse_paciente),
    # e então gera um dataset .csv que poderemos usar no treinamento ou análise
    BASE_DIR = os.path.abspath(os.path.join(os.path.dirname(__file__), ".."))
    json_path = os.path.join(BASE_DIR, "data", "synthetic_patients.json")
    output_csv = os.path.join(BASE_DIR, "data", "dataset_treino.csv")
    reference_date = datetime.now()

    with open(json_path, "r", encoding="utf-8") as f:
        pacientes = json.load(f)

    # Aqui, parseamos cada paciente e montamos uma lista de 'features'
    features = [parse_paciente(p, reference_date=reference_date, periodo_limite=365) for p in pacientes]

    df = pd.DataFrame(features)

    # Salvando o dataset final em CSV
    df.to_csv(output_csv, index=False)
    print(f"CSV gerado: {output_csv}")


if __name__ == "__main__":
    create_dataset()
