using System.ComponentModel.DataAnnotations;

namespace SmartDentAPI.DTOs.Create
{
    /// <summary>
    /// DTO para criação de um novo procedimento odontológico.
    /// </summary>
    /// <remarks>
    /// Esse DTO é utilizado para receber os dados necessários para criar um procedimento.
    /// Inclui o ID da consulta à qual o procedimento está associado, o tipo do procedimento
    /// e uma descrição opcional.
    /// </remarks>
    public class ProcedimentoCreateDTO
    {
        /// <summary>
        /// Identificador da consulta à qual o procedimento pertence.
        /// Campo obrigatório.
        /// </summary>
        [Required(ErrorMessage = "O idConsulta é obrigatório.")]
        public int IdConsulta { get; set; }

        /// <summary>
        /// Tipo (nome) do procedimento.
        /// Campo obrigatório para identificar o procedimento.
        /// </summary>
        [Required(ErrorMessage = "O tipo de procedimento é obrigatório.")]
        public string TipoProcedimento { get; set; }

        /// <summary>
        /// Descrição adicional do procedimento.
        /// Esse campo não é obrigatório e pode ser nulo.
        /// </summary>
        public string Descricao { get; set; }
    }
}