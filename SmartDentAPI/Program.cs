using Microsoft.EntityFrameworkCore;
using SmartDentAPI.Data;
using SmartDentAPI.Interfaces;
using SmartDentAPI.ML;
using SmartDentAPI.Repositories;
using SmartDentAPI.Config;

/// <summary>
/// Programa principal que configura e inicia a API SmartDent.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

// Configurar o Entity Framework para SQL Server (Azure SQL Database)
// Aqui configuramos o EF para utilizar o banco de dados SQL usando a connection string definida no appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Injeção de dependências dos repositórios
// Registramos os repositórios para IPacienteRepository, IConsultaRepository, IProcedimentoRepository e IAlertaRepository
// Assim, a injeção de dependência garante que a implementação concreta seja fornecida onde necessário.
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<IConsultaRepository, ConsultaRepository>();
builder.Services.AddScoped<IProcedimentoRepository, ProcedimentoRepository>();
builder.Services.AddScoped<IAlertaRepository, AlertaRepository>();

// Registrar o serviço de análise de acompanhamento como Singleton para reutilizar o modelo treinado entre requisições
builder.Services.AddSingleton<ServicoAnaliseAcompanhamento>();

// Registro do HttpClient para chamadas HTTP (útil para integração com outros serviços, se necessário)
builder.Services.AddHttpClient();

// Adicionar suporte para Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configurar Swagger com documentação personalizada
builder.Services.AddSwaggerDocumentation();

/// <summary>
/// Constrói a aplicação a partir das configurações realizadas.
/// </summary>
var app = builder.Build();

// Verificação básica de conexão com o banco (sem aplicar migrações)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    // Testa a conexão com o banco, sem tentar criar ou migrar tabelas
    dbContext.Database.CanConnect();
    
    // Chama o seeder para popular o banco de dados, se necessário
    await DataSeeder.SeedDatabase(scope.ServiceProvider);
}

// Configuração para servir arquivos estáticos (CSS, JS, imagens)
app.UseStaticFiles();

// Usar a configuração personalizada do Swagger
app.UseSwaggerDocumentation();

// Configuração do middleware de autorização (se houver regras de autorização definidas)
app.UseAuthorization();

// Mapeia os controllers para responder às requisições HTTP
app.MapControllers();

/// <summary>
/// Inicia a aplicação e escuta as requisições HTTP.
/// </summary>
app.Run();