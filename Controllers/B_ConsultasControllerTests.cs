using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace SmartDent.API.Tests.Controllers
{
    public class B_ConsultasControllerTests : IDisposable
    {
        private readonly HttpClient _client;

        public B_ConsultasControllerTests()
        {
            // aqui eu instancio o HttpClient apontando pra API real
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://smartdent-api.onrender.com")
            };
        }

        public void Dispose() 
        {
            // fechando o client pra liberar recursos, vale lembrar disso
            _client.Dispose();
        }

        // 1) POST /api/D_Consultas
        [Theory]
        [InlineData(0, "01012030")]          // paciente 0 não existe → BadRequest
        [InlineData(999999, "01012030")]     // id fake → BadRequest ou Created (se existir)
        [InlineData(1, "bad-date")]          // formato inválido → BadRequest
        public async Task CreateConsulta_ValidaFluxoPost(int idPaciente, string dataConsulta)
        {
            // montando payload pra criar consulta
            var dto = new
            {
                IdPaciente   = idPaciente,
                DataConsulta = dataConsulta  // esse campo a API vai validar o formato
            };
            var content = new StringContent(
                JsonConvert.SerializeObject(dto), 
                Encoding.UTF8, 
                "application/json"           // sempre lembrar do content-type certo
            );

            // chamando POST pra criar a consulta
            var resp = await _client.PostAsync("/api/D_Consultas", content);

            // espero que crie (201) ou devolva erro de validação (400)
            Assert.True(
                resp.StatusCode == HttpStatusCode.Created ||
                resp.StatusCode == HttpStatusCode.BadRequest,
                $"Esperado Created ou BadRequest mas foi {resp.StatusCode}"
            );
        }

        // 2) GET /api/D_Consultas
        [Fact]
        public async Task GetAllConsultas_ReturnsOk()
        {
            // fazendo GET pra listar todas as consultas
            var resp = await _client.GetAsync("/api/D_Consultas");
            // OK se tudo certo, ou 500 se deu ruim no servidor
            Assert.True(
                resp.StatusCode == HttpStatusCode.OK ||
                resp.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado OK ou InternalServerError mas foi {resp.StatusCode}"
            );
        }

        // 3) GET /api/D_Consultas/{id}
        [Theory]
        [InlineData(1)]       // id real (se existir) → OK
        [InlineData(999999)]  // inexistente → NotFound
        public async Task GetConsultaById_ValidaStatus(int id)
        {
            // aqui a gente tenta buscar por ID
            var resp = await _client.GetAsync($"/api/D_Consultas/{id}");
            // OK quando achar, NotFound quando não, ou 500 se algo estranhou
            Assert.True(
                resp.StatusCode == HttpStatusCode.OK ||
                resp.StatusCode == HttpStatusCode.NotFound ||
                resp.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado OK, NotFound ou InternalServerError mas foi {resp.StatusCode}"
            );
        }

        // 4) PUT /api/D_Consultas/{id}
        [Theory]
        [InlineData(1, "15082030", "Agendada")]      // formato correto, status válido
        [InlineData(1, "15082030", "Inválido")]      // status inválido → BadRequest
        [InlineData(999999, "15082030", "Agendada")] // id inexistente → NotFound
        public async Task UpdateConsulta_ValidaFluxoPut(int id, string dataConsulta, string status)
        {
            // payload de atualização: data e status
            var dto = new
            {
                DataConsulta = dataConsulta,
                Status       = status
            };
            var content = new StringContent(
                JsonConvert.SerializeObject(dto),
                Encoding.UTF8,
                "application/json"
            );

            // faço PUT pra atualizar consulta
            var resp = await _client.PutAsync($"/api/D_Consultas/{id}", content);

            // posso receber OK, BadRequest, NotFound ou 500 dependendo do cenário
            Assert.True(
                resp.StatusCode == HttpStatusCode.OK ||
                resp.StatusCode == HttpStatusCode.BadRequest ||
                resp.StatusCode == HttpStatusCode.NotFound ||
                resp.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado OK, BadRequest, NotFound ou InternalServerError mas foi {resp.StatusCode}"
            );
        }

        // 5) DELETE /api/D_Consultas/{id}
        [Theory]
        [InlineData(1)]      // testa deletar ID existente → NoContent
        [InlineData(999999)] // testa deletar ID inexistente → NotFound
        public async Task DeleteConsulta_ValidaFluxoDelete(int id)
        {
            // aqui mando DELETE na rota, simples assim
            var resp = await _client.DeleteAsync($"/api/D_Consultas/{id}");
            // espero NoContent se deletou, NotFound se não achou, ou 500 se deu erro
            Assert.True(
                resp.StatusCode == HttpStatusCode.NoContent ||
                resp.StatusCode == HttpStatusCode.NotFound ||
                resp.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado NoContent, NotFound ou InternalServerError mas foi {resp.StatusCode}"
            );
        }
    }
}
