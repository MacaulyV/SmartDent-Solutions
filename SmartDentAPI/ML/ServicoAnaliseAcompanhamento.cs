using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using SmartDentAPI.Models;

namespace SmartDentAPI.ML
{
    /// <summary>
    /// Serviço para análise de necessidade de acompanhamento especializado usando ML.NET
    /// </summary>
    public class ServicoAnaliseAcompanhamento
    {
        private readonly MLContext _mlContext;
        private ITransformer _modelo;
        private readonly string _caminhoModelo;
        private readonly Random _random = new Random();
        
        /// <summary>
        /// Construtor do serviço de análise de acompanhamento
        /// </summary>
        public ServicoAnaliseAcompanhamento()
        {
            _mlContext = new MLContext(seed: 42);
            _caminhoModelo = Path.Combine(AppContext.BaseDirectory, "modelo_analise_acompanhamento.zip");
        }
        
        /// <summary>
        /// Treina o modelo com base em dados históricos
        /// </summary>
        /// <param name="pacientes">Lista de pacientes para treinar o modelo</param>
        /// <param name="consultas">Lista de consultas do histórico</param>
        /// <returns>Acurácia do modelo treinado</returns>
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
                    l2Regularization: 0.01f, // Adicionar regularização para melhorar generalização
                    optimizationTolerance: 1e-4f)); // Aumentar precisão do treinamento
            
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
        /// Carrega um modelo previamente treinado
        /// </summary>
        /// <returns>Verdadeiro se o modelo foi carregado com sucesso</returns>
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
        /// Analisa se um paciente necessita de acompanhamento especializado
        /// </summary>
        /// <param name="paciente">Dados do paciente</param>
        /// <param name="consultas">Consultas do paciente</param>
        /// <returns>Resultado da análise com justificativa</returns>
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
                
                // Gerar justificativa com base nos dados analisados
                resultado.Justificativa = GerarJustificativa(dadosEntrada, resultado);
                
                // Adicionar detalhes sobre consultas analisadas
                AdicionarDetalhesConsultas(dadosEntrada, resultado);
                
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
                    Score = dadosEntrada.NumConsultasNaoRealizadas >= 2 || dadosEntrada.NumConsultasAgendadasExpiradas >= 1 ? 0.85f : 0.15f,
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
                
                // Gerar justificativa com base nos dados analisados
                resultado.Justificativa = GerarJustificativa(dadosEntrada, resultado);
                
                // Adicionar detalhes sobre consultas analisadas
                AdicionarDetalhesConsultas(dadosEntrada, resultado);
                
                return resultado;
            }
        }
        
        /// <summary>
        /// Prepara os dados de entrada para o modelo a partir do paciente e suas consultas
        /// </summary>
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
        /// Gera uma justificativa personalizada com base nos dados analisados
        /// </summary>
        private string GerarJustificativa(DadosEntradaAcompanhamento dados, ResultadoAnaliseAcompanhamento resultado)
        {
            if (!resultado.NecessitaAcompanhamento)
            {
                return "O paciente não apresenta indicadores suficientes de necessidade de acompanhamento especializado. " +
                       "O histórico de consultas está regular e não há um padrão de faltas ou consultas não atualizadas que " +
                       "justifique uma intervenção especial no momento.";
            }
            
            List<string> motivos = new List<string>();
            
            // Analisar faltas
            if (dados.NumConsultasNaoRealizadas >= 2)
            {
                motivos.Add($"O paciente faltou a {dados.NumConsultasNaoRealizadas} consultas sem justificativa prévia");
            }
            
            // Analisar consultas agendadas não atualizadas
            if (dados.NumConsultasAgendadasExpiradas >= 1)
            {
                motivos.Add($"Existem {dados.NumConsultasAgendadasExpiradas} consultas agendadas para datas passadas que não foram atualizadas");
            }
            
            // Analisar percentual de faltas
            if (dados.PercentualFaltas >= 25)
            {
                motivos.Add($"O percentual de faltas é de {dados.PercentualFaltas:F1}%, considerado alto");
            }
            
            // Analisar tempo desde a última consulta
            if (dados.DiasDesdeuUltimaConsulta > 180) // Mais de 6 meses
            {
                motivos.Add($"Passaram-se {dados.DiasDesdeuUltimaConsulta:F0} dias desde a última consulta realizada");
            }
            
            if (motivos.Count == 0)
            {
                motivos.Add("A combinação de fatores no histórico do paciente sugere necessidade de acompanhamento");
            }
            
            string justificativa = "O paciente necessita de acompanhamento especializado pelos seguintes motivos: " +
                                   string.Join("; ", motivos) + ".";
            
            return justificativa;
        }
        
        /// <summary>
        /// Adiciona detalhes sobre as consultas analisadas ao resultado
        /// </summary>
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
        /// Gera dados de treinamento com base nos dados históricos
        /// </summary>
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
        /// Gera exemplos sintéticos para treinamento
        /// </summary>
        private List<DadosEntradaAcompanhamento> GerarDadosSinteticos(int quantidade)
        {
            var dadosSinteticos = new List<DadosEntradaAcompanhamento>();
            
            for (int i = 0; i < quantidade / 2; i++)
            {
                // Criar exemplo positivo (necessita acompanhamento)
                dadosSinteticos.Add(new DadosEntradaAcompanhamento
                {
                    Idade = _random.Next(18, 80),
                    NumConsultasTotal = _random.Next(3, 15),
                    NumConsultasNaoRealizadas = _random.Next(2, 5),
                    NumConsultasAgendadasExpiradas = _random.Next(0, 3),
                    PercentualFaltas = _random.Next(25, 70),
                    DiasDesdeuUltimaConsulta = _random.Next(180, 500),
                    NecessitaAcompanhamento = true
                });
                
                // Criar exemplo negativo (não necessita acompanhamento)
                dadosSinteticos.Add(new DadosEntradaAcompanhamento
                {
                    Idade = _random.Next(18, 80),
                    NumConsultasTotal = _random.Next(1, 20),
                    NumConsultasNaoRealizadas = _random.Next(0, 1),
                    NumConsultasAgendadasExpiradas = 0,
                    PercentualFaltas = _random.Next(0, 15),
                    DiasDesdeuUltimaConsulta = _random.Next(0, 120),
                    NecessitaAcompanhamento = false
                });
            }
            
            return dadosSinteticos;
        }
    }
} 