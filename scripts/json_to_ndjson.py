import json
import os
import random
from datetime import datetime, timedelta

# Quantidade total de pacientes
N = 100_000

# Distribuição das classes (ajustada para melhor equilíbrio)
percent_uso_excessivo = 0.25   # 25%
percent_potencial_excesso = 0.25  # 25%
percent_moderado = 0.25  # 25%
percent_nenhum = 0.25    # 25%

quant_uso_excessivo = int(N * percent_uso_excessivo)
quant_potencial_excesso = int(N * percent_potencial_excesso)
quant_moderado = int(N * percent_moderado)
quant_nenhum = N - (quant_uso_excessivo + quant_potencial_excesso + quant_moderado)  # Garante que soma 100k

# Caminhos
base_dir = os.path.dirname(os.path.abspath(__file__))
data_dir = os.path.join(base_dir, "..", "data")
os.makedirs(data_dir, exist_ok=True)
ndjson_path = os.path.join(data_dir, 'synthetic_patients.ndjson')

# Lista de nomes completos
nomes = [
    "João Almeida", "Larissa Oliveira Silva", "Carlos Pereira",
    "Mariana Santos", "Bruno Costa", "Fernanda Souza",
    "Rafael Lima", "Patrícia Gomes", "André Martins",
    "Carolina Dias", "Eduardo Rocha", "Juliana Barbosa",
    "Diego Carvalho", "Renata Castro", "Victor Mendes",
    "Aline Ferreira", "Leonardo Nascimento", "Camila Figueiredo",
    "Gabriel Teixeira", "Bianca Ramos"
]

# Dicionário organizado de planos odontológicos
planos_odontologicos = {
    "Individuais": [
        "Dental Júnior",
        "Bem Estar",
        "Bem Estar White",
        "Bem Estar Pró",
        "Bem Estar Orto",
        "Bem Estar Orto White"
    ],
    "Empresariais": [
        "Convencional",
        "Integral",
        "Integral Plus",
        "Integral Doc",
        "Integral Doc Plus",
        "Premium",
        "Superior",
        "Classical",
        "Classical Doc",
        "Ômega",
        "Master",
        "Maximum White"
    ]
}

# Lista de 50 empresas brasileiras
empresas = [
    "Petrobras", "Vale", "Itaú Unibanco", "Bradesco", "Banco do Brasil",
    "Ambev", "BNDES", "JBS", "Embraer", "Magazine Luiza",
    "B2W Digital", "RaiaDrogasil", "Lojas Americanas", "CPFL Energia", "Cemig",
    "Eletrobras", "Natura", "BRF", "Suzano", "Gerdau",
    "Itaúsa", "BTG Pactual", "Cielo", "Ultrapar", "Grupo Pão de Açúcar",
    "Santander", "Klabin", "WEG", "Rumo", "Tim",
    "Localiza", "BR Distribuidora", "MRV", "Votorantim", "Grupo Globo",
    "Lojas Renner", "Riachuelo", "Centauro", "Marisa", "Dafiti",
    "Via Varejo", "Mercado Livre", "Loggi", "Stone", "Totvs",
    "Movile", "Hering", "Arezzo", "Lojas Quero-Quero", "BR Malls"
]

