using System.ComponentModel.DataAnnotations;

namespace SmartDentAPI.DTOs.Update
{
    /// <summary>
    /// DTO utilizado para atualizar os dados de um procedimento odontológico.
    /// </summary>
    /// <remarks>
    /// Este DTO permite atualizar o tipo de procedimento e sua descrição.
    /// O campo TipoProcedimento é obrigatório para garantir que o procedimento seja identificado corretamente.
    /// </remarks>
    public class ProcedimentoUpdateDTO
    {
        /// <summary>
        /// Tipo (nome) do procedimento.
        /// Campo obrigatório.
        /// </summary>
        [Required(ErrorMessage = "O tipo de procedimento é obrigatório.")]
        public string TipoProcedimento { get; set; }

        /// <summary>
        /// Descrição adicional do procedimento.
        /// Campo opcional.
        /// </summary>
        public string Descricao { get; set; }
    }
}