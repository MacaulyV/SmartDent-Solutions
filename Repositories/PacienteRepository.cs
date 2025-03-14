using System;
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
    /// Repositório responsável pela manipulação de pacientes no banco de dados.
    /// Fornece métodos para executar operações CRUD e consultas específicas relacionadas aos pacientes.
    /// </summary>
    public class PacienteRepository : IPacienteRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor que injeta o contexto do banco de dados.
        /// </summary>
        /// <param name="context">Instância do ApplicationDbContext para acesso ao banco.</param>
        public PacienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Retorna uma lista de empresas associadas aos pacientes.
        /// Filtra os pacientes para garantir que a propriedade Empresa não seja nula ou vazia,
        /// depois seleciona e retorna valores distintos.
        /// </summary>
        /// <returns>Lista assíncrona de strings representando os nomes das empresas.</returns>
        public async Task<List<string>> GetEmpresasComPacientesAsync()
        {
            // Supondo que seu DbContext seja _context, filtra os pacientes com empresa definida.
            return await _context.Pacientes
                .Where(p => !string.IsNullOrEmpty(p.Empresa))  // Garante que a empresa não seja nula/vazia
                .Select(p => p.Empresa)
                .Distinct()
                .ToListAsync();
        }
        
        /// <summary>
        /// Retorna os pacientes que pertencem a uma determinada empresa.
        /// A comparação é feita de forma case-insensitive.
        /// </summary>
        /// <param name="empresa">Nome da empresa.</param>
        /// <returns>Lista assíncrona de pacientes que pertencem à empresa informada.</returns>
        public async Task<IEnumerable<Paciente>> GetPacientesByEmpresaAsync(string empresa)
        {
            return await _context.Pacientes
                .Where(p => p.Empresa.ToLower() == empresa.ToLower())
                .ToListAsync();
        }

        /// <summary>
        /// Retorna todos os pacientes do banco de dados.
        /// </summary>
        /// <returns>Lista assíncrona de todos os pacientes.</returns>
        public async Task<IEnumerable<Paciente>> GetAllAsync()
        {
            return await _context.Pacientes.ToListAsync();
        }

        /// <summary>
        /// Retorna um paciente específico com base no ID.
        /// </summary>
        /// <param name="idPaciente">ID do paciente.</param>
        /// <returns>O paciente correspondente ou null se não encontrado.</returns>
        public async Task<Paciente> GetByIdAsync(int idPaciente)
        {
            return await _context.Pacientes.FindAsync(idPaciente);
        }

        /// <summary>
        /// Retorna um paciente com base no CPF.
        /// </summary>
        /// <param name="cpf">CPF do paciente.</param>
        /// <returns>O paciente correspondente ou null se não encontrado.</returns>
        public async Task<Paciente> GetByCPFAsync(string cpf)
        {
            return await _context.Pacientes.FirstOrDefaultAsync(p => p.CPF == cpf);
        }

        /// <summary>
        /// Adiciona um novo paciente ao banco de dados.
        /// Antes de inserir, verifica se já existe um paciente com o mesmo CPF para evitar duplicidade.
        /// Em seguida, gera um ID aleatório para o paciente.
        /// </summary>
        /// <param name="paciente">O objeto Paciente a ser adicionado.</param>
        /// <returns>Uma tarefa assíncrona representando a operação.</returns>
        public async Task AddAsync(Paciente paciente)
        {
            // Validação: Verificar se já existe um paciente com o mesmo CPF.
            var existing = await GetByCPFAsync(paciente.CPF);
            if(existing != null)
            {
                throw new Exception("Já existe um paciente com esse CPF.");
            }
            // Gerar ID aleatório para o paciente.
            paciente.IdPaciente = Paciente.GerarIdAleatorio();
            await _context.Pacientes.AddAsync(paciente);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Atualiza os dados de um paciente existente no banco de dados.
        /// </summary>
        /// <param name="paciente">O objeto Paciente com dados atualizados.</param>
        /// <returns>Uma tarefa assíncrona representando a operação.</returns>
        public async Task UpdateAsync(Paciente paciente)
        {
            _context.Pacientes.Update(paciente);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Remove um paciente do banco de dados com base no seu ID.
        /// </summary>
        /// <param name="idPaciente">ID do paciente a ser removido.</param>
        /// <returns>Uma tarefa assíncrona representando a operação.</returns>
        public async Task DeleteAsync(int idPaciente)
        {
            var paciente = await GetByIdAsync(idPaciente);
            if(paciente != null)
            {
                _context.Pacientes.Remove(paciente);
                await _context.SaveChangesAsync();
            }
        }
    }
}
