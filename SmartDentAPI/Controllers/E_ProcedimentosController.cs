using Microsoft.AspNetCore.Mvc;
using SmartDentAPI.DTOs.Create;
using SmartDentAPI.DTOs.Response;
using SmartDentAPI.DTOs.Update;
using SmartDentAPI.Interfaces;
using SmartDentAPI.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDentAPI.Controllers
{
    /// <summary>
    /// Controller responsável por gerenciar os procedimentos odontológicos.
    /// </summary>
    /// <remarks>
    /// Este controller expõe endpoints para criar, consultar, atualizar e deletar procedimentos.
    /// Ele também utiliza uma tabela de custos predefinida para validar e definir o custo de cada procedimento.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Procedimentos")]
    public class ProcedimentosController : ControllerBase
    {
        private readonly IProcedimentoRepository _procedimentoRepo;
        private readonly IConsultaRepository _consultaRepo;
        
        // Tabela de custos predefinida para os tipos de procedimento (todos os tipos organizados por categoria)
        private readonly Dictionary<string, decimal> _tabelaCustos = new Dictionary<string, decimal>
        {
            // Consultas e Diagnóstico
            { "Consulta odontológica geral", 150m },
            { "Avaliação clínica e diagnóstico", 120m },
            { "Consulta para clareamento", 100m },
            { "Consulta para próteses", 130m },
            { "Acompanhamento ortodôntico", 140m },

            // Prevenção e Profilaxia
            { "Limpeza dental (profilaxia)", 120m },
            { "Aplicação de flúor", 80m },
            { "Aplicação de selante", 90m },
            { "Instrução de higiene bucal", 70m },

            // Urgência e Emergência 24h
            { "Atendimento odontológico de urgência", 200m },
            { "Alívio de dor", 150m },
            { "Drenagem de abscessos", 250m },
            { "Controle de hemorragias", 180m },

            // Radiologia e Exames
            { "Radiografia intraoral", 180m },
            { "Radiografia panorâmica", 300m },
            { "Documentação ortodôntica completa (Exames)", 400m },
            { "Tomografia computadorizada", 700m },

            // Dentística (Tratamentos Restauradores)
            { "Restauração em resina composta", 300m },
            { "Restauração em amálgama", 280m },
            { "Troca de restaurações antigas", 320m },

            // Cirurgia Oral e Extrações
            { "Extração de dente comum", 200m },
            { "Extração de dente do siso", 450m },
            { "Frenectomia lingual e labial", 500m },

            // Endodontia (Tratamento de Canal)
            { "Canal em dentes anteriores", 550m },
            { "Canal em dentes posteriores", 750m },
            { "Retratamento endodôntico", 800m },

            // Periodontia (Tratamento da Gengiva)
            { "Tratamento de gengivite", 350m },
            { "Raspagem de tártaro", 400m },
            { "Cirurgia periodontal", 600m },

            // Odontopediatria (Atendimento Infantil)
            { "Atendimento odontológico para crianças", 250m },
            { "Aplicação de flúor e selante", 150m },
            { "Tratamento restaurador em dentes de leite", 200m },
            { "Extração de dentes de leite", 180m },

            // Ortodontia (Aparelhos Dentários)
            { "Documentação ortodôntica completa (Ortodontia)", 400m },
            { "Instalação de aparelho fixo metálico", 1800m },
            { "Manutenção mensal do aparelho", 250m },
            { "Retirada do aparelho ortodôntico", 350m },
            { "Mantenedores ortodônticos", 500m },

            // Odontologia Estética
            { "Clareamento dental caseiro", 400m },
            { "Clareamento estético em consultório", 600m },

            // Próteses Dentárias
            { "Prótese fixa (coroa unitária)", 2000m },
            { "Prótese removível total (dentadura)", 2500m },
            { "Prótese removível parcial", 2200m },
            { "Prótese sobre cerâmica ou resina", 2800m },
            { "Placa de mordida para bruxismo", 900m }
        };

        /// <summary>
        /// Construtor que injeta os repositórios de procedimento e consulta.
        /// </summary>
        /// <param name="procedimentoRepo">Repositório de procedimentos.</param>
        /// <param name="consultaRepo">Repositório de consultas.</param>
        public ProcedimentosController(IProcedimentoRepository procedimentoRepo, IConsultaRepository consultaRepo)
        {
            _procedimentoRepo = procedimentoRepo;
            _consultaRepo = consultaRepo;
        }

        /// <summary>
        /// Método auxiliar para formatar um valor decimal no formato de moeda brasileira.
        /// </summary>
        /// <param name="valor">O valor a ser formatado.</param>
        /// <returns>O valor formatado, por exemplo, "R$ 1.234,56".</returns>
        private string FormatCurrency(decimal valor)
        {
            return valor.ToString("C", CultureInfo.GetCultureInfo("pt-BR"));
        }

        /// <summary>
        /// Cria um novo procedimento.
        /// </summary>
        /// <remarks>
        /// Valida se a consulta existe e se já não há um procedimento associado a ela.
        /// Em seguida, verifica se o tipo de procedimento informado está presente na tabela de custos,
        /// atribui o custo e cria o procedimento.
        /// 
        /// Exemplo de requisição:
        /// 
        /// ```json
        /// {
        ///     "idConsulta": 10001,
        ///     "tipoProcedimento": "Restauração em resina composta",
        ///     "descricao": "Restauração de resina fotopolimerizável em dente 36"
        /// }
        /// ```
        /// </remarks>
        /// <param name="dto">Objeto ProcedimentoCreateDTO contendo os dados para criação.</param>
        /// <returns>Retorna o procedimento criado com status 201 Created, ou erros de validação/negócio.</returns>
        /// <response code="201">Procedimento criado com sucesso</response>
        /// <response code="400">Erro na validação dos dados ou consulta não encontrada</response>
        [HttpPost]
        [SwaggerResponse(201, "Procedimento criado com sucesso", typeof(ProcedimentoResponseDTO))]
        [SwaggerResponse(400, "Requisição inválida", typeof(object))]
        [SwaggerOperation(
            Summary = "Criar novo procedimento",
            Description = "Cadastra um novo procedimento para uma consulta existente",
            OperationId = "CreateProcedimento",
            Tags = new[] { "Procedimentos" }
        )]
        public async Task<IActionResult> CreateProcedimento([FromBody] ProcedimentoCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validar se a consulta existe
            var consulta = await _consultaRepo.GetByIdAsync(dto.IdConsulta);
            if (consulta == null)
            {
                return BadRequest(new { error = "Consulta não encontrada para o id informado." });
            }

            // Verificar se já existe um procedimento associado a essa consulta
            var procedimentosExistentes = await _procedimentoRepo.GetByConsultaIdAsync(dto.IdConsulta);
            if (procedimentosExistentes.Any())
            {
                return BadRequest(new { error = "Já existe um procedimento associado a esta consulta." });
            }

            // Validar se o tipo de procedimento informado está na tabela de custos
            if (!_tabelaCustos.ContainsKey(dto.TipoProcedimento))
            {
                return BadRequest(new { error = "Tipo de procedimento inválido." });
            }
            var custo = _tabelaCustos[dto.TipoProcedimento];

            var procedimento = new Procedimento
            {
                IdConsulta = dto.IdConsulta,
                TipoProcedimento = dto.TipoProcedimento,
                Descricao = dto.Descricao,
                Custo = custo
            };

            try
            {
                await _procedimentoRepo.AddAsync(procedimento);

                var response = new ProcedimentoResponseDTO
                {
                    IdProcedimento = procedimento.IdProcedimento,
                    IdConsulta = procedimento.IdConsulta,
                    TipoProcedimento = procedimento.TipoProcedimento,
                    Descricao = procedimento.Descricao,
                    // Formata o custo para string no padrão R$ 1.234,56
                    Custo = FormatCurrency(procedimento.Custo)
                };

                return CreatedAtAction(nameof(GetProcedimentoById), new { id = response.IdProcedimento }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Retorna todos os procedimentos cadastrados.
        /// </summary>
        /// <returns>Uma lista de ProcedimentoResponseDTO com todos os procedimentos.</returns>
        /// <response code="200">Lista de procedimentos retornada com sucesso</response>
        [HttpGet]
        [SwaggerResponse(200, "Lista de procedimentos", typeof(List<ProcedimentoResponseDTO>))]
        [SwaggerOperation(
            Summary = "Listar todos procedimentos",
            Description = "Retorna a lista completa de procedimentos odontológicos cadastrados",
            OperationId = "GetAllProcedimentos",
            Tags = new[] { "Procedimentos" }
        )]
        public async Task<IActionResult> GetAllProcedimentos()
        {
            var list = await _procedimentoRepo.GetAllAsync();
            var response = new List<ProcedimentoResponseDTO>();

            foreach (var proc in list)
            {
                response.Add(new ProcedimentoResponseDTO
                {
                    IdProcedimento = proc.IdProcedimento,
                    IdConsulta = proc.IdConsulta,
                    TipoProcedimento = proc.TipoProcedimento,
                    Descricao = proc.Descricao,
                    Custo = FormatCurrency(proc.Custo)
                });
            }

            return Ok(response);
        }

        /// <summary>
        /// Retorna um procedimento específico pelo seu ID.
        /// </summary>
        /// <param name="id">ID do procedimento a ser consultado.</param>
        /// <returns>O procedimento encontrado ou erro se não existir.</returns>
        /// <response code="200">Procedimento encontrado com sucesso</response>
        /// <response code="404">Procedimento não encontrado</response>
        [HttpGet("{id}")]
        [SwaggerResponse(200, "Procedimento encontrado", typeof(ProcedimentoResponseDTO))]
        [SwaggerResponse(404, "Procedimento não encontrado", typeof(object))]
        [SwaggerOperation(
            Summary = "Obter procedimento por ID",
            Description = "Retorna os detalhes de um procedimento odontológico específico",
            OperationId = "GetProcedimentoById",
            Tags = new[] { "Procedimentos" }
        )]
        public async Task<IActionResult> GetProcedimentoById(int id)
        {
            var procedimento = await _procedimentoRepo.GetByIdAsync(id);
            if (procedimento == null)
                return NotFound(new { error = "Procedimento não encontrado." });

            var response = new ProcedimentoResponseDTO
            {
                IdProcedimento = procedimento.IdProcedimento,
                IdConsulta = procedimento.IdConsulta,
                TipoProcedimento = procedimento.TipoProcedimento,
                Descricao = procedimento.Descricao,
                Custo = FormatCurrency(procedimento.Custo)
            };

            return Ok(response);
        }

        /// <summary>
        /// Atualiza um procedimento existente.
        /// </summary>
        /// <remarks>
        /// Permite atualizar o tipo de procedimento (recalculando o custo) e a descrição.
        /// 
        /// Exemplo de requisição:
        /// 
        /// ```json
        /// {
        ///     "tipoProcedimento": "Restauração em amálgama",
        ///     "descricao": "Restauração de amálgama no dente 36"
        /// }
        /// ```
        /// </remarks>
        /// <param name="id">ID do procedimento a ser atualizado.</param>
        /// <param name="dto">Dados atualizados do procedimento.</param>
        /// <returns>O procedimento atualizado ou erro se não encontrado/validação falhar.</returns>
        /// <response code="200">Procedimento atualizado com sucesso</response>
        /// <response code="400">Erro na validação dos dados</response>
        /// <response code="404">Procedimento não encontrado</response>
        [HttpPut("{id}")]
        [SwaggerResponse(200, "Procedimento atualizado com sucesso", typeof(ProcedimentoResponseDTO))]
        [SwaggerResponse(400, "Requisição inválida", typeof(object))]
        [SwaggerResponse(404, "Procedimento não encontrado", typeof(object))]
        [SwaggerOperation(
            Summary = "Atualizar procedimento",
            Description = "Atualiza os dados de um procedimento existente, incluindo o recálculo do custo se o tipo for alterado",
            OperationId = "UpdateProcedimento",
            Tags = new[] { "Procedimentos" }
        )]
        public async Task<IActionResult> UpdateProcedimento(int id, [FromBody] ProcedimentoUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var procedimentoExistente = await _procedimentoRepo.GetByIdAsync(id);
            if (procedimentoExistente == null)
                return NotFound(new { error = "Procedimento não encontrado." });

            // Validar se o novo tipo de procedimento está na tabela de custos
            if (!_tabelaCustos.ContainsKey(dto.TipoProcedimento))
            {
                return BadRequest(new { error = "Tipo de procedimento inválido." });
            }
            var novoCusto = _tabelaCustos[dto.TipoProcedimento];

            // Atualizar apenas os campos permitidos: tipo e descrição
            procedimentoExistente.TipoProcedimento = dto.TipoProcedimento;
            procedimentoExistente.Descricao = dto.Descricao;
            procedimentoExistente.Custo = novoCusto;

            try
            {
                await _procedimentoRepo.UpdateAsync(procedimentoExistente);
                var response = new ProcedimentoResponseDTO
                {
                    IdProcedimento = procedimentoExistente.IdProcedimento,
                    IdConsulta = procedimentoExistente.IdConsulta,
                    TipoProcedimento = procedimentoExistente.TipoProcedimento,
                    Descricao = procedimentoExistente.Descricao,
                    Custo = FormatCurrency(procedimentoExistente.Custo)
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Exclui um procedimento existente.
        /// </summary>
        /// <param name="id">ID do procedimento a ser excluído.</param>
        /// <returns>Status 204 se excluído com sucesso, ou erro.</returns>
        /// <response code="204">Procedimento excluído com sucesso</response>
        /// <response code="404">Procedimento não encontrado</response>
        /// <response code="400">Erro ao excluir procedimento</response>
        [HttpDelete("{id}")]
        [SwaggerResponse(204, "Procedimento excluído com sucesso")]
        [SwaggerResponse(404, "Procedimento não encontrado", typeof(object))]
        [SwaggerResponse(400, "Erro ao excluir procedimento", typeof(object))]
        [SwaggerOperation(
            Summary = "Excluir procedimento",
            Description = "Remove um procedimento odontológico do sistema",
            OperationId = "DeleteProcedimento",
            Tags = new[] { "Procedimentos" }
        )]
        public async Task<IActionResult> DeleteProcedimento(int id)
        {
            try
            {
                await _procedimentoRepo.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
