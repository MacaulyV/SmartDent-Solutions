using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartDentAPI.DTOs.Create
{
    /// <summary>
    /// DTO para criação de um novo paciente.
    /// </summary>
    /// <remarks>
    /// Este DTO é utilizado para receber os dados necessários para criar um paciente,
    /// incluindo informações pessoais e detalhes do plano odontológico.
    /// 
    /// Exemplo de requisição:
    /// 
    /// ```json
    /// {
    ///     "nomeCompleto": "Maria Silva",
    ///     "cpf": "12345678901",
    ///     "dataNascimento": "01052000",
    ///     "email": "maria.silva@email.com",
    ///     "telefone": "11987654321",
    ///     "endereco": "Rua das Flores, 123 - São Paulo/SP",
    ///     "planoOdontologico": "Bem-Estar Pró",
    ///     "empresa": "Individual"
    /// }
    /// ```
    /// </remarks>
    [SwaggerSchema(
        Title = "Criação de Paciente", 
        Description = "Dados para cadastro de um novo paciente no sistema")]
    public class PacienteCreateDTO
    {
        /// <summary>
        /// Nome completo do paciente.
        /// Campo obrigatório.
        /// </summary>
        /// <example>Maria Silva</example>
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [SwaggerSchema(Description = "Nome completo do paciente")]
        public string NomeCompleto { get; set; }

        /// <summary>
        /// CPF do paciente.
        /// Deve conter exatamente 11 dígitos.
        /// Campo obrigatório.
        /// </summary>
        /// <example>12345678901</example>
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter exatamente 11 dígitos.")]
        [SwaggerSchema(Description = "CPF do paciente (apenas números, sem pontuação)")]
        public string CPF { get; set; }

        /// <summary>
        /// Data de nascimento do paciente.
        /// Deve ser informada com 8 dígitos no formato "ddmmaaaa".
        /// Campo obrigatório.
        /// </summary>
        /// <example>01052000</example>
        [Required(ErrorMessage = "A data de nascimento é obrigatória. Informe 8 dígitos (ddmmaaaa).")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "A data de nascimento deve ter 8 dígitos.")]
        [SwaggerSchema(Description = "Data de nascimento no formato ddmmaaaa (dia, mês, ano)")]
        public string DataNascimento { get; set; }

        /// <summary>
        /// E-mail do paciente.
        /// Campo obrigatório, com validação para formato de e-mail.
        /// </summary>
        /// <example>maria.silva@email.com</example>
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        [SwaggerSchema(Description = "E-mail válido do paciente")]
        public string Email { get; set; }

        /// <summary>
        /// Telefone do paciente.
        /// Deve conter exatamente 11 dígitos.
        /// Campo obrigatório.
        /// </summary>
        /// <example>11987654321</example>
        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O telefone deve ter exatamente 11 dígitos.")]
        [SwaggerSchema(Description = "Telefone do paciente com DDD (11 dígitos, apenas números)")]
        public string Telefone { get; set; }

        /// <summary>
        /// Endereço do paciente.
        /// Campo opcional.
        /// </summary>
        /// <example>Rua das Flores, 123 - São Paulo/SP</example>
        [SwaggerSchema(Description = "Endereço completo do paciente")]
        public string Endereco { get; set; }

        /// <summary>
        /// Plano odontológico do paciente.
        /// Campo obrigatório.
        /// </summary>
        /// <example>Bem-Estar Pró</example>
        [Required(ErrorMessage = "O plano odontológico é obrigatório.")]
        [SwaggerSchema(Description = "Plano odontológico do paciente (ex: Bem-Estar, Bem-Estar Pró, Bem-Estar Orto)")]
        public string PlanoOdontologico { get; set; }

        /// <summary>
        /// Empresa associada ao paciente.
        /// Campo opcional com valor padrão "Individual".
        /// </summary>
        /// <example>Individual</example>
        [StringLength(100, ErrorMessage = "O nome da empresa deve ter até 100 caracteres.")]
        [SwaggerSchema(Description = "Empresa associada ao paciente. Utilizar 'Individual' para pacientes sem vínculo empresarial")]
        public string Empresa { get; set; } = "Individual";
    }
}
