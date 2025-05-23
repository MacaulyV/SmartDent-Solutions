using System.ComponentModel.DataAnnotations;

namespace SmartDentAPI.DTOs.Create
{
    /// <summary>
    /// DTO para atualização dos dados de um paciente.
    /// </summary>
    /// <remarks>
    /// Este DTO é utilizado para receber os dados que podem ser atualizados de um paciente,
    /// incluindo nome, e-mail, telefone, endereço, plano odontológico e empresa. 
    /// Campos obrigatórios são validados para garantir integridade dos dados.
    /// </remarks>
    public class PacienteUpdateDTO
    {
        /// <summary>
        /// Nome completo do paciente.
        /// Campo obrigatório.
        /// </summary>
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        public string NomeCompleto { get; set; }

        /// <summary>
        /// E-mail do paciente.
        /// Deve ser um e-mail válido e é obrigatório.
        /// </summary>
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        public string Email { get; set; }

        /// <summary>
        /// Telefone do paciente.
        /// Campo obrigatório e deve ter exatamente 11 dígitos.
        /// </summary>
        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O telefone deve ter exatamente 11 dígitos.")]
        public string Telefone { get; set; }

        /// <summary>
        /// Endereço do paciente.
        /// Campo opcional.
        /// </summary>
        public string Endereco { get; set; }

        /// <summary>
        /// Plano odontológico do paciente.
        /// Campo obrigatório.
        /// </summary>
        [Required(ErrorMessage = "O plano odontológico é obrigatório.")]
        public string PlanoOdontologico { get; set; }
        
        /// <summary>
        /// Nome da empresa associada ao paciente.
        /// Campo opcional com valor padrão "Individual".
        /// </summary>
        [StringLength(100, ErrorMessage = "O nome da empresa deve ter até 100 caracteres.")]
        public string Empresa { get; set; } = "Individual";
    }
}
