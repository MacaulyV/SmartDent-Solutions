using Swashbuckle.AspNetCore.Annotations;

namespace SmartDentAPI.DTOs.Response
{
    /// <summary>
    /// DTO para retornar os dados de uma consulta odontológica.
    /// </summary>
    /// <remarks>
    /// Esse DTO é utilizado para enviar as informações essenciais de uma consulta para o cliente,
    /// como o ID da consulta, o ID do paciente, a data da consulta (em formato "dd/MM/yyyy HH:mm")
    /// e o status da consulta (por exemplo, "Agendada", "Realizada", "Cancelada").
    /// 
    /// Exemplo de resposta:
    /// 
    /// ```json
    /// {
    ///     "idConsulta": 10001,
    ///     "idPaciente": 123456,
    ///     "dataConsulta": "01/05/2024 14:30",
    ///     "status": "Agendada"
    /// }
    /// ```
    /// </remarks>
    [SwaggerSchema(
        Title = "Dados da Consulta", 
        Description = "Informações detalhadas de uma consulta odontológica")]
    public class ConsultaResponseDTO
    {
        /// <summary>
        /// Identificador único da consulta.
        /// </summary>
        /// <example>10001</example>
        [SwaggerSchema(Description = "ID único da consulta no sistema")]
        public int IdConsulta { get; set; }

        /// <summary>
        /// Identificador do paciente associado à consulta.
        /// </summary>
        /// <example>123456</example>
        [SwaggerSchema(Description = "ID do paciente que realizará a consulta")]
        public int IdPaciente { get; set; }

        /// <summary>
        /// Data e hora da consulta, representada como string.
        /// Exemplo: "25/05/2024 14:00".
        /// </summary>
        /// <example>01/05/2024 14:30</example>
        [SwaggerSchema(Description = "Data e hora da consulta em formato legível (dd/MM/yyyy HH:mm)")]
        public string DataConsulta { get; set; }

        /// <summary>
        /// Status da consulta.
        /// Pode ser "Agendada", "Realizada", "Cancelada", entre outros.
        /// </summary>
        /// <example>Agendada</example>
        [SwaggerSchema(Description = "Situação atual da consulta")]
        public string Status { get; set; }
    }
}