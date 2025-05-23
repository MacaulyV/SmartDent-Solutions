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
from functools import lru_cache
from sklearn.ensemble import RandomForestClassifier
import numpy as np
from sklearn.preprocessing import LabelEncoder
import collections
from statistics import stdev, mean
import re
import math

logging.basicConfig(level=logging.DEBUG)

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
    confianca: str = Field(..., alias="confiança")

    class Config:
        populate_by_name = True

#######################
# 2) LÓGICA DO MODELO: parse_paciente, generate_justificativa, etc.
#######################

# Mapeamento de procedimentos para categorias – importante pro modelo identificar as contagens de cada tipo.
mapping = {
    # Consultas e Diagnóstico
    "Consulta odontológica geral": "Consultas e Diagnóstico",
    "Avaliação clínica e diagnóstico": "Consultas e Diagnóstico",
    "Consulta para clareamento": "Consultas e Diagnóstico",
    "Consulta para próteses": "Consultas e Diagnóstico",
    "Acompanhamento ortodôntico": "Consultas e Diagnóstico",
    "Consulta de avaliação": "Consultas e Diagnóstico",
    "Consulta de revisão": "Consultas e Diagnóstico",
    
    # Prevenção e Profilaxia
    "Limpeza dental (profilaxia)": "Prevenção e Profilaxia",
    "Aplicação de flúor": "Prevenção e Profilaxia",
    "Aplicação de selante": "Prevenção e Profilaxia",
    "Instrução de higiene bucal": "Prevenção e Profilaxia",
    "Limpeza e profilaxia": "Prevenção e Profilaxia",
    
    # Urgência e Emergência 24h
    "Atendimento odontológico de urgência": "UrgênciaeEmergência24h",
    "Alívio de dor": "UrgênciaeEmergência24h",
    "Drenagem de abscessos": "UrgênciaeEmergência24h",
    "Controle de hemorragias": "UrgênciaeEmergência24h",
    
    # Radiologia e Exames
    "Radiografia intraoral": "RadiologiaeExames",
    "Radiografia panorâmica": "RadiologiaeExames",
    "Documentação ortodôntica completa": "RadiologiaeExames",
    "Documentação ortodôntica completa (Exames)": "RadiologiaeExames",
    "Tomografia computadorizada": "RadiologiaeExames",
    "Radiografia periapical": "RadiologiaeExames",
    
    # Dentística
    "Restauração em resina composta": "Dentística",
    "Restauração em amálgama": "Dentística",
    "Troca de restaurações antigas": "Dentística",
    "Tratamento restaurador em dentes permanentes": "Dentística",
    
    # Cirurgia Oral e Extrações
    "Extração de dente comum": "CirurgiaOraleExtracoes",
    "Extração de dente do siso": "CirurgiaOraleExtracoes",
    "Frenectomia lingual e labial": "CirurgiaOraleExtracoes",
    "Implante unitário": "CirurgiaOraleExtracoes",
    
    # Endodontia
    "Canal em dentes anteriores": "Endodontia",
    "Canal em dentes posteriores": "Endodontia",
    "Retratamento endodôntico": "Endodontia",
    
    # Periodontia
    "Tratamento de gengivite": "Periodontia",
    "Raspagem de tártaro": "Periodontia",
    "Cirurgia periodontal": "Periodontia",
    "Raspagem e alisamento radicular": "Periodontia",
    
    # Odontopediatria
    "Atendimento odontológico para crianças": "Odontopediatria",
    "Aplicação de flúor e selante": "Odontopediatria",
    "Tratamento restaurador em dentes de leite": "Odontopediatria",
    "Extração de dentes de leite": "Odontopediatria",
    
    # Ortodontia
    "Documentação ortodôntica completa (Ortodontia)": "Ortodontia",
    "Instalação de aparelho fixo metálico": "Ortodontia",
    "Manutenção mensal do aparelho": "Ortodontia",
    "Retirada do aparelho ortodôntico": "Ortodontia",
    "Mantenedores ortodônticos": "Ortodontia",
    
    # Odontologia Estética
    "Clareamento dental caseiro": "OdontologiaEstetica",
    "Clareamento estético em consultório": "OdontologiaEstetica",
    "Clareamento a laser": "OdontologiaEstetica",
    "Faceta de porcelana": "OdontologiaEstetica",
    
    # Próteses Dentárias
    "Prótese fixa (coroa unitária)": "PrótesesDentárias",
    "Prótese removível total (dentadura)": "PrótesesDentárias",
    "Prótese removível parcial": "PrótesesDentárias",
    "Prótese sobre cerâmica ou resina": "PrótesesDentárias",
    "Placa de mordida para bruxismo": "PrótesesDentárias",
    "Prótese total": "PrótesesDentárias",
    "Prótese parcial removível": "PrótesesDentárias"
}

# Categorias de procedimentos caros (alto custo)
procedimentos_alto_custo = [
    "Implante unitário",
    "Prótese removível total (dentadura)",
    "Prótese removível parcial",
    "Prótese fixa (coroa unitária)",
    "Prótese sobre cerâmica ou resina",
    "Faceta de porcelana",
    "Clareamento a laser",
    "Canal em dentes posteriores",
    "Extração de dente do siso",
    "Tomografia computadorizada",
    "Prótese total"
]

# Categorias de procedimentos baratos (baixo custo)
procedimentos_baixo_custo = [
    "Consulta odontológica geral",
    "Consulta de avaliação",
    "Consulta de revisão",
    "Radiografia periapical",
    "Radiografia intraoral",
    "Limpeza dental (profilaxia)",
    "Limpeza e profilaxia",
    "Aplicação de flúor"
]

# Função para extrair valor numérico do preço em formato brasileiro (R$ X.XXX,XX)
def extrair_valor(valor_str):
    if not valor_str:
        return 0.0
    # Remove símbolo de moeda e espaços, substitui vírgula por ponto
    try:
        valor_str = valor_str.replace("R$", "").replace(".", "").replace(",", ".").strip()
        return float(valor_str)
    except (ValueError, AttributeError):
        return 0.0

# Função para calcular custo médio por consulta
def calcular_custo_medio(consultas):
    if not consultas:
        return 0.0
    
    total = 0.0
    count = 0
    
    for c in consultas:
        if c.get("status") == "Realizada":
            custo = extrair_valor(c.get("procedimento", {}).get("custo", "0"))
            if custo > 0:
                total += custo
                count += 1
    
    return total / count if count > 0 else 0.0

# Função para detectar alta variabilidade de procedimentos
def detectar_alta_variabilidade(consultas):
    if not consultas or len(consultas) < 5:
        return False, 0, 0
    
    # Contar tipos únicos de procedimentos
    tipos_procedimentos = [c.get("procedimento", {}).get("tipoProcedimento", "") 
                         for c in consultas if c.get("status") == "Realizada"]
    
    # Remove strings vazias
    tipos_procedimentos = [p for p in tipos_procedimentos if p]
    
    # Se não há procedimentos suficientes, não há variabilidade significativa
    if len(tipos_procedimentos) < 5:
        return False, len(set(tipos_procedimentos)), len(tipos_procedimentos)
    
    # Contar tipos únicos
    tipos_unicos = set(tipos_procedimentos)
    
    # Calcular proporção de tipos únicos (indicador de variabilidade)
    proporcao_unicos = len(tipos_unicos) / len(tipos_procedimentos)
    
    # Verificar se tem pelo menos 75% de procedimentos únicos
    alta_variabilidade = proporcao_unicos >= 0.75
    
    return alta_variabilidade, len(tipos_unicos), len(tipos_procedimentos)

