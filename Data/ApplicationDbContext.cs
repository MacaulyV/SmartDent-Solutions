using Microsoft.EntityFrameworkCore;
using SmartDentAPI.Models;

namespace SmartDentAPI.Data
{
    /// <summary>
    /// Representa o contexto do banco de dados para a aplicação SmartDent.
    /// Este contexto é responsável por mapear as entidades (Paciente, Consulta, Procedimento e Alerta)
    /// para as respectivas tabelas no banco de dados.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Construtor que recebe as opções de configuração do contexto.
        /// </summary>
        /// <param name="options">Opções de configuração do DbContext.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }

        /// <summary>
        /// Representa a tabela de Pacientes no banco de dados.
        /// </summary>
        public DbSet<Paciente> Pacientes { get; set; }

        /// <summary>
        /// Representa a tabela de Consultas no banco de dados.
        /// </summary>
        public DbSet<Consulta> Consultas { get; set; }

        /// <summary>
        /// Representa a tabela de Procedimentos no banco de dados.
        /// </summary>
        public DbSet<Procedimento> Procedimentos { get; set; }

        /// <summary>
        /// Representa a tabela de Alertas no banco de dados.
        /// </summary>
        public DbSet<Alerta> Alertas { get; set; }

        /// <summary>
        /// Configura os relacionamentos e regras de mapeamento entre as entidades.
        /// </summary>
        /// <param name="modelBuilder">Construtor de modelo utilizado para definir o mapeamento.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Chama o método base para garantir que a configuração padrão seja aplicada
            base.OnModelCreating(modelBuilder);

            // Configura o relacionamento entre Alerta e Paciente.
            // Cada alerta está associado a um paciente e, se o paciente for deletado, os alertas associados
            // serão removidos em cascata.
            modelBuilder.Entity<Alerta>()
                .HasOne(a => a.Paciente)
                .WithMany() // Caso a entidade Paciente não possua uma coleção de alertas, usamos WithMany()
                .HasForeignKey(a => a.IdPaciente)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
