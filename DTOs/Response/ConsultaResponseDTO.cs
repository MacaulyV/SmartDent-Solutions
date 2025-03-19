namespace SmartDentAPI.DTOs.Response
{
    /// <summary>
    /// DTO para retornar os dados de uma consulta odontológica.
    /// </summary>
    /// <remarks>
    /// Esse DTO é utilizado para enviar as informações essenciais de uma consulta para o cliente,
    /// como o ID da consulta, o ID do paciente, a data da consulta (em formato "dd/MM/yyyy HH:mm")
    /// e o status da consulta (por exemplo, "Agendada", "Realizada", "Cancelada").
    /// </remarks>
    public class ConsultaResponseDTO
    {
        /// <summary>
        /// Identificador único da consulta.
        /// </summary>
        public int IdConsulta { get; set; }

        /// <summary>
        /// Identificador do paciente associado à consulta.
        /// </summary>
        public int IdPaciente { get; set; }

        /// <summary>
        /// Data e hora da consulta, representada como string.
        /// Exemplo: "25/05/2024 14:00".
        /// </summary>
        public string DataConsulta { get; set; }

        /// <summary>
        /// Status da consulta.
        /// Pode ser "Agendada", "Realizada", "Cancelada", entre outros.
        /// </summary>
        public string Status { get; set; }
    }
}