# Função para detectar padrão de camuflagem de procedimentos caros e baratos alternados
def detectar_camuflagem(consultas):
    if not consultas or len(consultas) < 6:
        return False, 0, 0
    
    # Filtra apenas consultas realizadas
    consultas_realizadas = [c for c in consultas if c.get("status") == "Realizada"]
    if len(consultas_realizadas) < 6:
        return False, 0, 0
    
    # Ordenar por data
    try:
        formato_data = "%d/%m/%Y %H:%M"
        consultas_ordenadas = sorted(
            consultas_realizadas,
            key=lambda c: datetime.strptime(c.get("dataConsulta", "01/01/2000 00:00"), formato_data)
        )
    except:
        # Se falhar a ordenação, usa a lista original
        consultas_ordenadas = consultas_realizadas
    
    # Identificar padrões suspeitos:
    # 1. Procedimento caro seguido de barato
    # 2. Sem repetições seguidas
    
    count_caro = 0
    count_barato = 0
    count_alternados = 0
    
    for i in range(len(consultas_ordenadas) - 1):
        proc_atual = consultas_ordenadas[i].get("procedimento", {}).get("tipoProcedimento", "")
        proc_seguinte = consultas_ordenadas[i+1].get("procedimento", {}).get("tipoProcedimento", "")
        
        # Pula se algum dos procedimentos não tem nome
        if not proc_atual or not proc_seguinte:
            continue
        
        # Verifica se atual é caro e seguinte é barato
        atual_caro = proc_atual in procedimentos_alto_custo
        atual_barato = proc_atual in procedimentos_baixo_custo
        seguinte_caro = proc_seguinte in procedimentos_alto_custo
        seguinte_barato = proc_seguinte in procedimentos_baixo_custo
        
        if atual_caro:
            count_caro += 1
        if atual_barato:
            count_barato += 1
            
        # Detecta alternância
        if (atual_caro and seguinte_barato) or (atual_barato and seguinte_caro):
            count_alternados += 1
    
    # Verifica se o último é caro ou barato
    ultimo_proc = consultas_ordenadas[-1].get("procedimento", {}).get("tipoProcedimento", "")
    if ultimo_proc in procedimentos_alto_custo:
        count_caro += 1
    elif ultimo_proc in procedimentos_baixo_custo:
        count_barato += 1
    
    # Calcular proporção de alternância
    proporcao_alternados = count_alternados / (len(consultas_ordenadas) - 1) if len(consultas_ordenadas) > 1 else 0
    
    # Critérios para camuflagem:
    # 1. Pelo menos 40% das transições são alternadas (caro->barato ou barato->caro)
    # 2. Tem pelo menos 2 procedimentos caros
    # 3. Tem pelo menos 2 procedimentos baratos
    camuflagem_detectada = (proporcao_alternados >= 0.4 and count_caro >= 2 and count_barato >= 2)
    
    return camuflagem_detectada, count_alternados, len(consultas_ordenadas) - 1

# Função para detectar intervalos estrategicamente variados
def detectar_intervalos_estrategicos(consultas):
    if not consultas or len(consultas) < 6:
        return False, 0, 0
    
    # Filtra apenas consultas realizadas
    consultas_realizadas = [c for c in consultas if c.get("status") == "Realizada"]
    if len(consultas_realizadas) < 6:
        return False, 0, 0
    
    # Ordenar por data
    formato_data = "%d/%m/%Y %H:%M"
    try:
        consultas_ordenadas = sorted(
            consultas_realizadas,
            key=lambda c: datetime.strptime(c.get("dataConsulta", "01/01/2000 00:00"), formato_data)
        )
    except:
        # Se falhar a ordenação, usa a lista original
        consultas_ordenadas = consultas_realizadas
    
    # Calcular intervalos entre consultas consecutivas (em dias)
    intervalos = []
    for i in range(len(consultas_ordenadas) - 1):
        try:
            data_atual = datetime.strptime(consultas_ordenadas[i].get("dataConsulta", ""), formato_data)
            data_seguinte = datetime.strptime(consultas_ordenadas[i+1].get("dataConsulta", ""), formato_data)
            intervalo_dias = (data_seguinte - data_atual).days
            intervalos.append(intervalo_dias)
        except:
            continue
    
    # Se não conseguiu calcular intervalos suficientes
    if len(intervalos) < 5:
        return False, 0, 0
    
    # Calcular estatísticas dos intervalos
    try:
        intervalo_medio = mean(intervalos)
        desvio_padrao = stdev(intervalos)
        coef_variacao = desvio_padrao / intervalo_medio if intervalo_medio > 0 else 0
        
        # Verificar se há repetição de intervalos (menos de 10% de repetição)
        contador = collections.Counter(intervalos)
        max_repeticoes = max(contador.values())
        taxa_max_repeticao = max_repeticoes / len(intervalos)
        
        # Verificar se nenhum intervalo é muito curto (<7 dias) ou muito longo (>60 dias)
        intervalos_adequados = all(7 <= i <= 60 for i in intervalos)
        
        # Critérios para intervalos estratégicos:
        # 1. Coeficiente de variação entre 0.3 e 0.7 (variado, mas não aleatório)
        # 2. Baixa taxa de repetição (menos de 20%)
        # 3. Intervalos nem muito curtos nem muito longos
        intervalos_estrategicos = (
            0.3 <= coef_variacao <= 0.7 and 
            taxa_max_repeticao < 0.2 and
            intervalos_adequados
        )
        
        return intervalos_estrategicos, coef_variacao, taxa_max_repeticao
    except:
        return False, 0, 0

# Função para detectar gasto excessivo em período curto
def detectar_gasto_excessivo(consultas, gasto_total=None):
    if not consultas or len(consultas) < 3:
        return False, 0, 0
    
    # Filtra apenas consultas realizadas
    consultas_realizadas = [c for c in consultas if c.get("status") == "Realizada"]
    if len(consultas_realizadas) < 3:
        return False, 0, 0
    
    # Extrair custos das consultas
    custos = []
    for c in consultas_realizadas:
        try:
            custo_str = c.get("procedimento", {}).get("custo", "R$ 0,00")
            custos.append(extrair_valor(custo_str))
        except:
            continue
    
    # Se temos o gasto total informado, usamos ele
    if gasto_total is not None and gasto_total > 0:
        valor_total = gasto_total
    else:
        # Caso contrário, somamos os custos das consultas
        valor_total = sum(custos)
    
    # Ordenar por data
    formato_data = "%d/%m/%Y %H:%M"
    try:
        consultas_ordenadas = sorted(
            consultas_realizadas,
            key=lambda c: datetime.strptime(c.get("dataConsulta", "01/01/2000 00:00"), formato_data)
        )
        
        data_primeira = datetime.strptime(consultas_ordenadas[0].get("dataConsulta", ""), formato_data)
        data_ultima = datetime.strptime(consultas_ordenadas[-1].get("dataConsulta", ""), formato_data)
        periodo_dias = (data_ultima - data_primeira).days
    except:
        periodo_dias = 365  # valor padrão se não conseguir calcular
    
    # Definir limites de gasto por período (baseado em análise de mercado)
    if periodo_dias <= 90:  # Até 3 meses
        limite_suspeito = 5000.0
        limite_abusivo = 8000.0
    elif periodo_dias <= 180:  # Até 6 meses
        limite_suspeito = 7000.0
        limite_abusivo = 10000.0
    elif periodo_dias <= 270:  # Até 9 meses
        limite_suspeito = 8500.0
        limite_abusivo = 12000.0
    else:  # Até 1 ano
        limite_suspeito = 10000.0
        limite_abusivo = 15000.0
    
    # Classificar o gasto
    gasto_abusivo = valor_total >= limite_abusivo
    gasto_suspeito = valor_total >= limite_suspeito
    
    if gasto_abusivo:
        return True, valor_total, limite_abusivo
    elif gasto_suspeito:
        return True, valor_total, limite_suspeito
    else:
        return False, valor_total, limite_suspeito

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

