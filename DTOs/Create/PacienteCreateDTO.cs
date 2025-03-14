using System.ComponentModel.DataAnnotations;

namespace SmartDentAPI.DTOs.Create
{
    /// <summary>
    /// DTO para criação de um novo paciente.
    /// </summary>
    /// <remarks>
    /// Este DTO é utilizado para receber os dados necessários para criar um paciente,
    /// incluindo informações pessoais e detalhes do plano odontológico.
    /// </remarks>
    public class PacienteCreateDTO
    {
        /// <summary>
        /// Nome completo do paciente.
        /// Campo obrigatório.
        /// </summary>
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        public string NomeCompleto { get; set; }

        /// <summary>
        /// CPF do paciente.
        /// Deve conter exatamente 11 dígitos.
        /// Campo obrigatório.
        /// </summary>
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter exatamente 11 dígitos.")]
        public string CPF { get; set; }

        /// <summary>
        /// Data de nascimento do paciente.
        /// Deve ser informada com 8 dígitos no formato "ddmmaaaa".
        /// Campo obrigatório.
        /// </summary>
        [Required(ErrorMessage = "A data de nascimento é obrigatória. Informe 8 dígitos (ddmmaaaa).")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "A data de nascimento deve ter 8 dígitos.")]
        public string DataNascimento { get; set; }

        /// <summary>
        /// E-mail do paciente.
        /// Campo obrigatório, com validação para formato de e-mail.
        /// </summary>
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        public string Email { get; set; }

        /// <summary>
        /// Telefone do paciente.
        /// Deve conter exatamente 11 dígitos.
        /// Campo obrigatório.
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
        /// Empresa associada ao paciente.
        /// Campo opcional com valor padrão "Individual".
        /// </summary>
        [StringLength(100, ErrorMessage = "O nome da empresa deve ter até 100 caracteres.")]
        public string Empresa { get; set; } = "Individual";
    }
}
