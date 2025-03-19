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
using System.Threading.Tasks;

namespace SmartDentAPI.Controllers
{
    /// <summary>
    /// Controller responsável por gerenciar os alertas gerados pela aplicação.
    /// </summary>
    /// <remarks>
    /// Este controller expõe endpoints para criar, consultar, atualizar e deletar alertas.
    /// Ele também filtra alertas por tipo e por paciente, garantindo que apenas tipos permitidos sejam utilizados.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class H_AlertasController : ControllerBase
    {
        private readonly IAlertaRepository _alertaRepo;
        private readonly IPacienteRepository _pacienteRepo;

        // Lista de tipos de alerta permitidos
        private readonly List<string> _tiposAlertaPermitidos = new List<string>
        {
            "UsoExcessivo",
            "Uso Moderado com Tendência a Excesso",
        };

        /// <summary>
        /// Construtor que injeta os repositórios de alerta e paciente.
        /// </summary>
        /// <param name="alertaRepo">Repositório para operações de alerta.</param>
        /// <param name="pacienteRepo">Repositório para operações de paciente.</param>
        public H_AlertasController(IAlertaRepository alertaRepo, IPacienteRepository pacienteRepo)
        {
            _alertaRepo = alertaRepo;
            _pacienteRepo = pacienteRepo;
        }

        /// <summary>
        /// POST: api/Alertas
        /// Cria um novo alerta.
        /// </summary>
        /// <remarks>
        /// Este endpoint cria um alerta para um paciente existente, validando que o paciente existe 
        /// e que o tipo de alerta informado é permitido.
        /// Após a criação, retorna os dados do alerta com a data de geração formatada.
        /// </remarks>
        /// <param name="dto">Objeto AlertaCreateDTO contendo os dados necessários para a criação do alerta.</param>
        /// <returns>Retorna 201 Created com os dados do alerta ou um erro caso haja problemas de validação ou de negócio.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateAlerta([FromBody] AlertaCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var paciente = await _pacienteRepo.GetByIdAsync(dto.IdPaciente);
            if (paciente == null)
                return BadRequest(new { error = "Paciente não encontrado para o id informado." });

            if (!_tiposAlertaPermitidos.Contains(dto.TipoAlerta))
            {
                return BadRequest(new { error = "TipoAlerta inválido. As opções permitidas são: " + string.Join(", ", _tiposAlertaPermitidos) });
            }

            var alerta = new Alerta
            {
                IdPaciente = dto.IdPaciente,
                TipoAlerta = dto.TipoAlerta,
                GrauRisco = dto.GrauRisco,
                Justificativa = dto.Justificativa,
                DataGeracao = DateTime.UtcNow
            };

            try
            {
                await _alertaRepo.AddAsync(alerta);

                var response = new AlertaResponseDTO
                {
                    IdAlerta = alerta.IdAlerta,
                    IdPaciente = alerta.IdPaciente,
                    TipoAlerta = alerta.TipoAlerta,
                    GrauRisco = alerta.GrauRisco,
                    Justificativa = alerta.Justificativa,
                    DataGeracao = alerta.DataGeracao
                };

                return CreatedAtAction(nameof(GetAlertasByPaciente), new { idPaciente = alerta.IdPaciente }, new
                {
                    response.IdAlerta,
                    response.IdPaciente,
                    response.TipoAlerta,
                    response.GrauRisco,
                    response.Justificativa,
                    DataGeracao = alerta.DataGeracao.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// GET: api/Alertas/Uso-Excessivo
        /// Retorna todos os alertas do tipo "UsoExcessivo".
        /// </summary>
        /// <remarks>
        /// Este endpoint filtra os alertas retornados para incluir apenas aqueles cujo tipo é "UsoExcessivo",
        /// utilizando uma comparação case-insensitive.
        /// </remarks>
        /// <returns>Uma lista de alertas do tipo "UsoExcessivo" com a data de geração formatada.</returns>
        [HttpGet("Uso-Excessivo")]
        public async Task<IActionResult> GetAlertasUsoExcessivo()
        {
            var alertas = (await _alertaRepo.GetAllAsync())
                .Where(a => a.TipoAlerta.Equals("UsoExcessivo", StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            if (!alertas.Any())
                return NotFound(new { error = "Nenhum alerta do tipo 'UsoExcessivo' encontrado." });

            var response = alertas.Select(a => new
            {
                a.IdAlerta,
                a.IdPaciente,
                a.TipoAlerta,
                a.GrauRisco,
                a.Justificativa,
                DataGeracao = a.DataGeracao.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
            });

            return Ok(response);
        }

        /// <summary>
        /// GET: api/Alertas/Tendencia-Excesso
        /// Retorna todos os alertas do tipo "Uso Moderado com Tendência a Excesso".
        /// </summary>
        /// <remarks>
        /// Este endpoint filtra os alertas para incluir apenas aqueles cujo tipo é "Uso Moderado com Tendência a Excesso",
        /// usando comparação case-insensitive.
        /// </remarks>
        /// <returns>Uma lista de alertas do tipo "Uso Moderado com Tendência a Excesso" com a data de geração formatada.</returns>
        [HttpGet("Tendencia-Excesso")]
        public async Task<IActionResult> GetAlertasUsoModerado()
        {
            var alertas = (await _alertaRepo.GetAllAsync())
                .Where(a => a.TipoAlerta.Equals("Uso Moderado com Tendência a Excesso", StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            if (!alertas.Any())
                return NotFound(new { error = "Nenhum alerta do tipo 'Uso Moderado com Tendência a Excesso' encontrado." });

            var response = alertas.Select(a => new
            {
                a.IdAlerta,
                a.IdPaciente,
                a.TipoAlerta,
                a.GrauRisco,
                a.Justificativa,
                DataGeracao = a.DataGeracao.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
            });

            return Ok(response);
        }

        /// <summary>
        /// GET: api/Alertas
        /// Retorna todos os alertas cadastrados no sistema.
        /// </summary>
        /// <remarks>
        /// Este endpoint retorna a lista completa de alertas, formatando a data de geração para melhor legibilidade.
        /// </remarks>
        /// <returns>Uma lista de alertas com as datas de geração formatadas.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAlertas()
        {
            var alertas = await _alertaRepo.GetAllAsync();
            var response = alertas.Select(a => new
            {
                a.IdAlerta,
                a.IdPaciente,
                a.TipoAlerta,
                a.GrauRisco,
                a.Justificativa,
                DataGeracao = a.DataGeracao.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
            });
            return Ok(response);
        }

        /// <summary>
        /// GET: api/Alertas/paciente/{idPaciente}
        /// Retorna os alertas associados a um paciente específico.
        /// </summary>
        /// <param name="idPaciente">ID do paciente para o qual se deseja obter os alertas.</param>
        /// <returns>Uma lista de alertas para o paciente informado com as datas formatadas.</returns>
        [HttpGet("paciente/{idPaciente}")]
        public async Task<IActionResult> GetAlertasByPaciente(int idPaciente)
        {
            var alertas = (await _alertaRepo.GetByPacienteIdAsync(idPaciente)).ToList();
            if (!alertas.Any())
                return NotFound(new { error = "Nenhum alerta encontrado para este paciente." });

            var response = alertas.Select(a => new
            {
                a.IdAlerta,
                a.IdPaciente,
                a.TipoAlerta,
                a.GrauRisco,
                a.Justificativa,
                DataGeracao = a.DataGeracao.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
            });
            return Ok(response);
        }

        /// <summary>
        /// PUT: api/Alertas/{id}
        /// Atualiza os dados de um alerta existente.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite a atualização do tipo de alerta, grau de risco e justificativa de um alerta.
        /// Só são aceitos tipos de alerta que estejam na lista de permitidos.
        /// </remarks>
        /// <param name="id">ID do alerta a ser atualizado.</param>
        /// <param name="dto">Objeto AlertaUpdateDTO contendo os dados atualizados.</param>
        /// <returns>Retorna os dados atualizados do alerta com a data de geração formatada.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAlerta(int id, [FromBody] AlertaUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var alertaExistente = await _alertaRepo.GetByIdAsync(id);
            if (alertaExistente == null)
                return NotFound(new { error = "Alerta não encontrado." });

            if (!_tiposAlertaPermitidos.Contains(dto.TipoAlerta))
            {
                return BadRequest(new { error = "TipoAlerta inválido. As opções permitidas são: " + string.Join(", ", _tiposAlertaPermitidos) });
            }

            alertaExistente.TipoAlerta = dto.TipoAlerta;
            alertaExistente.GrauRisco = dto.GrauRisco;
            alertaExistente.Justificativa = dto.Justificativa;
            alertaExistente.DataGeracao = DateTime.UtcNow;

            try
            {
                await _alertaRepo.UpdateAsync(alertaExistente);

                var response = new AlertaResponseDTO
                {
                    IdAlerta = alertaExistente.IdAlerta,
                    IdPaciente = alertaExistente.IdPaciente,
                    TipoAlerta = alertaExistente.TipoAlerta,
                    GrauRisco = alertaExistente.GrauRisco,
                    Justificativa = alertaExistente.Justificativa,
                    DataGeracao = alertaExistente.DataGeracao
                };

                return Ok(new
                {
                    response.IdAlerta,
                    response.IdPaciente,
                    response.TipoAlerta,
                    response.GrauRisco,
                    response.Justificativa,
                    DataGeracao = alertaExistente.DataGeracao.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// DELETE: api/Alertas/deletar-TodosAlertas
        /// Remove todos os alertas cadastrados no sistema.
        /// </summary>
        /// <remarks>
        /// Este endpoint exclui todos os alertas do banco de dados.
        /// </remarks>
        /// <returns>Retorna NoContent se a operação for bem-sucedida.</returns>
        [HttpDelete("deletar-TodosAlertas")]
        public async Task<IActionResult> DeleteAllAlertas()
        {
            try
            {
                var alertas = await _alertaRepo.GetAllAsync();
                foreach (var alerta in alertas)
                {
                    await _alertaRepo.DeleteAsync(alerta.IdAlerta);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// DELETE: api/Alertas/{id}
        /// Remove um alerta específico com base no seu ID.
        /// </summary>
        /// <param name="id">ID do alerta a ser removido.</param>
        /// <returns>Retorna NoContent se a exclusão for bem-sucedida, ou um erro caso contrário.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlerta(int id)
        {
            try
            {
                await _alertaRepo.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
