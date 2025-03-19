using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartDentAPI.Data;
using SmartDentAPI.Interfaces;
using SmartDentAPI.Models;

namespace SmartDentAPI.Repositories
{
    /// <summary>
    /// Repositório responsável pela manipulação de consultas no banco de dados.
    /// Fornece métodos assíncronos para realizar operações CRUD em consultas.
    /// </summary>
    public class ConsultaRepository : IConsultaRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor que injeta a instância do contexto do banco de dados.
        /// </summary>
        /// <param name="context">Instância do ApplicationDbContext para acesso ao banco.</param>
        public ConsultaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todas as consultas registradas no banco de dados.
        /// </summary>
        /// <returns>Uma lista assíncrona de objetos Consulta.</returns>
        public async Task<IEnumerable<Consulta>> GetAllAsync()
        {
            return await _context.Consultas.ToListAsync();
        }

        /// <summary>
        /// Retorna uma consulta específica baseada no seu ID.
        /// </summary>
        /// <param name="idConsulta">O ID da consulta.</param>
        /// <returns>O objeto Consulta correspondente ou null se não encontrado.</returns>
        public async Task<Consulta> GetByIdAsync(int idConsulta)
        {
            return await _context.Consultas.FindAsync(idConsulta);
        }

        /// <summary>
        /// Retorna todas as consultas associadas a um paciente específico.
        /// </summary>
        /// <param name="idPaciente">O ID do paciente.</param>
        /// <returns>Uma lista assíncrona de consultas para o paciente informado.</returns>
        public async Task<IEnumerable<Consulta>> GetByPacienteIdAsync(int idPaciente)
        {
            // Filtra apenas as consultas cujo IdPaciente é igual ao id informado.
            return await _context.Consultas
                .Where(c => c.IdPaciente == idPaciente)
                .ToListAsync();
        }

        /// <summary>
        /// Adiciona uma nova consulta ao banco de dados.
        /// Antes de adicionar, gera um ID aleatório para a consulta.
        /// </summary>
        /// <param name="consulta">O objeto Consulta a ser adicionado.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public async Task AddAsync(Consulta consulta)
        {
            // Removemos a validação de existência de paciente e de conflito de horário
            // Gera um ID aleatório para a consulta antes de adicioná-la.
            consulta.IdConsulta = Consulta.GerarIdAleatorio();
            await _context.Consultas.AddAsync(consulta);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Atualiza os dados de uma consulta existente no banco de dados.
        /// </summary>
        /// <param name="consulta">O objeto Consulta com os dados atualizados.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public async Task UpdateAsync(Consulta consulta)
        {
            _context.Consultas.Update(consulta);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Remove uma consulta do banco de dados com base no seu ID.
        /// </summary>
        /// <param name="idConsulta">O ID da consulta a ser removida.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public async Task DeleteAsync(int idConsulta)
        {
            var consulta = await GetByIdAsync(idConsulta);
            if (consulta != null)
            {
                _context.Consultas.Remove(consulta);
                await _context.SaveChangesAsync();
            }
        }
    }
}
