using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartDentAPI.DTOs.Update
{
    /// <summary>
    /// DTO utilizado para atualizar os dados de um procedimento odontológico.
    /// </summary>
    /// <remarks>
    /// Este DTO permite atualizar o tipo de procedimento e sua descrição.
    /// O campo TipoProcedimento é obrigatório para garantir que o procedimento seja identificado corretamente.
    /// 
    /// Exemplo de requisição:
    /// 
    /// ```json
    /// {
    ///     "tipoProcedimento": "Restauração em amálgama",
    ///     "descricao": "Restauração de amálgama no dente 36"
    /// }
    /// ```
    /// </remarks>
    [SwaggerSchema(
        Title = "Atualização de Procedimento", 
        Description = "Dados para atualizar um procedimento odontológico existente")]
    public class ProcedimentoUpdateDTO
    {
        /// <summary>
        /// Tipo (nome) do procedimento.
        /// Campo obrigatório.
        /// </summary>
        /// <example>Restauração em amálgama</example>
        [Required(ErrorMessage = "O tipo de procedimento é obrigatório.")]
        [SwaggerSchema(Description = "Tipo ou nome do procedimento odontológico realizado (deve constar na tabela de custos)")]
        public string TipoProcedimento { get; set; }

        /// <summary>
        /// Descrição adicional do procedimento.
        /// Campo opcional.
        /// </summary>
        /// <example>Restauração de amálgama no dente 36</example>
        [SwaggerSchema(Description = "Descrição detalhada do procedimento com informações adicionais")]
        public string Descricao { get; set; }
    }
}