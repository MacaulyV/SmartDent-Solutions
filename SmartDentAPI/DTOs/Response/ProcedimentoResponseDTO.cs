using Swashbuckle.AspNetCore.Annotations;

namespace SmartDentAPI.DTOs.Response
{
    /// <summary>
    /// Data Transfer Object (DTO) para a resposta dos dados de um procedimento odontológico.
    /// </summary>
    /// <remarks>
    /// Este DTO é utilizado para retornar os dados do procedimento para o cliente,
    /// incluindo o ID do procedimento, o ID da consulta à qual pertence, o tipo do procedimento,
    /// a descrição e o custo (armazenado como string).
    /// 
    /// Exemplo de resposta:
    /// 
    /// ```json
    /// {
    ///     "idProcedimento": 1001,
    ///     "idConsulta": 10001,
    ///     "tipoProcedimento": "Restauração em resina composta",
    ///     "descricao": "Restauração de resina fotopolimerizável em dente 36",
    ///     "custo": "R$ 300,00"
    /// }
    /// ```
    /// </remarks>
    [SwaggerSchema(
        Title = "Dados do Procedimento", 
        Description = "Informações detalhadas de um procedimento odontológico")]
    public class ProcedimentoResponseDTO
    {
        /// <summary>
        /// Identificador único do procedimento.
        /// </summary>
        /// <example>1001</example>
        [SwaggerSchema(Description = "ID único do procedimento no sistema")]
        public int IdProcedimento { get; set; }

        /// <summary>
        /// Identificador da consulta à qual este procedimento está associado.
        /// </summary>
        /// <example>10001</example>
        [SwaggerSchema(Description = "ID da consulta associada ao procedimento")]
        public int IdConsulta { get; set; }

        /// <summary>
        /// Tipo (nome) do procedimento.
        /// </summary>
        /// <example>Restauração em resina composta</example>
        [SwaggerSchema(Description = "Tipo ou nome do procedimento odontológico realizado")]
        public string TipoProcedimento { get; set; }

        /// <summary>
        /// Descrição adicional do procedimento.
        /// </summary>
        /// <example>Restauração de resina fotopolimerizável em dente 36</example>
        [SwaggerSchema(Description = "Descrição detalhada do procedimento com informações adicionais")]
        public string Descricao { get; set; }

        /// <summary>
        /// Custo do procedimento, armazenado como string.
        /// </summary>
        /// <example>R$ 300,00</example>
        [SwaggerSchema(Description = "Valor do procedimento em formato de moeda brasileira")]
        public string Custo { get; set; }
    }
}