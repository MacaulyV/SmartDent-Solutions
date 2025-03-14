using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using SmartDentAPI.Data;
using SmartDentAPI.Interfaces;
using SmartDentAPI.Models;

namespace SmartDentAPI.Data
{
    /// <summary>
    /// Classe responsável por semear (popular) o banco de dados com dados iniciais para testes.
    /// </summary>
    public class DataSeeder
    {
        /// <summary>
        /// Método estático que popula o banco de dados com dados de pacientes, consultas e procedimentos.
        /// Utiliza repositórios para realizar operações CRUD de forma assíncrona.
        /// </summary>
        /// <param name="serviceProvider">Provedor de serviços para obter as dependências (repositórios).</param>
        public static async Task SeedDatabase(IServiceProvider serviceProvider)
        {
            // Cria um escopo para obter instâncias dos repositórios necessários.
            using (var scope = serviceProvider.CreateScope())
            {
                // Obtém os repositórios para pacientes, consultas e procedimentos via injeção de dependência.
                var pacienteRepo = scope.ServiceProvider.GetRequiredService<IPacienteRepository>();
                var consultaRepo = scope.ServiceProvider.GetRequiredService<IConsultaRepository>();
                var procedimentoRepo = scope.ServiceProvider.GetRequiredService<IProcedimentoRepository>();

                var random = new Random();

                // Lista de nomes de pacientes para gerar dados aleatórios.
                var nomes = new List<string>
                {
                    "João Almeida", "Larissa Oliveira Silva", "Carlos Pereira",
                    "Mariana Santos", "Bruno Costa", "Fernanda Souza",
                    "Rafael Lima", "Patrícia Gomes", "André Martins",
                    "Carolina Dias", "Eduardo Rocha", "Juliana Barbosa",
                    "Diego Carvalho", "Renata Castro", "Victor Mendes",
                    "Aline Ferreira", "Leonardo Nascimento", "Camila Figueiredo",
                    "Gabriel Teixeira", "Bianca Ramos"
                };

                // Dicionário organizado de planos odontológicos, dividido entre categorias Individuais e Empresariais.
                var planosOdontologicos = new Dictionary<string, List<string>>
                {
                    { "Individuais", new List<string>
                        {
                            "Dental Júnior",
                            "Bem Estar",
                            "Bem Estar White",
                            "Bem Estar Pró",
                            "Bem Estar Orto",
                            "Bem Estar Orto White"
                        }
                    },
                    { "Empresariais", new List<string>
                        {
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
                        }
                    }
                };

                // Lista de 50 empresas brasileiras para atribuição aleatória aos pacientes.
                var empresas = new List<string>
                {
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
                };

                // Lista de cidades para atribuir endereços de forma aleatória.
                var cidades = new List<string>
                {
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
                };

                // Dicionário de procedimentos organizados por categoria, com seus respectivos custos.
                var tiposProcedimentos = new Dictionary<string, Dictionary<string, decimal>>
                {
                    { "Consultas e Diagnóstico", new Dictionary<string, decimal>
                        {
                            { "Consulta odontológica geral", 150m },
                            { "Avaliação clínica e diagnóstico", 120m },
                            { "Consulta para clareamento", 100m },
                            { "Consulta para próteses", 130m },
                            { "Acompanhamento ortodôntico", 140m }
                        }
                    },
                    { "Prevenção e Profilaxia", new Dictionary<string, decimal>
                        {
                            { "Limpeza dental (profilaxia)", 120m },
                            { "Aplicação de flúor", 80m },
                            { "Aplicação de selante", 90m },
                            { "Instrução de higiene bucal", 70m }
                        }
                    },
                    { "Urgência e Emergência 24h", new Dictionary<string, decimal>
                        {
                            { "Atendimento odontológico de urgência", 200m },
                            { "Alívio de dor", 150m },
                            { "Drenagem de abscessos", 250m },
                            { "Controle de hemorragias", 180m }
                        }
                    },
                    { "Radiologia e Exames", new Dictionary<string, decimal>
                        {
                            { "Radiografia intraoral", 180m },
                            { "Radiografia panorâmica", 300m },
                            { "Documentação ortodôntica completa", 400m },
                            { "Tomografia computadorizada", 700m }
                        }
                    },
                    { "Dentística (Tratamentos Restauradores)", new Dictionary<string, decimal>
                        {
                            { "Restauração em resina composta", 300m },
                            { "Restauração em amálgama", 280m },
                            { "Troca de restaurações antigas", 320m }
                        }
                    },
                    { "Cirurgia Oral e Extrações", new Dictionary<string, decimal>
                        {
                            { "Extração de dente comum", 200m },
                            { "Extração de dente do siso", 450m },
                            { "Frenectomia lingual e labial", 500m }
                        }
                    },
                    { "Endodontia (Tratamento de Canal)", new Dictionary<string, decimal>
                        {
                            { "Canal em dentes anteriores", 550m },
                            { "Canal em dentes posteriores", 750m },
                            { "Retratamento endodôntico", 800m }
                        }
                    },
                    { "Periodontia (Tratamento da Gengiva)", new Dictionary<string, decimal>
                        {
                            { "Tratamento de gengivite", 350m },
                            { "Raspagem de tártaro", 400m },
                            { "Cirurgia periodontal", 600m }
                        }
                    },
                    { "Odontopediatria (Atendimento Infantil)", new Dictionary<string, decimal>
                        {
                            { "Atendimento odontológico para crianças", 250m },
                            { "Aplicação de flúor e selante", 150m },
                            { "Tratamento restaurador em dentes de leite", 200m },
                            { "Extração de dentes de leite", 180m }
                        }
                    },
                    { "Ortodontia (Aparelhos Dentários)", new Dictionary<string, decimal>
                        {
                            { "Documentação ortodôntica completa", 400m },
                            { "Instalação de aparelho fixo metálico", 1800m },
                            { "Manutenção mensal do aparelho", 250m },
                            { "Retirada do aparelho ortodôntico", 350m },
                            { "Mantenedores ortodônticos", 500m }
                        }
                    },
                    { "Odontologia Estética", new Dictionary<string, decimal>
                        {
                            { "Clareamento dental caseiro", 400m },
                            { "Clareamento estético em consultório", 600m }
                        }
                    },
                    { "Próteses Dentárias", new Dictionary<string, decimal>
                        {
                            { "Prótese fixa (coroa unitária)", 2000m },
                            { "Prótese removível total (dentadura)", 2500m },
                            { "Prótese removível parcial", 2200m },
                            { "Prótese sobre cerâmica ou resina", 2800m },
                            { "Placa de mordida para bruxismo", 900m }
                        }
                    }
                };

                // Define uma data base inicial para as consultas, um ano atrás da data atual.
                DateTime dataBaseInicial = DateTime.Now.AddYears(-1).Date;

                // Loop para criar 100 pacientes com dados e consultas aleatórias.
                for (int i = 0; i < 0; i++)
                {
                    // Seleciona um nome aleatório para o paciente.
                    string nomePaciente = nomes[random.Next(nomes.Count)];

                    // Seleciona aleatoriamente uma categoria de plano odontológico e, dentro dela, um plano.
                    var categoriaAleatoria = planosOdontologicos.Keys.ElementAt(random.Next(planosOdontologicos.Count));
                    var planoSelecionado = planosOdontologicos[categoriaAleatoria][random.Next(planosOdontologicos[categoriaAleatoria].Count)];

                    // Define a empresa: 80% dos pacientes terão uma empresa selecionada aleatoriamente, caso contrário, "Individual".
                    string empresaSelecionada = (random.NextDouble() >= 0.20) ? empresas[random.Next(empresas.Count)] : "Individual";

                    // Gera um CPF de 11 dígitos de forma aleatória.
                    var cpfGerado = (((long)(random.NextDouble() * 90000000000L)) + 10000000000L).ToString();

                    // Cria o objeto Paciente com os dados gerados aleatoriamente.
                    var paciente = new Paciente
                    {
                        NomeCompleto = nomePaciente,
                        CPF = cpfGerado,
                        DataNascimento = random.Next(1, 28).ToString("00") +
                                         random.Next(1, 13).ToString("00") +
                                         random.Next(1980, 2001).ToString(),
                        Email = $"{nomePaciente.Replace(" ", ".").ToLower()}@exemplo.com",
                        Telefone = "11" + random.Next(100000000, 999999999).ToString(),
                        Endereco = $"Rua Exemplo, {random.Next(1, 500)}, Bairro {random.Next(1, 10)}, {cidades[random.Next(cidades.Count)]}",
                        PlanoOdontologico = planoSelecionado,
                        Empresa = empresaSelecionada,
                        NumConsultas = 0
                    };

                    // Adiciona o paciente ao banco de dados.
                    await pacienteRepo.AddAsync(paciente);

                    // Distribuição ajustada para determinar o número de consultas que o paciente terá:
                    // 5% dos pacientes terão 1-3 consultas (Nenhum risco)
                    // 73% dos pacientes terão 4-7 consultas (Uso moderado)
                    // 10% dos pacientes terão 8-10 consultas (Potencial uso excessivo)
                    // 12% dos pacientes terão 11-30 consultas (Uso Excessivo - nível crítico)
                    double regime = random.NextDouble();
                    int numConsultas;
                    if (regime < 0.05)
                        numConsultas = random.Next(1, 4);
                    else if (regime < 0.78) // 0.05 + 0.73 = 0.78
                        numConsultas = random.Next(4, 8);
                    else if (regime < 0.88) // próximos 10%
                        numConsultas = random.Next(8, 11);
                    else // últimos 12%
                        numConsultas = random.Next(11, 31);

                    // Inicia a data para as consultas a partir da data base inicial.
                    DateTime dataConsultaAtual = dataBaseInicial;

                    // Para pacientes no regime moderado, força a repetição de pelo menos um procedimento.
                    // Seleciona aleatoriamente um procedimento da categoria "Prevenção e Profilaxia".
                    string procedimentoRepetido = null;
                    if (regime >= 0.10 && regime < 0.90 && numConsultas >= 2)
                    {
                        var catPrev = tiposProcedimentos["Prevenção e Profilaxia"];
                        procedimentoRepetido = catPrev.Keys.ElementAt(random.Next(catPrev.Count));
                    }

                    // Lista de índices de consulta nos quais o procedimento repetido será forçado.
                    var indicesRepetidos = new List<int>();
                    if (procedimentoRepetido != null)
                    {
                        // Se houver pelo menos 2 consultas, força que 2 delas tenham o mesmo procedimento.
                        int idx1 = random.Next(0, numConsultas);
                        int idx2;
                        do { idx2 = random.Next(0, numConsultas); } while (idx2 == idx1);
                        indicesRepetidos.Add(idx1);
                        indicesRepetidos.Add(idx2);
                    }

                    // Loop para criar as consultas do paciente.
                    for (int j = 0; j < numConsultas; j++)
                    {
                        // Determina um intervalo (gap) em dias para a próxima consulta.
                        // 20% de chance de um gap menor (1 a 3 dias), caso contrário, um gap maior (7 a 90 dias).
                        int gap = (random.NextDouble() < 0.2) ? random.Next(1, 4) : random.Next(7, 91);
                        dataConsultaAtual = dataConsultaAtual.AddDays(gap);

                        // Gera uma hora aleatória para a consulta entre 8h e 19h, com minutos aleatórios.
                        int hora = random.Next(8, 19);
                        int minuto = random.Next(0, 60);
                        DateTime dataConsulta = dataConsultaAtual.Date.AddHours(hora).AddMinutes(minuto);
                        string dataConsultaTexto = dataConsulta.ToString("ddMMyyyyHHmm");

                        // Para a última consulta, distribui status com percentuais específicos:
                        // 15% Cancelada, 15% Não Realizada, 35% Realizada, 35% Agendada.
                        string statusConsulta;
                        if (j == numConsultas - 1)
                        {
                            double r = random.NextDouble();
                            if (r < 0.15)
                                statusConsulta = "Cancelada";
                            else if (r < 0.30)
                                statusConsulta = "Não Realizada";
                            else if (r < 0.65)
                                statusConsulta = "Realizada";
                            else
                                statusConsulta = "Agendada";
                        }
                        else
                        {
                            double r = random.NextDouble();
                            // Para as demais consultas, distribui 7,5% Cancelada, 7,5% Não Realizada, 85% Realizada.
                            if (r < 0.075)
                                statusConsulta = "Cancelada";
                            else if (r < 0.15)
                                statusConsulta = "Não Realizada";
                            else
                                statusConsulta = "Realizada";
                        }

                        // Cria o objeto Consulta com os dados gerados.
                        var consulta = new Consulta
                        {
                            IdPaciente = paciente.IdPaciente,
                            DataConsulta = dataConsultaTexto,
                            DataConsultaConvertida = dataConsulta,
                            Status = statusConsulta,
                        };

                        // Adiciona a consulta ao banco de dados.
                        await consultaRepo.AddAsync(consulta);

                        // Seleciona uma categoria aleatória de procedimentos.
                        // Se o índice atual for um dos que deve ter o procedimento repetido, força o procedimento repetido.
                        Dictionary<string, decimal> categoriaProcedimentos;
                        KeyValuePair<string, decimal> procedimentoSelecionado;
                        if (procedimentoRepetido != null && indicesRepetidos.Contains(j))
                        {
                            // Usa a categoria "Prevenção e Profilaxia" para o procedimento repetido.
                            categoriaProcedimentos = tiposProcedimentos["Prevenção e Profilaxia"];
                            procedimentoSelecionado = categoriaProcedimentos.First(p => p.Key == procedimentoRepetido);
                        }
                        else
                        {
                            // Seleciona uma categoria aleatória de procedimentos e depois um procedimento aleatório dentro dela.
                            categoriaProcedimentos = tiposProcedimentos.ElementAt(random.Next(tiposProcedimentos.Count)).Value;
                            procedimentoSelecionado = categoriaProcedimentos.ElementAt(random.Next(categoriaProcedimentos.Count));
                        }

                        // Cria o objeto Procedimento para a consulta.
                        var procedimento = new Procedimento
                        {
                            IdConsulta = consulta.IdConsulta,
                            TipoProcedimento = procedimentoSelecionado.Key,
                            Descricao = "",
                            Custo = procedimentoSelecionado.Value
                        };

                        // Adiciona o procedimento ao banco de dados.
                        await procedimentoRepo.AddAsync(procedimento);
                    }

                    // Atualiza o número de consultas do paciente e salva a alteração.
                    paciente.NumConsultas = numConsultas;
                    await pacienteRepo.UpdateAsync(paciente);
                }
            }
        }
    }
}
