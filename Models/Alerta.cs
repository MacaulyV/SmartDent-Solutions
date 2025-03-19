using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartDentAPI.Models
{
    /// <summary>
    /// Representa um alerta gerado após a análise de risco de um paciente.
    /// Essa classe é utilizada para armazenar informações sobre o risco identificado,
    /// incluindo o tipo de alerta, o grau de risco e uma justificativa detalhada.
    /// </summary>
    public class Alerta
    {
        /// <summary>
        /// Identificador único do alerta.
        /// É gerado aleatoriamente usando o método GerarIdAleatorio.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdAlerta { get; set; } = GerarIdAleatorio();

        /// <summary>
        /// Identificador do paciente ao qual o alerta está associado.
        /// Campo obrigatório para relacionar o alerta a um paciente.
        /// </summary>
        [Required]
        public int IdPaciente { get; set; }

        /// <summary>
        /// Propriedade de navegação para o paciente.
        /// Indica que esse alerta pertence a um paciente específico.
        /// </summary>
        [ForeignKey("IdPaciente")]
        public virtual Paciente Paciente { get; set; }

        /// <summary>
        /// Tipo de alerta gerado, por exemplo, "Uso Excessivo Identificado".
        /// Campo obrigatório que descreve o motivo do alerta.
        /// </summary>
        [Required]
        public string TipoAlerta { get; set; }  // Ex: "Uso Excessivo Identificado"

        /// <summary>
        /// Grau de risco associado ao alerta, por exemplo, "59%".
        /// Esse valor indica a severidade do alerta.
        /// </summary>
        [Required]
        public string GrauRisco { get; set; }  // Ex: "59%"

        /// <summary>
        /// Justificativa detalhada que explica o motivo do alerta.
        /// Campo obrigatório que pode incluir informações sobre a análise realizada.
        /// </summary>
        [Required]
        public string Justificativa { get; set; }

        /// <summary>
        /// Data e hora em que o alerta foi gerado.
        /// Valor padrão é a data/hora atual em UTC.
        /// </summary>
        [Required]
        public DateTime DataGeracao { get; set; } = DateTime.UtcNow;

        // Instância estática de Random para garantir que o método GerarIdAleatorio não crie múltiplas instâncias
        private static readonly Random _rand = new Random();

        /// <summary>
        /// Gera um identificador aleatório para o alerta.
        /// O valor gerado é um inteiro entre 100000000 e 999999999.
        /// </summary>
        /// <returns>Um inteiro aleatório usado como ID do alerta.</returns>
        public static int GerarIdAleatorio()
        {
            // Utiliza a instância estática de Random para gerar um número aleatório.
            return _rand.Next(100000000, 1000000000);
        }
    }
}
