using System.Collections.Generic;

namespace SmartDentAPI.DTOs.Response
{
    /// <summary>
    /// DTO para retornar a ficha completa de um paciente, incluindo dados pessoais,
    /// informações de consultas e os procedimentos associados a cada consulta.
    /// </summary>
    /// <remarks>
    /// Esse DTO é utilizado para fornecer uma visão detalhada do paciente,
    /// reunindo informações pessoais, dados financeiros e históricos de consultas.
    /// </remarks>
    public class FichaPacienteResponseDTO
    {
        /// <summary>
        /// Identificador único do paciente.
        /// </summary>
        public int IdPaciente { get; set; }

        /// <summary>
        /// Nome completo do paciente.
        /// </summary>
        public string NomeCompleto { get; set; }

        /// <summary>
        /// CPF do paciente.
        /// </summary>
        public string CPF { get; set; }

        /// <summary>
        /// Data de nascimento do paciente, representada como string.
        /// </summary>
        public string DataNascimento { get; set; }

        /// <summary>
        /// E-mail do paciente.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Telefone do paciente.
        /// </summary>
        public string Telefone { get; set; }

        /// <summary>
        /// Endereço do paciente.
        /// </summary>
        public string Endereco { get; set; }

        /// <summary>
        /// Plano odontológico do paciente.
        /// </summary>
        public string PlanoOdontologico { get; set; }

        /// <summary>
        /// Empresa associada ao paciente. Valor padrão é "Individual".
        /// </summary>
        public string Empresa { get; set; } = "Individual";
        
        /// <summary>
        /// Número total de consultas realizadas pelo paciente.
        /// </summary>
        public int NumConsultas { get; set; }

        /// <summary>
        /// Gasto total do paciente, representado como string.
        /// </summary>
        public string GastoTotal { get; set; }

        /// <summary>
        /// Lista de consultas associadas ao paciente.
        /// Cada consulta contém dados específicos e o procedimento realizado (se houver).
        /// </summary>
        public List<ConsultaData> Consultas { get; set; }

        /// <summary>
        /// Classe que representa os dados de uma consulta do paciente.
        /// </summary>
        public class ConsultaData
        {
            /// <summary>
            /// Identificador único da consulta.
            /// </summary>
            public int IdConsulta { get; set; }

            /// <summary>
            /// Data da consulta, representada como string.
            /// </summary>
            public string DataConsulta { get; set; }

            /// <summary>
            /// Status da consulta (ex.: "Agendada", "Realizada", "Cancelada").
            /// </summary>
            public string Status { get; set; }
            
            /// <summary>
            /// Procedimento associado a esta consulta, se houver.
            /// </summary>
            public ProcedimentoData Procedimento { get; set; }
        }

        /// <summary>
        /// Classe que representa os dados de um procedimento odontológico associado a uma consulta.
        /// </summary>
        public class ProcedimentoData
        {
            /// <summary>
            /// Identificador único do procedimento.
            /// </summary>
            public int IdProcedimento { get; set; }

            /// <summary>
            /// Tipo (nome) do procedimento.
            /// </summary>
            public string TipoProcedimento { get; set; }

            /// <summary>
            /// Descrição do procedimento.
            /// </summary>
            public string Descricao { get; set; }

            /// <summary>
            /// Custo do procedimento, representado como string.
            /// </summary>
            public string Custo { get; set; }
        }
    }
}