# Lista de cidades brasileiras
cidades = [
    "São Paulo", "Rio de Janeiro", "Brasília", "Salvador", "Fortaleza",
    "Belo Horizonte", "Manaus", "Curitiba", "Recife", "Porto Alegre",
    "Belém", "Goiânia", "Guarulhos", "Campinas", "São Luís",
    "Maceió", "Duque de Caxias", "Nova Iguaçu", "Campo Grande", "Teresina",
    "São Bernardo do Campo", "Santo André", "Osasco", "São José dos Campos", "Ribeirão Preto",
    "Sorocaba", "Mauá", "Uberlândia", "Contagem", "Aracaju",
    "Feira de Santana", "Caxias do Sul", "Joinville", "Juiz de Fora", "Londrina",
    "Ananindeua", "Niterói", "Florianópolis", "Macapá", "Mogi das Cruzes",
    "Santos", "Vila Velha", "Diadema", "Olinda", "Carapicuíba",
    "Serra", "Pelotas", "Montes Claros", "Uberaba", "Várzea Grande",
    "Foz do Iguaçu", "Bauru", "Ponta Grossa", "Santo Antônio de Jesus", "Blumenau",
    "Taubaté", "Vila Nova de Goiânia", "Maringá", "Petrolina", "Boa Vista",
    "Vitória", "Ribeirão das Neves", "Canoas", "Serra Talhada", "Camaçari",
    "Jacareí", "Barueri", "Itaboraí", "Itaquaquecetuba", "Pouso Alegre",
    "São José do Rio Preto", "Governador Valadares", "Cachoeiro de Itapemirim", "Imperatriz", "Divinópolis",
    "Arapiraca", "Santarém", "Itabuna", "Guarujá", "Itaúna",
    "Limeira", "Franca", "Itapecerica da Serra", "Marília", "Cabo Frio",
    "Presidente Prudente", "Ribeirão Pires", "Itapevi", "Guaratinguetá", "Volta Redonda",
    "Sapucaia do Sul", "Barbacena", "Varginha", "Ituverava", "Avaré",
    "Itapeva", "São Vicente", "Taquara", "Itatiba"
]

# Lista de procedimentos com preços médios reais em R$ (valores baseados em mercado brasileiro)
procedimentos_com_precos = {
    # Consultas e Diagnóstico
    "Consulta odontológica geral": 120.00,
    "Avaliação clínica e diagnóstico": 150.00,
    "Consulta para clareamento": 100.00,
    "Consulta para próteses": 130.00,
    "Acompanhamento ortodôntico": 140.00,
    
    # Prevenção e Profilaxia
    "Limpeza dental (profilaxia)": 150.00,
    "Aplicação de flúor": 80.00,
    "Aplicação de selante": 90.00,
    "Instrução de higiene bucal": 70.00,
    
    # Urgência e Emergência 24h
    "Atendimento odontológico de urgência": 220.00,
    "Alívio de dor": 180.00,
    "Drenagem de abscessos": 250.00,
    "Controle de hemorragias": 200.00,
    
    # Radiologia e Exames
    "Radiografia intraoral": 50.00,
    "Radiografia panorâmica": 120.00,
    "Documentação ortodôntica completa (Exames)": 350.00,
    "Tomografia computadorizada": 450.00,
    
    # Dentística
    "Restauração em resina composta": 180.00,
    "Restauração em amálgama": 150.00,
    "Troca de restaurações antigas": 200.00,
    
    # Cirurgia Oral e Extrações
    "Extração de dente comum": 180.00,
    "Extração de dente do siso": 400.00,
    "Frenectomia lingual e labial": 350.00,
    
    # Endodontia
    "Canal em dentes anteriores": 500.00,
    "Canal em dentes posteriores": 800.00,
    "Retratamento endodôntico": 950.00,
    
    # Periodontia
    "Tratamento de gengivite": 250.00,
    "Raspagem de tártaro": 200.00,
    "Cirurgia periodontal": 600.00,
    
    # Odontopediatria
    "Atendimento odontológico para crianças": 150.00,
    "Aplicação de flúor e selante": 180.00,
    "Tratamento restaurador em dentes de leite": 160.00,
    "Extração de dentes de leite": 120.00,
    
    # Ortodontia
    "Documentação ortodôntica completa (Ortodontia)": 350.00,
    "Instalação de aparelho fixo metálico": 1800.00,
    "Manutenção mensal do aparelho": 180.00,
    "Retirada do aparelho ortodôntico": 250.00,
    "Mantenedores ortodônticos": 350.00,
    
    # Odontologia Estética
    "Clareamento dental caseiro": 450.00,
    "Clareamento estético em consultório": 800.00,
    
    # Próteses Dentárias
    "Prótese fixa (coroa unitária)": 1200.00,
    "Prótese removível total (dentadura)": 1800.00,
    "Prótese removível parcial": 1500.00,
    "Prótese sobre cerâmica ou resina": 2500.00,
    "Placa de mordida para bruxismo": 600.00
}

