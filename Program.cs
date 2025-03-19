using Microsoft.EntityFrameworkCore;
using SmartDentAPI.Data;
using SmartDentAPI.Interfaces;
using SmartDentAPI.Repositories;

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

// Registro do HttpClient para chamadas HTTP (útil para integração com outros serviços, se necessário)
builder.Services.AddHttpClient();

// Adicionar suporte para Controllers e documentação Swagger/OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/// <summary>
/// Constrói a aplicação a partir das configurações realizadas.
/// </summary>
var app = builder.Build();

// Aplica as migrations automaticamente (cria/atualiza as tabelas no banco de dados do Azure SQL)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
    
    // Chama o seeder para popular o banco de dados, se necessário
    await DataSeeder.SeedDatabase(scope.ServiceProvider);
}

// Habilita o Swagger sempre, mas personaliza a UI no ambiente de produção
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    if (!app.Environment.IsDevelopment())
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartDent API V1");
        options.RoutePrefix = string.Empty; // Deixa o Swagger como página inicial (opcional)
    }
});

// Configuração do middleware de autorização (se houver regras de autorização definidas)
app.UseAuthorization();

// Mapeia os controllers para responder às requisições HTTP
app.MapControllers();

/// <summary>
/// Inicia a aplicação e escuta as requisições HTTP.
/// </summary>
app.Run();
