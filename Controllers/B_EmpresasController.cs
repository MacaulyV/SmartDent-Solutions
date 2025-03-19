using Microsoft.AspNetCore.Mvc;
using SmartDentAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDentAPI.Controllers
{
    /// <summary>
    /// Controller responsável por gerenciar a consulta de empresas ativas.
    /// </summary>
    /// <remarks>
    /// Este controller expõe um endpoint para retornar as empresas que possuem pacientes cadastrados,
    /// excluindo o valor "Individual" que representa pacientes com planos próprios.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class B_EmpresasController : ControllerBase
    {
        private readonly IPacienteRepository _pacienteRepo;

        /// <summary>
        /// Construtor que injeta o repositório de pacientes.
        /// </summary>
        /// <param name="pacienteRepo">Instância do IPacienteRepository para acesso aos dados dos pacientes.</param>
        public B_EmpresasController(IPacienteRepository pacienteRepo)
        {
            _pacienteRepo = pacienteRepo;
        }

        /// <summary>
        /// GET: api/Empresas/EmpresasAtivas
        /// Retorna todas as empresas ativas (exceto "Individual") que possuem pacientes associados.
        /// </summary>
        /// <remarks>
        /// Este endpoint consulta o repositório de pacientes para obter uma lista distinta de empresas,
        /// removendo o valor "Individual" para representar apenas empresas reais.
        /// </remarks>
        /// <returns>Uma lista de nomes de empresas ou uma mensagem de erro se nenhuma for encontrada.</returns>
        [HttpGet("EmpresasAtivas")]
        public async Task<IActionResult> GetEmpresasComPacientes()
        {
            try
            {
                // Retorna todas as empresas distintas que possuem pelo menos 1 paciente
                var empresas = await _pacienteRepo.GetEmpresasComPacientesAsync();

                // Remove "Individual", pois corresponde a pacientes com planos próprios
                empresas = empresas.Where(e => !string.Equals(e, "Individual", StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (empresas == null || !empresas.Any())
                {
                    return NotFound(new { error = "Nenhuma empresa encontrada com pacientes associados." });
                }

                return Ok(empresas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Ocorreu um erro ao buscar as empresas.", details = ex.Message });
            }
        }
    }
}