# Lista de procedimentos (apenas os nomes, sem preços)
procedimentos = list(procedimentos_com_precos.keys())

# Mapeamento de procedimentos para categorias (para contagens)
procedimento_para_categoria = {
    # Consultas e Diagnóstico
    "Consulta odontológica geral": "ConsultaseDiagnóstico",
    "Avaliação clínica e diagnóstico": "ConsultaseDiagnóstico",
    "Consulta para clareamento": "ConsultaseDiagnóstico",
    "Consulta para próteses": "ConsultaseDiagnóstico",
    "Acompanhamento ortodôntico": "ConsultaseDiagnóstico",
    
    # Prevenção e Profilaxia
    "Limpeza dental (profilaxia)": "Limpeza",
    "Aplicação de flúor": "Limpeza",
    "Aplicação de selante": "Limpeza",
    "Instrução de higiene bucal": "Limpeza",
    
    # Urgência e Emergência
    "Atendimento odontológico de urgência": "Limpeza",
    "Alívio de dor": "Limpeza",
    "Drenagem de abscessos": "Limpeza",
    "Controle de hemorragias": "Limpeza",
    
    # Radiologia e Exames
    "Radiografia intraoral": "RadiologiaeExames",
    "Radiografia panorâmica": "RadiologiaeExames",
    "Documentação odontológica completa (Exames)": "RadiologiaeExames",
    "Tomografia computadorizada": "RadiologiaeExames",
    
    # Dentística
    "Restauração em resina composta": "Restauração",
    "Restauração em amálgama": "Restauração",
    "Troca de restaurações antigas": "Restauração",
    
    # Cirurgia
    "Extração de dente comum": "Cirurgia",
    "Extração de dente do siso": "Cirurgia",
    "Frenectomia lingual e labial": "Cirurgia",
    
    # Endodontia
    "Canal em dentes anteriores": "Endodontia",
    "Canal em dentes posteriores": "Endodontia",
    "Retratamento endodôntico": "Endodontia",
    
    # Periodontia
    "Tratamento de gengivite": "Limpeza",
    "Raspagem de tártaro": "Limpeza",
    "Cirurgia periodontal": "Cirurgia",
    
    # Odontopediatria
    "Atendimento odontológico para crianças": "ConsultaseDiagnóstico",
    "Aplicação de flúor e selante": "Limpeza",
    "Tratamento restaurador em dentes de leite": "Restauração",
    "Extração de dentes de leite": "Cirurgia",
    
    # Ortodontia
    "Documentação ortodôntica completa (Ortodontia)": "Ortodontia",
    "Instalação de aparelho fixo metálico": "Ortodontia",
    "Manutenção mensal do aparelho": "Ortodontia",
    "Retirada do aparelho ortodôntico": "Ortodontia",
    "Mantenedores ortodônticos": "Ortodontia",
    
    # Odontologia Estética
    "Clareamento dental caseiro": "Restauração",
    "Clareamento estético em consultório": "Restauração",
    
    # Próteses Dentárias
    "Prótese fixa (coroa unitária)": "PrótesesDentárias",
    "Prótese removível total (dentadura)": "PrótesesDentárias",
    "Prótese removível parcial": "PrótesesDentárias",
    "Prótese sobre cerâmica ou resina": "PrótesesDentárias",
    "Placa de mordida para bruxismo": "PrótesesDentárias"
}

