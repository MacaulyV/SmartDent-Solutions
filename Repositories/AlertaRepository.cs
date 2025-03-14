using Microsoft.EntityFrameworkCore;
using SmartDentAPI.Data;
using SmartDentAPI.Interfaces;
using SmartDentAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDentAPI.Repositories
{
    /// <summary>
    /// Implementação do repositório de alertas.
    /// Esta classe fornece métodos assíncronos para manipulação dos alertas no banco de dados,
    /// utilizando o Entity Framework para acesso e operações.
    /// </summary>
    public class AlertaRepository : IAlertaRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor que injeta a instância do contexto do banco de dados.
        /// </summary>
        /// <param name="context">Instância do ApplicationDbContext para acesso ao banco.</param>
        public AlertaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os alertas, incluindo os dados dos pacientes associados.
        /// </summary>
        /// <returns>Uma lista assíncrona de alertas.</returns>
        public async Task<IEnumerable<Alerta>> GetAllAsync()
        {
            return await _context.Alertas.Include(a => a.Paciente).ToListAsync();
        }

        /// <summary>
        /// Retorna os alertas associados a um paciente específico.
        /// </summary>
        /// <param name="idPaciente">O ID do paciente.</param>
        /// <returns>Uma lista assíncrona de alertas para o paciente informado.</returns>
        public async Task<IEnumerable<Alerta>> GetByPacienteIdAsync(int idPaciente)
        {
            return await _context.Alertas
                .Where(a => a.IdPaciente == idPaciente)
                .Include(a => a.Paciente)
                .ToListAsync();
        }

        /// <summary>
        /// Retorna um alerta específico com base no seu ID.
        /// </summary>
        /// <param name="idAlerta">O ID do alerta.</param>
        /// <returns>O alerta correspondente ou null se não encontrado.</returns>
        public async Task<Alerta> GetByIdAsync(int idAlerta)
        {
            return await _context.Alertas
                .Include(a => a.Paciente)
                .FirstOrDefaultAsync(a => a.IdAlerta == idAlerta);
        }

        /// <summary>
        /// Adiciona um novo alerta ao banco de dados.
        /// </summary>
        /// <param name="alerta">O objeto alerta a ser adicionado.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public async Task AddAsync(Alerta alerta)
        {
            await _context.Alertas.AddAsync(alerta);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Atualiza um alerta existente no banco de dados.
        /// </summary>
        /// <param name="alerta">O objeto alerta atualizado.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public async Task UpdateAsync(Alerta alerta)
        {
            _context.Alertas.Update(alerta);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Remove um alerta do banco de dados com base no seu ID.
        /// </summary>
        /// <param name="idAlerta">O ID do alerta a ser removido.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public async Task DeleteAsync(int idAlerta)
        {
            var alerta = await GetByIdAsync(idAlerta);
            if (alerta != null)
            {
                _context.Alertas.Remove(alerta);
                await _context.SaveChangesAsync();
            }
        }
    }
}
