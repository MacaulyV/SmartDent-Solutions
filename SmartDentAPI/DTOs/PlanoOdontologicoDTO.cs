namespace SmartDentAPI.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) para representar um plano odontológico.
    /// </summary>
    /// <remarks>
    /// Esse DTO é utilizado para transferir informações básicas do plano odontológico,
    /// neste caso, somente o nome do plano.
    /// </remarks>
    public class PlanoOdontologicoDTO
    {
        /// <summary>
        /// Nome do plano odontológico.
        /// </summary>
        public string Nome { get; set; }
    }
}