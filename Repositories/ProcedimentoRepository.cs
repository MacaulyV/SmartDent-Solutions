using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartDentAPI.Data;
using SmartDentAPI.Interfaces;
using SmartDentAPI.Models;

namespace SmartDentAPI.Repositories
{
    /// <summary>
    /// Repositório responsável por gerenciar os procedimentos odontológicos no banco de dados.
    /// Fornece métodos assíncronos para operações CRUD (criar, ler, atualizar e deletar) dos procedimentos.
    /// </summary>
    public class ProcedimentoRepository : IProcedimentoRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor que injeta o ApplicationDbContext.
        /// </summary>
        /// <param name="context">Instância do contexto do banco de dados.</param>
        public ProcedimentoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todas os procedimentos registrados no banco de dados.
        /// </summary>
        /// <returns>Uma lista assíncrona de objetos Procedimento.</returns>
        public async Task<IEnumerable<Procedimento>> GetAllAsync()
        {
            return await _context.Procedimentos.ToListAsync();
        }

        /// <summary>
        /// Retorna um procedimento específico com base no seu ID.
        /// </summary>
        /// <param name="idProcedimento">O ID do procedimento.</param>
        /// <returns>O objeto Procedimento correspondente ou null se não encontrado.</returns>
        public async Task<Procedimento> GetByIdAsync(int idProcedimento)
        {
            return await _context.Procedimentos.FindAsync(idProcedimento);
        }

        /// <summary>
        /// Retorna todos os procedimentos associados a uma determinada consulta.
        /// </summary>
        /// <param name="idConsulta">O ID da consulta.</param>
        /// <returns>Uma lista assíncrona de procedimentos para a consulta especificada.</returns>
        public async Task<IEnumerable<Procedimento>> GetByConsultaIdAsync(int idConsulta)
        {
            return await _context.Procedimentos
                .Where(p => p.IdConsulta == idConsulta)
                .ToListAsync();
        }

        /// <summary>
        /// Adiciona um novo procedimento ao banco de dados.
        /// Gera um ID aleatório para o procedimento antes de adicioná-lo.
        /// </summary>
        /// <param name="procedimento">O objeto Procedimento a ser adicionado.</param>
        /// <returns>Uma tarefa assíncrona representando a operação de adição.</returns>
        public async Task AddAsync(Procedimento procedimento)
        {
            // Gera um ID aleatório para o procedimento.
            procedimento.IdProcedimento = Procedimento.GerarIdAleatorio();
            await _context.Procedimentos.AddAsync(procedimento);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Atualiza os dados de um procedimento existente no banco de dados.
        /// </summary>
        /// <param name="procedimento">O objeto Procedimento com os dados atualizados.</param>
        /// <returns>Uma tarefa assíncrona representando a operação de atualização.</returns>
        public async Task UpdateAsync(Procedimento procedimento)
        {
            _context.Procedimentos.Update(procedimento);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Remove um procedimento do banco de dados com base no seu ID.
        /// </summary>
        /// <param name="idProcedimento">O ID do procedimento a ser removido.</param>
        /// <returns>Uma tarefa assíncrona representando a operação de remoção.</returns>
        public async Task DeleteAsync(int idProcedimento)
        {
            var procedimento = await GetByIdAsync(idProcedimento);
            if (procedimento != null)
            {
                _context.Procedimentos.Remove(procedimento);
                await _context.SaveChangesAsync();
            }
        }
    }
}
