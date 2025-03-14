using Microsoft.AspNetCore.Mvc;
using SmartDentAPI.DTOs.Create;
using SmartDentAPI.DTOs.Update;
using SmartDentAPI.DTOs.Response;
using SmartDentAPI.Interfaces;
using SmartDentAPI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartDentAPI.Controllers
{
    /// <summary>
    /// Controller responsável por encaminhar as análises de IA.
    /// </summary>
    /// <remarks>
    /// Este controller fornece endpoints para enviar dados de pacientes (ou múltiplos) para a FastAPI, 
    /// que realiza a análise do risco odontológico. Os endpoints suportam a análise por paciente individual, 
    /// por empresa ou por cidade, além de um endpoint para análise de todos os pacientes.
    /// Também inclui métodos auxiliares para montar o payload, enviar a requisição para a FastAPI 
    /// e processar a resposta retornada.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class G_AIController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IPacienteRepository _pacienteRepo;
        private readonly IConsultaRepository _consultaRepo;
        private readonly IProcedimentoRepository _procedimentoRepo;
        private readonly IAlertaRepository _alertaRepo; // Repositório de alertas

        /// <summary>
        /// Construtor que injeta as dependências necessárias para o funcionamento da análise de IA.
        /// </summary>
        /// <param name="httpClientFactory">Fábrica para criar instâncias de HttpClient.</param>
        /// <param name="pacienteRepo">Repositório de pacientes.</param>
        /// <param name="consultaRepo">Repositório de consultas.</param>
        /// <param name="procedimentoRepo">Repositório de procedimentos.</param>
        /// <param name="alertaRepo">Repositório de alertas.</param>
        public G_AIController(
            IHttpClientFactory httpClientFactory,
            IPacienteRepository pacienteRepo,
            IConsultaRepository consultaRepo,
            IProcedimentoRepository procedimentoRepo,
            IAlertaRepository alertaRepo)
        {
            _httpClientFactory = httpClientFactory;
            _pacienteRepo = pacienteRepo;
            _consultaRepo = consultaRepo;
            _procedimentoRepo = procedimentoRepo;
            _alertaRepo = alertaRepo;
        }

        /// <summary>
        /// POST: api/ai/analisar-Uso-Json
        /// Recebe um payload JSON e o encaminha para a FastAPI para análise de risco.
        /// </summary>
        /// <remarks>
        /// Este endpoint é utilizado para enviar os dados (em formato JSON) para a FastAPI, 
        /// que realiza a análise e retorna o resultado. Após o processamento, a resposta é formatada 
        /// e retornada ao cliente.
        /// </remarks>
        /// <param name="payload">Objeto contendo os dados para análise.</param>
        /// <returns>Resultado processado da análise da FastAPI ou mensagem de erro.</returns>
        [HttpPost("analisar-Uso-Json")]
        public async Task<IActionResult> AnalisarUso([FromBody] object payload)
        {
            var client = _httpClientFactory.CreateClient();
            var fastApiUrl = "https://smartdent-ai.onrender.com/analisar-uso";

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(fastApiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var resultadoProcessado = await ProcessarERetornarResultado(responseContent);
                return Ok(resultadoProcessado);
            }
            else
            {
                return StatusCode((int)response.StatusCode, new
                {
                    error = "Erro ao chamar a FastAPI",
                    details = await response.Content.ReadAsStringAsync()
                });
            }
        }

        /// <summary>
        /// GET: api/ai/analisar-Paciente/{idPaciente}
        /// Busca a ficha completa de um paciente pelo ID e envia para a FastAPI para análise.
        /// </summary>
        /// <param name="idPaciente">ID do paciente a ser analisado.</param>
        /// <returns>Resultado da análise do paciente.</returns>
        [HttpGet("analisar-Paciente/{idPaciente}")]
        public async Task<IActionResult> AnalisarPaciente(int idPaciente)
        {
            var paciente = await _pacienteRepo.GetByIdAsync(idPaciente);
            if (paciente == null)
                return NotFound(new { error = "Paciente não encontrado." });

            var payload = await MontarPayloadPaciente(paciente);
            return await EnviarParaFastApi(payload);
        }

        /// <summary>
        /// GET: api/ai/analisar-empresa/individual
        /// Busca a ficha completa de todos os pacientes da empresa "Individual" e envia para a FastAPI.
        /// </summary>
        /// <returns>Resultado da análise para os pacientes da empresa "Individual".</returns>
        [HttpGet("analisar-PacientePlanoIndividual")]
        public async Task<IActionResult> AnalisarEmpresaIndividual()
        {
            var pacientes = await _pacienteRepo.GetPacientesByEmpresaAsync("Individual");
            if (pacientes == null || !pacientes.Any())
                return NotFound(new { error = "Nenhum paciente encontrado para a empresa 'Individual'." });

            var payloadList = new List<object>();
            foreach (var paciente in pacientes)
            {
                var payloadPaciente = await MontarPayloadPaciente(paciente);
                payloadList.Add(payloadPaciente);
            }

            return await EnviarParaFastApi(payloadList);
        }

        /// <summary>
        /// GET: api/ai/analisar-empresa?empresa=NomeDaEmpresa
        /// Busca a ficha completa de todos os pacientes de uma empresa especificada e envia para a FastAPI.
        /// </summary>
        /// <param name="empresa">Nome da empresa.</param>
        /// <returns>Resultado da análise para os pacientes da empresa informada.</returns>
        [HttpGet("analisar-PacientesByEmpresa")]
        public async Task<IActionResult> AnalisarEmpresa([FromQuery] string empresa)
        {
            if (string.IsNullOrWhiteSpace(empresa))
                return BadRequest(new { error = "O parâmetro 'empresa' é obrigatório." });

            var pacientes = await _pacienteRepo.GetPacientesByEmpresaAsync(empresa);
            if (pacientes == null || !pacientes.Any())
                return NotFound(new { error = $"Nenhum paciente encontrado para a empresa '{empresa}'." });

            var payloadList = new List<object>();
            foreach (var paciente in pacientes)
            {
                var payloadPaciente = await MontarPayloadPaciente(paciente);
                payloadList.Add(payloadPaciente);
            }

            return await EnviarParaFastApi(payloadList);
        }

        /// <summary>
        /// GET: api/ai/analisar-cidade?cidade=NomeDaCidade
        /// Busca a ficha completa de todos os pacientes cuja cidade (extraída do campo Endereco) contenha o nome informado e envia para a FastAPI.
        /// </summary>
        /// <param name="cidade">Nome da cidade a ser filtrada.</param>
        /// <returns>Resultado da análise para os pacientes da cidade informada.</returns>
        [HttpGet("analisar-PacientesByCidade")]
        public async Task<IActionResult> AnalisarCidade([FromQuery] string cidade)
        {
            if (string.IsNullOrWhiteSpace(cidade))
                return BadRequest(new { error = "O parâmetro 'cidade' é obrigatório." });

            var todosPacientes = await _pacienteRepo.GetAllAsync();
            var pacientesFiltrados = todosPacientes
                .Where(p => !string.IsNullOrEmpty(p.Endereco) &&
                            p.Endereco.IndexOf(cidade, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            if (!pacientesFiltrados.Any())
                return NotFound(new { error = $"Nenhum paciente encontrado para a cidade '{cidade}'." });

            var payloadList = new List<object>();
            foreach (var paciente in pacientesFiltrados)
            {
                var payloadPaciente = await MontarPayloadPaciente(paciente);
                payloadList.Add(payloadPaciente);
            }

            return await EnviarParaFastApi(payloadList);
        }

        /// <summary>
        /// GET: api/ai/analisar-todos
        /// Busca a ficha completa de todos os pacientes e envia para a FastAPI para análise.
        /// </summary>
        /// <returns>Resultado da análise para todos os pacientes cadastrados.</returns>
        [HttpGet("analisar-TodosPaciente")]
        public async Task<IActionResult> AnalisarTodos()
        {
            var pacientes = await _pacienteRepo.GetAllAsync();
            if (pacientes == null || !pacientes.Any())
                return NotFound(new { error = "Nenhum paciente encontrado." });

            var payloadList = new List<object>();
            foreach (var paciente in pacientes)
            {
                var payloadPaciente = await MontarPayloadPaciente(paciente);
                payloadList.Add(payloadPaciente);
            }

            return await EnviarParaFastApi(payloadList);
        }

        /// <summary>
        /// Monta o payload para um paciente de acordo com o formato esperado pela FastAPI.
        /// </summary>
        /// <param name="paciente">Objeto Paciente para o qual o payload será montado.</param>
        /// <returns>Um objeto anônimo contendo os dados do paciente e suas consultas formatadas.</returns>
        private async Task<object> MontarPayloadPaciente(Paciente paciente)
        {
            var consultas = (await _consultaRepo.GetByPacienteIdAsync(paciente.IdPaciente)).ToList();

            // Conta apenas as consultas com status "Realizada"
            int consultasRealizadasCount = consultas.Count(c => c.Status == "Realizada");

            var consultasJson = new List<object>();

            foreach (var consulta in consultas)
            {
                DateTime dataConsulta;
                if (consulta.DataConsultaConvertida.HasValue)
                    dataConsulta = consulta.DataConsultaConvertida.Value;
                else if (DateTime.TryParseExact(
                             consulta.DataConsulta,
                             "ddMMyyyyHHmm",
                             CultureInfo.InvariantCulture,
                             DateTimeStyles.None,
                             out DateTime dt))
                {
                    dataConsulta = dt;
                }
                else
                {
                    continue;
                }

                var procedimentos = (await _procedimentoRepo.GetByConsultaIdAsync(consulta.IdConsulta)).ToList();
                object procedimentoJson = null;
                if (procedimentos.Any())
                {
                    var proc = procedimentos.First();
                    procedimentoJson = new
                    {
                        idProcedimento = proc.IdProcedimento,
                        tipoProcedimento = proc.TipoProcedimento,
                        descricao = proc.Descricao,
                        custo = proc.Custo.ToString("F2", CultureInfo.InvariantCulture)
                    };
                }

                consultasJson.Add(new
                {
                    idConsulta = consulta.IdConsulta,
                    dataConsulta = dataConsulta.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                    status = consulta.Status,
                    procedimento = procedimentoJson
                });
            }

            // Calcula o gasto total considerando apenas as consultas com status "Realizada"
            decimal gastoTotal = 0;
            foreach (var c in consultas)
            {
                if (c.Status == "Realizada")
                {
                    var procs = await _procedimentoRepo.GetByConsultaIdAsync(c.IdConsulta);
                    gastoTotal += procs.Sum(p => p.Custo); // NÃO dividir por 100
                }
            }

            var payload = new
            {
                idPaciente = paciente.IdPaciente,
                nomeCompleto = paciente.NomeCompleto,
                cpf = paciente.CPF,
                dataNascimento = paciente.DataNascimento,
                empresa = paciente.Empresa,
                email = paciente.Email,
                telefone = paciente.Telefone,
                endereco = paciente.Endereco,
                planoOdontologico = paciente.PlanoOdontologico,
                // Agora, numConsultas reflete somente as consultas realizadas
                numConsultas = consultasRealizadasCount,
                gastoTotal = gastoTotal.ToString("C", new CultureInfo("pt-BR")),
                consultas = consultasJson
            };

            return payload;
        }

        /// <summary>
        /// Envia o payload para a FastAPI, processa a resposta e retorna o resultado.
        /// </summary>
        /// <param name="payload">O objeto ou lista de objetos contendo os dados do(s) paciente(s).</param>
        /// <returns>O resultado processado da análise da FastAPI ou um erro, se ocorrer.</returns>
        private async Task<IActionResult> EnviarParaFastApi(object payload)
        {
            var client = _httpClientFactory.CreateClient();
            var fastApiUrl = "https://smartdent-ai.onrender.com/analisar-uso";
            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var responseFast = await client.PostAsync(fastApiUrl, content);
            if (responseFast.IsSuccessStatusCode)
            {
                var responseContent = await responseFast.Content.ReadAsStringAsync();
                var resultadoProcessado = await ProcessarERetornarResultado(responseContent);
                return Ok(resultadoProcessado);
            }
            else
            {
                return StatusCode((int)responseFast.StatusCode, new
                {
                    error = "Erro ao chamar a FastAPI",
                    details = await responseFast.Content.ReadAsStringAsync()
                });
            }
        }

        /// <summary>
        /// Processa a resposta da FastAPI, formata os campos necessários e salva ou atualiza alertas conforme a análise.
        /// </summary>
        /// <param name="responseContent">A resposta em JSON da FastAPI.</param>
        /// <returns>O resultado processado, podendo ser um objeto único ou uma lista de objetos.</returns>
        private async Task<object> ProcessarERetornarResultado(string responseContent)
        {
            // Primeiro, tenta desserializar como objeto único
            try
            {
                var resultado = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
                if (resultado != null)
                {
                    FormatarResultado(resultado);
                    await SalvarOuAtualizarAlerta(resultado);
                    return resultado;
                }
            }
            catch
            {
                // Se não conseguir como objeto único, tenta como lista
                var listaResultado = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(responseContent);
                if (listaResultado != null)
                {
                    foreach (var item in listaResultado)
                    {
                        FormatarResultado(item);
                        await SalvarOuAtualizarAlerta(item);
                    }

                    return listaResultado;
                }
            }

            return responseContent;
        }

        /// <summary>
        /// Formata os campos dataAnalise, gastoTotal e confiança (ou confianca) no resultado da análise.
        /// </summary>
        /// <param name="resultado">Dicionário contendo os resultados a serem formatados.</param>
        private void FormatarResultado(Dictionary<string, object> resultado)
        {
            if (resultado.ContainsKey("dataAnalise"))
            {
                if (DateTime.TryParseExact(resultado["dataAnalise"].ToString(), "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                {
                    resultado["dataAnalise"] = dt.ToString("dd/MM/yyyy HH:mm");
                }
            }

            if (resultado.ContainsKey("gastoTotal"))
            {
                if (decimal.TryParse(resultado["gastoTotal"].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture,
                        out decimal valor))
                {
                    resultado["gastoTotal"] = valor.ToString("C", new CultureInfo("pt-BR"));
                }
            }

            if (resultado.ContainsKey("confidence"))
            {
                if (decimal.TryParse(resultado["confidence"].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture,
                        out decimal confValue))
                {
                    resultado.Remove("confidence");
                    resultado["confiança"] = confValue.ToString("F2") + "%";
                }
            }
            else if (resultado.ContainsKey("confianca"))
            {
                if (decimal.TryParse(resultado["confianca"].ToString(), NumberStyles.Any, CultureInfo.InvariantCulture,
                        out decimal confValue))
                {
                    resultado.Remove("confianca");
                    resultado["confiança"] = confValue.ToString("F2") + "%";
                }
            }
        }

        /// <summary>
        /// Verifica se já existe um alerta para o mesmo paciente e tipo de alerta e, se existir, atualiza-o;
        /// caso contrário, insere um novo alerta.
        /// </summary>
        /// <param name="dados">Dicionário contendo os dados da análise retornada pela FastAPI.</param>
        private async Task SalvarOuAtualizarAlerta(Dictionary<string, object> dados)
        {
            // Verifica se os campos necessários estão presentes
            if (!dados.ContainsKey("idPaciente") || !dados.ContainsKey("tipoAlerta"))
                return;

            if (!int.TryParse(dados["idPaciente"].ToString(), out int idPaciente))
                return;

            string tipoAlerta = dados["tipoAlerta"].ToString();

            // Verifica se o tipo de alerta contém "Excessivo" ou é "Uso Moderado com Tendência a Excesso"
            if (!(tipoAlerta.IndexOf("Excessivo", StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                  tipoAlerta.Equals("Uso Moderado com Tendência a Excesso", StringComparison.InvariantCultureIgnoreCase)))
            {
                return;
            }

            string grauRisco = dados.ContainsKey("grauRisco") ? dados["grauRisco"].ToString() : "";
            string justificativa = dados.ContainsKey("justificativa") ? dados["justificativa"].ToString() : "";

            // Verifica se já existe um alerta para este paciente com o mesmo tipo
            var alertasExistentes = await _alertaRepo.GetByPacienteIdAsync(idPaciente);
            var alertaExistente = alertasExistentes.FirstOrDefault(a =>
                a.TipoAlerta.Equals(tipoAlerta, StringComparison.InvariantCultureIgnoreCase));

            if (alertaExistente != null)
            {
                // Atualiza os campos desejados e a data de geração
                alertaExistente.GrauRisco = grauRisco;
                alertaExistente.Justificativa = justificativa;
                alertaExistente.DataGeracao = DateTime.UtcNow;
                await _alertaRepo.UpdateAsync(alertaExistente);
            }
            else
            {
                // Cria um novo alerta
                var novoAlerta = new Alerta
                {
                    IdPaciente = idPaciente,
                    TipoAlerta = tipoAlerta,
                    GrauRisco = grauRisco,
                    Justificativa = justificativa,
                    DataGeracao = DateTime.UtcNow
                };
                await _alertaRepo.AddAsync(novoAlerta);
            }
        }
    }
}
