using Microsoft.ML.Data;
using System;
using System.Collections.Generic;

namespace SmartDentAPI.ML
{
    /// <summary>
    /// Dados de entrada para o modelo de análise de acompanhamento especializado.
    /// </summary>
    /// <remarks>
    /// Esta classe define os atributos usados pelo modelo de ML.NET para predizer se um paciente 
    /// necessita de acompanhamento especializado com base em seu histórico de consultas.
    /// Os atributos decorados com LoadColumn são as features utilizadas no treinamento e predição.
    /// </remarks>
    public class DadosEntradaAcompanhamento
    {
        /// <summary>
        /// Identificador único do paciente no sistema.
        /// </summary>
        /// <remarks>
        /// Este campo não é utilizado como feature pelo modelo ML.NET.
        /// </remarks>
        [NoColumn]
        public int IdPaciente { get; set; }

        /// <summary>
        /// Idade do paciente em anos, calculada a partir da data de nascimento.
        /// </summary>
        /// <remarks>
        /// A idade pode ser relevante para análise de padrões específicos por faixa etária.
        /// </remarks>
        [LoadColumn(0)]
        public float Idade { get; set; }

        /// <summary>
        /// Número total de consultas registradas para o paciente.
        /// </summary>
        /// <remarks>
        /// Usado para analisar o volume de interações do paciente com a clínica.
        /// </remarks>
        [LoadColumn(1)]
        public float NumConsultasTotal { get; set; }

        /// <summary>
        /// Número de consultas não realizadas (faltas) pelo paciente.
        /// </summary>
        /// <remarks>
        /// Um indicador importante de possível desengajamento do paciente com o tratamento.
        /// </remarks>
        [LoadColumn(2)]
        public float NumConsultasNaoRealizadas { get; set; }

        /// <summary>
        /// Número de consultas agendadas cuja data já passou, mas que não foram atualizadas.
        /// </summary>
        /// <remarks>
        /// Indica possíveis falhas no processo de acompanhamento ou atualização de registros.
        /// </remarks>
        [LoadColumn(3)]
        public float NumConsultasAgendadasExpiradas { get; set; }

        /// <summary>
        /// Percentual de faltas em relação ao total de consultas do paciente.
        /// </summary>
        /// <remarks>
        /// Proporciona uma visão relativa do índice de faltas, normalizada pelo total de consultas.
        /// </remarks>
        [LoadColumn(4)]
        public float PercentualFaltas { get; set; }

        /// <summary>
        /// Tempo, em dias, desde a última consulta realizada pelo paciente.
        /// </summary>
        /// <remarks>
        /// Ajuda a identificar pacientes que estão há muito tempo sem comparecer a consultas.
        /// </remarks>
        [LoadColumn(5)]
        public float DiasDesdeuUltimaConsulta { get; set; }

        /// <summary>
        /// Rótulo (label) que indica se o paciente necessita de acompanhamento especializado.
        /// </summary>
        /// <remarks>
        /// Esta é a variável alvo (target) que o modelo tentará predizer durante o treinamento e inferência.
        /// </remarks>
        [LoadColumn(6)]
        [ColumnName("Label")]
        public bool NecessitaAcompanhamento { get; set; }

        /// <summary>
        /// Lista de consultas do paciente para análise detalhada.
        /// </summary>
        /// <remarks>
        /// Não é utilizada diretamente pelo modelo ML.NET, mas auxilia na extração de features e análise.
        /// </remarks>
        [NoColumn]
        public List<ConsultaInfo> Consultas { get; set; } = new List<ConsultaInfo>();
    }

    /// <summary>
    /// Informações resumidas de uma consulta para análise pelo modelo.
    /// </summary>
    /// <remarks>
    /// Contém os dados essenciais de uma consulta, utilizados para gerar as features
    /// e para produzir informações detalhadas sobre o comportamento do paciente.
    /// </remarks>
    public class ConsultaInfo
    {
        /// <summary>
        /// Identificador único da consulta.
        /// </summary>
        public int IdConsulta { get; set; }