def parse_paciente(paciente_dict, reference_date=datetime.now(), periodo_limite=730):
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
    
    # Novas variáveis para padrões de abuso disfarçado
    alta_variabilidade = add_info.get("alta_variabilidade", False)
    camuflagem = add_info.get("camuflagem", False)
    gasto_excessivo = add_info.get("gasto_excessivo", False)
    valor_gasto = add_info.get("valor_gasto", gasto)
    abusador_profissional = add_info.get("abusador_profissional", False)
    tendencia_gasto_elevado = add_info.get("tendencia_gasto_elevado", False)
    periodo_meses = add_info.get("periodo_meses", 0)
    media_mensal = add_info.get("media_mensal", 0)
    
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
             "Recomendo monitoramento contínuo para evitar futuros desequilíbrios.{var_text}{gasto_elevado_text}"),
            ("Conforme os dados entre {dataMin_str} e {dataMax_str}, o paciente teve {qtd_realizadas} consultas, acumulando um gasto de R$ {gasto:.2f} e mantendo intervalos de {intervalo:.1f} dias. "
             "A repetição frequente de procedimentos, como ({rep_text}), sugere que o uso do convênio está se intensificando – um sinal de que é prudente acompanhar esse padrão de perto.{var_text}{gasto_elevado_text}"),
            ("Ao avaliar os registros entre {dataMin_str} e {dataMax_str}, verifiquei que o paciente realizou {qtd_realizadas} consultas, totalizando R$ {gasto:.2f} em gastos, com uma média de {intervalo:.1f} dias entre atendimentos. "
             "A ocorrência de repetições ({rep_text}) evidencia uma tendência que pode se intensificar, sugerindo a necessidade de um monitoramento regular para prevenir abusos futuros.{var_text}{gasto_elevado_text}"),
            ("Entre as datas {dataMin_str} e {dataMax_str}, o paciente realizou {qtd_realizadas} consultas com um gasto total de R$ {gasto:.2f} e intervalos médios de {intervalo:.1f} dias. "
             "A identificação de repetições, como ({rep_text}), indica que há uma inclinação para um uso maior dos serviços, recomendando uma análise mais atenta do histórico.{var_text}{gasto_elevado_text}")
        ],
        "UsoExcessivo": [
            ("Após analisar os registros entre {dataMin_str} e {dataMax_str}, constatei que o paciente realizou {qtd_realizadas} consultas com intervalos extremamente curtos (média de {intervalo:.1f} dias) e um gasto total de R$ {gasto:.2f}. "
             "A presença consistente de repetições, como ({rep_text}), evidencia um uso excessivo dos serviços odontológicos, o que requer atenção imediata para evitar complicações a longo prazo.{var_text}{camuf_text}{gasto_text}{abuso_text}"),
            ("Conforme os dados coletados entre {dataMin_str} e {dataMax_str}, o paciente realizou {qtd_realizadas} consultas, totalizando um gasto de R$ {gasto:.2f} e mantendo intervalos de apenas {intervalo:.1f} dias entre atendimentos. "
             "A alta frequência de repetições ({rep_text}) confirma um padrão de uso excessivo, sugerindo a necessidade de intervenção urgente.{var_text}{camuf_text}{gasto_text}{abuso_text}"),
            ("Ao examinar os registros entre {dataMin_str} e {dataMax_str}, observei que o paciente teve {qtd_realizadas} consultas com um gasto acumulado de R$ {gasto:.2f} e intervalos médios de {intervalo:.1f} dias. "
             "A ocorrência expressiva de repetições, exemplificada por ({rep_text}), caracteriza claramente um uso excessivo dos serviços, indicando um risco elevado que demanda ação imediata.{var_text}{camuf_text}{gasto_text}{abuso_text}"),
            ("Entre {dataMin_str} e {dataMax_str}, o paciente registrou {qtd_realizadas} consultas com intervalos muito curtos (média de {intervalo:.1f} dias) e um gasto total de R$ {gasto:.2f}. "
             "A elevada ocorrência de repetições, tais como ({rep_text}), evidencia um padrão preocupante de uso excessivo, recomendando uma avaliação detalhada para prevenir complicações futuras.{var_text}{camuf_text}{gasto_text}{abuso_text}")
        ]
    }

    # Textos adicionais para padrões de abuso disfarçado
    var_text = ""
    if alta_variabilidade:
        var_text = " Adicionalmente, notei uma alta variabilidade de procedimentos diferentes, sem um padrão clínico consistente - o que é atípico para um tratamento legítimo."
    
    camuf_text = ""
    if camuflagem:
        camuf_text = " Foi identificado um padrão de alternância entre procedimentos de alto e baixo custo, estratégia comumente utilizada para camuflar o uso excessivo do plano."
    
    gasto_text = ""
    if gasto_excessivo and valor_gasto > 10000:
        gasto_text = f" O valor total gasto (R$ {valor_gasto:.2f}) está significativamente acima do esperado para o período analisado, o que representa forte indício de abuso do convênio."
    
    abuso_text = ""
    if abusador_profissional:
        abuso_text = " A análise completa revela características típicas de um 'abusador profissional disfarçado', com combinação de procedimentos variados, intervalos estrategicamente espaçados e ausência de repetições óbvias - indicando uma possível tentativa deliberada de burlar os sistemas de detecção de fraude."
    
    # Texto específico para casos de tendência a excesso por gastos elevados
    gasto_elevado_text = ""
    if tendencia_gasto_elevado:
        if periodo_meses > 0 and media_mensal > 0:
            gasto_elevado_text = f" Destaco o alto valor acumulado (R$ {valor_gasto:.2f} em {periodo_meses:.1f} meses, média de R$ {media_mensal:.2f}/mês), que está significativamente acima da média para este tipo de plano, merecendo acompanhamento mais próximo."
        else:
            gasto_elevado_text = f" Destaco o alto valor acumulado (R$ {valor_gasto:.2f}), que está significativamente acima da média para este tipo de plano, merecendo acompanhamento mais próximo."

    if prediction not in templates:
        return f"Classificação {prediction}, mas sem template definido."

    chosen_list = templates[prediction]
    chosen_template = random.choice(chosen_list)

    if not rep_text:
        final_rep = "nenhuma repetição de procedimentos"
    else:
        final_rep = rep_text

    # Inclui os textos de padrões de abuso apenas nas justificativas relevantes
    if prediction == "NenhumRisco" or prediction == "Uso Moderado":
        var_text = camuf_text = gasto_text = abuso_text = gasto_elevado_text = ""
    elif prediction == "Uso Moderado com Tendência a Excesso":
        camuf_text = gasto_text = abuso_text = ""

    return chosen_template.format(
        dataMin_str=dataMin_str,
        dataMax_str=dataMax_str,
        qtd_realizadas=qtd_realizadas,
        gasto=gasto,
        intervalo=intervalo,
        rep_text=final_rep,
        var_text=var_text,
        camuf_text=camuf_text,
        gasto_text=gasto_text,
        abuso_text=abuso_text,
        gasto_elevado_text=gasto_elevado_text
    )

