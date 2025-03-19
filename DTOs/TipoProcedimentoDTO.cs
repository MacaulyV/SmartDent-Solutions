namespace SmartDentAPI.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) para representar o tipo de procedimento odontológico.
    /// </summary>
    /// <remarks>
    /// Esse DTO é usado para transferir dados resumidos do procedimento, contendo o nome do procedimento e seu custo.
    /// Ideal para operações de listagem ou exibição, sem incluir informações detalhadas.
    /// </remarks>
    public class TipoProcedimentoDTO
    {
        /// <summary>
        /// O tipo (nome) do procedimento.
        /// </summary>
        public string TipoProcedimento { get; set; }

        /// <summary>
        /// O custo do procedimento.
        /// </summary>
        public decimal Custo { get; set; }
    }
}