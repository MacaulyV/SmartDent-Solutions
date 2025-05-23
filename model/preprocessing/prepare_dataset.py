import json
import os
import pandas as pd
from datetime import datetime

def parse_paciente(paciente, reference_date=datetime.now(), periodo_limite=730):
    # Função responsável por analisar os dados de um paciente e gerar algumas métricas básicas.
    # Ela vai olhar as consultas, filtrar as que estão dentro de um período (ex.: 365 dias),
    # depois conta quantas foram realizadas, agendadas, canceladas, etc.
    # No final, retorna uma espécie de "resumo" do uso desse paciente.

    # Validação forte dos dados de entrada
    erros = []
    # Checa campos obrigatórios
    obrigatorios = ["idPaciente", "gastoTotal"]
    for campo in obrigatorios:
        if campo not in paciente or paciente[campo] is None:
            erros.append(f"Faltando campo obrigatório: {campo}")
    # Checa valores negativos
    for campo in ["qtd_realizadas", "qtd_agendadas", "qtd_canceladas"]:
        if campo in paciente and paciente[campo] is not None and paciente[campo] < 0:
            erros.append(f"Valor negativo em {campo}")
    # Checa data de nascimento futura
    if "dataNascimento" in paciente and paciente["dataNascimento"]:
        try:
            data_nasc = pd.to_datetime(paciente["dataNascimento"])
            if data_nasc > reference_date:
                erros.append("Data de nascimento futura!")
        except Exception:
            erros.append("Data de nascimento inválida!")
    # Se houver erro grave, retorna None para ignorar esse paciente
    if erros:
        print(f"ERRO no paciente {paciente.get('idPaciente', 'N/A')}: {erros}")
        return None

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
    datas_realizadas.sort()

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

    # Features derivadas de comportamento temporal
    # 1. Tempo entre as últimas 3 consultas
    if len(datas_realizadas) >= 3:
        tempo_ultimas3 = (datas_realizadas[-1] - datas_realizadas[-3]).days
    else:
        tempo_ultimas3 = None
    # 2. Maior intervalo (gap) entre consultas
    if len(datas_realizadas) > 1:
        gaps = [(datas_realizadas[i+1] - datas_realizadas[i]).days for i in range(len(datas_realizadas)-1)]
        maior_gap = max(gaps)
    else:
        maior_gap = None
    # 3. Máximo de procedimentos em um mesmo mês
    if datas_realizadas:
        meses = [d.strftime("%Y-%m") for d in datas_realizadas]
        from collections import Counter
        max_procs_mes = max(Counter(meses).values())
    else:
        max_procs_mes = None

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
        "procedimentosRepetidos_str": procedimentosRepetidos_str,
        # Novas features temporais:
        "tempo_ultimas3": tempo_ultimas3,
        "maior_gap": maior_gap,
        "max_procs_mes": max_procs_mes
    }

    # Adicionando colunas específicas de cada categoria (quantidade de procedimentos por tipo)
    for cat in categories:
        col_name = "count_" + cat.replace(" ", "").replace("(", "").replace(")", "").replace("/", "")
        result[col_name] = cat_counts[cat]

    return result

# Função para processar um conjunto de pacientes diretamente do JSON
def process_patients_from_json(json_path, reference_date=datetime.now(), periodo_limite=730):
    """
    Processa um arquivo JSON de pacientes e retorna um DataFrame pronto para análise
    """
    try:
        # Carrega os pacientes do JSON
        with open(json_path, "r", encoding="utf-8") as f:
            pacientes = json.load(f)
        # Remove campos de texto desnecessários ANTES de processar
        campos_texto = [
            'nome', 'empresa', 'email', 'telefone', 'endereco', 'cidade',
            'dataNascimento', 'cpf', 'data_inicio', 'historico_procedimentos'
        ]
        for p in pacientes:
            for campo in campos_texto:
                if campo in p:
                    del p[campo]
        # Processa cada paciente
        features = [parse_paciente(p, reference_date, periodo_limite) for p in pacientes]
        # Converte para DataFrame
        df = pd.DataFrame(features)
        return df
    except Exception as e:
        print(f"Erro ao processar pacientes do arquivo {json_path}: {str(e)}")
        return None

def load_dataset_from_ndjson(ndjson_path, reference_date=None, periodo_limite=730):
    """
    Carrega dados diretamente do arquivo NDJSON (já processados)
    
    Args:
        ndjson_path: Caminho para o arquivo NDJSON
        reference_date: Data de referência (não utilizada nesta versão)
    
    Returns:
        DataFrame com os dados prontos para treinamento
    """
    if not os.path.exists(ndjson_path):
        print(f"ERRO: Arquivo NDJSON não encontrado: {ndjson_path}")
        return None
    
    print(f"Carregando dados do arquivo NDJSON: {ndjson_path}")
    
    # Lista para armazenar cada registro
    records = []
    
    # Contador para mostrar progresso
    count = 0
    
    # Abrir o arquivo NDJSON e ler linha por linha
    with open(ndjson_path, 'r', encoding='utf-8') as f:
        for line in f:
            try:
                # Converter a linha JSON em um dicionário Python
                record = json.loads(line)
                
                # Renomear 'risco' para 'label' para manter compatibilidade
                if 'risco' in record:
                    record['label'] = record.pop('risco')
                
                # Remove campos de texto desnecessários ANTES de adicionar
                campos_texto = [
                    'nome', 'empresa', 'email', 'telefone', 'endereco', 'cidade',
                    'dataNascimento', 'cpf', 'data_inicio', 'historico_procedimentos'
                ]
                for campo in campos_texto:
                    if campo in record:
                        del record[campo]
                
                # Adicionar à lista de registros
                records.append(record)
                
                # Atualizar contador e mostrar progresso
                count += 1
                if count % 10000 == 0:
                    print(f"Processados {count} registros...")
                    
            except Exception as e:
                print(f"Erro ao processar linha: {str(e)}")
                continue
    
    # Verificar se algum registro foi processado
    if not records:
        print("ERRO: Nenhum registro processado com sucesso!")
        return None
    
    # Converter para DataFrame
    df = pd.DataFrame(records)
    
    # Remover colunas que não são necessárias para o treinamento
    cols_to_drop = []
    for col in ['nome', 'data_inicio', 'historico_procedimentos']:
        if col in df.columns:
            cols_to_drop.append(col)
    
    if cols_to_drop:
        df = df.drop(columns=cols_to_drop)
    
    print(f"Dataset processado com {len(df)} registros.")
    print(f"Distribuição das classes:\n{df['label'].value_counts()}")
    
    return df

# Exemplo de uso simples
if __name__ == "__main__":
    # Opção 1: Carregar do JSON tradicional
    json_path = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "..", "data", "synthetic_patients.json")
    if os.path.exists(json_path):
        df = process_patients_from_json(json_path)
        if df is not None:
            print(f"Processados {len(df)} pacientes do JSON com sucesso")
            print(f"Distribuição de labels: \n{df['label'].value_counts()}")
    
    # Opção 2: Carregar do NDJSON (novo formato)
    ndjson_path = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "..", "data", "synthetic_patients.ndjson")
    if os.path.exists(ndjson_path):
        df = load_dataset_from_ndjson(ndjson_path)
        if df is not None:
            print(f"Processados {len(df)} pacientes do NDJSON com sucesso")
            print(f"Distribuição de labels: \n{df['label'].value_counts()}")