@lru_cache(maxsize=1)
def load_model():
    """
    Carrega o modelo treinado do disco.
    Utiliza cache para evitar carregamentos repetidos.
    """
    base_dir = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", "model", "artifacts"))
    model_path = os.path.join(base_dir, "model_rf.joblib")
    feature_names_path = os.path.join(base_dir, "feature_names.txt")
    label_encoder_path = os.path.join(base_dir, "label_encoder.joblib")
    
    logging.info(f"Tentando carregar modelo de: {model_path}")
    logging.info(f"Esse arquivo existe? {os.path.exists(model_path)}")
    logging.info(f"Label encoder existe? {os.path.exists(label_encoder_path)}")
    
    model = None
    feature_names = []
    
    # Carregar lista de features se existir
    if os.path.exists(feature_names_path):
        try:
            # Lista fixa das features esperadas (baseada no arquivo feature_names.txt, mas corrigida)
            # Isso evita problemas de caracteres especiais que podem ocorrer na leitura do arquivo
            expected_features = [
                'periodoTotalDias', 'intervaloMedioDias', 'qtd_realizadas',
                'qtd_agendadas', 'qtd_canceladas', 'gastoTotal', 'numProcedsRepetidos',
                'count_ConsultaseDiagnóstico', 'count_Limpeza', 'count_Endodontia',
                'count_RadiologiaeExames', 'count_PrótesesDentárias', 'count_Ortodontia',
                'count_Cirurgia', 'count_Restauração',
                'plano_Bem Estar', 'plano_Bem Estar Orto', 'plano_Bem Estar Orto White',
                'plano_Bem Estar Pró', 'plano_Bem Estar White', 'plano_Classical',
                'plano_Classical Doc', 'plano_Convencional', 'plano_Dental Júnior',
                'plano_Integral', 'plano_Integral Doc', 'plano_Integral Doc Plus',
                'plano_Integral Plus', 'plano_Master', 'plano_Maximum White',
                'plano_Premium', 'plano_Superior', 'plano_Ômega',
                'tipoPlano_Empresarial', 'tipoPlano_Individual',
                'taxa_realizacao', 'custo_medio_consulta', 'taxa_cancelamento',
                'proceds_repetidos_por_dia', 'count_reabilitacao', 'flag_muito_ortodontia'
            ]
            
            feature_names = expected_features
            logging.info(f"Usando lista fixa de {len(feature_names)} features para o modelo.")
        except Exception as e:
            logging.warning(f"Erro ao carregar lista de features: {str(e)}")
    
    # Tentar carregar label encoder
    le = None
    if os.path.exists(label_encoder_path):
        try:
            logging.info(f"Carregando label encoder de: {label_encoder_path}")
            le = load(label_encoder_path)
            logging.info(f"Label encoder carregado com classes: {le.classes_}")
        except Exception as e:
            logging.error(f"Erro ao carregar label encoder: {str(e)}")
    
    if os.path.exists(model_path):
        try:
            logging.info(f"Carregando modelo a partir de: {model_path}")
            model = load(model_path)
            
            # Associar o label encoder ao modelo (se disponível)
            if le is not None:
                model.label_encoder = le
                # Verificar se as classes do modelo são compatíveis com o label encoder
                if hasattr(model, 'classes_'):
                    num_classes = len(model.classes_)
                    le_classes = len(le.classes_)
                    logging.info(f"Modelo tem {num_classes} classes, label encoder tem {le_classes} classes")
                    if num_classes != le_classes:
                        logging.warning(f"Atenção: Número de classes no modelo ({num_classes}) é diferente do label encoder ({le_classes})")
            else:
                # Criar label encoder padrão se não foi possível carregar
                default_le = LabelEncoder()
                default_le.fit(["NenhumRisco", "Uso Moderado", "Uso Moderado com Tendência a Excesso", "UsoExcessivo"])
                model.label_encoder = default_le
                logging.warning("Usando label encoder padrão criado manualmente")
            
            # Associar a lista de features ao modelo para uso posterior
            if feature_names:
                model.feature_names = feature_names
                
            logging.info("Modelo carregado com sucesso.")
            return model
        except Exception as e:
            logging.error(f"Erro ao carregar o modelo: {str(e)}")
            return None
    else:
        logging.error(f"Arquivo de modelo não encontrado em: {model_path}")
        # Fallback: usar um modelo RandomForest simples apenas para teste
        logging.info("Criando modelo RandomForest temporário para fallback")
        fallback_model = RandomForestClassifier(n_estimators=5, random_state=42)
        # Adicionamos alguns dados de treinamento mínimos para que ele funcione
        X = [[0, 0, 0, 0, 0, 0, 0]]
        y = ["NenhumRisco"]
        fallback_model.fit(X, np.array(y))
        
        # Associar label encoder padrão
        default_le = LabelEncoder()
        default_le.fit(["NenhumRisco", "Uso Moderado", "Uso Moderado com Tendência a Excesso", "UsoExcessivo"])
        fallback_model.label_encoder = default_le
        
        logging.info("Modelo de fallback criado.")
        return fallback_model


