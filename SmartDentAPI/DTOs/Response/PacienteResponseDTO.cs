namespace SmartDentAPI.DTOs.Response
{
    /// <summary>
    /// DTO para retornar os dados de um paciente.
    /// </summary>
    /// <remarks>
    /// Este DTO é utilizado para enviar as informações básicas do paciente para o cliente,
    /// incluindo o ID, nome, CPF, plano odontológico, empresa, número de consultas e gasto total.
    /// </remarks>
    public class PacienteResponseDTO
    {
        /// <summary>
        /// Identificador único do paciente.
        /// </summary>
        public int IdPaciente { get; set; }

        /// <summary>
        /// Nome completo do paciente.
        /// </summary>
        public string NomeCompleto { get; set; }

        /// <summary>
        /// CPF do paciente.
        /// </summary>
        public string CPF { get; set; }

        /// <summary>
        /// Plano odontológico do paciente.
        /// </summary>
        public string PlanoOdontologico { get; set; }

        /// <summary>
        /// Empresa associada ao paciente.
        /// Valor padrão é "Individual".
        /// </summary>
        public string Empresa { get; set; } = "Individual";

        /// <summary>
        /// Número de consultas realizadas pelo paciente.
        /// </summary>
        public int NumConsultas { get; set; }

        /// <summary>
        /// Gasto total do paciente, representado como string.
        /// </summary>
        public string GastoTotal { get; set; }
    }
}