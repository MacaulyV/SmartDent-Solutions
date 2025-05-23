using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using SmartDentAPI.Models;

namespace SmartDentAPI.ML
{
    /// <summary>
    /// Serviço para análise de necessidade de acompanhamento especializado usando ML.NET.
    /// </summary>
    /// <remarks>
    /// Esta classe implementa um serviço de Machine Learning que utiliza a biblioteca ML.NET para 
    /// analisar o histórico de consultas dos pacientes e determinar quais deles necessitam de um 
    /// acompanhamento mais próximo devido a fatores como faltas recorrentes, consultas não atualizadas 
    /// ou longos períodos sem atendimento.
    /// 
    /// O serviço é capaz de:
    /// - Treinar um modelo com dados históricos
    /// - Salvar e carregar modelos treinados
    /// - Realizar análises individuais de pacientes
    /// - Gerar justificativas e recomendações detalhadas
    /// </remarks>
    public class ServicoAnaliseAcompanhamento
    {
        private readonly MLContext _mlContext;
        private ITransformer _modelo;
        private readonly string _caminhoModelo;
        private readonly Random _random = new Random();
        
        /// <summary>
        /// Construtor do serviço de análise de acompanhamento.
        /// </summary>
        /// <remarks>
        /// Inicializa o contexto ML.NET com uma semente fixa para garantir reprodutibilidade
        /// e configura o caminho para salvar/carregar o modelo treinado.
        /// </remarks>
        public ServicoAnaliseAcompanhamento()
        {
            _mlContext = new MLContext(seed: 42);
            _caminhoModelo = Path.Combine(AppContext.BaseDirectory, "modelo_analise_acompanhamento.zip");
        }
        
        /// <summary>
        /// Treina o modelo com base em dados históricos de pacientes e suas consultas.
        /// </summary>
        /// <param name="pacientes">Lista de pacientes para treinar o modelo.</param>
        /// <param name="consultas">Lista de consultas do histórico.</param>
        /// <returns>Acurácia do modelo treinado, como um valor entre 0 e 1.</returns>
        /// <remarks>
        /// Este método processa os dados históricos, gera as features necessárias e treina um
        /// modelo de classificação binária utilizando regressão logística. O modelo treinado é
        /// salvo para uso futuro. Se os dados históricos forem insuficientes, o método 
        /// complementa o treinamento com dados sintéticos.
        /// </remarks>
        public float TreinarModelo(List<Paciente> pacientes, List<Consulta> consultas)
        {
            // Gerar dados de treinamento com base nos dados reais do sistema
            var dadosTreinamento = GerarDadosTreinamento(pacientes, consultas);
            
            // Carregar dados no formato ML.NET
            var dataView = _mlContext.Data.LoadFromEnumerable(dadosTreinamento);
            
            // Pipeline de treinamento com features relevantes para acompanhamento
            var pipeline = _mlContext.Transforms.Concatenate("Features", 
                    "Idade", "NumConsultasTotal", "NumConsultasNaoRealizadas", 
                    "NumConsultasAgendadasExpiradas", "PercentualFaltas", "DiasDesdeuUltimaConsulta")
                .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
                .Append(_mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression(
                    labelColumnName: "Label",
                    featureColumnName: "Features",
                    l1Regularization: 0.1f, // Adicionar regularização L1 para seleção de features
                    l2Regularization: 0.2f, // Aumentar regularização L2
                    optimizationTolerance: 1e-5f, // Aumentar precisão do treinamento
                    historySize: 20)); // Aumentar histórico para LBFGS
            
            // Treinar modelo
            _modelo = pipeline.Fit(dataView);
            
            // Salvar modelo em arquivo
            if (File.Exists(_caminhoModelo))
            {
                File.Delete(_caminhoModelo); // Excluir modelo anterior
            }
            _mlContext.Model.Save(_modelo, dataView.Schema, _caminhoModelo);
            
            // Avaliar modelo
            var predicoes = _modelo.Transform(dataView);
            var metricas = _mlContext.BinaryClassification.Evaluate(predicoes);
            
            // Registrar métricas detalhadas para análise
            Console.WriteLine($"Acurácia: {metricas.Accuracy:P2}");
            Console.WriteLine($"AUC: {metricas.AreaUnderRocCurve:P2}");
            Console.WriteLine($"F1Score: {metricas.F1Score:P2}");
            
            return (float)metricas.Accuracy;
        }
        
