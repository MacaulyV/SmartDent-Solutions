using System.ComponentModel.DataAnnotations;

namespace SmartDentAPI.DTOs.Update
{
    /// <summary>
    /// DTO para atualização dos dados de um alerta.
    /// </summary>
    /// <remarks>
    /// Este DTO é usado para atualizar informações de um alerta, incluindo o tipo de alerta,
    /// o grau de risco (por exemplo, "59%") e a justificativa que explica o motivo do alerta.
    /// Todos os campos são obrigatórios para garantir que o alerta contenha as informações necessárias.
    /// </remarks>
    public class AlertaUpdateDTO
    {
        /// <summary>
        /// Tipo de alerta.
        /// Por exemplo, "Uso Excessivo Identificado".
        /// Campo obrigatório.
        /// </summary>
        [Required]
        public string TipoAlerta { get; set; }

        /// <summary>
        /// Grau de risco associado ao alerta.
        /// Exemplo: "59%".
        /// Campo obrigatório.
        /// </summary>
        [Required]
        public string GrauRisco { get; set; } // ex: "59%"

        /// <summary>
        /// Justificativa detalhada para o alerta.
        /// Campo obrigatório que explica o motivo do alerta.
        /// </summary>
        [Required]
        public string Justificativa { get; set; }
    }
}