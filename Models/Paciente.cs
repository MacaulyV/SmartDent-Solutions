using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartDentAPI.Models
{
    /// <summary>
    /// Representa um paciente no sistema SmartDent.
    /// Esta classe mapeia a tabela "Pacientes" no banco de dados.
    /// </summary>
    [Table("Pacientes")]
    public class Paciente
    {
        /// <summary>
        /// Identificador único do paciente. 
        /// A geração do ID é controlada externamente, por isso não é autogerado.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdPaciente { get; set; }

        /// <summary>
        /// Nome completo do paciente.
        /// Campo obrigatório com tamanho máximo de 100 caracteres.
        /// </summary>
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome completo deve ter até 100 caracteres.")]
        public string NomeCompleto { get; set; }

        /// <summary>
        /// CPF do paciente.
        /// Campo obrigatório e deve conter exatamente 11 dígitos.
        /// </summary>
        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve conter exatamente 11 dígitos.")]
        public string CPF { get; set; }

        /// <summary>
        /// Data de nascimento do paciente.
        /// Este campo é armazenado como string no formato "ddmmaaaa".
        /// </summary>
        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "A data de nascimento deve ter 8 dígitos (ddmmaaaa).")]
        public string DataNascimento { get; set; }

        /// <summary>
        /// Propriedade não mapeada, utilizada para converter a data de nascimento para DateTime.
        /// </summary>
        [NotMapped]
        public DateTime? DataNascimentoConvertida { get; set; }

        /// <summary>
        /// E-mail do paciente.
        /// Campo obrigatório, com validação de formato e tamanho máximo de 100 caracteres.
        /// </summary>
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        [StringLength(100)]
        public string Email { get; set; }

        /// <summary>
        /// Telefone do paciente.
        /// Campo obrigatório e deve conter exatamente 11 dígitos.
        /// </summary>
        [Required(ErrorMessage = "O telefone é obrigatório. Deve conter 11 dígitos.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O telefone deve ter exatamente 11 dígitos.")]
        public string Telefone { get; set; }

        /// <summary>
        /// Endereço do paciente.
        /// Campo opcional com tamanho máximo de 200 caracteres.
        /// </summary>
        [StringLength(200, ErrorMessage = "O endereço pode ter no máximo 200 caracteres.")]
        public string Endereco { get; set; }

        /// <summary>
        /// Plano odontológico do paciente.
        /// Campo obrigatório com tamanho máximo de 50 caracteres.
        /// </summary>
        [Required(ErrorMessage = "O plano odontológico é obrigatório.")]
        [StringLength(50)]
        public string PlanoOdontologico { get; set; }

        /// <summary>
        /// Número de consultas realizadas pelo paciente.
        /// Valor padrão é 0.
        /// </summary>
        public int NumConsultas { get; set; } = 0;
        
        /// <summary>
        /// Nome da empresa associada ao paciente.
        /// Campo opcional com tamanho máximo de 100 caracteres. O valor padrão é "Individual".
        /// </summary>
        [StringLength(100, ErrorMessage = "O nome da empresa deve ter até 100 caracteres.")]
        public string Empresa { get; set; } = "Individual"; 

        /// <summary>
        /// Gera um identificador aleatório para um paciente.
        /// Este método pode ser utilizado para testes ou para gerar IDs quando necessário.
        /// </summary>
        /// <returns>Um número aleatório entre 1 e 999999999.</returns>
        public static int GerarIdAleatorio()
        {
            // Cria uma instância de Random para gerar um número aleatório.
            Random rnd = new Random();
            return rnd.Next(1, 999999999);
        }
    }
}
