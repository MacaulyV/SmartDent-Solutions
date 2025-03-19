using Microsoft.AspNetCore.Mvc;
using SmartDentAPI.DTOs;
using System.Collections.Generic;

namespace SmartDentAPI.Controllers
{
    /// <summary>
    /// Controller responsável por fornecer os planos odontológicos disponíveis.
    /// </summary>
    /// <remarks>
    /// Este endpoint retorna um dicionário com duas categorias de planos: Individuais e Empresariais.
    /// Cada categoria contém uma lista de objetos PlanoOdontologicoDTO que representam os planos disponíveis.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class C_PlanosController : ControllerBase
    {
        /// <summary>
        /// GET: api/Planos/PlanosDisponiveis
        /// Retorna os planos odontológicos disponíveis, organizados em categorias.
        /// </summary>
        /// <remarks>
        /// Este endpoint retorna os planos divididos em "Individuais" e "Empresariais".
        /// Cada categoria possui uma lista de planos, cada um representado por um DTO contendo o nome do plano.
        /// </remarks>
        /// <returns>Um IActionResult contendo o dicionário de planos.</returns>
        [HttpGet("PlanosDisponiveis")] // Novo nome da rota
        public IActionResult GetPlanos()
        {
            var planos = new Dictionary<string, List<PlanoOdontologicoDTO>>
            {
                { "Individuais", new List<PlanoOdontologicoDTO>
                    {
                        new PlanoOdontologicoDTO { Nome = "Dental Júnior" },
                        new PlanoOdontologicoDTO { Nome = "Bem Estar" },
                        new PlanoOdontologicoDTO { Nome = "Bem Estar White" },
                        new PlanoOdontologicoDTO { Nome = "Bem Estar Pró" },
                        new PlanoOdontologicoDTO { Nome = "Bem Estar Orto" },
                        new PlanoOdontologicoDTO { Nome = "Bem Estar Orto White" }
                    }
                },
                { "Empresariais", new List<PlanoOdontologicoDTO>
                    {
                        new PlanoOdontologicoDTO { Nome = "Convencional" },
                        new PlanoOdontologicoDTO { Nome = "Integral" },
                        new PlanoOdontologicoDTO { Nome = "Integral Plus" },
                        new PlanoOdontologicoDTO { Nome = "Integral Doc" },
                        new PlanoOdontologicoDTO { Nome = "Integral Doc Plus" },
                        new PlanoOdontologicoDTO { Nome = "Premium" },
                        new PlanoOdontologicoDTO { Nome = "Superior" },
                        new PlanoOdontologicoDTO { Nome = "Classical" },
                        new PlanoOdontologicoDTO { Nome = "Classical Doc" },
                        new PlanoOdontologicoDTO { Nome = "Ômega" },
                        new PlanoOdontologicoDTO { Nome = "Master" },
                        new PlanoOdontologicoDTO { Nome = "Maximum White" }
                    }
                }
            };

            return Ok(planos);
        }
    }
}
