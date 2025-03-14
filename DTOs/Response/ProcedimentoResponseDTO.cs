namespace SmartDentAPI.DTOs.Response
{
    /// <summary>
    /// Data Transfer Object (DTO) para a resposta dos dados de um procedimento odontológico.
    /// </summary>
    /// <remarks>
    /// Este DTO é utilizado para retornar os dados do procedimento para o cliente,
    /// incluindo o ID do procedimento, o ID da consulta à qual pertence, o tipo do procedimento,
    /// a descrição e o custo (armazenado como string).
    /// </remarks>
    public class ProcedimentoResponseDTO
    {
        /// <summary>
        /// Identificador único do procedimento.
        /// </summary>
        public int IdProcedimento { get; set; }

        /// <summary>
        /// Identificador da consulta à qual este procedimento está associado.
        /// </summary>
        public int IdConsulta { get; set; }

        /// <summary>
        /// Tipo (nome) do procedimento.
        /// </summary>
        public string TipoProcedimento { get; set; }

        /// <summary>
        /// Descrição adicional do procedimento.
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Custo do procedimento, armazenado como string.
        /// </summary>
        public string Custo { get; set; }
    }
}