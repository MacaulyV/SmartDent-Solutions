using System.ComponentModel.DataAnnotations;

namespace SmartDentAPI.DTOs.Create
{
    /// <summary>
    /// DTO para criação de um novo alerta.
    /// </summary>
    /// <remarks>
    /// Este DTO é utilizado para receber os dados necessários para criar um alerta,
    /// associando-o a um paciente e definindo o tipo de alerta, o grau de risco (ex.: "59%")
    /// e uma justificativa que explica o motivo do alerta.
    /// </remarks>
    public class AlertaCreateDTO
    {
        /// <summary>
        /// Identificador do paciente ao qual o alerta será associado.
        /// Campo obrigatório.
        /// </summary>
        [Required]
        public int IdPaciente { get; set; }

        /// <summary>
        /// Tipo do alerta que está sendo gerado.
        /// Por exemplo, "Uso Excessivo Identificado".
        /// Campo obrigatório.
        /// </summary>
        [Required]
        public string TipoAlerta { get; set; }

        /// <summary>
        /// Grau de risco associado ao alerta, representado como string (por exemplo, "59%").
        /// Campo obrigatório.
        /// </summary>
        [Required]
        public string GrauRisco { get; set; }  // ex: "59%"

        /// <summary>
        /// Justificativa que explica o motivo do alerta.
        /// Campo obrigatório.
        /// </summary>
        [Required]
        public string Justificativa { get; set; }
    }
}