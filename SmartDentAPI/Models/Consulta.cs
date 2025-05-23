using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartDentAPI.Models
{
    /// <summary>
    /// Representa uma consulta odontológica realizada por um paciente.
    /// Mapeia a tabela "Consultas" no banco de dados.
    /// </summary>
    [Table("Consultas")]
    public class Consulta
    {
        /// <summary>
        /// Identificador único da consulta.
        /// Este valor é fornecido externamente e não gerado automaticamente.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdConsulta { get; set; }

        /// <summary>
        /// Identificador do paciente que realizou a consulta.
        /// Campo obrigatório para associar a consulta a um paciente.
        /// </summary>
        [Required(ErrorMessage = "O idPaciente é obrigatório.")]
        public int IdPaciente { get; set; } 

        /// <summary>
        /// Data e hora da consulta, armazenada como string.
        /// Este campo é obrigatório e deve ter exatamente 12 dígitos (ex.: "ddMMyyyyHHmm").
        /// </summary>
        [Required(ErrorMessage = "A data da consulta é obrigatória.")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "DataConsultaTexto deve ter 12 dígitos.")]
        public string DataConsulta { get; set; }

        /// <summary>
        /// Propriedade não mapeada que armazena a data da consulta convertida para DateTime.
        /// Essa propriedade pode ser utilizada para operações que exijam manipulação de data/hora.
        /// </summary>
        [NotMapped]
        public DateTime? DataConsultaConvertida { get; set; }

        /// <summary>
        /// Status da consulta (ex: "Agendada", "Realizada", "Cancelada").
        /// O valor padrão é "Agendada".
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Agendada";

        /// <summary>
        /// Gera um identificador aleatório para uma consulta.
        /// Pode ser utilizado para fins de teste ou para gerar IDs quando necessário.
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
