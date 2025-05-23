using Microsoft.ML.Data;
using System;
using System.Collections.Generic;

namespace SmartDentAPI.ML
{
    /// <summary>
    /// Dados de entrada para o modelo de análise de acompanhamento
    /// </summary>
    public class DadosEntradaAcompanhamento
    {
        /// <summary>
        /// Identificador do paciente
        /// </summary>
        [NoColumn]
        public int IdPaciente { get; set; }

        /// <summary>
        /// Idade do paciente em anos
        /// </summary>
        [LoadColumn(0)]
        public float Idade { get; set; }

        /// <summary>
        /// Número total de consultas
        /// </summary>
        [LoadColumn(1)]
        public float NumConsultasTotal { get; set; }

        /// <summary>
        /// Número de consultas não realizadas (faltas)
        /// </summary>
        [LoadColumn(2)]
        public float NumConsultasNaoRealizadas { get; set; }

        /// <summary>
        /// Número de consultas agendadas expiradas (não atualizadas)
        /// </summary>
        [LoadColumn(3)]
        public float NumConsultasAgendadasExpiradas { get; set; }

        /// <summary>
        /// Percentual de faltas em relação ao total de consultas
        /// </summary>
        [LoadColumn(4)]
        public float PercentualFaltas { get; set; }

        /// <summary>
        /// Tempo (em dias) desde a última consulta realizada
        /// </summary>
        [LoadColumn(5)]
        public float DiasDesdeuUltimaConsulta { get; set; }

        /// <summary>
        /// Rótulo - Se o paciente necessita de acompanhamento especializado
        /// </summary>
        [LoadColumn(6)]
        [ColumnName("Label")]
        public bool NecessitaAcompanhamento { get; set; }

        /// <summary>
        /// Lista de consultas do paciente (não utilizada diretamente pelo modelo)
        /// </summary>
        [NoColumn]
        public List<ConsultaInfo> Consultas { get; set; } = new List<ConsultaInfo>();
    }

    /// <summary>
    /// Informações resumidas de consulta para análise
    /// </summary>
    public class ConsultaInfo
    {
        /// <summary>
        /// Identificador da consulta
        /// </summary>
        public int IdConsulta { get; set; }

        /// <summary>
        /// Data e hora da consulta
        /// </summary>
        public DateTime DataConsulta { get; set; }

        /// <summary>
        /// Status da consulta
        /// </summary>
        public string Status { get; set; }
    }

    /// <summary>
    /// Resultado da predição de necessidade de acompanhamento
    /// </summary>
    public class ResultadoAnaliseAcompanhamento
    {
        /// <summary>
        /// Identificador do paciente analisado
        /// </summary>
        public int IdPaciente { get; set; }

        /// <summary>
        /// Predicado se necessita acompanhamento especializado
        /// </summary>
        [ColumnName("PredictedLabel")]
        public bool NecessitaAcompanhamento { get; set; }

        /// <summary>
        /// Probabilidade de necessidade de acompanhamento
        /// </summary>
        public float Probabilidade { get; set; }

        /// <summary>
        /// Pontuação bruta da predição
        /// </summary>
        [ColumnName("Score")]
        public float Score { get; set; }

        /// <summary>
        /// Justificativa para a recomendação de acompanhamento
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
        /// Número de consultas agendadas expiradas
        /// </summary>
        public int NumConsultasAgendadasExpiradas { get; set; }

        /// <summary>
        /// Data da última consulta realizada
        /// </summary>
        public DateTime? DataUltimaConsulta { get; set; }
    }
} 