        /// <summary>
        /// Data e hora agendada para a consulta.
        /// </summary>
        public DateTime DataConsulta { get; set; }

        /// <summary>
        /// Status atual da consulta (ex: Agendada, Realizada, Não realizada).
        /// </summary>
        public string Status { get; set; }
    }

    /// <summary>
    /// Resultado da predição de necessidade de acompanhamento especializado.
    /// </summary>
    /// <remarks>
    /// Esta classe encapsula tanto o resultado da predição feita pelo modelo ML.NET
    /// quanto informações contextuais adicionais que ajudam a explicar e interpretar o resultado.
    /// </remarks>
    public class ResultadoAnaliseAcompanhamento
    {
        /// <summary>
        /// Identificador único do paciente analisado.
        /// </summary>
        public int IdPaciente { get; set; }

        /// <summary>
        /// Nome completo do paciente analisado.
        /// </summary>
        /// <remarks>
        /// Incluído para facilitar a apresentação dos resultados na interface.
        /// </remarks>
        public string NomePaciente { get; set; }

        /// <summary>
        /// Predicado que indica se o paciente necessita de acompanhamento especializado.
        /// </summary>
        /// <remarks>
        /// Este é o resultado binário (verdadeiro/falso) da predição feita pelo modelo ML.NET.
        /// </remarks>
        [ColumnName("PredictedLabel")]
        public bool NecessitaAcompanhamento { get; set; }

        /// <summary>
        /// Probabilidade de necessidade de acompanhamento, entre 0 e 1.
        /// </summary>
        /// <remarks>
        /// Quanto mais próximo de 1, mais confiante está o modelo na sua predição.
        /// </remarks>
        public float Probabilidade { get; set; }

        /// <summary>
        /// Pontuação bruta da predição antes da binarização.
        /// </summary>
        /// <remarks>
        /// Score é o valor bruto da função logística antes da aplicação do threshold.
        /// </remarks>
        [ColumnName("Score")]
        public float Score { get; set; }

        /// <summary>
        /// Justificativa textual para a recomendação de acompanhamento.
        /// </summary>
        /// <remarks>
        /// Explica em linguagem natural os motivos que levaram o modelo a fazer esta predição.
        /// </remarks>
        public string Justificativa { get; set; }

        /// <summary>
        /// Nível de alerta para o acompanhamento (ex: Crítico, Atenção, Padrão).
        /// </summary>
        /// <remarks>
        /// Categoriza a severidade da necessidade de acompanhamento para priorização.
        /// </remarks>
        public string NivelAlerta { get; set; }

        /// <summary>
        /// Sugestões de ações a serem tomadas com base na análise.
        /// </summary>
        /// <remarks>
        /// Fornece recomendações práticas sobre como proceder com o paciente analisado.
        /// </remarks>
        public string SugestaoAcao { get; set; }

        /// <summary>
        /// Detalhes das consultas consideradas na análise, em formato textual.
        /// </summary>
        /// <remarks>
        /// Lista de informações específicas sobre consultas que foram relevantes para a análise.
        /// </remarks>
        public List<string> DetalhesConsultas { get; set; } = new List<string>();

        /// <summary>
        /// Número total de consultas não realizadas pelo paciente.
        /// </summary>
        public int NumConsultasNaoRealizadas { get; set; }

        /// <summary>
        /// Número total de consultas agendadas expiradas (não atualizadas).
        /// </summary>
        public int NumConsultasAgendadasExpiradas { get; set; }

        /// <summary>
        /// Data da última consulta realizada pelo paciente.
        /// </summary>
        /// <remarks>
        /// Null se o paciente não tiver nenhuma consulta realizada.
        /// </remarks>
        public DateTime? DataUltimaConsulta { get; set; }
    }
} 