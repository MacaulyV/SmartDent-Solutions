using Microsoft.AspNetCore.Mvc;
using SmartDentAPI.DTOs.Create;
using SmartDentAPI.DTOs.Response;
using SmartDentAPI.DTOs.Update;
using SmartDentAPI.Interfaces;
using SmartDentAPI.Models;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class A_PacientesController : ControllerBase
    {
        private readonly IPacienteRepository _pacienteRepo;
        private readonly IConsultaRepository _consultaRepo;
        private readonly IProcedimentoRepository _procedimentoRepo;
        private readonly IAlertaRepository _alertaRepo;
        
        // Dicionário de planos odontológicos permitidos, categorizados
        private readonly Dictionary<string, List<string>> _planosPermitidos = new Dictionary<string, List<string>>
        {
            { "Individuais", new List<string>
                {
                    "Dental Júnior",          // Crianças de 0 a 7 anos
                    "Bem-Estar",              // Básico, cobre tratamentos essenciais
                    "Bem-Estar White",        // Adiciona clareamento dental com gel
                    "Bem-Estar Pró",          // Cobre próteses removíveis
                    "Bem-Estar Orto",         // Cobre tratamento ortodôntico (aparelho fixo)
                    "Bem-Estar Orto White"    // Ortodontia + clareamento (mais completo)
                }
            },
            { "Empresariais", new List<string>
                {
                    "Convencional",           // Básico, cobre apenas tratamentos essenciais
                    "Integral",               // Adiciona consulta para clareamento
                    "Integral Plus",          // Adiciona placa de mordida e placa de clareamento
                    "Integral Doc",           // Adiciona documentação ortodôntica completa
                    "Integral Doc Plus",      // Integral Doc + placa de mordida e clareamento
                    "Premium",                // Adiciona tratamento ortodôntico completo
                    "Superior",               // Adiciona próteses removíveis e próteses em cerâmica/resina
                    "Classical",              // Adiciona próteses removíveis completas (dentaduras e parciais)
                    "Classical Doc",          // Classical + documentação ortodôntica completa
                    "Ômega",                  // Classical + placa de mordida
                    "Master",                 // Superior + documentação ortodôntica + ortodontia
                    "Maximum White"           // Mais completo: ortodontia, clareamento, próteses avançadas, tomografia
                }
            }
        };

        /// <summary>
        /// Construtor que injeta as dependências dos repositórios.
        /// </summary>
        /// <param name="pacienteRepo">Repositório de pacientes.</param>
        /// <param name="consultaRepo">Repositório de consultas.</param>
        /// <param name="procedimentoRepo">Repositório de procedimentos.</param>
        /// <param name="alertaRepo">Repositório de alertas.</param>
        public A_PacientesController(
            IPacienteRepository pacienteRepo,
            IConsultaRepository consultaRepo,
            IProcedimentoRepository procedimentoRepo,
            IAlertaRepository alertaRepo)
        {
            _pacienteRepo = pacienteRepo;
            _consultaRepo = consultaRepo;
            _procedimentoRepo = procedimentoRepo;
            _alertaRepo = alertaRepo;
        }

        /// <summary>
        /// Método auxiliar para formatação de valores em moeda brasileira.
        /// </summary>
        /// <param name="valor">Valor decimal a ser formatado.</param>
        /// <returns>String formatada como moeda.</returns>
        private string FormatCurrency(decimal valor)
        {
            return valor.ToString("C", CultureInfo.GetCultureInfo("pt-BR"));
        }

        /// <summary>
        /// Cria um novo paciente.
        /// </summary>
        /// <param name="dto">Dados do paciente para criação.</param>
        /// <returns>ActionResult com o paciente criado ou mensagem de erro.</returns>
        [HttpPost]
        public async Task<IActionResult> CreatePaciente([FromBody] PacienteCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_planosPermitidos.Values.Any(lista => lista.Contains(dto.PlanoOdontologico)))
            {
                return BadRequest(new { error = "Plano odontológico inválido. Utilize um dos valores permitidos." });
            }

            var paciente = new Paciente
            {
                NomeCompleto = dto.NomeCompleto,
                CPF = dto.CPF,
                DataNascimento = dto.DataNascimento,
                Email = dto.Email,
                Telefone = dto.Telefone,
                Endereco = dto.Endereco,
                PlanoOdontologico = dto.PlanoOdontologico,
                Empresa = string.IsNullOrWhiteSpace(dto.Empresa) ? "Individual" : dto.Empresa,
                NumConsultas = 0
            };

            try
            {
                await _pacienteRepo.AddAsync(paciente);

                var response = new PacienteResponseDTO
                {
                    IdPaciente = paciente.IdPaciente,
                    NomeCompleto = paciente.NomeCompleto,
                    CPF = FormatCPF(paciente.CPF),
                    PlanoOdontologico = paciente.PlanoOdontologico,
                    Empresa = paciente.Empresa,
                    NumConsultas = paciente.NumConsultas,
                    GastoTotal = FormatCurrency(0) // Novo paciente inicia com gasto zero
                };

                return CreatedAtAction(nameof(GetPacienteById), new { id = response.IdPaciente }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        
        /// <summary>
        /// Obtém a ficha dos pacientes individuais, com dados detalhados de consultas e procedimentos.
        /// </summary>
        /// <returns>Lista de fichas dos pacientes individuais ou erro em caso de falha.</returns>
        [HttpGet("GetFichaPacientesByPlanoIndividual")]
        public async Task<IActionResult> GetFichaPacientesIndividuais()
        {
            try
            {
                // Busca os pacientes cujo campo Empresa é "Individual"
                var pacientes = await _pacienteRepo.GetPacientesByEmpresaAsync("Individual");

                if (pacientes == null || !pacientes.Any())
                {
                    return NotFound(new { error = "Nenhum paciente com plano Individual encontrado." });
                }

                var response = new List<FichaPacienteResponseDTO>();

                foreach (var p in pacientes)
                {
                    decimal gastoTotal = 0;
                    // Recupera as consultas do paciente
                    var consultas = await _consultaRepo.GetByPacienteIdAsync(p.IdPaciente);
                    var consultasDTO = new List<FichaPacienteResponseDTO.ConsultaData>();

                    foreach (var c in consultas)
                    {
                        // Formata a data da consulta
                        string dataFormatada = "";
                        if (c.DataConsultaConvertida.HasValue)
                        {
                            dataFormatada = c.DataConsultaConvertida.Value.ToString("dd/MM/yyyy HH:mm");
                        }
                        else if (DateTime.TryParseExact(c.DataConsulta, "ddMMyyyyHHmm",
                                     CultureInfo.InvariantCulture,
                                     DateTimeStyles.None, out DateTime dataConvertida))
                        {
                            dataFormatada = dataConvertida.ToString("dd/MM/yyyy HH:mm");
                        }
                        else
                        {
                            dataFormatada = "Data inválida";
                        }

                        // Recupera os procedimentos associados à consulta (primeiro procedimento encontrado)
                        var procList = await _procedimentoRepo.GetByConsultaIdAsync(c.IdConsulta);
                        FichaPacienteResponseDTO.ProcedimentoData procData = null;
                        if (procList.Any())
                        {
                            var proc = procList.First();
                            procData = new FichaPacienteResponseDTO.ProcedimentoData
                            {
                                IdProcedimento = proc.IdProcedimento,
                                TipoProcedimento = proc.TipoProcedimento,
                                Descricao = proc.Descricao,
                                Custo = FormatCurrency(proc.Custo)
                            };
                            // Só soma se a consulta tiver o status "Realizada"
                            if (c.Status.Equals("Realizada", StringComparison.OrdinalIgnoreCase))
                            {
                                gastoTotal += proc.Custo;
                            }
                        }

                        consultasDTO.Add(new FichaPacienteResponseDTO.ConsultaData
                        {
                            IdConsulta = c.IdConsulta,
                            DataConsulta = dataFormatada,
                            Status = c.Status,
                            Procedimento = procData
                        });
                    }

                    response.Add(new FichaPacienteResponseDTO
                    {
                        IdPaciente = p.IdPaciente,
                        NomeCompleto = p.NomeCompleto,
                        CPF = FormatCPF(p.CPF),
                        DataNascimento = DateTime.ParseExact(p.DataNascimento, "ddMMyyyy", CultureInfo.InvariantCulture)
                                            .ToString("dd/MM/yyyy"),
                        Email = p.Email,
                        Telefone = FormatTelefone(p.Telefone),
                        Endereco = p.Endereco,
                        PlanoOdontologico = p.PlanoOdontologico,
                        Empresa = p.Empresa,
                        NumConsultas = p.NumConsultas,
                        GastoTotal = FormatCurrency(gastoTotal),
                        Consultas = consultasDTO
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocorreu um erro ao buscar as fichas dos pacientes.", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtém a lista de pacientes para uma empresa específica.
        /// </summary>
        /// <param name="empresa">Nome da empresa.</param>
        /// <returns>Lista de pacientes ou erro se nenhum paciente for encontrado.</returns>
        [HttpGet("GetPacientesByEmpresa/{empresa}")]
        public async Task<IActionResult> GetPacientesByEmpresa(string empresa)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(empresa))
                {
                    return BadRequest(new { error = "O nome da empresa deve ser informado." });
                }

                var pacientes = await _pacienteRepo.GetPacientesByEmpresaAsync(empresa);

                if (pacientes == null || !pacientes.Any())
                {
                    return NotFound(new { error = "Nenhum paciente encontrado para a empresa informada." });
                }

                var response = new List<PacienteResponseDTO>();

                foreach (var p in pacientes)
                {
                    decimal gastoTotal = 0;
                    var consultas = await _consultaRepo.GetByPacienteIdAsync(p.IdPaciente);
                    foreach (var c in consultas)
                    {
                        // Somente consulta realizada contribui para o gasto total
                        if (c.Status.Equals("Realizada", StringComparison.OrdinalIgnoreCase))
                        {
                            var procedimentos = await _procedimentoRepo.GetByConsultaIdAsync(c.IdConsulta);
                            gastoTotal += procedimentos.Sum(proc => proc.Custo);
                        }
                    }

                    response.Add(new PacienteResponseDTO
                    {
                        IdPaciente = p.IdPaciente,
                        NomeCompleto = p.NomeCompleto,
                        CPF = FormatCPF(p.CPF),
                        PlanoOdontologico = p.PlanoOdontologico,
                        Empresa = p.Empresa,
                        NumConsultas = p.NumConsultas,
                        GastoTotal = FormatCurrency(gastoTotal)
                    });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocorreu um erro ao buscar os pacientes.", details = ex.Message });
            }
        }
        
        /// <summary>
        /// Obtém a ficha detalhada dos pacientes de uma empresa específica.
        /// </summary>
        /// <param name="empresa">Nome da empresa.</param>
        /// <returns>Ficha dos pacientes com detalhes de consultas e procedimentos.</returns>
        [HttpGet("GetFichaPacientesByEmpresa/{empresa}")]
        public async Task<IActionResult> GetFichaPacientesByEmpresa(string empresa)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(empresa))
                {
                    return BadRequest(new { error = "O nome da empresa deve ser informado." });
                }
                
                var pacientes = await _pacienteRepo.GetPacientesByEmpresaAsync(empresa);

                if (pacientes == null || !pacientes.Any())
                {
                    return NotFound(new { error = "Nenhum paciente encontrado para a empresa informada." });
                }
                
                var response = new List<FichaPacienteResponseDTO>();

                foreach (var p in pacientes)
                {
                    decimal gastoTotal = 0;
                    var consultas = await _consultaRepo.GetByPacienteIdAsync(p.IdPaciente);
                    var consultasDTO = new List<FichaPacienteResponseDTO.ConsultaData>();

                    foreach (var c in consultas)
                    {
                        string dataFormatada = "";
                        if (c.DataConsultaConvertida.HasValue)
                        {
                            dataFormatada = c.DataConsultaConvertida.Value.ToString("dd/MM/yyyy HH:mm");
                        }
                        else if (DateTime.TryParseExact(c.DataConsulta, "ddMMyyyyHHmm",
                                     CultureInfo.InvariantCulture,
                                     DateTimeStyles.None, out DateTime dataConvertida))
                        {
                            dataFormatada = dataConvertida.ToString("dd/MM/yyyy HH:mm");
                        }
                        else
                        {
                            dataFormatada = "Data inválida";
                        }

                        var procList = await _procedimentoRepo.GetByConsultaIdAsync(c.IdConsulta);
                        FichaPacienteResponseDTO.ProcedimentoData procData = null;
                        if (procList.Any())
                        {
                            var proc = procList.First();
                            procData = new FichaPacienteResponseDTO.ProcedimentoData
                            {
                                IdProcedimento = proc.IdProcedimento,
                                TipoProcedimento = proc.TipoProcedimento,
                                Descricao = proc.Descricao,
                                Custo = FormatCurrency(proc.Custo)
                            };
                            // Só soma se a consulta tiver o status "Realizada"
                            if (c.Status.Equals("Realizada", StringComparison.OrdinalIgnoreCase))
                            {
                                gastoTotal += proc.Custo;
                            }
                        }

                        consultasDTO.Add(new FichaPacienteResponseDTO.ConsultaData
                        {
                            IdConsulta = c.IdConsulta,
                            DataConsulta = dataFormatada,
                            Status = c.Status,
                            Procedimento = procData
                        });
                    }

                    response.Add(new FichaPacienteResponseDTO
                    {
                        IdPaciente = p.IdPaciente,
                        NomeCompleto = p.NomeCompleto,
                        CPF = FormatCPF(p.CPF),
                        DataNascimento = DateTime.ParseExact(p.DataNascimento, "ddMMyyyy", CultureInfo.InvariantCulture)
                                            .ToString("dd/MM/yyyy"),
                        Email = p.Email,
                        Telefone = FormatTelefone(p.Telefone),
                        Endereco = p.Endereco,
                        PlanoOdontologico = p.PlanoOdontologico,
                        Empresa = p.Empresa,
                        NumConsultas = p.NumConsultas,
                        GastoTotal = FormatCurrency(gastoTotal),
                        Consultas = consultasDTO
                    });

                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocorreu um erro ao buscar as fichas dos pacientes.", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtém a ficha detalhada de um paciente específico.
        /// </summary>
        /// <param name="id">Identificador do paciente.</param>
        /// <returns>Ficha detalhada do paciente ou erro se não encontrado.</returns>
        [HttpGet("FichaPaciente/{id}")]
        public async Task<IActionResult> GetFichaPaciente(int id)
        {
            var paciente = await _pacienteRepo.GetByIdAsync(id);
            if (paciente == null)
                return NotFound(new { error = "Paciente não encontrado." });

            var consultas = await _consultaRepo.GetByPacienteIdAsync(id);
            decimal gastoTotal = 0;

            var response = new FichaPacienteResponseDTO
            {
                IdPaciente = paciente.IdPaciente,
                NomeCompleto = paciente.NomeCompleto,
                CPF = FormatCPF(paciente.CPF),
                DataNascimento = DateTime.ParseExact(paciente.DataNascimento, "ddMMyyyy", CultureInfo.InvariantCulture)
                                    .ToString("dd/MM/yyyy"),
                Email = paciente.Email,
                Telefone = FormatTelefone(paciente.Telefone),
                Endereco = paciente.Endereco,
                PlanoOdontologico = paciente.PlanoOdontologico,
                Empresa = paciente.Empresa,
                NumConsultas = paciente.NumConsultas,
                GastoTotal = FormatCurrency(0),
                Consultas = new List<FichaPacienteResponseDTO.ConsultaData>()
            };

            foreach (var c in consultas)
            {
                string dataFormatada = "";
                if (c.DataConsultaConvertida.HasValue)
                {
                    dataFormatada = c.DataConsultaConvertida.Value.ToString("dd/MM/yyyy HH:mm");
                }
                else if (DateTime.TryParseExact(c.DataConsulta, "ddMMyyyyHHmm",
                             CultureInfo.InvariantCulture,
                             DateTimeStyles.None, out DateTime dataConvertida))
                {
                    dataFormatada = dataConvertida.ToString("dd/MM/yyyy HH:mm");
                }
                else
                {
                    dataFormatada = "Data inválida";
                }

                var procList = await _procedimentoRepo.GetByConsultaIdAsync(c.IdConsulta);
                var proc = procList.FirstOrDefault();

                FichaPacienteResponseDTO.ProcedimentoData procData = null;
                if (proc != null)
                {
                    procData = new FichaPacienteResponseDTO.ProcedimentoData
                    {
                        IdProcedimento = proc.IdProcedimento,
                        TipoProcedimento = proc.TipoProcedimento,
                        Descricao = proc.Descricao,
                        Custo = FormatCurrency(proc.Custo)
                    };
                    // Soma o custo somente se o status da consulta for "Realizada"
                    if (c.Status.Equals("Realizada", StringComparison.OrdinalIgnoreCase))
                    {
                        gastoTotal += proc.Custo;
                    }
                }

                var consultaData = new FichaPacienteResponseDTO.ConsultaData
                {
                    IdConsulta = c.IdConsulta,
                    DataConsulta = dataFormatada,
                    Status = c.Status,
                    Procedimento = procData
                };

                response.Consultas.Add(consultaData);
            }
            response.GastoTotal = FormatCurrency(gastoTotal);

            return Ok(response);
        }

        /// <summary>
        /// Obtém a lista de pacientes individuais.
        /// </summary>
        /// <returns>Lista de pacientes individuais ou erro em caso de falha.</returns>
        [HttpGet("GetPacientesByPlanoIndividual")]
        public async Task<IActionResult> GetPacientesIndividuais()
        {
            var pacientes = await _pacienteRepo.GetAllAsync();
            var response = new List<PacienteResponseDTO>();

            foreach (var paciente in pacientes.Where(p => p.Empresa == "Individual"))
            {
                decimal gastoTotal = 0;
                var consultas = await _consultaRepo.GetByPacienteIdAsync(paciente.IdPaciente);
                foreach (var c in consultas)
                {
                    // Só soma se a consulta tiver o status "Realizada"
                    if (c.Status.Equals("Realizada", StringComparison.OrdinalIgnoreCase))
                    {
                        var procedimentos = await _procedimentoRepo.GetByConsultaIdAsync(c.IdConsulta);
                        gastoTotal += procedimentos.Sum(p => p.Custo);
                    }
                }

                response.Add(new PacienteResponseDTO
                {
                    IdPaciente = paciente.IdPaciente,
                    NomeCompleto = paciente.NomeCompleto,
                    CPF = FormatCPF(paciente.CPF),
                    PlanoOdontologico = paciente.PlanoOdontologico,
                    Empresa = paciente.Empresa,
                    NumConsultas = paciente.NumConsultas,
                    GastoTotal = FormatCurrency(gastoTotal)
                });
            }

            return Ok(response);
        }

        /// <summary>
        /// Obtém os dados de um paciente específico pelo seu identificador.
        /// </summary>
        /// <param name="id">Identificador do paciente.</param>
        /// <returns>Dados do paciente ou erro se não encontrado.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPacienteById(int id)
        {
            var paciente = await _pacienteRepo.GetByIdAsync(id);
            if (paciente == null)
                return NotFound(new { error = "Paciente não encontrado." });

            decimal gastoTotal = 0;
            var consultas = await _consultaRepo.GetByPacienteIdAsync(paciente.IdPaciente);
            foreach (var c in consultas)
            {
                // Soma o custo somente se a consulta tiver o status "Realizada"
                if (c.Status.Equals("Realizada", StringComparison.OrdinalIgnoreCase))
                {
                    var procedimentos = await _procedimentoRepo.GetByConsultaIdAsync(c.IdConsulta);
                    gastoTotal += procedimentos.Sum(p => p.Custo);
                }
            }

            var response = new PacienteResponseDTO
            {
                IdPaciente = paciente.IdPaciente,
                NomeCompleto = paciente.NomeCompleto,
                CPF = FormatCPF(paciente.CPF),
                PlanoOdontologico = paciente.PlanoOdontologico,
                Empresa = paciente.Empresa,
                NumConsultas = paciente.NumConsultas,
                GastoTotal = FormatCurrency(gastoTotal)
            };

            return Ok(response);
        }

        /// <summary>
        /// Busca os dados de um paciente para edição.
        /// </summary>
        /// <param name="id">Identificador do paciente.</param>
        /// <returns>Dados do paciente para edição ou erro se não encontrado.</returns>
        [HttpGet("BuscarPacienteEdit/{id}")]
        public async Task<IActionResult> BuscarPacienteEdit(int id)
        {
            var paciente = await _pacienteRepo.GetByIdAsync(id);
            if (paciente == null)
                return NotFound(new { error = "Paciente não encontrado." });

            var response = new PacienteEditResponseDTO
            {
                NomeCompleto = paciente.NomeCompleto,
                Email = paciente.Email,
                Telefone = FormatTelefone(paciente.Telefone),
                Endereco = paciente.Endereco,
                PlanoOdontologico = paciente.PlanoOdontologico,
                Empresa = paciente.Empresa
            };

            return Ok(response);
        }

        /// <summary>
        /// Atualiza os dados de um paciente existente.
        /// </summary>
        /// <param name="id">Identificador do paciente.</param>
        /// <param name="dto">Dados atualizados do paciente.</param>
        /// <returns>Paciente atualizado ou mensagem de erro.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaciente(int id, [FromBody] PacienteUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pacienteExistente = await _pacienteRepo.GetByIdAsync(id);
            if (pacienteExistente == null)
                return NotFound(new { error = "Paciente não encontrado." });

            pacienteExistente.NomeCompleto = dto.NomeCompleto;
            pacienteExistente.Email = dto.Email;
            pacienteExistente.Telefone = dto.Telefone;
            pacienteExistente.Endereco = dto.Endereco;
            pacienteExistente.PlanoOdontologico = dto.PlanoOdontologico;

            if (dto.Empresa == null || string.IsNullOrWhiteSpace(dto.Empresa))
            {
                pacienteExistente.Empresa = "Individual";
            }
            else
            {
                pacienteExistente.Empresa = dto.Empresa;
            }

            try
            {
                await _pacienteRepo.UpdateAsync(pacienteExistente);

                decimal gastoTotal = 0;
                var consultas = await _consultaRepo.GetByPacienteIdAsync(pacienteExistente.IdPaciente);
                foreach (var c in consultas)
                {
                    // Verifica se a consulta foi realizada antes de somar o custo
                    if (c.Status.Equals("Realizada", StringComparison.OrdinalIgnoreCase))
                    {
                        var procedimentos = await _procedimentoRepo.GetByConsultaIdAsync(c.IdConsulta);
                        gastoTotal += procedimentos.Sum(p => p.Custo);
                    }
                }

                var response = new PacienteResponseDTO
                {
                    IdPaciente = pacienteExistente.IdPaciente,
                    NomeCompleto = pacienteExistente.NomeCompleto,
                    CPF = FormatCPF(pacienteExistente.CPF),
                    PlanoOdontologico = pacienteExistente.PlanoOdontologico,
                    Empresa = pacienteExistente.Empresa,
                    NumConsultas = pacienteExistente.NumConsultas,
                    GastoTotal = FormatCurrency(gastoTotal)
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        } 

        /// <summary>
        /// Exclui um paciente e todos os seus registros relacionados.
        /// </summary>
        /// <param name="id">Identificador do paciente a ser excluído.</param>
        /// <returns>Status 204 em caso de sucesso ou mensagem de erro.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaciente(int id)
        {
            try
            {
                var paciente = await _pacienteRepo.GetByIdAsync(id);
                if (paciente == null)
                    return NotFound(new { error = "Paciente não encontrado." });

                var consultas = await _consultaRepo.GetByPacienteIdAsync(id);
                foreach (var consulta in consultas)
                {
                    var procedimentos = await _procedimentoRepo.GetByConsultaIdAsync(consulta.IdConsulta);
                    foreach (var proc in procedimentos)
                    {
                        await _procedimentoRepo.DeleteAsync(proc.IdProcedimento);
                    }
                    await _consultaRepo.DeleteAsync(consulta.IdConsulta);
                }

                await _pacienteRepo.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Método auxiliar para formatar o CPF em formato padrão.
        /// </summary>
        /// <param name="cpf">CPF em formato numérico.</param>
        /// <returns>CPF formatado (ex: 123.456.789-10).</returns>
        private string FormatCPF(string cpf)
        {
            if (!string.IsNullOrWhiteSpace(cpf) && cpf.Length == 11)
            {
                return $"{cpf.Substring(0, 3)}.{cpf.Substring(3, 3)}.{cpf.Substring(6, 3)}-{cpf.Substring(9, 2)}";
            }
            return cpf;
        }

        /// <summary>
        /// Método auxiliar para formatar números de telefone.
        /// </summary>
        /// <param name="telefone">Número de telefone sem formatação.</param>
        /// <returns>Número de telefone formatado.</returns>
        private string FormatTelefone(string telefone)
        {
            if (!string.IsNullOrWhiteSpace(telefone))
            {
                if (telefone.Length == 11)
                {
                    return $"({telefone.Substring(0, 2)}) {telefone.Substring(2, 5)}-{telefone.Substring(7, 4)}";
                }
                else if (telefone.Length == 10)
                {
                    return $"({telefone.Substring(0, 2)}) {telefone.Substring(2, 4)}-{telefone.Substring(6, 4)}";
                }
            }
            return telefone;
        }
    }
}
