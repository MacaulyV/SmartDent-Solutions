using System;
using System.Collections.Generic;

namespace SmartDentAPI.DTOs.Response
{
    /// <summary>
    /// DTO para resposta da análise de necessidade de acompanhamento
    /// </summary>
    public class AcompanhamentoAnaliseResponseDTO
    {
        /// <summary>
        /// ID do paciente analisado
        /// </summary>
        public int IdPaciente { get; set; }
        
        /// <summary>
        /// Nome do paciente analisado
        /// </summary>
        public string NomePaciente { get; set; }
        
        /// <summary>
        /// Indica se o paciente necessita de acompanhamento especializado
        /// </summary>
        public bool NecessitaAcompanhamento { get; set; }
        
        /// <summary>
        /// Probabilidade de necessidade de acompanhamento (0-100%)
        /// </summary>
        public int ProbabilidadePercentual { get; set; }
        
        /// <summary>
        /// Justificativa detalhada para a recomendação
        /// </summary>
        public string Justificativa { get; set; }
        
        /// <summary>
        /// Detalhes das consultas consideradas na análise
        /// </summary>
        public List<string> DetalhesConsultas { get; set; } = new List<string>();
        
        /// <summary>
        /// Número de consultas não realizadas
        /// </summary>
        public int NumConsultasNaoRealizadas { get; set; }
        
        /// <summary>
        /// Número de consultas agendadas expiradas (não atualizadas)
        /// </summary>
        public int NumConsultasAgendadasExpiradas { get; set; }
        
        /// <summary>
        /// Data da última consulta realizada
        /// </summary>
        public DateTime? DataUltimaConsulta { get; set; }
        
        /// <summary>
        /// Data e hora da análise
        /// </summary>
        public DateTime DataAnalise { get; set; } = DateTime.Now;
    }
} 