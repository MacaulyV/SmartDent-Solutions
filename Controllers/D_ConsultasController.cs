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
    /// Controller responsável por gerenciar as operações relacionadas às consultas odontológicas.
    /// </summary>
    /// <remarks>
    /// Este controller expõe endpoints para criar, consultar, atualizar e deletar consultas.
    /// Além disso, ao criar ou deletar uma consulta, o contador de consultas do paciente é atualizado.
    /// Também garante a integridade dos dados, validando o formato da data (ddMMyyyyHHmm) e o status da consulta.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class D_ConsultasController : ControllerBase
    {
        private readonly IConsultaRepository _consultaRepo;
        private readonly IPacienteRepository _pacienteRepo;
        private readonly IProcedimentoRepository _procedimentoRepo; // ADICIONE ESTA LINHA

        /// <summary>
        /// Construtor que injeta os repositórios de consulta, paciente e procedimento.
        /// </summary>
        /// <param name="consultaRepo">Repositório para operações de consulta.</param>
        /// <param name="pacienteRepo">Repositório para operações de paciente.</param>
        /// <param name="procedimentoRepo">Repositório para operações de procedimento.</param>
        public D_ConsultasController(
            IConsultaRepository consultaRepo,
            IPacienteRepository pacienteRepo,
            IProcedimentoRepository procedimentoRepo) // ADICIONE
        {
            _consultaRepo = consultaRepo;
            _pacienteRepo = pacienteRepo;
            _procedimentoRepo = procedimentoRepo; // ARMAZENA NO CAMPO
        }

        /// <summary>
        /// POST: api/Consultas
        /// Cria uma nova consulta.
        /// </summary>
        /// <remarks>
        /// Este endpoint cria uma nova consulta para um paciente existente.
        /// Realiza validações para confirmar que o paciente existe, que a data está no formato correto 
        /// (ddMMyyyyHHmm) e que a data da consulta não é retroativa.
        /// Após criar a consulta, incrementa o contador de consultas do paciente.
        /// </remarks>
        /// <param name="dto">Objeto ConsultaCreateDTO contendo os dados da nova consulta.</param>
        /// <returns>Retorna um objeto ConsultaResponseDTO com os dados da consulta criada.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateConsulta([FromBody] ConsultaCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validação: verificar se o paciente existe
            var paciente = await _pacienteRepo.GetByIdAsync(dto.IdPaciente);
            if (paciente == null)
                return BadRequest(new { error = "Paciente não encontrado para o id informado." });

            // Converter a data da consulta (formato: ddMMyyyyHHmm)
            if (!DateTime.TryParseExact(dto.DataConsulta, "ddMMyyyyHHmm", 
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataConsulta))
            {
                return BadRequest(new { error = "Formato de data/hora inválido. Use ddMMyyyyHHmm." });
            }

            if (dataConsulta < DateTime.Now)
            {
                return BadRequest(new { error = "A data da consulta não pode ser retroativa." });
            }

            // Criação da consulta
            var consulta = new Consulta
            {
                IdPaciente = dto.IdPaciente,
                DataConsulta = dto.DataConsulta,
                DataConsultaConvertida = dataConsulta,
                Status = "Agendada"
            };

            try
            {
                await _consultaRepo.AddAsync(consulta);

                // **Incrementa o contador de consultas do paciente**
                paciente.NumConsultas += 1;
                await _pacienteRepo.UpdateAsync(paciente);

                var response = new ConsultaResponseDTO
                {
                    IdConsulta = consulta.IdConsulta,
                    IdPaciente = consulta.IdPaciente,
                    DataConsulta = dataConsulta.ToString("dd/MM/yyyy HH:mm"),
                    Status = consulta.Status,
                };

                return CreatedAtAction(nameof(GetConsultaById), new { id = response.IdConsulta }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// GET: api/Consultas
        /// Retorna a lista de todas as consultas cadastradas.
        /// </summary>
        /// <remarks>
        /// Este endpoint retorna todas as consultas cadastradas no sistema, convertendo a data do formato interno "ddMMyyyyHHmm"
        /// para o formato "dd/MM/yyyy HH:mm" para melhor legibilidade.
        /// </remarks>
        /// <returns>Uma lista de objetos ConsultaResponseDTO representando as consultas.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllConsultas()
        {
            var list = await _consultaRepo.GetAllAsync();
            var response = new List<ConsultaResponseDTO>();

            foreach (var consulta in list)
            {
                DateTime.TryParseExact(consulta.DataConsulta, "ddMMyyyyHHmm", 
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataConvertida);

                response.Add(new ConsultaResponseDTO
                {
                    IdConsulta = consulta.IdConsulta,
                    IdPaciente = consulta.IdPaciente,
                    DataConsulta = dataConvertida.ToString("dd/MM/yyyy HH:mm"),
                    Status = consulta.Status,
                });
            }

            return Ok(response);
        }

        /// <summary>
        /// GET: api/Consultas/{id}
        /// Retorna os dados de uma consulta específica com base no seu ID.
        /// </summary>
        /// <param name="id">ID da consulta a ser recuperada.</param>
        /// <returns>Objeto ConsultaResponseDTO contendo os dados da consulta ou NotFound se não existir.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetConsultaById(int id)
        {
            var consulta = await _consultaRepo.GetByIdAsync(id);
            if (consulta == null)
                return NotFound(new { error = "Consulta não encontrada." });

            DateTime.TryParseExact(consulta.DataConsulta, "ddMMyyyyHHmm", 
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataConsulta);

            var response = new ConsultaResponseDTO
            {
                IdConsulta = consulta.IdConsulta,
                IdPaciente = consulta.IdPaciente,
                DataConsulta = dataConsulta.ToString("dd/MM/yyyy HH:mm"),
                Status = consulta.Status,
            };

            return Ok(response);
        }

        /// <summary>
        /// PUT: api/Consultas/{id}
        /// Atualiza uma consulta existente.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite atualizar a data e o status de uma consulta.
        /// A nova data deve estar no formato "ddMMyyyyHHmm" e não pode ser retroativa.
        /// Apenas valores válidos para o status ("Agendada", "Realizada", "Cancelada" ou "Não Realizada") são aceitos.
        /// </remarks>
        /// <param name="id">ID da consulta a ser atualizada.</param>
        /// <param name="dto">Objeto ConsultaUpdateDTO contendo os dados atualizados.</param>
        /// <returns>Objeto ConsultaResponseDTO com os dados atualizados ou um erro de validação/negócio.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConsulta(int id, [FromBody] ConsultaUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var consultaExistente = await _consultaRepo.GetByIdAsync(id);
            if (consultaExistente == null)
                return NotFound(new { error = "Consulta não encontrada." });

            // Converter nova data
            if (!DateTime.TryParseExact(dto.DataConsulta, "ddMMyyyyHHmm", 
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime novaData))
            {
                return BadRequest(new { error = "Formato de data/hora inválido. Use ddMMyyyyHHmm." });
            }

            // Verificar data retroativa (opcional)
            if (novaData < DateTime.Now)
            {
                return BadRequest(new { error = "A data da consulta não pode ser retroativa." });
            }

            // Validar se o status informado é permitido
            var statusPermitidos = new List<string> { "Agendada", "Realizada", "Cancelada", "Não Realizada" };
            if (!statusPermitidos.Contains(dto.Status))
            {
                return BadRequest(new { error = "Status inválido. Permite-se somente 'Agendada', 'Realizada', 'Cancelada' ou 'Não Realizada'." });
            }

            // Atualizar campos
            consultaExistente.DataConsulta = dto.DataConsulta;
            consultaExistente.DataConsultaConvertida = novaData;
            consultaExistente.Status = dto.Status; // Apenas valores válidos serão aceitos

            try
            {
                await _consultaRepo.UpdateAsync(consultaExistente);

                var response = new ConsultaResponseDTO
                {
                    IdConsulta = consultaExistente.IdConsulta,
                    IdPaciente = consultaExistente.IdPaciente,
                    DataConsulta = novaData.ToString("dd/MM/yyyy HH:mm"),
                    Status = consultaExistente.Status,
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// DELETE: api/Consultas/{id}
        /// Remove uma consulta com base no seu ID.
        /// </summary>
        /// <remarks>
        /// Este endpoint remove a consulta e, em seguida, exclui todos os procedimentos associados a ela.
        /// Também atualiza o contador de consultas do paciente, decrementando-o se aplicável.
        /// </remarks>
        /// <param name="id">ID da consulta a ser deletada.</param>
        /// <returns>Retorna NoContent se a exclusão for bem-sucedida, ou um erro caso contrário.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsulta(int id)
        {
            try
            {
                // Busca a consulta a ser deletada
                var consulta = await _consultaRepo.GetByIdAsync(id);
                if (consulta == null)
                    return NotFound(new { error = "Consulta não encontrada." });

                // Buscar e excluir todos os procedimentos associados à consulta
                var procedimentos = await _procedimentoRepo.GetByConsultaIdAsync(consulta.IdConsulta);
                foreach (var proc in procedimentos)
                {
                    await _procedimentoRepo.DeleteAsync(proc.IdProcedimento);
                }

                // Buscar o paciente para decrementar o contador, se aplicável
                var paciente = await _pacienteRepo.GetByIdAsync(consulta.IdPaciente);
                if (paciente != null && paciente.NumConsultas > 0)
                {
                    paciente.NumConsultas -= 1;
                    await _pacienteRepo.UpdateAsync(paciente);
                }

                // Deleta a consulta
                await _consultaRepo.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
