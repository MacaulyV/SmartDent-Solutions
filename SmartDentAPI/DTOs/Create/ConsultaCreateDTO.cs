using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartDentAPI.DTOs.Create
{
    /// <summary>
    /// DTO para criação de uma nova consulta odontológica.
    /// </summary>
    /// <remarks>
    /// Este DTO é utilizado para receber os dados necessários para criar uma consulta,
    /// incluindo o ID do paciente e a data da consulta.
    /// A data deve ser informada no formato "ddMMyyyyHHmm" (12 dígitos).
    /// 
    /// Exemplo de requisição:
    /// 
    /// ```json
    /// {
    ///     "idPaciente": 123456,
    ///     "dataConsulta": "010520241430"
    /// }
    /// ```
    /// </remarks>
    [SwaggerSchema(
        Title = "Criação de Consulta", 
        Description = "Dados para agendamento de uma nova consulta no sistema")]
    public class ConsultaCreateDTO
    {
        /// <summary>
        /// Identificador do paciente que realizou a consulta.
        /// Campo obrigatório.
        /// </summary>
        /// <example>123456</example>
        [Required(ErrorMessage = "O idPaciente é obrigatório.")]
        [SwaggerSchema(Description = "ID do paciente existente no sistema")]
        public int IdPaciente { get; set; }

        /// <summary>
        /// Data e hora da consulta, no formato "ddMMyyyyHHmm".
        /// Deve conter exatamente 12 dígitos.
        /// Campo obrigatório.
        /// </summary>
        /// <example>010520241430</example>
        [Required(ErrorMessage = "A data da consulta é obrigatória. Informe 12 dígitos (ddMMyyyyHHmm).")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "A data da consulta deve ter 12 dígitos.")]
        [SwaggerSchema(Description = "Data e hora da consulta no formato ddMMyyyyHHmm (dia, mês, ano, hora, minuto)")]
        public string DataConsulta { get; set; }
    }
}