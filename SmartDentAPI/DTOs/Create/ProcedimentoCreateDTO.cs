using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartDentAPI.DTOs.Create
{
    /// <summary>
    /// DTO para criação de um novo procedimento odontológico.
    /// </summary>
    /// <remarks>
    /// Esse DTO é utilizado para receber os dados necessários para criar um procedimento.
    /// Inclui o ID da consulta à qual o procedimento está associado, o tipo do procedimento
    /// e uma descrição opcional.
    /// 
    /// Exemplo de requisição:
    /// 
    /// ```json
    /// {
    ///     "idConsulta": 10001,
    ///     "tipoProcedimento": "Restauração",
    ///     "descricao": "Restauração de resina fotopolimerizável em dente 36"
    /// }
    /// ```
    /// </remarks>
    [SwaggerSchema(
        Title = "Criação de Procedimento", 
        Description = "Dados para registro de um novo procedimento odontológico")]
    public class ProcedimentoCreateDTO
    {
        /// <summary>
        /// Identificador da consulta à qual o procedimento pertence.
        /// Campo obrigatório.
        /// </summary>
        /// <example>10001</example>
        [Required(ErrorMessage = "O idConsulta é obrigatório.")]
        [SwaggerSchema(Description = "ID da consulta existente no sistema")]
        public int IdConsulta { get; set; }

        /// <summary>
        /// Tipo (nome) do procedimento.
        /// Campo obrigatório para identificar o procedimento.
        /// </summary>
        /// <example>Restauração</example>
        [Required(ErrorMessage = "O tipo de procedimento é obrigatório.")]
        [SwaggerSchema(Description = "Tipo ou nome do procedimento odontológico realizado")]
        public string TipoProcedimento { get; set; }

        /// <summary>
        /// Descrição adicional do procedimento.
        /// Esse campo não é obrigatório e pode ser nulo.
        /// </summary>
        /// <example>Restauração de resina fotopolimerizável em dente 36</example>
        [SwaggerSchema(Description = "Descrição detalhada do procedimento com informações adicionais")]
        public string Descricao { get; set; }
    }
}