        /// <summary>
        /// Carrega um modelo previamente treinado a partir do arquivo salvo.
        /// </summary>
        /// <returns>Verdadeiro se o modelo foi carregado com sucesso, falso caso contrário.</returns>
        /// <remarks>
        /// Verifica se existe um arquivo de modelo no caminho configurado e, se existir,
        /// carrega-o para uso nas predições. Isso evita a necessidade de treinar o modelo
        /// a cada inicialização do serviço.
        /// </remarks>
        public bool CarregarModelo()
        {
            if (File.Exists(_caminhoModelo))
            {
                _modelo = _mlContext.Model.Load(_caminhoModelo, out _);
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Analisa se um paciente necessita de acompanhamento especializado.
        /// </summary>
        /// <param name="paciente">Dados do paciente a ser analisado.</param>
        /// <param name="consultas">Lista de consultas do paciente.</param>
        /// <returns>Resultado detalhado da análise com justificativa e sugestões.</returns>
        /// <remarks>
        /// Este método realiza a predição utilizando o modelo de ML.NET treinado.
        /// Se o modelo não estiver disponível, tenta carregá-lo ou treiná-lo com os dados fornecidos.
        /// Além da predição, enriquece o resultado com justificativas, níveis de alerta e
        /// sugestões de ação personalizadas com base nos dados do paciente.
        /// </remarks>
        public ResultadoAnaliseAcompanhamento AnalisarNecessidadeAcompanhamento(Paciente paciente, List<Consulta> consultas)
        {
            if (_modelo == null)
            {
                var modeloCarregado = CarregarModelo();
                if (!modeloCarregado)
                {
                    // Se o modelo não estiver disponível, treine com os dados atuais
                    // (em produção, seria recomendável ter um modelo pré-treinado)
                    TreinarModelo(new List<Paciente> { paciente }, consultas);
                }
            }
            
            // Preparar dados de entrada para o modelo
            var dadosEntrada = PrepararDadosEntrada(paciente, consultas);
            
            try
            {
                // Criar o motor de predição com definição explícita de esquema
                var predictionEngine = _mlContext.Model.CreatePredictionEngine<DadosEntradaAcompanhamento, ResultadoAnaliseAcompanhamento>(
                    _modelo,
                    ignoreMissingColumns: true);
                
                // Fazer a predição
                var resultado = predictionEngine.Predict(dadosEntrada);
                
                // Adicionar dados complementares ao resultado
                resultado.IdPaciente = paciente.IdPaciente;
                resultado.NumConsultasNaoRealizadas = (int)dadosEntrada.NumConsultasNaoRealizadas;
                resultado.NumConsultasAgendadasExpiradas = (int)dadosEntrada.NumConsultasAgendadasExpiradas;
                
                // Encontrar a data da última consulta
                var ultimaConsulta = dadosEntrada.Consultas
                    .Where(c => c.Status == "Realizada")
                    .OrderByDescending(c => c.DataConsulta)
                    .FirstOrDefault();
                
                if (ultimaConsulta != null)
                {
                    resultado.DataUltimaConsulta = ultimaConsulta.DataConsulta;
                }
                
                // Gerar justificativa, nível de alerta e sugestão de ação
                DefinirAnaliseCompleta(dadosEntrada, resultado);
                
                return resultado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar predictionEngine: {ex.Message}");
                // Fallback para análise manual quando o modelo ML falhar
                var resultado = new ResultadoAnaliseAcompanhamento
                {
                    IdPaciente = paciente.IdPaciente,
                    NecessitaAcompanhamento = dadosEntrada.NumConsultasNaoRealizadas >= 2 || dadosEntrada.NumConsultasAgendadasExpiradas >= 1,
                    Score = dadosEntrada.NumConsultasNaoRealizadas >= 2 || dadosEntrada.NumConsultasAgendadasExpiradas >= 1 ? 0.85f : 0.15f, // Simula score alto para necessidade
                    Probabilidade = dadosEntrada.NumConsultasNaoRealizadas >= 2 || dadosEntrada.NumConsultasAgendadasExpiradas >= 1 ? 0.85f : 0.15f, // Simula probabilidade
                    NumConsultasNaoRealizadas = (int)dadosEntrada.NumConsultasNaoRealizadas,
                    NumConsultasAgendadasExpiradas = (int)dadosEntrada.NumConsultasAgendadasExpiradas
                };
                
                // Encontrar a data da última consulta
                var ultimaConsulta = dadosEntrada.Consultas
                    .Where(c => c.Status == "Realizada")
                    .OrderByDescending(c => c.DataConsulta)
                    .FirstOrDefault();
                
                if (ultimaConsulta != null)
                {
                    resultado.DataUltimaConsulta = ultimaConsulta.DataConsulta;
                }
                
                // Gerar justificativa, nível de alerta e sugestão de ação para o fallback
                DefinirAnaliseCompleta(dadosEntrada, resultado);
                
                return resultado;
            }
        }
        
        /// <summary>
        /// Prepara os dados de entrada para o modelo a partir do paciente e suas consultas.
        /// </summary>
        /// <param name="paciente">Paciente a ser analisado.</param>
        /// <param name="consultas">Lista de consultas, potencialmente incluindo consultas de outros pacientes.</param>
        /// <returns>Objeto com os dados de entrada formatados para o modelo.</returns>
        /// <remarks>
        /// Este método extrai as features necessárias para o modelo a partir dos dados brutos do paciente e suas consultas.
        /// Realiza cálculos como idade, percentual de faltas, dias desde a última consulta, etc.
        /// </remarks>
        private DadosEntradaAcompanhamento PrepararDadosEntrada(Paciente paciente, List<Consulta> consultas)
        {
            // Filtrar apenas consultas deste paciente
            var consultasPaciente = consultas
                .Where(c => c.IdPaciente == paciente.IdPaciente)
                .ToList();
            
            // Converter a data de nascimento para calcular a idade
            DateTime dataNascimento;
            if (paciente.DataNascimentoConvertida.HasValue)
            {
                dataNascimento = paciente.DataNascimentoConvertida.Value;
            }
            else
            {
                // Se a propriedade convertida não estiver disponível, converter manualmente
                dataNascimento = DateTime.ParseExact(paciente.DataNascimento, "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            
            // Calcular idade em anos
            var idade = (DateTime.Now - dataNascimento).TotalDays / 365.25;
            
            // Preparar lista de consultas para análise
            var dataAtual = DateTime.Now;
            var listaConsultasInfo = new List<ConsultaInfo>();
            
            foreach (var consulta in consultasPaciente)
            {
                DateTime dataConsulta;
                if (consulta.DataConsultaConvertida.HasValue)
                {
                    dataConsulta = consulta.DataConsultaConvertida.Value;
                }
                else
                {
                    // Se a propriedade convertida não estiver disponível, converter manualmente
                    dataConsulta = DateTime.ParseExact(consulta.DataConsulta, "ddMMyyyyHHmm", System.Globalization.CultureInfo.InvariantCulture);
                }
                
                listaConsultasInfo.Add(new ConsultaInfo
                {
                    IdConsulta = consulta.IdConsulta,
                    DataConsulta = dataConsulta,
                    Status = consulta.Status
                });
            }
            
            // Contabilizar métricas relevantes
            int totalConsultas = listaConsultasInfo.Count;
            int consultasNaoRealizadas = listaConsultasInfo.Count(c => c.Status == "Não realizada");
            
            // Identificar consultas agendadas expiradas (consultas agendadas para datas passadas)
            int consultasAgendadasExpiradas = listaConsultasInfo.Count(c => 
                c.Status == "Agendada" && c.DataConsulta < dataAtual);
            
            // Calcular percentual de faltas
            float percentualFaltas = totalConsultas > 0 
                ? (float)consultasNaoRealizadas / totalConsultas * 100 
                : 0;
            
            // Calcular dias desde a última consulta realizada
            float diasDesdeUltimaConsulta = 0;
            var ultimaConsultaRealizada = listaConsultasInfo
                .Where(c => c.Status == "Realizada")
                .OrderByDescending(c => c.DataConsulta)
                .FirstOrDefault();
            
            if (ultimaConsultaRealizada != null)
            {
                diasDesdeUltimaConsulta = (float)(dataAtual - ultimaConsultaRealizada.DataConsulta).TotalDays;
            }
            
            // Criar objeto de entrada para o modelo
            return new DadosEntradaAcompanhamento
            {
                IdPaciente = paciente.IdPaciente,
                Idade = (float)idade,
                NumConsultasTotal = totalConsultas,
                NumConsultasNaoRealizadas = consultasNaoRealizadas,
                NumConsultasAgendadasExpiradas = consultasAgendadasExpiradas,
                PercentualFaltas = percentualFaltas,
                DiasDesdeuUltimaConsulta = diasDesdeUltimaConsulta,
                Consultas = listaConsultasInfo
            };
        }
        
        /// <summary>
        /// Define a justificativa, nível de alerta e sugestão de ação para o resultado da análise.
        /// </summary>
        /// <param name="dados">Dados de entrada utilizados na análise.</param>
        /// <param name="resultado">Resultado da análise a ser enriquecido com informações adicionais.</param>
        /// <remarks>
        /// Este método analisa os dados do paciente e o resultado da predição para adicionar:
        /// - Um nível de alerta (Crítico, Atenção, Moderado ou Padrão) com base na severidade
        /// - Uma justificativa detalhada explicando os motivos da análise
        /// - Sugestões de ações que podem ser tomadas com base no resultado
        /// - Detalhes específicos sobre as consultas consideradas na análise
        /// 
        /// Estas informações auxiliam os profissionais de saúde a entender o resultado
        /// e definir próximos passos para o acompanhamento do paciente.
        /// </remarks>
        private void DefinirAnaliseCompleta(DadosEntradaAcompanhamento dados, ResultadoAnaliseAcompanhamento resultado)
        {
            List<string> motivos = new List<string>();
            string sugestao = "";

            if (resultado.NecessitaAcompanhamento)
            {
                // Nível de Alerta e Sugestões
                if (resultado.Score > 0.8 || dados.NumConsultasNaoRealizadas >= 3 || dados.NumConsultasAgendadasExpiradas >= 2)
                {
                    resultado.NivelAlerta = "Crítico";
                    sugestao = "Recomenda-se contato imediato com o paciente para entender os motivos das faltas/consultas expiradas e reengajá-lo no tratamento. Avaliar a possibilidade de oferecer horários alternativos ou discutir a importância da continuidade do tratamento.";
                }
                else if (resultado.Score > 0.6 || dados.NumConsultasNaoRealizadas >= 2 || dados.NumConsultasAgendadasExpiradas >= 1)
                {
                    resultado.NivelAlerta = "Atenção";
                    sugestao = "Sugere-se um contato proativo com o paciente para verificar a situação das consultas e reforçar a importância do acompanhamento. Oferecer suporte para reagendamento pode ser útil.";
                }
                else
                {
                    resultado.NivelAlerta = "Moderado"; // Caso o modelo preveja acompanhamento com score menor
                    sugestao = "Monitorar o paciente e, caso o padrão persista, realizar um contato para entender melhor suas necessidades e dificuldades em seguir o plano de tratamento.";
                }

                // Justificativa
                if (dados.NumConsultasNaoRealizadas > 0)
                {
                    motivos.Add($"identificou-se um histórico de {dados.NumConsultasNaoRealizadas} consulta(s) registrada(s) como 'Não realizada(s)'. Este é um indicador importante de desengajamento ou possíveis barreiras que o paciente pode estar enfrentando.");
                }
                if (dados.NumConsultasAgendadasExpiradas > 0)
                {
                    motivos.Add($"foram encontradas {dados.NumConsultasAgendadasExpiradas} consulta(s) com status 'Agendada' cuja data já passou, indicando uma possível falha no acompanhamento ou atualização do status da consulta.");
                }
                if (dados.PercentualFaltas >= 25)
                {
                    motivos.Add($"o paciente apresenta um percentual de faltas de {dados.PercentualFaltas:F1}%. Índices acima de 20-25% geralmente sinalizam uma necessidade de intervenção para melhorar a adesão ao tratamento.");
                }
                if (dados.DiasDesdeuUltimaConsulta > 180)
                {
                    motivos.Add($"o sistema registrou que a última consulta efetivamente realizada ocorreu há mais de {dados.DiasDesdeuUltimaConsulta:F0} dias ({Math.Round(dados.DiasDesdeuUltimaConsulta / 30.0, 1)} meses). Um período extenso sem acompanhamento pode comprometer os resultados do tratamento.");
                }
                
                if (motivos.Count == 0 && resultado.NecessitaAcompanhamento) // Se o modelo previu, mas as regras manuais não pegaram
                {
                     motivos.Add("a análise preditiva, com base no comportamento histórico e características do paciente, identificou um padrão que sugere um risco elevado de descontinuidade ou complicações no tratamento, justificando um acompanhamento mais próximo.");
                }

                if (motivos.Any())
                {
                    resultado.Justificativa = $"A análise indica necessidade de acompanhamento especializado. Nível de alerta: {resultado.NivelAlerta}. Detalhamento: " + string.Join(" Adicionalmente, ", motivos);
                }
                else
                {
                    resultado.Justificativa = $"A análise indica necessidade de acompanhamento especializado (Nível de alerta: {resultado.NivelAlerta}), porém os motivos específicos não foram claramente identificados pelas regras atuais. Revisar dados do paciente.";
                }
                resultado.SugestaoAcao = sugestao;

            }
            else
            {
                resultado.NivelAlerta = "Padrão";
                resultado.Justificativa = "O paciente demonstra um bom engajamento com o plano de tratamento. Não foram identificados indicadores de risco que demandem acompanhamento especializado no momento. Manter o monitoramento regular.";
                resultado.SugestaoAcao = "Continuar com o acompanhamento padrão e monitorar o histórico de consultas. Incentivar a manutenção da regularidade.";
            }

            // Adicionar detalhes sobre consultas analisadas
            AdicionarDetalhesConsultas(dados, resultado);
        }

        /// <summary>
        /// Adiciona detalhes sobre as consultas analisadas ao resultado.
        /// </summary>
        /// <param name="dados">Dados de entrada com as informações das consultas.</param>
        /// <param name="resultado">Resultado da análise onde serão adicionados os detalhes.</param>
        /// <remarks>
        /// Enriquece o resultado da análise com informações textuais específicas sobre consultas 
        /// que foram relevantes para a avaliação, como faltas, consultas não atualizadas e a 
        /// última consulta realizada.
        /// </remarks>
        private void AdicionarDetalhesConsultas(DadosEntradaAcompanhamento dados, ResultadoAnaliseAcompanhamento resultado)
        {
            // Adicionar detalhes das consultas não realizadas
            foreach (var consulta in dados.Consultas.Where(c => c.Status == "Não realizada"))
            {
                resultado.DetalhesConsultas.Add($"Consulta ID {consulta.IdConsulta} marcada para {consulta.DataConsulta:dd/MM/yyyy HH:mm} não foi realizada");
            }
            
            // Adicionar detalhes das consultas agendadas expiradas
            foreach (var consulta in dados.Consultas.Where(c => c.Status == "Agendada" && c.DataConsulta < DateTime.Now))
            {
                resultado.DetalhesConsultas.Add($"Consulta ID {consulta.IdConsulta} agendada para {consulta.DataConsulta:dd/MM/yyyy HH:mm} não foi atualizada após a data marcada");
            }
            
            // Adicionar detalhes da última consulta realizada
            var ultimaConsulta = dados.Consultas
                .Where(c => c.Status == "Realizada")
                .OrderByDescending(c => c.DataConsulta)
                .FirstOrDefault();
            
            if (ultimaConsulta != null)
            {
                resultado.DetalhesConsultas.Add($"Última consulta realizada: ID {ultimaConsulta.IdConsulta} em {ultimaConsulta.DataConsulta:dd/MM/yyyy HH:mm}");
            }
            else
            {
                resultado.DetalhesConsultas.Add("Não há registro de consultas realizadas");
            }
        }
        
        /// <summary>
        /// Gera dados de treinamento com base nos dados históricos.
        /// </summary>
        /// <param name="pacientes">Lista de pacientes do sistema.</param>
        /// <param name="consultas">Lista de consultas do sistema.</param>
        /// <returns>Lista de dados formatados para treinamento do modelo.</returns>
        /// <remarks>
        /// Este método processa os dados históricos dos pacientes e suas consultas, 
        /// transformando-os em exemplos adequados para o treinamento do modelo.
        /// 
        /// Para garantir um conjunto de dados balanceado e robusto, o método:
        /// 1. Extrai padrões dos dados reais do sistema
        /// 2. Complementa com exemplos sintéticos para melhorar a generalização
        /// 3. Calcula as features necessárias (idade, faltas, etc.) a partir dos dados brutos
        /// 
        /// Se não houver dados suficientes no sistema, o método gera apenas exemplos sintéticos.
        /// </remarks>
        private List<DadosEntradaAcompanhamento> GerarDadosTreinamento(List<Paciente> pacientes, List<Consulta> consultas)
        {
            var dadosTreinamento = new List<DadosEntradaAcompanhamento>();
            
            Console.WriteLine($"Iniciando geração de dados de treinamento com {pacientes.Count} pacientes e {consultas.Count} consultas");
            
            // Garantir que há dados suficientes
            if (pacientes.Count < 1 || consultas.Count < 1)
            {
                // Se não houver dados suficientes, gerar exemplos sintéticos somente
                Console.WriteLine("Dados insuficientes para treinamento real, gerando apenas exemplos sintéticos");
                return GerarDadosSinteticos(200);
            }
            
            // Agrupar consultas por paciente
            var consultasPorPaciente = new Dictionary<int, List<Consulta>>();
            foreach (var consulta in consultas)
            {
                if (!consultasPorPaciente.ContainsKey(consulta.IdPaciente))
                {
                    consultasPorPaciente[consulta.IdPaciente] = new List<Consulta>();
                }
                consultasPorPaciente[consulta.IdPaciente].Add(consulta);
            }
            
            // Contar pacientes com consultas
            int pacientesComConsultas = 0;
            
            // Para cada paciente, gerar dados de treinamento
            foreach (var paciente in pacientes)
            {
                try
                {
                    // Verificar se o paciente tem consultas
                    if (!consultasPorPaciente.ContainsKey(paciente.IdPaciente) || 
                        consultasPorPaciente[paciente.IdPaciente].Count == 0)
                    {
                        continue;
                    }
                    
                    pacientesComConsultas++;
                    
                    var dados = PrepararDadosEntrada(paciente, consultasPorPaciente[paciente.IdPaciente]);
                    
                    // Simular a necessidade de acompanhamento baseada nos padrões observados
                    // Lógica: pacientes com mais faltas ou consultas não atualizadas têm maior probabilidade
                    bool necessitaAcompanhamento = false;
                    
                    if (dados.NumConsultasNaoRealizadas >= 2 || 
                        dados.NumConsultasAgendadasExpiradas >= 1 ||
                        dados.PercentualFaltas >= 25 ||
                        dados.DiasDesdeuUltimaConsulta > 180)
                    {
                        necessitaAcompanhamento = true;
                    }
                    
                    dados.NecessitaAcompanhamento = necessitaAcompanhamento;
                    dadosTreinamento.Add(dados);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao processar paciente {paciente.IdPaciente}: {ex.Message}");
                    // Continuar para o próximo paciente
                }
            }
            
            Console.WriteLine($"Foram processados {pacientesComConsultas} pacientes com consultas");
            
            // Mesmo que tenhamos processado alguns pacientes reais, sempre adicionamos exemplos sintéticos
            // para melhorar o treinamento e evitar overfitting
            
            // Adicionar exemplos sintéticos para melhorar o treinamento
            var dadosSinteticos = GerarDadosSinteticos(200);
            dadosTreinamento.AddRange(dadosSinteticos);
            
            Console.WriteLine($"Total de exemplos de treinamento: {dadosTreinamento.Count}");
            
            return dadosTreinamento;
        }
        
        /// <summary>
        /// Gera exemplos sintéticos para complementar o treinamento do modelo.
        /// </summary>
        /// <param name="quantidade">Número de exemplos sintéticos a serem gerados.</param>
        /// <returns>Lista de dados sintéticos para treinamento.</returns>
        /// <remarks>
        /// Este método cria exemplos artificiais que ajudam a:
        /// 1. Balancear o conjunto de treinamento (exemplos positivos e negativos)
        /// 2. Melhorar a capacidade de generalização do modelo
        /// 3. Cobrir casos de borda que podem não estar presentes nos dados reais
        /// 
        /// Os exemplos são gerados com distribuições que refletem padrões reais,
        /// como pacientes que faltam muito e têm longo período sem consulta (casos positivos)
        /// versus pacientes com baixas taxas de falta e consultas recentes (casos negativos).
        /// </remarks>
        private List<DadosEntradaAcompanhamento> GerarDadosSinteticos(int quantidade)
        {
            var dadosSinteticos = new List<DadosEntradaAcompanhamento>();
            
            for (int i = 0; i < quantidade; i++)
            {
                bool necessitaAcompanhamento = _random.Next(0, 2) == 0; // 50% chance
                
                if (necessitaAcompanhamento)
                {
                    // Criar exemplo positivo (necessita acompanhamento)
                    dadosSinteticos.Add(new DadosEntradaAcompanhamento
                    {
                        Idade = _random.Next(18, 85),
                        NumConsultasTotal = _random.Next(2, 25),
                        // Gera mais faltas e consultas expiradas para casos positivos
                        NumConsultasNaoRealizadas = _random.Next(1, _random.Next(2,6)), // Pelo menos 1, max 5
                        NumConsultasAgendadasExpiradas = _random.Next(0, _random.Next(1,4)), // Pode ter 0, max 3
                        PercentualFaltas = _random.Next(15, 80), // Percentual de faltas mais alto
                        DiasDesdeuUltimaConsulta = _random.Next(90, 730), // Tempo maior desde a última consulta
                        NecessitaAcompanhamento = true
                    });
                }
                else
                {
                    // Criar exemplo negativo (não necessita acompanhamento)
                    dadosSinteticos.Add(new DadosEntradaAcompanhamento
                    {
                        Idade = _random.Next(18, 85),
                        NumConsultasTotal = _random.Next(1, 30),
                        NumConsultasNaoRealizadas = _random.Next(0, 2), // Poucas ou nenhuma falta
                        NumConsultasAgendadasExpiradas = _random.Next(0,1), // Nenhuma ou no max 1 expirada (acaso)
                        PercentualFaltas = _random.Next(0, 20), // Percentual de faltas baixo
                        DiasDesdeuUltimaConsulta = _random.Next(0, 150), // Consulta recente
                        NecessitaAcompanhamento = false
                    });
                }
            }
            
            return dadosSinteticos;
        }
    }
} 