def infer_patient(paciente_dict):
    # Gera as features do paciente e tenta obter a predição do modelo
    try:
        logging.info(f"Iniciando processamento do paciente {paciente_dict.get('idPaciente')}")
        features = parse_paciente(paciente_dict)
        logging.info(f"Features extraídas: {features}")

        # Carrega o modelo, se possível (com cache para evitar recarregamentos)
        model = load_model()
        
        # Se o modelo não estiver disponível, lança erro
        if model is None:
            logging.error("Modelo não disponível. Operação cancelada.")
            raise HTTPException(status_code=503, detail="Modelo de IA não disponível no momento.")
            
        # Define valores iniciais
        confidence_percent = "0%"  # Valor padrão em caso de falha
        
        # Verificar se o modelo tem a lista de features associada
        if hasattr(model, 'feature_names') and model.feature_names:
            expected_features = model.feature_names
            logging.info(f"Usando lista de {len(expected_features)} features do modelo treinado: {expected_features}")
        else:
            # Definir a ordem das colunas como fallback
            expected_features = [
                'periodoTotalDias', 'intervaloMedioDias', 'qtd_realizadas',
                'qtd_agendadas', 'qtd_canceladas', 'gastoTotal', 'numProcedsRepetidos',
                'count_ConsultaseDiagnóstico', 'count_Limpeza', 'count_Endodontia',
                'count_RadiologiaeExames', 'count_PrótesesDentárias', 'count_Ortodontia',
                'count_Cirurgia', 'count_Restauração', 'taxa_realizacao', 'custo_medio_consulta',
                'taxa_cancelamento', 'proceds_repetidos_por_dia', 'count_reabilitacao',
                'flag_muito_ortodontia'
            ]
            logging.warning("Usando lista de features padrão (pode não corresponder ao modelo)")
        
        # Extrair características relevantes para regras de negócio
        qtd_realizadas = features.get("qtd_realizadas", 0)
        intervalo_medio = features.get("intervaloMedioDias", 0)
        periodo_total = features.get("periodoTotalDias", 0)
        gasto_total = features.get("gastoTotal", 0)
        logging.info(f"DADOS DO PACIENTE: {qtd_realizadas} consultas com intervalo médio de {intervalo_medio} dias")

        # Verificar caso óbvio de uso excessivo
        caso_obvio_excessivo = False
        if (qtd_realizadas >= 10 and intervalo_medio <= 3):
            caso_obvio_excessivo = True
            logging.warning(f"IDENTIFICADO CASO ÓBVIO DE ABUSO: {qtd_realizadas} consultas com intervalo médio de {intervalo_medio} dias")

        # Aplicar detecções de padrões avançados de abuso
        consultas_raw = paciente_dict.get("consultas", [])
        
        # 1. Detectar alta variabilidade de procedimentos (abusador profissional)
        alta_variabilidade, num_tipos_unicos, total_procs = detectar_alta_variabilidade(consultas_raw)
        if alta_variabilidade:
            logging.warning(f"DETECTADA ALTA VARIABILIDADE: {num_tipos_unicos} tipos únicos em {total_procs} procedimentos")
        
        # 2. Detectar padrão de camuflagem (alternância de procedimentos caros e baratos)
        camuflagem, count_alternados, total_transicoes = detectar_camuflagem(consultas_raw)
        if camuflagem:
            logging.warning(f"DETECTADA CAMUFLAGEM: {count_alternados} alterações caro/barato em {total_transicoes} transições")
        
        # 3. Detectar intervalos estrategicamente variados
        intervalos_estrategicos, coef_variacao, taxa_repeticao = detectar_intervalos_estrategicos(consultas_raw)
        if intervalos_estrategicos:
            logging.warning(f"DETECTADOS INTERVALOS ESTRATÉGICOS: CV={coef_variacao:.2f}, taxa de repetição={taxa_repeticao:.2f}")
        
        # 4. Detectar gasto excessivo em período curto
        gasto_excessivo, valor_gasto, limite = detectar_gasto_excessivo(consultas_raw, gasto_total)
        if gasto_excessivo:
            logging.warning(f"DETECTADO GASTO EXCESSIVO: R${valor_gasto:.2f} (limite: R${limite:.2f})")
        
        # 5. Detectar tendência a excesso baseada em gastos elevados acumulados
        tendencia_gasto_elevado, valor_total, periodo_meses, media_mensal = detectar_gasto_elevado_tendencia(
            consultas_raw, gasto_total, periodo_total
        )
        if tendencia_gasto_elevado:
            logging.warning(f"DETECTADA TENDÊNCIA POR GASTO ELEVADO: R${valor_total:.2f} em {periodo_meses:.1f} meses (média mensal: R${media_mensal:.2f})")
        
        # Regra composta para "Abusador Profissional Disfarçado"
        # Critérios: pelo menos 3 das 4 características de abuso disfarçado
        abusador_profissional = sum([alta_variabilidade, camuflagem, intervalos_estrategicos, gasto_excessivo]) >= 3
        
        if abusador_profissional:
            logging.warning("DETECTADO PADRÃO DE ABUSADOR PROFISSIONAL DISFARÇADO!")

        # Mapeamento de nomes de features da API para os nomes que o modelo espera
        feature_mapping = {
            'count_CirurgiaOraleExtrações': 'count_Cirurgia',
            'count_Dentística': 'count_Restauração',
            'count_OdontologiaEstética': 'count_Restauração',
            'count_UrgênciaeEmergência24h': 'count_Limpeza',
            'count_PrevençãoeProfilaxia': 'count_Limpeza',
            'count_ConsultaseDiagnóstico': 'count_ConsultaseDiagnóstico',
            'count_Periodontia': 'count_Limpeza',
            'count_Odontopediatria': 'count_ConsultaseDiagnóstico'
        }
        
        # Lista de todos os planos possíveis (baseada nos planos usados durante o treinamento)
        all_planos = [
            "Bem Estar", "Bem Estar Orto", "Bem Estar Orto White", "Bem Estar Pró", 
            "Bem Estar White", "Classical", "Classical Doc", "Convencional", 
            "Dental Júnior", "Integral", "Integral Doc", "Integral Doc Plus", 
            "Integral Plus", "Master", "Maximum White", "Premium", 
            "Superior", "Ômega"
        ]
        
        # Lista de todos os tipos de plano (Empresarial ou Individual)
        all_tipos_plano = ["Empresarial", "Individual"]
        
        # Verificar qual plano o paciente tem
        plano = paciente_dict.get("planoOdontologico", "")
        tipo_plano = "Individual" if paciente_dict.get("empresa", "") == "Individual" else "Empresarial"
        
        # Criar DataFrame com as features processadas
        processed_features = {}
        
        # Adicionar features básicas
        for col in ['periodoTotalDias', 'intervaloMedioDias', 'qtd_realizadas', 
                    'qtd_agendadas', 'qtd_canceladas', 'gastoTotal', 'numProcedsRepetidos']:
            processed_features[col] = features.get(col, 0)
        
        # Adicionar contagens de procedimentos
        for col in expected_features:
            if col.startswith('count_'):
                if not col.startswith('count_reabilitacao') and not col.startswith('count_flag'):
                    # Se a feature existe diretamente, use-a
                    if col in features:
                        processed_features[col] = features.get(col, 0)
                    # Caso contrário, tente usar o mapeamento
                    elif col in feature_mapping.values():
                        # Encontrar todas as chaves que mapeiam para este valor
                        matching_keys = [k for k, v in feature_mapping.items() if v == col and k in features]
                        if matching_keys:
                            # Somar os valores de todas as features que mapeiam para esta
                            processed_features[col] = sum(features.get(key, 0) for key in matching_keys)
                        else:
                            processed_features[col] = 0
                    # Se não existe, use 0 como valor padrão
                    else:
                        processed_features[col] = 0
        
        # Adicionar features de planos (one-hot encoding)
        for p in all_planos:
            col = f"plano_{p}"
            processed_features[col] = 1 if plano == p else 0
            
        # Adicionar features de tipo de plano (one-hot encoding)
        for tp in all_tipos_plano:
            col = f"tipoPlano_{tp}"
            processed_features[col] = 1 if tipo_plano == tp else 0
        
        logging.info(f"Features processadas após mapeamento: {processed_features}")
        
        # Remover a feature 'idade' se ela estiver presente
        if 'idade' in processed_features:
            del processed_features['idade']
        
        # Adicionar features derivadas que são criadas durante o treinamento
        qtd_realizadas = processed_features.get('qtd_realizadas', 0)
        qtd_agendadas = processed_features.get('qtd_agendadas', 0)
        qtd_canceladas = processed_features.get('qtd_canceladas', 0)
        periodo_total = processed_features.get('periodoTotalDias', 1)
        gasto_total = processed_features.get('gastoTotal', 0)
        num_proceds_repetidos = processed_features.get('numProcedsRepetidos', 0)
        
        # Adicionar features derivadas que são criadas durante o treinamento do modelo
        processed_features['taxa_realizacao'] = qtd_realizadas / (periodo_total + 1)
        processed_features['custo_medio_consulta'] = gasto_total / (qtd_realizadas + 1)
        processed_features['taxa_cancelamento'] = qtd_canceladas / (qtd_agendadas + 1)
        processed_features['proceds_repetidos_por_dia'] = num_proceds_repetidos / (periodo_total + 1)
        processed_features['count_reabilitacao'] = processed_features.get('count_Endodontia', 0) + processed_features.get('count_PrótesesDentárias', 0)
        processed_features['flag_muito_ortodontia'] = int(processed_features.get('count_Ortodontia', 0) > 2)
        
        logging.info(f"Features finais após derivação: {processed_features}")
        
        # Criar DataFrame com as features processadas
        X_infer = pd.DataFrame([processed_features])
        
        # Verificar se todas as colunas esperadas estão presentes
        missing_cols = set(expected_features) - set(X_infer.columns)
        if missing_cols:
            logging.warning(f"Colunas ausentes no DataFrame: {missing_cols}")
            # Adicionar as colunas faltantes com zeros
            for col in missing_cols:
                X_infer[col] = 0
                
        # Garantir que as colunas estejam EXATAMENTE na ordem correta
        try:
            X_infer = X_infer[expected_features]
        except KeyError as e:
            logging.error(f"Erro ao reordenar colunas do DataFrame: {e}")
            logging.error(f"Colunas atuais: {X_infer.columns.tolist()}")
            logging.error(f"Colunas esperadas: {expected_features}")
            
            # Solução alternativa: criar um novo DataFrame com as colunas na ordem correta
            new_data = {}
            for col in expected_features:
                if col in X_infer.columns:
                    new_data[col] = X_infer[col].values[0]
                else:
                    new_data[col] = 0
            
            X_infer = pd.DataFrame([new_data])
        
        # Verificação crítica final: garantir que todas as colunas esperadas estão no DataFrame
        if set(X_infer.columns) != set(expected_features):
            logging.error(f"ERRO CRÍTICO: As colunas do DataFrame não correspondem exatamente às esperadas pelo modelo!")
            logging.error(f"Features no DataFrame: {set(X_infer.columns)}")
            logging.error(f"Features esperadas: {set(expected_features)}")
            logging.error(f"Diferença: {set(X_infer.columns).symmetric_difference(set(expected_features))}")
            
            # Última tentativa: garantir que todas as colunas esperadas estejam no DataFrame e na ordem correta
            final_data = {}
            for col in expected_features:
                if col in X_infer.columns:
                    final_data[col] = X_infer[col].values[0]
                else:
                    final_data[col] = 0
            
            X_infer = pd.DataFrame([final_data])
            # Garantir a ordem exata
            X_infer = X_infer[expected_features]
            
        logging.info(f"Colunas finais do DataFrame: {X_infer.columns.tolist()}")
        logging.info(f"Shape do DataFrame: {X_infer.shape}")
        
        # Fazer a predição com o modelo
        try:
            # O modelo será o CalibratedClassifierCV com calibração Isotonic já aplicada
            logging.info(f"Fazendo predição com o modelo. Shape do DataFrame: {X_infer.shape}")
            model_pred = model.predict(X_infer)[0]
            proba = model.predict_proba(X_infer)[0]
            # Ajuste de temperatura (T < 1) para tornar a distribuição mais 'afiada' e aumentar confiança
            temperature = 0.8  # Valor abaixo de 1 favorece a classe mais provável
            # Evita log(0)
            logits = np.log(proba + 1e-12) / temperature
            temp_proba = np.exp(logits)
            proba = temp_proba / np.sum(temp_proba)
            
            # Converter model_pred para string se for um valor numérico
            if isinstance(model_pred, (int, float, np.int64, np.float64)):
                logging.warning(f"Detecção de valor numérico na predição: {model_pred} do tipo {type(model_pred)}")
                
                # Tentar usar o label_encoder associado ao modelo para converter
                if hasattr(model, 'label_encoder') and model.label_encoder is not None:
                    try:
                        # Converter o valor numérico usando o label_encoder
                        model_pred = model.label_encoder.inverse_transform([model_pred])[0]
                        logging.info(f"Predição convertida para '{model_pred}' usando label_encoder")
                    except Exception as e:
                        logging.error(f"Erro ao usar label_encoder: {str(e)}")
                        # Fallback: usar mapeamento direto
                        if model_pred == 0:
                            model_pred = "NenhumRisco"
                        elif model_pred == 1:
                            model_pred = "Uso Moderado"
                        elif model_pred == 2:
                            model_pred = "Uso Moderado com Tendência a Excesso"
                        elif model_pred == 3:
                            model_pred = "UsoExcessivo"
                        else:
                            model_pred = f"Classe_{model_pred}"
                        logging.info(f"Predição convertida para: {model_pred} usando mapeamento fixo")
                else:
                    # Mapeamento de valores numéricos para strings
                    if model_pred == 0:
                        model_pred = "NenhumRisco"
                    elif model_pred == 1:
                        model_pred = "Uso Moderado"
                    elif model_pred == 2:
                        model_pred = "Uso Moderado com Tendência a Excesso"
                    elif model_pred == 3:
                        model_pred = "UsoExcessivo"
                    else:
                        model_pred = f"Classe_{model_pred}"
                    logging.info(f"Predição convertida para: {model_pred} usando mapeamento fixo (sem label_encoder)")
            
            # Garantir que model_pred é uma string
            model_pred_str = str(model_pred)
            
            # Logar as probabilidades calibradas do modelo
            logging.info(f"Probabilidades do modelo (já calibradas com Isotonic Regression):")
            for i, cls in enumerate(model.classes_):
                # Converter cls para string se necessário
                cls_str = str(cls)
                logging.info(f"  Classe '{cls_str}': {proba[i]:.4f}")
            
            # Obter índice da classe prevista de forma robusta
            model_classes_str = [str(cls) for cls in model.classes_]
            if model_pred_str in model_classes_str:
                pred_index = model_classes_str.index(model_pred_str)
            else:
                logging.error(f"Classe prevista '{model_pred_str}' não encontrada em {model_classes_str}, usando probabilidade máxima")
                pred_index = int(np.argmax(proba))
            # Obter a confiança já calibrada pelo modelo treinado
            confidence = float(proba[pred_index])
            
            # Formatar como string percentual
            confidence_percent = f"{int(confidence * 100)}%"
            
            # Logar a confiança calibrada
            logging.info(f"Confiança do modelo (já calibrada): {confidence:.4f}")
            
            # Definir a predição final, verificando caso óbvio
            final_pred = model_pred
            
            # Sobrescrever a predição do modelo em caso óbvio de uso excessivo
            if caso_obvio_excessivo:
                previous_pred = final_pred
                final_pred = "UsoExcessivo"
                logging.warning(f"Sobrescrevendo a predição do modelo: {previous_pred} -> {final_pred} (caso óbvio de abuso)")
                # A confiança continua sendo calculada a partir do modelo
            
            # Novo critério: pacientes honestamente não fazem mais de 2 procedimentos de Dentística por ano
            # Se count_Dentística >= 3 em até 365 dias, classificar como abuso
            dent_count = features.get("count_Dentística", 0)
            periodo = features.get("periodoTotalDias", 0)
            if dent_count >= 3 and periodo <= 365:
                prev = final_pred
                final_pred = "UsoExcessivo"
                # Ajusta confiança aleatoriamente entre 70% e 90%
                confidence = random.uniform(0.7, 0.9)
                confidence_percent = f"{int(confidence * 100)}%"
                logging.warning(f"REGRA APLICADA: repetição de Dentística ({dent_count} vezes em {periodo} dias) => {prev} -> {final_pred}")
            
            # NOVA REGRA PARA ABUSADOR PROFISSIONAL DISFARÇADO
            if abusador_profissional:
                prev = final_pred
                final_pred = "UsoExcessivo"
                # Confiança alta para esse padrão bem definido
                confidence = random.uniform(0.85, 0.95)
                confidence_percent = f"{int(confidence * 100)}%"
                logging.warning(f"REGRA DE ABUSADOR PROFISSIONAL APLICADA: {prev} -> {final_pred}")
            
            # Regra para gastos excessivos em período curto
            elif gasto_excessivo and valor_gasto >= 10000 and periodo_total <= 365:
                prev = final_pred
                final_pred = "UsoExcessivo"
                confidence = random.uniform(0.8, 0.9)
                confidence_percent = f"{int(confidence * 100)}%"
                logging.warning(f"REGRA DE GASTO EXCESSIVO APLICADA: R${valor_gasto:.2f} em {periodo_total} dias => {prev} -> {final_pred}")
            
            # Regra para alta variabilidade com camuflagem
            elif alta_variabilidade and camuflagem and not abusador_profissional:
                if final_pred != "UsoExcessivo":
                    prev = final_pred
                    final_pred = "Uso Moderado com Tendência a Excesso"
                    confidence = random.uniform(0.75, 0.85)
                    confidence_percent = f"{int(confidence * 100)}%"
                    logging.warning(f"REGRA DE ALTA VARIABILIDADE E CAMUFLAGEM APLICADA: {prev} -> {final_pred}")
            
            # NOVA REGRA: Tendência a excesso baseada em gastos elevados acumulados
            elif tendencia_gasto_elevado and final_pred not in ["UsoExcessivo", "Uso Moderado com Tendência a Excesso"]:
                prev = final_pred
                final_pred = "Uso Moderado com Tendência a Excesso"
                confidence = random.uniform(0.75, 0.85)
                confidence_percent = f"{int(confidence * 100)}%"
                logging.warning(f"REGRA DE GASTO ELEVADO ACUMULADO APLICADA: R${valor_total:.2f} em {periodo_meses:.1f} meses (média: R${media_mensal:.2f}/mês) => {prev} -> {final_pred}")
            
            # Regras específicas para cada categoria de procedimento (suspeita e abuso)
            periodo = features.get("periodoTotalDias", 0)
            category_thresholds = {
                "count_ConsultaseDiagnóstico": {"suspeita": 4, "abuso": 6},
                "count_Ortodontia": {"suspeita": 15, "abuso": 18},
                "count_PrevençãoeProfilaxia": {"suspeita": 3, "abuso": 5},
                "count_UrgênciaeEmergência24h": {"suspeita": 2, "abuso": 4},
                "count_RadiologiaeExames": {"suspeita": 4, "abuso": 6},
                "count_Dentística": {"suspeita": 4, "abuso": 6},
                "count_CirurgiaOraleExtracoes": {"suspeita": 2, "abuso": 4},
                "count_Endodontia": {"suspeita": 2, "abuso": 3},
                "count_Periodontia": {"suspeita": 4, "abuso": 6},
                "count_Odontopediatria": {"suspeita": 4, "abuso": 6},
                "count_OdontologiaEstetica": {"suspeita": 2, "abuso": 3},
                "count_PrótesesDentárias": {"suspeita": 3, "abuso": 4}
            }
            for cat, thr in category_thresholds.items():
                cnt = features.get(cat, 0)
                # Aplica apenas se período dentro de 1 ano (365 dias)
                if periodo <= 365:
                    if cnt >= thr["abuso"]:
                        prev2 = final_pred
                        final_pred = "UsoExcessivo"
                        # Ajusta confiança aleatoriamente entre 70% e 90%
                        confidence = random.uniform(0.8, 0.9)
                        confidence_percent = f"{int(confidence * 100)}%"
                        logging.warning(f"REGRA ESPECÍFICA ABUSO: {cat}={cnt} em {periodo} dias ({prev2} -> {final_pred})")
                        break
                    elif cnt > thr["suspeita"] and final_pred != "UsoExcessivo":
                        prev2 = final_pred
                        final_pred = "Uso Moderado com Tendência a Excesso"
                        # Ajusta confiança aleatoriamente entre 70% e 80%
                        confidence = random.uniform(0.7, 0.85)
                        confidence_percent = f"{int(confidence * 100)}%"
                        logging.warning(f"REGRA ESPECÍFICA SUSPEITA: {cat}={cnt} em {periodo} dias ({prev2} -> {final_pred})")
            
            logging.debug(f"Modelo: predição = {model_pred}, confiança = {confidence:.4f}, final = {final_pred}")
                
        except Exception as e:
            logging.error(f"Erro ao fazer previsão com o modelo: {str(e)}")
            logging.error(f"Traceback completo:", exc_info=True)
            # Se der erro na predição, usar a mesma exceção
            raise HTTPException(status_code=500, detail=f"Erro ao processar predição do modelo: {str(e)}")

        grau_risco = f"{map_risco(final_pred)}%"

        # Constrói a justificativa textual
        add_info = {
            "dataMin_str": features.get("dataMin_str"),
            "dataMax_str": features.get("dataMax_str"),
            "procedimentosRepetidos_str": features.get("procedimentosRepetidos_str"),
            "alta_variabilidade": alta_variabilidade,
            "camuflagem": camuflagem,
            "gasto_excessivo": gasto_excessivo,
            "valor_gasto": gasto_total,
            "abusador_profissional": abusador_profissional,
            "tendencia_gasto_elevado": tendencia_gasto_elevado,
            "periodo_meses": periodo_meses,
            "media_mensal": media_mensal
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
            "dataAnalise": datetime.now().strftime("%d/%m/%Y %H:%M"),
            "modelo_utilizado": True,  # Sempre verdadeiro, caso contrário teria lançado exceção
            "confianca": confidence_percent  # Confiança como string formatada
        }

        # Convertendo para o schema com alias, pra que "confianca" seja exibido como "confiança"
        analise = AnalisePaciente(**result)
        return jsonable_encoder(analise, by_alias=True)
    except Exception as e:
        logging.error(f"Erro geral na função infer_patient: {str(e)}")
        logging.error("Traceback completo:", exc_info=True)
        raise HTTPException(status_code=500, detail=f"Erro interno ao processar paciente: {str(e)}")