# Procedimentos agrupados por categoria para facilitar uso futuro
procedimentos_por_categoria = {
    "ConsultaseDiagnóstico": [p for p, c in procedimento_para_categoria.items() if c == "ConsultaseDiagnóstico"],
    "Limpeza": [p for p, c in procedimento_para_categoria.items() if c == "Limpeza"],
    "RadiologiaeExames": [p for p, c in procedimento_para_categoria.items() if c == "RadiologiaeExames"],
    "Restauração": [p for p, c in procedimento_para_categoria.items() if c == "Restauração"],
    "Cirurgia": [p for p, c in procedimento_para_categoria.items() if c == "Cirurgia"],
    "Endodontia": [p for p, c in procedimento_para_categoria.items() if c == "Endodontia"],
    "Ortodontia": [p for p, c in procedimento_para_categoria.items() if c == "Ortodontia"],
    "PrótesesDentárias": [p for p, c in procedimento_para_categoria.items() if c == "PrótesesDentárias"]
}

def gerar_paciente(idx, risco):
    # Definindo a quantidade base de consultas realizadas no ano de acordo com o risco.
    # Os intervalos foram escolhidos com base em dados reais do Brasil e ajustados para permitir variações e exceções, simulando casos reais e outliers.
    if risco == "UsoExcessivo":
        # Uso excessivo: 10 a 18 consultas no ano.
        # Justificativa: Acima de 10-12 consultas/ano já é considerado muito suspeito ou abuso pelos convênios.
        # O limite superior (18) permite simular casos extremos e outliers, comuns em bases reais.
        base_qtd_realizadas = random.randint(10, 18)
    elif risco == "Uso Moderado com Tendência a Excesso":
        # Tendência a excesso: 7 a 12 consultas no ano.
        # Justificativa: Acima de 7-8 já é considerado "radical" ou "pesado" e pode levantar alerta, mas ainda não é abuso claro.
        # O limite superior (12) encosta no limiar do "excessivo", permitindo simular casos de transição.
        base_qtd_realizadas = random.randint(7, 12)
    elif risco == "Uso Moderado":
        # Uso moderado: 3 a 6 consultas no ano.
        # Justificativa: A média da população brasileira é de 2 a 4 consultas/ano.
        # O limite superior (6) permite simular pessoas que usam um pouco acima da média, mas sem levantar suspeita.
        base_qtd_realizadas = random.randint(3, 6)
    else:  # NenhumRisco
        # Nenhum risco: 0 a 4 consultas no ano.
        # Justificativa: 0-1 consulta é considerado pouco, 2 é o mínimo recomendado, até 4 cobre variações de quem faz só prevenção ou faltou algum ano.
        # O limite superior (4) foi escolhido para permitir uma margem maior de exceções e simular a realidade de quem quase não vai ao dentista.
        base_qtd_realizadas = random.randint(0, 4)
        
    # Aplica um pequeno ruído gaussiano para simular variação natural, mas mantém dentro dos limites realistas
    # Motivo: O random.gauss(0, 1) gera uma variação mais "orgânica" em torno do valor base, simulando a diversidade real de comportamento dos pacientes.
    # Para base 0 (NenhumRisco), permite que o paciente realmente não tenha ido ao dentista.
    # Para os demais, sempre pelo menos 1 consulta, nunca mais que 18 (limite superior realista).
    if base_qtd_realizadas == 0:
        # Para casos de NenhumRisco, pode ser 0 mesmo
        qtd_realizadas = 0
    else:
        # Para os demais, sempre pelo menos 1 consulta
        variacao = int(round(random.gauss(0, 1)))  # ruído normal, média 0, desvio padrão 1
        qtd_realizadas = max(1, min(18, base_qtd_realizadas + variacao))

    # O número de consultas agendadas é proporcional ao uso: quem vai mais ao dentista tende a agendar mais.
    # O valor mínimo é 0, o máximo é 2 ou um terço das realizadas (o que for maior), para evitar valores irreais.
    qtd_agendadas = random.randint(0, max(2, qtd_realizadas // 3))

    # O número de cancelamentos também é proporcional ao uso, mas mais restrito.
    # O valor mínimo é 0, o máximo é 1 ou um quarto das realizadas (o que for maior), simulando que nem todo mundo cancela.
    qtd_canceladas = random.randint(0, max(1, qtd_realizadas // 4))

    # Gera o histórico de procedimentos primeiro, de acordo com o perfil de risco
    procedimentos_hist = []
    if risco == "UsoExcessivo":
        # Paciente faz muitos tratamentos combinados e urgências
        tipos = ["Ortodontia", "Cirurgia", "Endodontia", "ConsultaseDiagnóstico"]
        for _ in range(qtd_realizadas):
            cat = random.choice(tipos)
            proc = random.choice(procedimentos_por_categoria[cat])
            procedimentos_hist.append(proc)
    elif risco == "NenhumRisco":
        # Paciente faz mais limpezas e checkups
        for _ in range(qtd_realizadas):
            cat = random.choice(["ConsultaseDiagnóstico", "Limpeza"])
            proc = random.choice(procedimentos_por_categoria[cat])
            procedimentos_hist.append(proc)
    else:
        # Mix de procedimentos comuns
        for _ in range(qtd_realizadas):
            cat = random.choices(
                ["ConsultaseDiagnóstico", "Limpeza", "Restauração", "PrótesesDentárias"],
                weights=[0.5, 0.25, 0.15, 0.10]
            )[0]
            proc = random.choice(procedimentos_por_categoria[cat])
            procedimentos_hist.append(proc)

    # Define o intervalo médio de acordo com o tipo de procedimento mais frequente
    categorias_hist = [procedimento_para_categoria.get(p, "") for p in procedimentos_hist]
    if "Ortodontia" in categorias_hist:
        # Consultas mensais (tratamento ortodôntico)
        intervaloMedioDias = int(random.triangular(25, 35, 30))
    elif "Cirurgia" in categorias_hist:
        # Acompanhamento pós-operatório
        intervaloMedioDias = int(random.triangular(5, 15, 10))
    elif "Endodontia" in categorias_hist:
        # Tratamento de canal
        intervaloMedioDias = int(random.triangular(10, 25, 15))
    elif risco == "UsoExcessivo":
        # Paciente vai toda semana ou a cada 2-3 semanas
        intervaloMedioDias = int(random.triangular(2, 21, 7))
    elif risco == "Uso Moderado com Tendência a Excesso":
        # Vai 1x por mês ou está intensificando
        intervaloMedioDias = int(random.triangular(10, 40, 20))
    elif risco == "Uso Moderado":
        # Vai a cada 1, 2 ou 3 meses
        intervaloMedioDias = int(random.triangular(25, 90, 45))
    else:
        # Check-up, limpeza, rotina
        intervaloMedioDias = int(random.triangular(60, 365, 180))

    # Calcula o período total de acompanhamento
    # Período mínimo = intervalo médio * qtd_realizadas, com ruído de 5% a 30%
    periodoTotalDias = int(qtd_realizadas * intervaloMedioDias * random.uniform(1.05, 1.3))
    # Garante que nunca seja <30 dias e não passe de 3 anos (1095 dias)
    periodoTotalDias = min(max(30, periodoTotalDias), 1095)

    gasto_total = 0
    for proc in procedimentos_hist:
        gasto_total += procedimentos_com_precos[proc]
        gasto_total += procedimentos_com_precos[proc] * random.uniform(-0.2, 0.2)
    gasto_total = round(gasto_total, 2)

    numProcedsRepetidos = sum(1 for proc in set(procedimentos_hist)
                              if procedimentos_hist.count(proc) > 1)

    data_inicio = datetime.now() - timedelta(days=periodoTotalDias)
    categorias_do_historico = categorias_hist
    categorias_padrao = [
        "ConsultaseDiagnóstico", "Limpeza", "Endodontia",
        "RadiologiaeExames", "PrótesesDentárias", "Ortodontia",
        "Cirurgia", "Restauração"
    ]
    proc_counts = {
        f"count_{cat}": categorias_do_historico.count(cat)
        for cat in categorias_padrao
    }

    eh_plano_empresarial = random.random() < 0.6
    if eh_plano_empresarial:
        plano = random.choice(planos_odontologicos["Empresariais"])
        empresa = random.choice(empresas)
    else:
        plano = random.choice(planos_odontologicos["Individuais"])
        empresa = None

    cidade = random.choice(cidades)
    endereco = f"Rua {random.choice(['das Flores', 'dos Pinheiros', 'São José', 'Amazonas', 'Principal', 'Brasil', 'das Palmeiras'])}, {random.randint(1, 999)}, {cidade}"

    paciente = {
        "idPaciente": idx + 1,
        "nome": random.choice(nomes),
        "cpf": f"{random.randint(100, 999)}.{random.randint(100, 999)}.{random.randint(100, 999)}-{random.randint(10, 99)}",
        "dataNascimento": (datetime.now() - timedelta(days=random.randint(365*18, 365*65))).strftime("%Y-%m-%d"),
        "email": f"{random.choice(['joao', 'maria', 'pedro', 'ana', 'carlos', 'paula', 'lucas', 'fernanda'])}.{random.randint(1, 999)}@{random.choice(['gmail.com', 'hotmail.com', 'outlook.com', 'yahoo.com.br'])}",
        "telefone": f"({random.randint(11, 99)}) {random.randint(90000, 99999)}-{random.randint(1000, 9999)}",
        "endereco": endereco,
        "cidade": cidade,
        "plano": plano,
        "tipoPlano": "Empresarial" if eh_plano_empresarial else "Individual",
        "empresa": empresa,
        "data_inicio": data_inicio.strftime("%Y-%m-%d"),
        "periodoTotalDias": periodoTotalDias,
        "intervaloMedioDias": intervaloMedioDias,
        "qtd_realizadas": qtd_realizadas,
        "qtd_agendadas": qtd_agendadas,
        "qtd_canceladas": qtd_canceladas,
        "gastoTotal": gasto_total,
        "numProcedsRepetidos": numProcedsRepetidos,
        "historico_procedimentos": procedimentos_hist,
        **proc_counts,
        "label": risco
    }
    return paciente

# Construção da ordem de geração com labels balanceados
categorias = (
    ["UsoExcessivo"] * quant_uso_excessivo +
    ["Uso Moderado com Tendência a Excesso"] * quant_potencial_excesso +
    ["Uso Moderado"] * quant_moderado +
    ["NenhumRisco"] * quant_nenhum
)
random.shuffle(categorias)

print(f"Gerando {N} pacientes NDJSON no arquivo: {ndjson_path}")
with open(ndjson_path, 'w', encoding='utf-8') as f_ndjson:
    for idx, label in enumerate(categorias):
        paciente = gerar_paciente(idx, label)
        f_ndjson.write(json.dumps(paciente, ensure_ascii=False) + '\n')
        if (idx + 1) % 10000 == 0:
            print(f"Gerados {idx + 1} pacientes...")

print(f"Arquivo NDJSON finalizado e balanceado com sucesso: {ndjson_path}")
print("Distribuição das classes:")
print(f"  UsoExcessivo: {quant_uso_excessivo} ({percent_uso_excessivo*100:.1f}%)")
print(f"  Uso Moderado com Tendência a Excesso: {quant_potencial_excesso} ({percent_potencial_excesso*100:.1f}%)")
print(f"  Uso Moderado: {quant_moderado} ({percent_moderado*100:.1f}%)")
print(f"  NenhumRisco: {quant_nenhum} ({percent_nenhum*100:.1f}%)")
