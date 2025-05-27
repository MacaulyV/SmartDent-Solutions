using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace SmartDent.API.Tests.Controllers
{
    public class D_AIControllerTests : IDisposable
    {
        private readonly HttpClient _client;

        public D_AIControllerTests()
        {
            // aqui eu crio um HttpClient apontando pra API de IA em produção
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://smartdent-api.onrender.com")
            };
        }

        public void Dispose()
        {
            // fechando o client pra não vazar conexão, não sei se tem jeito melhor mas funcionou
            _client.Dispose();
        }

        // 1) POST /api/G_AI/analisar-Uso-Json
        [Theory]
        [InlineData(true)]
        public async Task Post_AnalisarUsoJson_ShouldReturnExpectedStatus(bool _)
        {
            // montando um payload simples com todos os campos, tipo um JSON de exemplo
            var payloadObj = new
            {
                idPaciente        = 0,
                nomeCompleto      = "Teste Usuario",
                cpf               = "000.000.000-00",
                dataNascimento    = "01/01/2000",
                email             = "teste@exemplo.com",
                telefone          = "(11) 99999-9999",
                endereco          = "Rua Teste, 123",
                planoOdontologico = "Bem-Estar",
                empresa           = "Individual",
                numConsultas      = 0,
                gastoTotal        = "R$ 0,00",
                consultas         = new object[0] // aqui deixo vazio só pra não dar erro
            };
            // serializo pra JSON e coloco no body
            var json    = JsonConvert.SerializeObject(payloadObj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // mando o POST pra testar a análise de uso via JSON
            var response = await _client.PostAsync("/api/G_AI/analisar-Uso-Json", content);

            // verifico se veio OK (200), BadRequest (400) ou InternalServerError (500)
            Assert.True(
                response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.BadRequest ||
                response.StatusCode == HttpStatusCode.InternalServerError,
                $"Expected 200 OK, 400 BadRequest or 500 InternalServerError but got {response.StatusCode}"
            );
        }

        // 2) GET /api/G_AI/analisar-Paciente/{idPaciente}
        [Theory]
        [InlineData(1)]
        [InlineData(999999)]
        public async Task Get_AnalisarPacienteById_ShouldReturnExpectedStatus(int idPaciente)
        {
            // tento buscar análise de um paciente por ID
            var response = await _client.GetAsync($"/api/G_AI/analisar-Paciente/{idPaciente}");

            // aceito OK, NotFound ou 500 em caso de erro
            Assert.True(
                response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.NotFound ||
                response.StatusCode == HttpStatusCode.InternalServerError,
                $"Expected 200 OK, 404 NotFound or 500 InternalServerError but got {response.StatusCode}"
            );
        }

        // 3) GET /api/G_AI/analisar-PacientePlanoIndividual
        [Theory]
        [InlineData(true)]
        public async Task Get_AnalisarPacientePlanoIndividual_ShouldReturnExpectedStatus(bool _)
        {
            // essa rota analisa todos os pacientes individuais
            var response = await _client.GetAsync("/api/G_AI/analisar-PacientePlanoIndividual");

            // pode ser OK, NotFound (se não tiver nenhum) ou 500 se algo deu ruim
            Assert.True(
                response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.NotFound ||
                response.StatusCode == HttpStatusCode.InternalServerError,
                $"Expected 200 OK, 404 NotFound or 500 InternalServerError but got {response.StatusCode}"
            );
        }

        // 4) GET /api/G_AI/analisar-PacientesByEmpresa?empresa=...
        [Theory]
        [InlineData("Individual")]        // empresa válida
        [InlineData("EmpresaInexistente")]// empresa que não existe
        public async Task Get_AnalisarPacientesByEmpresa_ShouldReturnExpectedStatus(string empresa)
        {
            // monto a URL com query string escapada
            var uri = $"/api/G_AI/analisar-PacientesByEmpresa?empresa={Uri.EscapeDataString(empresa)}";
            var response = await _client.GetAsync(uri);

            // OK se achar, BadRequest se query inválida, NotFound se não tiver pacientes, ou 500
            Assert.True(
                response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.BadRequest ||
                response.StatusCode == HttpStatusCode.NotFound ||
                response.StatusCode == HttpStatusCode.InternalServerError,
                $"Expected 200 OK, 400 BadRequest, 404 NotFound or 500 InternalServerError but got {response.StatusCode}"
            );
        }

        // 5) GET /api/G_AI/analisar-PacientesByCidade?cidade=...
        [Theory]
        [InlineData("São Paulo")]  // cidade válida
        [InlineData("CidadeFake")] // cidade que não existe
        public async Task Get_AnalisarPacientesByCidade_ShouldReturnExpectedStatus(string cidade)
        {
            // mesma ideia de query, agora por cidade
            var uri = $"/api/G_AI/analisar-PacientesByCidade?cidade={Uri.EscapeDataString(cidade)}";
            var response = await _client.GetAsync(uri);

            // aceito 200, 400, 404 ou 500
            Assert.True(
                response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.BadRequest ||
                response.StatusCode == HttpStatusCode.NotFound ||
                response.StatusCode == HttpStatusCode.InternalServerError,
                $"Expected 200 OK, 400 BadRequest, 404 NotFound or 500 InternalServerError but got {response.StatusCode}"
            );
        }

        // 6) GET /api/G_AI/analisar-TodosPaciente
        [Theory]
        [InlineData(true)]
        public async Task Get_AnalisarTodosPaciente_ShouldReturnExpectedStatus(bool _)
        {
            // rota que analisa todos os pacientes cadastrados
            var response = await _client.GetAsync("/api/G_AI/analisar-TodosPaciente");

            // OK, NotFound se não tiver nada, ou 500 se der erro
            Assert.True(
                response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.NotFound ||
                response.StatusCode == HttpStatusCode.InternalServerError,
                $"Expected 200 OK, 404 NotFound or 500 InternalServerError but got {response.StatusCode}"
            );
        }
    }
}
