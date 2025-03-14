using Microsoft.EntityFrameworkCore;
using SmartDentAPI.Data;
using SmartDentAPI.Interfaces;
using SmartDentAPI.Repositories;

/// <summary>
/// Programa principal que configura e inicia a API SmartDent.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

// Captura a connection string para fins de debug (remova este log em produção!)
var oracleConn = builder.Configuration.GetConnectionString("OracleConnection");
Console.WriteLine($"[DEBUG] Connection string: {oracleConn}");

// Configurar o Entity Framework para Oracle
// Aqui configuramos o EF para utilizar o banco de dados Oracle usando a connection string definida no appsettings.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

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

// Chama o seeder para popular o banco de dados
// Essa operação utiliza um escopo de serviço para garantir que o seeder seja executado corretamente ao iniciar a aplicação.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DataSeeder.SeedDatabase(services);
}

// Habilita o Swagger sempre, tanto no ambiente de desenvolvimento quanto em produção
app.UseSwagger();
app.UseSwaggerUI();

// Configuração do middleware de autorização (se houver regras de autorização definidas)
app.UseAuthorization();

// Mapeia os controllers para responder às requisições HTTP
app.MapControllers();

/// <summary>
/// Inicia a aplicação e escuta as requisições HTTP.
/// </summary>
app.Run();
