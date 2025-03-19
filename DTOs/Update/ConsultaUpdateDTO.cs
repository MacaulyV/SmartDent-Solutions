using System.ComponentModel.DataAnnotations;

namespace SmartDentAPI.DTOs.Update
{
    /// <summary>
    /// DTO para atualização dos dados de uma consulta odontológica.
    /// </summary>
    /// <remarks>
    /// Este DTO permite atualizar a data e o status de uma consulta.
    /// A data deve estar no formato "ddMMyyyyHHmm" e conter exatamente 12 dígitos.
    /// O status indica a situação da consulta, podendo ser "Agendada", "Realizada", "Cancelada", etc.
    /// </remarks>
    public class ConsultaUpdateDTO
    {
        /// <summary>
        /// Nova data da consulta no formato "ddMMyyyyHHmm".
        /// Campo obrigatório para atualização.
        /// </summary>
        [Required(ErrorMessage = "A nova data da consulta é obrigatória (ddMMyyyyHHmm).")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "A data deve ter 12 dígitos.")]
        public string DataConsulta { get; set; }

        /// <summary>
        /// Novo status da consulta.
        /// Pode ser "Agendada", "Realizada", "Cancelada" ou outro status definido.
        /// Campo obrigatório.
        /// </summary>
        [Required(ErrorMessage = "O status é obrigatório.")]
        [StringLength(20)]
        public string Status { get; set; }
    }
}