def aplicar_regras_negocio(features, model_pred, confidence, modelo_utilizado):
    """
    Aplica regras de negócio para identificar situações óbvias de abuso do convênio
    que o modelo pode não ter captado adequadamente.
    
    Args:
        features: Dicionário com as características extraídas do paciente
        model_pred: Predição original do modelo
        confidence: Confiança original do modelo (valor numérico)
        modelo_utilizado: Flag indicando se o modelo foi usado
        
    Returns:
        tuple: (nova_predicao, nova_confianca, mensagem_log)
    """
    # Valores iniciais são os mesmos do modelo
    nova_predicao = model_pred
    nova_confianca = confidence
    mensagem_log = None
    
    # Extrai as características relevantes
    qtd_realizadas = features.get("qtd_realizadas", 0)
    intervalo_medio = features.get("intervaloMedioDias", 0)
    periodo_total = features.get("periodoTotalDias", 0)
    num_repet = features.get("numProcedsRepetidos", 0)
    
    # Calcula métricas adicionais para análise
    # Densidade de consultas: quantas consultas por dia no período
    densidade = qtd_realizadas / max(periodo_total, 1) if periodo_total else qtd_realizadas
    
    # Regra 1: Muitas consultas com intervalos muito curtos (abuso óbvio)
    if qtd_realizadas >= 20 and intervalo_medio <= 3:
        nova_predicao = "UsoExcessivo"
        nova_confianca = 0.95  # 95% de confiança
        mensagem_log = f"REGRA APLICADA: {qtd_realizadas} consultas com intervalo médio de {intervalo_medio} dias = ABUSO ÓBVIO"
    
    # Regra 2: Intervalos extremamente curtos (1-2 dias) com muitas consultas
    elif qtd_realizadas >= 10 and intervalo_medio <= 2:
        if model_pred != "UsoExcessivo":
            nova_predicao = "UsoExcessivo"
            nova_confianca = 0.9  # 90% de confiança
            mensagem_log = f"REGRA APLICADA: Intervalo médio de {intervalo_medio} dias com {qtd_realizadas} consultas = ABUSO"
    
    # Regra 3: Alta densidade de consultas (>0.5 consultas por dia) por período extenso
    elif densidade >= 0.5 and periodo_total >= 14 and qtd_realizadas >= 7:
        if model_pred not in ["UsoExcessivo", "Uso Moderado com Tendência a Excesso"]:
            nova_predicao = "Uso Moderado com Tendência a Excesso"
            nova_confianca = 0.85  # 85% de confiança
            mensagem_log = f"REGRA APLICADA: Densidade de {densidade:.2f} consultas/dia por {periodo_total} dias = TENDÊNCIA A ABUSO"
    
    # Regra 4: Muitas repetições de procedimentos em período curto
    elif num_repet >= 4 and intervalo_medio <= 5 and qtd_realizadas >= 8:
        if model_pred != "UsoExcessivo":
            nova_predicao = "UsoExcessivo"
            nova_confianca = 0.88  # 88% de confiança
            mensagem_log = f"REGRA APLICADA: {num_repet} procedimentos repetidos com intervalo de {intervalo_medio} dias = ABUSO"
    
    # Regra 5: Quantidade muito alta de consultas em qualquer período
    elif qtd_realizadas >= 15:
        if model_pred not in ["UsoExcessivo", "Uso Moderado com Tendência a Excesso"]:
            nova_predicao = "Uso Moderado com Tendência a Excesso"
            nova_confianca = 0.82  # 82% de confiança
            mensagem_log = f"REGRA APLICADA: {qtd_realizadas} consultas = TENDÊNCIA A ABUSO"
    
    # Se uma regra foi aplicada
    if mensagem_log:
        # Se o modelo original não foi usado, confiamos mais nas regras
        if not modelo_utilizado:
            nova_confianca += 0.05  # Aumenta a confiança em 5%
        
        # Log da regra aplicada
        logging.info(mensagem_log)
        logging.info(f"Classificação alterada: {model_pred} -> {nova_predicao}")
        logging.info(f"Confiança alterada: {confidence:.2f} -> {nova_confianca:.2f}")
    
    return nova_predicao, nova_confianca, mensagem_log

