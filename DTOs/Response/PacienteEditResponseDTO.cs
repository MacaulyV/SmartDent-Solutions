namespace SmartDentAPI.DTOs.Response
{
    /// <summary>
    /// DTO para retorno dos dados de edição de um paciente.
    /// </summary>
    /// <remarks>
    /// Este DTO é utilizado quando os dados do paciente precisam ser editados.
    /// Retorna as informações que podem ser alteradas, como nome, e-mail, telefone, endereço, plano odontológico e empresa.
    /// </remarks>
    public class PacienteEditResponseDTO
    {
        /// <summary>
        /// Nome completo do paciente.
        /// </summary>
        public string NomeCompleto { get; set; }

        /// <summary>
        /// E-mail do paciente.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Telefone do paciente.
        /// </summary>
        public string Telefone { get; set; }

        /// <summary>
        /// Endereço do paciente.
        /// </summary>
        public string Endereco { get; set; }

        /// <summary>
        /// Plano odontológico do paciente.
        /// </summary>
        public string PlanoOdontologico { get; set; }

        /// <summary>
        /// Empresa associada ao paciente.
        /// Valor padrão é "Individual".
        /// </summary>
        public string Empresa { get; set; } = "Individual";
    }
}