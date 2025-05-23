using System.ComponentModel.DataAnnotations;

namespace SmartDentAPI.DTOs.Create
{
    /// <summary>
    /// DTO para autenticação de pacientes através de nome completo e CPF
    /// </summary>
    public class PacienteLoginDTO
    {
        /// <summary>
        /// Nome completo do paciente, deve ser igual ao cadastrado no sistema
        /// </summary>
        [Required(ErrorMessage = "O nome completo é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome completo deve ter no máximo 100 caracteres")]
        public string NomeCompleto { get; set; }

        /// <summary>
        /// CPF do paciente, apenas números (11 dígitos)
        /// </summary>
        [Required(ErrorMessage = "O CPF é obrigatório")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter 11 dígitos")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter apenas números")]
        public string CPF { get; set; }
    }
} 