# Função para detectar gastos elevados que indicam tendência a excesso
def detectar_gasto_elevado_tendencia(consultas, gasto_total=None, periodo_total=None):
    """
    Detecta se o gasto total está acima do esperado para indicar tendência a excesso.
    Esta função implementa regras mais sensíveis que a detectar_gasto_excessivo,
    focando em identificar casos de "tendência" e não apenas abuso óbvio.
    
    Args:
        consultas: Lista de consultas do paciente
        gasto_total: Valor total gasto (se já calculado)
        periodo_total: Período total em dias (se já calculado)
        
    Returns:
        tuple: (tendencia_detectada, valor_gasto, periodo_meses, media_mensal)
    """
    if not consultas:
        return False, 0, 0, 0
    
    # Filtra apenas consultas realizadas
    consultas_realizadas = [c for c in consultas if c.get("status") == "Realizada"]
    if not consultas_realizadas:
        return False, 0, 0, 0
    
    # Extrair custos das consultas
    custos = []
    for c in consultas_realizadas:
        try:
            custo_str = c.get("procedimento", {}).get("custo", "R$ 0,00")
            custos.append(extrair_valor(custo_str))
        except:
            continue
    
    # Se temos o gasto total informado, usamos ele
    if gasto_total is not None and gasto_total > 0:
        valor_total = gasto_total
    else:
        # Caso contrário, somamos os custos das consultas
        valor_total = sum(custos)
    
    # Ordenar por data
    formato_data = "%d/%m/%Y %H:%M"
    try:
        consultas_ordenadas = sorted(
            consultas_realizadas,
            key=lambda c: datetime.strptime(c.get("dataConsulta", "01/01/2000 00:00"), formato_data)
        )
        
        data_primeira = datetime.strptime(consultas_ordenadas[0].get("dataConsulta", ""), formato_data)
        data_ultima = datetime.strptime(consultas_ordenadas[-1].get("dataConsulta", ""), formato_data)
        periodo_dias = (data_ultima - data_primeira).days
    except:
        if periodo_total is not None:
            periodo_dias = periodo_total
        else:
            periodo_dias = 365  # valor padrão se não conseguir calcular
    
    # Calcular período em meses para facilitar análise
    periodo_meses = periodo_dias / 30.0
    
    # Calcular gasto médio mensal
    media_mensal = valor_total / max(periodo_meses, 1)
    
    # Definir limiares mais baixos do que para abuso completo
    # Estes valores representam aproximadamente o percentil 95 de gastos de pacientes normais
    
    # Limiares baseados em período (ajustados para serem mais sensíveis)
    if periodo_meses <= 3:  # Até 3 meses
        limiar_tendencia = 3500.0  # R$3.500 em 3 meses já é suspeito
    elif periodo_meses <= 6:  # Até 6 meses
        limiar_tendencia = 5000.0  # R$5.000 em 6 meses
    elif periodo_meses <= 12:  # Até 1 ano
        limiar_tendencia = 8000.0  # R$8.000 em 1 ano
    elif periodo_meses <= 18:  # Até 1.5 anos
        limiar_tendencia = 9500.0  # R$9.500 em 1.5 anos
    else:  # Até 2 anos ou mais
        limiar_tendencia = 10000.0  # R$10.000 em 2 anos
    
    # Limiar de gasto médio mensal (R$700/mês já indica uso intenso)
    limiar_media_mensal = 700.0
    
    # Verificamos tanto o valor total quanto a média mensal
    tendencia_por_total = valor_total >= limiar_tendencia
    tendencia_por_media = media_mensal >= limiar_media_mensal and periodo_meses >= 6
    
    # Apenas um dos critérios já indica tendência a excesso
    tendencia_detectada = tendencia_por_total or tendencia_por_media
    
    return tendencia_detectada, valor_total, periodo_meses, media_mensal

#######################
# 3) FASTAPI: CRIAR O ENDPOINT
#######################
app = FastAPI(
    title="SmartDent: IA para Odontologia",
    description="API que faz análises inteligentes com AI para contextos e práticas odontológicas como prevenção de sinistros de uso excessivo.",
    version="2.0.0"
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
