using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartDentAPI.DTOs.Update
{
    /// <summary>
    /// DTO para atualização dos dados de uma consulta odontológica.
    /// </summary>
    /// <remarks>
    /// Este DTO permite atualizar a data e o status de uma consulta.
    /// A data deve estar no formato "ddMMyyyyHHmm" e conter exatamente 12 dígitos.
    /// O status indica a situação da consulta, podendo ser "Agendada", "Realizada", "Cancelada", etc.
    /// 
    /// Exemplo de requisição:
    /// 
    /// ```json
    /// {
    ///     "dataConsulta": "010520241600",
    ///     "status": "Realizada"
    /// }
    /// ```
    /// </remarks>
    [SwaggerSchema(
        Title = "Atualização de Consulta", 
        Description = "Dados para atualizar uma consulta existente")]
    public class ConsultaUpdateDTO
    {
        /// <summary>
        /// Nova data da consulta no formato "ddMMyyyyHHmm".
        /// Campo obrigatório para atualização.
        /// </summary>
        /// <example>010520241600</example>
        [Required(ErrorMessage = "A nova data da consulta é obrigatória (ddMMyyyyHHmm).")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "A data deve ter 12 dígitos.")]
        [SwaggerSchema(Description = "Data e hora da consulta no formato ddMMyyyyHHmm (dia, mês, ano, hora, minuto)")]
        public string DataConsulta { get; set; }

        /// <summary>
        /// Novo status da consulta.
        /// Pode ser "Agendada", "Realizada", "Cancelada" ou outro status definido.
        /// Campo obrigatório.
        /// </summary>
        /// <example>Realizada</example>
        [Required(ErrorMessage = "O status é obrigatório.")]
        [StringLength(20)]
        [SwaggerSchema(Description = "Status da consulta (Agendada, Realizada, Cancelada, Não Realizada)")]
        public string Status { get; set; }
    }
}