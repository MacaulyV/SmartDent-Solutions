using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartDentAPI.Models
{
    /// <summary>
    /// Representa um procedimento odontológico realizado em uma consulta.
    /// Mapeia a tabela "Procedimentos" no banco de dados.
    /// </summary>
    [Table("Procedimentos")]
    public class Procedimento
    {
        /// <summary>
        /// Identificador único do procedimento.
        /// Este valor não é gerado automaticamente pelo banco de dados.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdProcedimento { get; set; }

        /// <summary>
        /// Identificador da consulta à qual este procedimento está associado.
        /// Campo obrigatório.
        /// </summary>
        [Required]
        public int IdConsulta { get; set; }

        /// <summary>
        /// Tipo do procedimento realizado (ex: "Limpeza dental", "Extração de dente").
        /// Campo obrigatório com tamanho máximo de 50 caracteres.
        /// </summary>
        [Required(ErrorMessage = "O tipo de procedimento é obrigatório.")]
        [StringLength(50)]
        public string TipoProcedimento { get; set; }

        /// <summary>
        /// Descrição adicional do procedimento.
        /// Esse campo é opcional, por isso a utilização de string nullable.
        /// </summary>
        [StringLength(300)]
        public string? Descricao { get; set; }

        /// <summary>
        /// Custo do procedimento.
        /// Armazenado como decimal para precisão financeira.
        /// </summary>
        public decimal Custo { get; set; }

        /// <summary>
        /// Gera um identificador aleatório para um procedimento.
        /// Útil para testes ou para a criação de IDs quando necessário.
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
