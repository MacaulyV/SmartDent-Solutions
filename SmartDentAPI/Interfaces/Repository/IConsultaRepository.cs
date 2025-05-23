using System.Collections.Generic;
using System.Threading.Tasks;
using SmartDentAPI.Models;

namespace SmartDentAPI.Interfaces
{
    /// <summary>
    /// Define os métodos necessários para a manipulação dos dados de consultas odontológicas.
    /// Essa interface expõe operações CRUD e consultas específicas relacionadas às consultas.
    /// </summary>
    public interface IConsultaRepository
    {
        /// <summary>
        /// Obtém todas as consultas cadastradas no sistema.
        /// </summary>
        /// <returns>Uma coleção assíncrona de objetos Consulta.</returns>
        Task<IEnumerable<Consulta>> GetAllAsync();

        /// <summary>
        /// Obtém uma consulta específica com base no seu ID.
        /// </summary>
        /// <param name="idConsulta">O identificador único da consulta.</param>
        /// <returns>O objeto Consulta correspondente ou null, se não encontrado.</returns>
        Task<Consulta> GetByIdAsync(int idConsulta);

        /// <summary>
        /// Obtém todas as consultas associadas a um paciente específico.
        /// </summary>
        /// <param name="idPaciente">O identificador do paciente.</param>
        /// <returns>Uma coleção assíncrona de consultas para o paciente informado.</returns>
        Task<IEnumerable<Consulta>> GetByPacienteIdAsync(int idPaciente);

        /// <summary>
        /// Adiciona uma nova consulta ao sistema.
        /// </summary>
        /// <param name="consulta">O objeto Consulta a ser adicionado.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona de adição.</returns>
        Task AddAsync(Consulta consulta);

        /// <summary>
        /// Atualiza os dados de uma consulta existente.
        /// </summary>
        /// <param name="consulta">O objeto Consulta com os dados atualizados.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona de atualização.</returns>
        Task UpdateAsync(Consulta consulta);

        /// <summary>
        /// Remove uma consulta do sistema com base no seu ID.
        /// </summary>
        /// <param name="idConsulta">O identificador da consulta a ser removida.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona de remoção.</returns>
        Task DeleteAsync(int idConsulta);
    }
}
