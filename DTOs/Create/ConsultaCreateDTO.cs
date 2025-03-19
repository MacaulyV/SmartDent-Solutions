using System.ComponentModel.DataAnnotations;

namespace SmartDentAPI.DTOs.Create
{
    /// <summary>
    /// DTO para criação de uma nova consulta odontológica.
    /// </summary>
    /// <remarks>
    /// Este DTO é utilizado para receber os dados necessários para criar uma consulta,
    /// incluindo o ID do paciente e a data da consulta.
    /// A data deve ser informada no formato "ddMMyyyyHHmm" (12 dígitos).
    /// </remarks>
    public class ConsultaCreateDTO
    {
        /// <summary>
        /// Identificador do paciente que realizou a consulta.
        /// Campo obrigatório.
        /// </summary>
        [Required(ErrorMessage = "O idPaciente é obrigatório.")]
        public int IdPaciente { get; set; }

        /// <summary>
        /// Data e hora da consulta, no formato "ddMMyyyyHHmm".
        /// Deve conter exatamente 12 dígitos.
        /// Campo obrigatório.
        /// </summary>
        [Required(ErrorMessage = "A data da consulta é obrigatória. Informe 12 dígitos (ddMMyyyyHHmm).")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "A data da consulta deve ter 12 dígitos.")]
        public string DataConsulta { get; set; }
    }
}