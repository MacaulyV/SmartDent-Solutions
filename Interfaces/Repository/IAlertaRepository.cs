using SmartDentAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartDentAPI.Interfaces
{
    /// <summary>
    /// Define os métodos necessários para gerenciar os alertas gerados pelo sistema.
    /// Esta interface expõe operações CRUD para manipulação dos dados de alertas.
    /// </summary>
    public interface IAlertaRepository
    {
        /// <summary>
        /// Obtém todos os alertas cadastrados no sistema, incluindo os dados dos pacientes associados.
        /// </summary>
        /// <returns>Uma coleção assíncrona de objetos Alerta.</returns>
        Task<IEnumerable<Alerta>> GetAllAsync();

        /// <summary>
        /// Obtém os alertas associados a um paciente específico.
        /// </summary>
        /// <param name="idPaciente">O identificador do paciente.</param>
        /// <returns>Uma coleção assíncrona de alertas para o paciente informado.</returns>
        Task<IEnumerable<Alerta>> GetByPacienteIdAsync(int idPaciente);

        /// <summary>
        /// Obtém um alerta específico com base no seu ID.
        /// </summary>
        /// <param name="idAlerta">O identificador único do alerta.</param>
        /// <returns>O objeto Alerta correspondente ou null se não for encontrado.</returns>
        Task<Alerta> GetByIdAsync(int idAlerta);

        /// <summary>
        /// Adiciona um novo alerta ao sistema.
        /// </summary>
        /// <param name="alerta">O objeto Alerta a ser adicionado.</param>
        /// <returns>Uma tarefa assíncrona que representa a operação de adição.</returns>
        Task AddAsync(Alerta alerta);

        /// <summary>
        /// Atualiza os dados de um alerta existente.
        /// </summary>
        /// <param name="alerta">O objeto Alerta com os dados atualizados.</param>
        /// <returns>Uma tarefa assíncrona que representa a operação de atualização.</returns>
        Task UpdateAsync(Alerta alerta);

        /// <summary>
        /// Remove um alerta do sistema com base no seu ID.
        /// </summary>
        /// <param name="idAlerta">O identificador do alerta a ser removido.</param>
        /// <returns>Uma tarefa assíncrona que representa a operação de remoção.</returns>
        Task DeleteAsync(int idAlerta);
    }
}
