using System.Collections.Generic;
using System.Threading.Tasks;
using SmartDentAPI.Models;

namespace SmartDentAPI.Interfaces
{
    /// <summary>
    /// Define os métodos que devem ser implementados para gerenciar os dados dos pacientes.
    /// Inclui operações de CRUD e consultas específicas (como buscar pacientes por CPF ou empresa).
    /// </summary>
    public interface IPacienteRepository
    {
        /// <summary>
        /// Obtém todos os pacientes cadastrados no sistema.
        /// </summary>
        /// <returns>Uma coleção assíncrona de objetos Paciente.</returns>
        Task<IEnumerable<Paciente>> GetAllAsync();

        /// <summary>
        /// Obtém um paciente com base no seu identificador único.
        /// </summary>
        /// <param name="idPaciente">O ID do paciente.</param>
        /// <returns>O objeto Paciente correspondente ou null se não for encontrado.</returns>
        Task<Paciente> GetByIdAsync(int idPaciente);

        /// <summary>
        /// Busca um paciente usando o CPF.
        /// </summary>
        /// <param name="cpf">O CPF do paciente.</param>
        /// <returns>O objeto Paciente que possui o CPF especificado ou null se não existir.</returns>
        Task<Paciente> GetByCPFAsync(string cpf);

        /// <summary>
        /// Adiciona um novo paciente ao sistema.
        /// </summary>
        /// <param name="paciente">O objeto Paciente a ser adicionado.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        Task AddAsync(Paciente paciente);

        /// <summary>
        /// Atualiza os dados de um paciente existente.
        /// </summary>
        /// <param name="paciente">O objeto Paciente com os dados atualizados.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        Task UpdateAsync(Paciente paciente);

        /// <summary>
        /// Remove um paciente do sistema com base no seu ID.
        /// </summary>
        /// <param name="idPaciente">O identificador do paciente a ser removido.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        Task DeleteAsync(int idPaciente);

        /// <summary>
        /// Obtém os pacientes associados a uma determinada empresa.
        /// A busca é realizada de forma case-insensitive.
        /// </summary>
        /// <param name="empresa">O nome da empresa.</param>
        /// <returns>Uma coleção assíncrona de pacientes que pertencem à empresa especificada.</returns>
        Task<IEnumerable<Paciente>> GetPacientesByEmpresaAsync(string empresa);

        /// <summary>
        /// Retorna uma lista das empresas que possuem pacientes cadastrados.
        /// </summary>
        /// <returns>Uma lista assíncrona de strings contendo os nomes das empresas.</returns>
        Task<List<string>> GetEmpresasComPacientesAsync();
    }
}
