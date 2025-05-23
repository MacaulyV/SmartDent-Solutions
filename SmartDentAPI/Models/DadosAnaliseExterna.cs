using System.Collections.Generic;
using SmartDentAPI.Models;
using System.Text.Json.Serialization;

namespace SmartDentAPI.Models
{
    /// <summary>
    /// Classe para receber dados externos para análise
    /// </summary>
    public class DadosAnaliseExterna
    {
        /// <summary>
        /// Dados do paciente para análise
        /// </summary>
        public Paciente Paciente { get; set; }
        
        /// <summary>
        /// Lista de consultas do paciente
        /// </summary>
        public List<Consulta> Consultas { get; set; }
    }
    
    /// <summary>
    /// Classe para receber dados no formato frontend
    /// </summary>
    public class DadosAnaliseExternaFormatoFrontend
    {
        public int idPaciente { get; set; }
        
        public string nomeCompleto { get; set; }
        
        public string cpf { get; set; }
        
        public string dataNascimento { get; set; }
        
        public string email { get; set; }
        
        public string telefone { get; set; }
        
        public string endereco { get; set; }
        
        public string planoOdontologico { get; set; }
        
        public string empresa { get; set; }
        
        public int numConsultas { get; set; }
        
        public string gastoTotal { get; set; }
        
        public List<ConsultaFormatoFrontend> consultas { get; set; }
    }

    /// <summary>
    /// Formato de consulta para o frontend
    /// </summary>
    public class ConsultaFormatoFrontend
    {
        public int idConsulta { get; set; }
        
        public string dataConsulta { get; set; }
        
        public string status { get; set; }
        
        public ProcedimentoFormatoFrontend procedimento { get; set; }
    }

    /// <summary>
    /// Formato de procedimento para o frontend
    /// </summary>
    public class ProcedimentoFormatoFrontend
    {
        public int idProcedimento { get; set; }
        
        public string tipoProcedimento { get; set; }
        
        public string descricao { get; set; }
        
        public string custo { get; set; }
    }
} 