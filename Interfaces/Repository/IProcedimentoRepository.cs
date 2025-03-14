using System.Collections.Generic;
using System.Threading.Tasks;
using SmartDentAPI.Models;

namespace SmartDentAPI.Interfaces
{
    /// <summary>
    /// Interface para o repositório de procedimentos odontológicos.
    /// Define os métodos para operações CRUD e consultas específicas relacionadas aos procedimentos.
    /// </summary>
    public interface IProcedimentoRepository
    {
        /// <summary>
        /// Obtém todos os procedimentos cadastrados no sistema.
        /// </summary>
        /// <returns>Uma lista assíncrona de objetos Procedimento.</returns>
        Task<IEnumerable<Procedimento>> GetAllAsync();

        /// <summary>
        /// Obtém um procedimento específico com base no seu ID.
        /// </summary>
        /// <param name="idProcedimento">O identificador único do procedimento.</param>
        /// <returns>O objeto Procedimento correspondente ou null se não for encontrado.</returns>
        Task<Procedimento> GetByIdAsync(int idProcedimento);

        /// <summary>
        /// Obtém todos os procedimentos associados a uma determinada consulta.
        /// </summary>
        /// <param name="idConsulta">O identificador da consulta.</param>
        /// <returns>Uma lista assíncrona de objetos Procedimento para a consulta informada.</returns>
        Task<IEnumerable<Procedimento>> GetByConsultaIdAsync(int idConsulta);

        /// <summary>
        /// Adiciona um novo procedimento ao sistema.
        /// </summary>
        /// <param name="procedimento">O objeto Procedimento a ser adicionado.</param>
        /// <returns>Uma tarefa assíncrona que representa a operação de adição.</returns>
        Task AddAsync(Procedimento procedimento);

        /// <summary>
        /// Atualiza os dados de um procedimento existente.
        /// </summary>
        /// <param name="procedimento">O objeto Procedimento com os dados atualizados.</param>
        /// <returns>Uma tarefa assíncrona que representa a operação de atualização.</returns>
        Task UpdateAsync(Procedimento procedimento);

        /// <summary>
        /// Remove um procedimento do sistema com base no seu ID.
        /// </summary>
        /// <param name="idProcedimento">O identificador do procedimento a ser removido.</param>
        /// <returns>Uma tarefa assíncrona que representa a operação de remoção.</returns>
        Task DeleteAsync(int idProcedimento);
    }
}
