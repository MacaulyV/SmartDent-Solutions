using System;

namespace SmartDentAPI.DTOs.Response
{
    /// <summary>
    /// DTO para retornar os dados de um alerta gerado pela análise da IA.
    /// </summary>
    /// <remarks>
    /// Este DTO contém as informações essenciais sobre o alerta, incluindo o ID do alerta,
    /// o ID do paciente associado, o tipo de alerta, o grau de risco (ex: "59%"), uma justificativa detalhada,
    /// e a data/hora em que o alerta foi gerado.
    /// </remarks>
    public class AlertaResponseDTO
    {
        /// <summary>
        /// Identificador único do alerta.
        /// </summary>
        public int IdAlerta { get; set; }

        /// <summary>
        /// Identificador do paciente associado a este alerta.
        /// </summary>
        public int IdPaciente { get; set; }

        /// <summary>
        /// Tipo do alerta gerado.
        /// Por exemplo, "Uso Excessivo Identificado".
        /// </summary>
        public string TipoAlerta { get; set; }

        /// <summary>
        /// Grau de risco associado ao alerta, representado como uma string (ex: "59%").
        /// </summary>
        public string GrauRisco { get; set; }  // ex: "59%"

        /// <summary>
        /// Justificativa que explica o motivo do alerta.
        /// </summary>
        public string Justificativa { get; set; }

        /// <summary>
        /// Data e hora em que o alerta foi gerado.
        /// </summary>
        public DateTime DataGeracao { get; set; }
    }
}