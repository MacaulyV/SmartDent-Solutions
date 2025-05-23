using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartDentAPI.DTOs.Create
{
    /// <summary>
    /// DTO para criação de um novo alerta.
    /// </summary>
    /// <remarks>
    /// Este DTO é utilizado para receber os dados necessários para criar um alerta,
    /// associando-o a um paciente e definindo o tipo de alerta, o grau de risco (ex.: "59%")
    /// e uma justificativa que explica o motivo do alerta.
    /// 
    /// Exemplo de requisição:
    /// 
    /// ```json
    /// {
    ///     "idPaciente": 123456,
    ///     "tipoAlerta": "Uso Excessivo Identificado",
    ///     "grauRisco": "59%",
    ///     "justificativa": "Paciente realizou 5 consultas em menos de 30 dias sem justificativa clínica"
    /// }
    /// ```
    /// </remarks>
    [SwaggerSchema(
        Title = "Criação de Alerta", 
        Description = "Dados para registro de um novo alerta no sistema")]
    public class AlertaCreateDTO
    {
        /// <summary>
        /// Identificador do paciente ao qual o alerta será associado.
        /// Campo obrigatório.
        /// </summary>
        /// <example>123456</example>
        [Required(ErrorMessage = "O ID do paciente é obrigatório.")]
        [SwaggerSchema(Description = "ID do paciente existente no sistema")]
        public int IdPaciente { get; set; }

        /// <summary>
        /// Tipo do alerta que está sendo gerado.
        /// Por exemplo, "Uso Excessivo Identificado".
        /// Campo obrigatório.
        /// </summary>
        /// <example>Uso Excessivo Identificado</example>
        [Required(ErrorMessage = "O tipo de alerta é obrigatório.")]
        [SwaggerSchema(Description = "Tipo ou categoria do alerta")]
        public string TipoAlerta { get; set; }

        /// <summary>
        /// Grau de risco associado ao alerta, representado como string (por exemplo, "59%").
        /// Campo obrigatório.
        /// </summary>
        /// <example>59%</example>
        [Required(ErrorMessage = "O grau de risco é obrigatório.")]
        [SwaggerSchema(Description = "Indicação percentual do grau de risco associado ao alerta")]
        public string GrauRisco { get; set; }  // ex: "59%"

        /// <summary>
        /// Justificativa que explica o motivo do alerta.
        /// Campo obrigatório.
        /// </summary>
        /// <example>Paciente realizou 5 consultas em menos de 30 dias sem justificativa clínica</example>
        [Required(ErrorMessage = "A justificativa é obrigatória.")]
        [SwaggerSchema(Description = "Explicação detalhada do motivo pelo qual o alerta foi gerado")]
        public string Justificativa { get; set; }
    }
}