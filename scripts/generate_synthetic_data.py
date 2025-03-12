import json
import random
import datetime
import os


def generate_synthetic_json(num_pacientes=1000):
    # Essa função aqui é pra gerar um conjunto de dados fictícios (pacientes, consultas) em um arquivo .json
    # A ideia é criar um volume grande de pacientes e consultas prá treinar o modelo sem usar dados reais.

    # Definindo as categorias e os procedimentos disponíveis atuais da Odontoprev
    procedimentos_por_categoria = {
        "Consultas e Diagnóstico": [
            {"tipoProcedimento": "Consulta odontológica geral", "custo": "R$ 150,00"},
            {"tipoProcedimento": "Avaliação clínica e diagnóstico", "custo": "R$ 120,00"},
            {"tipoProcedimento": "Consulta para clareamento", "custo": "R$ 100,00"},
            {"tipoProcedimento": "Consulta para próteses", "custo": "R$ 130,00"},
            {"tipoProcedimento": "Acompanhamento ortodôntico", "custo": "R$ 140,00"}
        ],
        "Prevenção e Profilaxia": [
            {"tipoProcedimento": "Limpeza dental (profilaxia)", "custo": "R$ 120,00"},
            {"tipoProcedimento": "Aplicação de flúor", "custo": "R$ 80,00"},
            {"tipoProcedimento": "Aplicação de selante", "custo": "R$ 90,00"},
            {"tipoProcedimento": "Instrução de higiene bucal", "custo": "R$ 70,00"}
        ],
        "Urgência e Emergência 24h": [
            {"tipoProcedimento": "Atendimento odontológico de urgência", "custo": "R$ 200,00"},
            {"tipoProcedimento": "Alívio de dor", "custo": "R$ 150,00"},
            {"tipoProcedimento": "Drenagem de abscessos", "custo": "R$ 250,00"},
            {"tipoProcedimento": "Controle de hemorragias", "custo": "R$ 180,00"}
        ],
        "Radiologia e Exames": [
            {"tipoProcedimento": "Radiografia intraoral", "custo": "R$ 180,00"},
            {"tipoProcedimento": "Radiografia panorâmica", "custo": "R$ 300,00"},
            {"tipoProcedimento": "Documentação ortodôntica completa", "custo": "R$ 400,00"},
            {"tipoProcedimento": "Tomografia computadorizada", "custo": "R$ 700,00"}
        ],
        "Dentistica": [
            {"tipoProcedimento": "Restauração em resina composta", "custo": "R$ 300,00"},
            {"tipoProcedimento": "Restauração em amálgama", "custo": "R$ 280,00"},
            {"tipoProcedimento": "Troca de restaurações antigas", "custo": "R$ 320,00"}
        ],
        "CirurgiaOralEExtracoes": [
            {"tipoProcedimento": "Extração de dente comum", "custo": "R$ 200,00"},
            {"tipoProcedimento": "Extração de dente do siso", "custo": "R$ 450,00"},
            {"tipoProcedimento": "Frenectomia lingual e labial", "custo": "R$ 500,00"}
        ],
        "Endodontia": [
            {"tipoProcedimento": "Canal em dentes anteriores", "custo": "R$ 550,00"},
            {"tipoProcedimento": "Canal em dentes posteriores", "custo": "R$ 750,00"},
            {"tipoProcedimento": "Retratamento endodôntico", "custo": "R$ 800,00"}
        ],
        "Periodontia": [
            {"tipoProcedimento": "Tratamento de gengivite", "custo": "R$ 350,00"},
            {"tipoProcedimento": "Raspagem de tártaro", "custo": "R$ 400,00"},
            {"tipoProcedimento": "Cirurgia periodontal", "custo": "R$ 600,00"}
        ],
        "Odontopediatria": [
            {"tipoProcedimento": "Atendimento odontológico para crianças", "custo": "R$ 250,00"},
            {"tipoProcedimento": "Aplicação de flúor e selante", "custo": "R$ 150,00"},
            {"tipoProcedimento": "Tratamento restaurador em dentes de leite", "custo": "R$ 200,00"},
            {"tipoProcedimento": "Extração de dentes de leite", "custo": "R$ 180,00"}
        ],
        "Ortodontia": [
            {"tipoProcedimento": "Documentação ortodôntica completa", "custo": "R$ 400,00"},
            {"tipoProcedimento": "Instalação de aparelho fixo metálico", "custo": "R$ 1800,00"},
            {"tipoProcedimento": "Manutenção mensal do aparelho", "custo": "R$ 250,00"},
            {"tipoProcedimento": "Retirada do aparelho ortodôntico", "custo": "R$ 350,00"},
            {"tipoProcedimento": "Mantenedores ortodônticos", "custo": "R$ 500,00"}
        ],
        "OdontologiaEstetica": [
            {"tipoProcedimento": "Clareamento dental caseiro", "custo": "R$ 400,00"},
            {"tipoProcedimento": "Clareamento estético em consultório", "custo": "R$ 600,00"}
        ],
        "ProteesDentarias": [
            {"tipoProcedimento": "Prótese fixa (coroa unitária)", "custo": "R$ 2000,00"},
            {"tipoProcedimento": "Prótese removível total (dentadura)", "custo": "R$ 2500,00"},
            {"tipoProcedimento": "Prótese removível parcial", "custo": "R$ 2200,00"},
            {"tipoProcedimento": "Prótese sobre cerâmica ou resina", "custo": "R$ 2800,00"},
            {"tipoProcedimento": "Placa de mordida para bruxismo", "custo": "R$ 900,00"}
        ]
    }

    # Aqui é uma lista única de procedimentos, só que mantendo o rótulo de categoria
    procedimentos_disponiveis = []
    for categoria, procedimentos in procedimentos_por_categoria.items():
        for proc in procedimentos:
            proc_copy = proc.copy()
            proc_copy["categoria"] = categoria
            procedimentos_disponiveis.append(proc_copy)

    pacientes = []
    base_date = datetime.datetime(2024, 1, 1)

    for i in range(num_pacientes):
        # Cada paciente vai ter seu id, nome, e dados básicos de forma gerada automaticamente
        id_paciente = 100000000 + i
        nome = f"Paciente {i}"
        cpf = f"{random.randint(10000000000, 99999999999)}"
        nascimento = base_date - datetime.timedelta(days=random.randint(8000, 18000))
        data_nascimento = nascimento.strftime("%d/%m/%Y")
        email = f"paciente{i}@exemplo.com"
        telefone = f"(11) {random.randint(10000000, 99999999)}"
        endereco = f"Rua Exemplo, {random.randint(1, 100)}, Bairro {random.randint(1, 10)}, Cidade"
        plano = random.choice(["Bem Estar White", "Bem Estar Pró", "Premium", "Básico"])
        empresa = random.choice(["Totvs", "EmpresaX", "EmpresaY", "Independente"])

        # Fixar o número de consultas para 20 pra manter um padrão e facilitar análise
        num_consultas = 20

        # Definir se esse paciente vai ser marcado como "abuso" (chance de 10%), pra simular uso excessivo
        abuso = random.random() < 0.10

        consultas = []
        if abuso:
            # Se for abuso, a ideia é gerar 20 consultas todas como "Realizada", bem seguidas,
            # destacando esse paciente como um possível caso de uso indevido.
            procedimento_escolhido = random.choice(procedimentos_disponiveis)
            # Escolhe o mesmo procedimento pra todas as consultas, reforçando o padrão
            last_date = base_date + datetime.timedelta(days=random.randint(0, 100))
            for j in range(num_consultas):
                id_consulta = 1000000 + i * 100 + j
                delta_days = random.randint(1, 7)
                consulta_date = last_date + datetime.timedelta(days=delta_days)
                last_date = consulta_date
                data_consulta = consulta_date.strftime("%d/%m/%Y %H:%M")
                status = "Realizada"
                consulta = {
                    "idConsulta": id_consulta,
                    "dataConsulta": data_consulta,
                    "status": status,
                    "procedimento": {
                        "idProcedimento": random.randint(100000000, 999999999),
                        "tipoProcedimento": procedimento_escolhido["tipoProcedimento"],
                        "descricao": None,
                        "custo": procedimento_escolhido["custo"]
                    }
                }
                consultas.append(consulta)
        else:
            # Caso normal: criar 20 consultas distribuídas entre "Realizada", "Agendada" e "Cancelada"
            for j in range(num_consultas):
                id_consulta = 1000000 + i * 100 + j
                days_offset = random.randint(0, 600)
                consulta_date = base_date + datetime.timedelta(days=days_offset)
                # Em 5% das consultas, a data sai como "Data inválida" só pra testar cenários de erro
                data_consulta = consulta_date.strftime("%d/%m/%Y %H:%M") if random.random() > 0.05 else "Data inválida"
                status = random.choice(["Realizada", "Agendada", "Cancelada"])
                procedimento_escolhido = random.choice(procedimentos_disponiveis)
                custo = procedimento_escolhido["custo"]
                consulta = {
                    "idConsulta": id_consulta,
                    "dataConsulta": data_consulta,
                    "status": status,
                    "procedimento": {
                        "idProcedimento": random.randint(100000000, 999999999),
                        "tipoProcedimento": procedimento_escolhido["tipoProcedimento"],
                        "descricao": None,
                        "custo": custo
                    }
                }
                consultas.append(consulta)

        # Agora vamos somar o total gasto, mas só nas consultas que tiverem status "Realizada"
        total_gasto = 0.0
        for c in consultas:
            if c["status"] == "Realizada":
                try:
                    custo_str = c["procedimento"]["custo"].replace("R$", "").replace(".", "").replace(",", ".").strip()
                    total_gasto += float(custo_str)
                except Exception as e:
                    # Se ocorrer alguma falha na conversão, só ignora
                    continue

        paciente_obj = {
            "idPaciente": id_paciente,
            "nomeCompleto": nome,
            "cpf": cpf,
            "dataNascimento": data_nascimento,
            "email": email,
            "telefone": telefone,
            "endereco": endereco,
            "planoOdontologico": plano,
            "empresa": empresa,
            "numConsultas": num_consultas,
            "gastoTotal": f"R$ {total_gasto:.2f}",
            "consultas": consultas
        }
        pacientes.append(paciente_obj)

    # Garantir que a pasta data exista na raiz do projeto
    BASE_DIR = os.path.dirname(os.path.abspath(__file__))
    PROJETO_DIR = os.path.abspath(os.path.join(BASE_DIR, ".."))
    data_dir = os.path.join(PROJETO_DIR, "data")
    if not os.path.exists(data_dir):
        os.makedirs(data_dir)

    # Escrever o arquivo JSON final com os dados de pacientes e consultas
    output_path = os.path.join(data_dir, "synthetic_patients.json")
    with open(output_path, "w", encoding="utf-8") as f:
        json.dump(pacientes, f, ensure_ascii=False, indent=2)
    print(f"Arquivo '{output_path}' gerado com sucesso! Total de pacientes: {len(pacientes)}")


if __name__ == "__main__":
    generate_synthetic_json()
