using Microsoft.AspNetCore.Mvc;
using SmartDentAPI.Interfaces;
using SmartDentAPI.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDentAPI.Controllers
{
    /// <summary>
    /// Controller responsável por fornecer os dados de cidades cadastradas no sistema.
    /// </summary>
    /// <remarks>
    /// Este controller expõe endpoints para listar cidades com pacientes cadastrados.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Cidades")]
    public class CidadesController : ControllerBase
    {
        private readonly IPacienteRepository _pacienteRepo;

        /// <summary>
        /// Construtor que injeta o repositório de pacientes.
        /// </summary>
        /// <param name="pacienteRepo">Instância do repositório de pacientes.</param>
        public CidadesController(IPacienteRepository pacienteRepo)
        {
            _pacienteRepo = pacienteRepo;
        }

        /// <summary>
        /// GET /api/cidade
        /// Retorna todas as cidades distintas dos pacientes cadastrados.
        /// </summary>
        /// <remarks>
        /// Este endpoint extrai a cidade do campo Endereco de cada paciente, assumindo que a cidade é a última parte da string separada por vírgula.
        /// Caso não haja pacientes ou endereços válidos, retorna NotFound.
        /// </remarks>
        /// <returns>
        /// Uma lista de cidades (strings) distintas.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetCidades()
        {
            var pacientes = await _pacienteRepo.GetAllAsync();
            if (pacientes == null || !pacientes.Any())
                return NotFound(new { error = "Nenhum paciente encontrado." });

            // Assume que o Endereco tem o formato "Rua Exemplo, {número}, Bairro {x}, {cidade}"
            var cidades = pacientes
                .Select(p =>
                {
                    if (!string.IsNullOrWhiteSpace(p.Endereco))
                    {
                        var partes = p.Endereco.Split(',');
                        // Retorna a última parte, que corresponde à cidade, removendo espaços em branco
                        return partes.Last().Trim();
                    }
                    return null;
                })
                .Where(cidade => !string.IsNullOrEmpty(cidade))
                .Distinct()
                .ToList();

            return Ok(cidades);
        }
    }
}
