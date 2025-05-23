using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using SmartDentAPI.Models;
using SmartDentAPI.Interfaces;
using SmartDentAPI.ML;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace SmartDentAPI.Controllers
{
    /// <summary>
    /// Controller para análise de necessidade de acompanhamento especializado
    /// </summary>
    /// <remarks>
    /// Este controller utiliza modelos de Machine Learning treinados com ML.NET para analisar
    /// históricos de consultas e identificar pacientes que necessitam de acompanhamento especializado
    /// devido a faltas ou consultas não atualizadas.
    /// </remarks>
    [Route("api/AnaliseAcompanhamento")]
    [ApiController]
    [SwaggerTag("AnaliseAcompanhamento")]
    public class AnaliseAcompanhamentoController : ControllerBase
    {
        private readonly IPacienteRepository _pacienteRepo;
        private readonly IConsultaRepository _consultaRepo;
        private readonly ServicoAnaliseAcompanhamento _servicoAnalise;

        /// <summary>
        /// Construtor que injeta as dependências necessárias
        /// </summary>
        public AnaliseAcompanhamentoController(
            IPacienteRepository pacienteRepo,
            IConsultaRepository consultaRepo,
            ServicoAnaliseAcompanhamento servicoAnalise)
        {
            _pacienteRepo = pacienteRepo;
            _consultaRepo = consultaRepo;
            _servicoAnalise = servicoAnalise;
        }

        /// <summary>
        /// Treina o modelo ML.NET com os dados existentes no sistema
        /// </summary>
        /// <returns>Resultado do treinamento do modelo</returns>
        [HttpPost("treinar-modelo")]
        [SwaggerOperation(
            Summary = "Treinar modelo de ML",
            Description = "Treina o modelo de Machine Learning com os dados existentes no sistema",
            OperationId = "TreinarModeloAcompanhamento",
            Tags = new[] { "AnaliseAcompanhamento" }
        )]
        public async Task<IActionResult> TreinarModelo()
        {
            try
            {
                // Obter todos os dados necessários para o treinamento
                var pacientes = (await _pacienteRepo.GetAllAsync()).ToList();
                var consultas = (await _consultaRepo.GetAllAsync()).ToList();

                if (!pacientes.Any())
                {
                    return BadRequest(new { erro = "Não há pacientes suficientes para treinar o modelo." });
                }

                if (!consultas.Any())
                {
                    return BadRequest(new { erro = "Não há consultas suficientes para treinar o modelo." });
                }

                // Converter as datas para trabalhar com DateTime
                foreach (var paciente in pacientes)
                {
                    ConverterDataNascimentoPaciente(paciente);
                }

                foreach (var consulta in consultas)
                {
                    ConverterDatasConsulta(consulta);
                }

                // Preparar dados de treinamento com consultas diferentes para cada paciente
                Dictionary<int, List<Consulta>> consultasPorPaciente = new Dictionary<int, List<Consulta>>();
                foreach (var consulta in consultas)
                {
                    if (!consultasPorPaciente.ContainsKey(consulta.IdPaciente))
                    {
                        consultasPorPaciente[consulta.IdPaciente] = new List<Consulta>();
                    }
                    consultasPorPaciente[consulta.IdPaciente].Add(consulta);
                }

                // Filtrar apenas pacientes com consultas
                var pacientesComConsultas = pacientes
                    .Where(p => consultasPorPaciente.ContainsKey(p.IdPaciente) && consultasPorPaciente[p.IdPaciente].Count > 0)
                    .ToList();

                if (pacientesComConsultas.Count < 5)
                {
                    return BadRequest(new { erro = "Não há dados suficientes para treinar o modelo. São necessários pelo menos 5 pacientes com consultas." });
                }

                // Treinar o modelo com dados reais
                var acuracia = _servicoAnalise.TreinarModelo(pacientesComConsultas, consultas);

                return Ok(new
                {
                    mensagem = $"Modelo treinado com sucesso. Acurácia: {acuracia:P2}",
                    acuracia = acuracia,
                    numeroRegistros = new
                    {
                        totalPacientes = pacientes.Count,
                        pacientesUsados = pacientesComConsultas.Count,
                        totalConsultas = consultas.Count
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    erro = $"Erro ao treinar o modelo: {ex.Message}",
                    detalhes = ex.StackTrace 
                });
            }
        }

        /// <summary>
        /// Analisa a necessidade de acompanhamento para um paciente específico
        /// </summary>
        /// <param name="idPaciente">ID do paciente</param>
        /// <returns>Resultado da análise</returns>
        [HttpGet("paciente/{idPaciente}")]
        [SwaggerOperation(
            Summary = "Análise de necessidade de acompanhamento",
            Description = "Analisa se um paciente necessita de acompanhamento especializado com base no histórico de consultas",
            OperationId = "AnalisarAcompanhamentoPaciente",
            Tags = new[] { "AnaliseAcompanhamento" }
        )]
        public async Task<IActionResult> AnalisarAcompanhamento(int idPaciente)
        {
            try
            {
                // Buscar os dados do paciente
                var paciente = await _pacienteRepo.GetByIdAsync(idPaciente);
                if (paciente == null)
                {
                    return NotFound(new { erro = "Paciente não encontrado." });
                }

                // Buscar as consultas do paciente
                var consultas = await _consultaRepo.GetByPacienteIdAsync(idPaciente);
                
                if (consultas == null || !consultas.Any())
                {
                    return Ok(new
                    {
                        idPaciente = idPaciente,
                        nomePaciente = paciente.NomeCompleto,
                        necessitaAcompanhamento = false,
                        justificativa = "O paciente não possui consultas registradas para análise."
                    });
                }

                // Converter datas
                ConverterDataNascimentoPaciente(paciente);
                foreach (var consulta in consultas)
                {
                    ConverterDatasConsulta(consulta);
                }
                
                try
                {
                    // Fazer a análise
                    var resultado = _servicoAnalise.AnalisarNecessidadeAcompanhamento(paciente, consultas.ToList());
                    
                    // Adicionar nome do paciente para o retorno
                    if (resultado != null)
                    {
                        var resultadoFormatado = new
                        {
                            resultado.IdPaciente,
                            NomePaciente = paciente.NomeCompleto,
                            resultado.NecessitaAcompanhamento,
                            ProbabilidadePercentual = (int)(resultado.Score * 100),
                            resultado.Justificativa,
                            resultado.DetalhesConsultas,
                            resultado.NumConsultasNaoRealizadas,
                            resultado.NumConsultasAgendadasExpiradas,
                            resultado.DataUltimaConsulta,
                            DataAnalise = DateTime.Now
                        };
                        
                        return Ok(resultadoFormatado);
                    }
                    else
                    {
                        return StatusCode(500, new { erro = "Erro ao processar resultado da análise." });
                    }
                }
                catch (InvalidOperationException)
                {
                    // Tentar treinar o modelo automaticamente com os dados disponíveis
                    try
                    {
                        var todasConsultas = await _consultaRepo.GetAllAsync();
                        var todosPacientes = await _pacienteRepo.GetAllAsync();
                        
                        foreach (var p in todosPacientes)
                        {
                            ConverterDataNascimentoPaciente(p);
                        }
                        
                        foreach (var c in todasConsultas)
                        {
                            ConverterDatasConsulta(c);
                        }
                        
                        var acuracia = _servicoAnalise.TreinarModelo(todosPacientes.ToList(), todasConsultas.ToList());
                        
                        // Tentar a análise novamente
                        var resultado = _servicoAnalise.AnalisarNecessidadeAcompanhamento(paciente, consultas.ToList());
                        
                        var resultadoFormatado = new
                        {
                            resultado.IdPaciente,
                            NomePaciente = paciente.NomeCompleto,
                            resultado.NecessitaAcompanhamento,
                            ProbabilidadePercentual = (int)(resultado.Score * 100),
                            resultado.Justificativa,
                            resultado.DetalhesConsultas,
                            resultado.NumConsultasNaoRealizadas,
                            resultado.NumConsultasAgendadasExpiradas,
                            resultado.DataUltimaConsulta,
                            DataAnalise = DateTime.Now,
                            ModeloTreinado = new
                            {
                                Acuracia = acuracia,
                                AcuraciaPercentual = (int)(acuracia * 100)
                            }
                        };
                        
                        return Ok(resultadoFormatado);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new
                        {
                            erro = "O modelo precisa ser treinado antes de realizar análises.",
                            detalhes = ex.Message,
                            dica = "Execute o endpoint /api/AnaliseAcompanhamento/treinar-modelo antes de fazer análises."
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = $"Erro ao fazer análise: {ex.Message}" });
            }
        }

        /// <summary>
        /// Faz análises para todos os pacientes de uma empresa
        /// </summary>
        /// <param name="empresa">Nome da empresa</param>
        /// <returns>Lista com resultados de análise para cada paciente</returns>
        [HttpGet("empresa")]
        [SwaggerOperation(
            Summary = "Análises por empresa",
            Description = "Analisa necessidades de acompanhamento para todos os pacientes de uma empresa",
            OperationId = "AnalisarAcompanhamentoByEmpresa",
            Tags = new[] { "AnaliseAcompanhamento" }
        )]
        public async Task<IActionResult> AnalisarAcompanhamentoByEmpresa([FromQuery] string empresa)
        {
            if (string.IsNullOrWhiteSpace(empresa))
            {
                return BadRequest(new { erro = "É necessário informar o nome da empresa." });
            }

            try
            {
                var pacientes = await _pacienteRepo.GetPacientesByEmpresaAsync(empresa);
                if (pacientes == null || !pacientes.Any())
                {
                    return NotFound(new { erro = $"Nenhum paciente encontrado para a empresa '{empresa}'." });
                }

                var consultas = await _consultaRepo.GetAllAsync();
                
                // Converter datas
                foreach (var paciente in pacientes)
                {
                    ConverterDataNascimentoPaciente(paciente);
                }
                
                foreach (var consulta in consultas)
                {
                    ConverterDatasConsulta(consulta);
                }

                var resultados = new List<object>();
                foreach (var paciente in pacientes)
                {
                    try
                    {
                        // Filtrar consultas deste paciente
                        var consultasPaciente = consultas.Where(c => c.IdPaciente == paciente.IdPaciente).ToList();
                        
                        if (consultasPaciente.Any())
                        {
                            var resultado = _servicoAnalise.AnalisarNecessidadeAcompanhamento(paciente, consultasPaciente);
                            resultados.Add(new
                            {
                                resultado.IdPaciente,
                                NomePaciente = paciente.NomeCompleto,
                                resultado.NecessitaAcompanhamento,
                                ProbabilidadePercentual = (int)(resultado.Score * 100),
                                resultado.Justificativa,
                                resultado.DetalhesConsultas,
                                resultado.NumConsultasNaoRealizadas,
                                resultado.NumConsultasAgendadasExpiradas,
                                resultado.DataUltimaConsulta,
                                DataAnalise = DateTime.Now
                            });
                        }
                        else
                        {
                            resultados.Add(new
                            {
                                IdPaciente = paciente.IdPaciente,
                                NomePaciente = paciente.NomeCompleto,
                                NecessitaAcompanhamento = false,
                                Justificativa = "O paciente não possui consultas registradas para análise."
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        resultados.Add(new
                        {
                            IdPaciente = paciente.IdPaciente,
                            NomePaciente = paciente.NomeCompleto,
                            Erro = $"Erro ao analisar paciente: {ex.Message}"
                        });
                    }
                }

                return Ok(new
                {
                    empresa = empresa,
                    numeroPacientes = pacientes.Count(),
                    resultados = resultados
                });
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new
                {
                    erro = "O modelo precisa ser treinado antes de realizar análises.",
                    dica = "Execute o endpoint /api/AnaliseAcompanhamento/treinar-modelo antes de fazer análises."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = $"Erro ao fazer análise: {ex.Message}" });
            }
        }

        /// <summary>
        /// Endpoint para análise de pacientes a partir de dados enviados diretamente.
        /// </summary>
        /// <param name="dadosJson">Dados do paciente e suas consultas para análise em formato JSON</param>
        /// <returns>Resultado da análise</returns>
        [HttpPost("analisar")]
        [SwaggerOperation(
            Summary = "Analisar necessidade de acompanhamento com dados fornecidos diretamente",
            Description = "Permite enviar dados de paciente e consultas para analisar necessidade de acompanhamento sem depender do banco de dados",
            OperationId = "AnalisarAcompanhamentoDadosExternos",
            Tags = new[] { "AnaliseAcompanhamento" }
        )]
        public IActionResult AnalisarDadosExternos([FromBody] JsonElement dadosJson)
        {
            try
            {
                // Verificar o formato JSON recebido e registrar para depuração
                var jsonRecebido = dadosJson.GetRawText();
                Console.WriteLine($"JSON recebido: {jsonRecebido}");
                
                // Verificar se os dados foram enviados no formato do frontend
                if (dadosJson.TryGetProperty("consultas", out var consultasElement) && 
                    consultasElement.ValueKind == JsonValueKind.Array)
                {
                    try 
                    {
                        // Usar o deserializador para converter diretamente para o modelo frontend
                        DadosAnaliseExternaFormatoFrontend dadosFrontend = JsonSerializer.Deserialize<DadosAnaliseExternaFormatoFrontend>(
                            jsonRecebido, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                            
                        if (dadosFrontend == null)
                        {
                            return BadRequest(new { erro = "Erro ao converter dados do frontend." });
                        }
                        
                        if (dadosFrontend.consultas == null || dadosFrontend.consultas.Count == 0)
                        {
                            return BadRequest(new { erro = "Lista de consultas é necessária para análise." });
                        }
                        
                        // Converter do formato do frontend para o formato da API
                        var paciente = new Paciente
                        {
                            IdPaciente = dadosFrontend.idPaciente,
                            NomeCompleto = dadosFrontend.nomeCompleto,
                            CPF = dadosFrontend.cpf?.Replace(".", "").Replace("-", ""), // Remover formatação do CPF
                            DataNascimento = ConverterDataNascimentoParaFormato(dadosFrontend.dataNascimento),
                            Email = dadosFrontend.email,
                            Telefone = dadosFrontend.telefone?.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", ""), // Remover formatação do telefone
                            Endereco = dadosFrontend.endereco,
                            PlanoOdontologico = dadosFrontend.planoOdontologico,
                            Empresa = dadosFrontend.empresa,
                            NumConsultas = dadosFrontend.numConsultas
                        };
                        
                        var consultas = new List<Consulta>();
                        foreach (var consultaFrontend in dadosFrontend.consultas)
                        {
                            if (consultaFrontend != null)
                            {
                                var consulta = new Consulta
                                {
                                    IdConsulta = consultaFrontend.idConsulta,
                                    IdPaciente = dadosFrontend.idPaciente,
                                    DataConsulta = ConverterDataConsultaParaFormato(consultaFrontend.dataConsulta),
                                    Status = consultaFrontend.status ?? "Agendada"
                                };
                                consultas.Add(consulta);
                            }
                        }
                        
                        // Garantir que temos pelo menos uma consulta válida
                        if (consultas.Count == 0)
                        {
                            return BadRequest(new { erro = "Nenhuma consulta válida encontrada na lista de consultas." });
                        }
                        
                        // Converter datas para DateTime
                        foreach (var consulta in consultas)
                        {
                            ConverterDatasConsulta(consulta);
                        }
                        ConverterDataNascimentoPaciente(paciente);
                        
                        // Realizar análise usando o modelo ML
                        try
                        {
                            var resultado = _servicoAnalise.AnalisarNecessidadeAcompanhamento(paciente, consultas);
                            
                            var resultadoFormatado = new
                            {
                                resultado.IdPaciente,
                                NomePaciente = paciente.NomeCompleto,
                                resultado.NecessitaAcompanhamento,
                                ProbabilidadePercentual = (int)(resultado.Score * 100),
                                resultado.Justificativa,
                                resultado.DetalhesConsultas,
                                resultado.NumConsultasNaoRealizadas,
                                resultado.NumConsultasAgendadasExpiradas,
                                resultado.DataUltimaConsulta,
                                DataAnalise = DateTime.Now
                            };
                            
                            return Ok(resultadoFormatado);
                        }
                        catch (InvalidOperationException)
                        {
                            // Tentar treinar o modelo automaticamente
                            var acuracia = _servicoAnalise.TreinarModelo(new List<Paciente> { paciente }, consultas);
                            
                            // Tentar novamente a análise
                            var resultado = _servicoAnalise.AnalisarNecessidadeAcompanhamento(paciente, consultas);
                            
                            var resultadoFormatado = new
                            {
                                resultado.IdPaciente,
                                NomePaciente = paciente.NomeCompleto,
                                resultado.NecessitaAcompanhamento,
                                ProbabilidadePercentual = (int)(resultado.Score * 100),
                                resultado.Justificativa,
                                resultado.DetalhesConsultas,
                                resultado.NumConsultasNaoRealizadas,
                                resultado.NumConsultasAgendadasExpiradas,
                                resultado.DataUltimaConsulta,
                                DataAnalise = DateTime.Now,
                                ModeloTreinado = new
                                {
                                    Acuracia = acuracia,
                                    AcuraciaPercentual = (int)(acuracia * 100)
                                }
                            };
                            
                            return Ok(resultadoFormatado);
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new { erro = "Erro ao processar dados no formato frontend.", detalhes = ex.Message, stack = ex.StackTrace });
                    }
                }
                // Formato original da API
                else 
                {
                    try 
                    {
                        // Converter o objeto JSON para DadosAnaliseExterna
                        var dadosString = dadosJson.GetRawText();
                        var dados = JsonSerializer.Deserialize<DadosAnaliseExterna>(dadosString, 
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        
                        if (dados == null || dados.Paciente == null)
                        {
                            return BadRequest(new { erro = "Dados do paciente são obrigatórios." });
                        }

                        if (dados.Consultas == null || !dados.Consultas.Any())
                        {
                            return Ok(new {
                                idPaciente = dados.Paciente.IdPaciente,
                                nomePaciente = dados.Paciente.NomeCompleto,
                                necessitaAcompanhamento = false,
                                justificativa = "O paciente não possui consultas para análise."
                            });
                        }

                        // Converter datas de texto para DateTime
                        foreach (var consulta in dados.Consultas)
                        {
                            ConverterDatasConsulta(consulta);
                        }
                        ConverterDataNascimentoPaciente(dados.Paciente);
                        
                        try
                        {
                            // Realizar análise usando o modelo ML
                            var resultado = _servicoAnalise.AnalisarNecessidadeAcompanhamento(dados.Paciente, dados.Consultas);
                            
                            var resultadoFormatado = new
                            {
                                resultado.IdPaciente,
                                NomePaciente = dados.Paciente.NomeCompleto,
                                resultado.NecessitaAcompanhamento,
                                ProbabilidadePercentual = (int)(resultado.Score * 100),
                                resultado.Justificativa,
                                resultado.DetalhesConsultas,
                                resultado.NumConsultasNaoRealizadas,
                                resultado.NumConsultasAgendadasExpiradas,
                                resultado.DataUltimaConsulta,
                                DataAnalise = DateTime.Now
                            };
                            
                            return Ok(resultadoFormatado);
                        }
                        catch (InvalidOperationException)
                        {
                            // Treinar o modelo com os dados fornecidos
                            var acuracia = _servicoAnalise.TreinarModelo(new List<Paciente> { dados.Paciente }, dados.Consultas);
                            
                            // Tentar novamente após treinar
                            var resultado = _servicoAnalise.AnalisarNecessidadeAcompanhamento(dados.Paciente, dados.Consultas);
                            
                            var resultadoFormatado = new
                            {
                                resultado.IdPaciente,
                                NomePaciente = dados.Paciente.NomeCompleto,
                                resultado.NecessitaAcompanhamento,
                                ProbabilidadePercentual = (int)(resultado.Score * 100),
                                resultado.Justificativa,
                                resultado.DetalhesConsultas,
                                resultado.NumConsultasNaoRealizadas,
                                resultado.NumConsultasAgendadasExpiradas,
                                resultado.DataUltimaConsulta,
                                DataAnalise = DateTime.Now,
                                ModeloTreinado = new
                                {
                                    Acuracia = acuracia,
                                    AcuraciaPercentual = (int)(acuracia * 100)
                                }
                            };
                            
                            return Ok(resultadoFormatado);
                        }
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(new { erro = "Erro ao processar dados no formato padrão.", detalhes = ex.Message, stack = ex.StackTrace });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Erro ao analisar dados.", detalhes = ex.Message, stack = ex.StackTrace });
            }
        }

        /// <summary>
        /// Converte a data de nascimento do formato "DD/MM/AAAA" para o formato "DDMMAAAA"
        /// </summary>
        private string ConverterDataNascimentoParaFormato(string dataNascimento)
        {
            if (string.IsNullOrEmpty(dataNascimento))
                return string.Empty;

            string dataLimpa = dataNascimento.Replace("/", "").Replace("-", "").Replace(".", "");
            
            // Se vier no formato AAAAMMDD
            if (dataLimpa.Length == 8 && int.TryParse(dataLimpa.Substring(0, 4), out int ano) && ano > 1900)
            {
                // Converter para DDMMAAAA
                return dataLimpa.Substring(6, 2) + dataLimpa.Substring(4, 2) + dataLimpa.Substring(0, 4);
            }
            
            return dataLimpa;
        }

        /// <summary>
        /// Converte a data da consulta do formato "DD/MM/AAAA HH:MM" para o formato "DDMMAAAAHHmm"
        /// </summary>
        private string ConverterDataConsultaParaFormato(string dataConsulta)
        {
            if (string.IsNullOrEmpty(dataConsulta))
                return string.Empty;

            try
            {
                // Formatos possíveis: DD/MM/AAAA HH:MM ou AAAA-MM-DD HH:MM:SS
                DateTime dataObj;
                if (DateTime.TryParse(dataConsulta, CultureInfo.GetCultureInfo("pt-BR"), DateTimeStyles.None, out dataObj) ||
                    DateTime.TryParse(dataConsulta, CultureInfo.InvariantCulture, DateTimeStyles.None, out dataObj))
                {
                    return dataObj.ToString("ddMMyyyyHHmm");
                }

                // Se não for possível analisar diretamente, remover caracteres especiais
                string dataLimpa = dataConsulta.Replace("/", "").Replace("-", "").Replace(":", "").Replace(" ", "").Replace(".", "");
                
                // Se o tamanho for 12, provavelmente já está no formato correto
                if (dataLimpa.Length == 12)
                    return dataLimpa;
                    
                // Se o tamanho for 8, completar com zeros para horário
                if (dataLimpa.Length == 8)
                    return dataLimpa + "0000";
                    
                return dataLimpa;
            }
            catch
            {
                // Em caso de erro, retornar data atual no formato correto
                return DateTime.Now.ToString("ddMMyyyyHHmm");
            }
        }

        /// <summary>
        /// Converte a data da consulta de string para DateTime
        /// </summary>
        private void ConverterDatasConsulta(Consulta consulta)
        {
            if (consulta.DataConsultaConvertida == null && !string.IsNullOrEmpty(consulta.DataConsulta))
            {
                try
                {
                    consulta.DataConsultaConvertida = DateTime.ParseExact(
                        consulta.DataConsulta,
                        "ddMMyyyyHHmm",
                        System.Globalization.CultureInfo.InvariantCulture);
                }
                catch
                {
                    // Se falhar no formato completo, tenta só com a data
                    if (consulta.DataConsulta.Length >= 8)
                    {
                        consulta.DataConsultaConvertida = DateTime.ParseExact(
                            consulta.DataConsulta.Substring(0, 8),
                            "ddMMyyyy",
                            System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
            }
        }
        
        /// <summary>
        /// Converte a data de nascimento do paciente de string para DateTime
        /// </summary>
        private void ConverterDataNascimentoPaciente(Paciente paciente)
        {
            if (paciente.DataNascimentoConvertida == null && !string.IsNullOrEmpty(paciente.DataNascimento))
            {
                try
                {
                    paciente.DataNascimentoConvertida = DateTime.ParseExact(
                        paciente.DataNascimento,
                        "ddMMyyyy",
                        System.Globalization.CultureInfo.InvariantCulture);
                }
                catch
                {
                    // Se falhar, tenta outros formatos comuns
                    try
                    {
                        paciente.DataNascimentoConvertida = DateTime.Parse(
                            paciente.DataNascimento,
                            System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        // Se ainda falhar, usa uma data padrão para não quebrar o processamento
                        paciente.DataNascimentoConvertida = new DateTime(1980, 1, 1);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Classe para receber dados externos para análise
    /// </summary>
    public class DadosAnaliseExterna
    {
        /// <summary>
        /// Dados do paciente para análise
        /// </summary>
        public Paciente Paciente { get; set; }
        
        /// <summary>
        /// Lista de consultas do paciente
        /// </summary>
        public List<Consulta> Consultas { get; set; }
    }

    /// <summary>
    /// Classe para receber dados no formato frontend
    /// </summary>
    public class DadosAnaliseExternaFormatoFrontend
    {
        public int idPaciente { get; set; }
        public string nomeCompleto { get; set; }
        public string cpf { get; set; }
        public string dataNascimento { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }
        public string endereco { get; set; }
        public string planoOdontologico { get; set; }
        public string empresa { get; set; }
        public int numConsultas { get; set; }
        public string gastoTotal { get; set; }
        public List<ConsultaFormatoFrontend> consultas { get; set; }
    }

    /// <summary>
    /// Formato de consulta para o frontend
    /// </summary>
    public class ConsultaFormatoFrontend
    {
        public int idConsulta { get; set; }
        public string dataConsulta { get; set; }
        public string status { get; set; }
        public ProcedimentoFormatoFrontend procedimento { get; set; }
    }

    /// <summary>
    /// Formato de procedimento para o frontend
    /// </summary>
    public class ProcedimentoFormatoFrontend
    {
        public int idProcedimento { get; set; }
        public string tipoProcedimento { get; set; }
        public string descricao { get; set; }
        public string custo { get; set; }
    }
} 