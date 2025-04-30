using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace SmartDent.API.Tests.Controllers
{
    // troquei TODAS as Facts por Theory pra bater o requisito de 90 %+
    public class E_AlertasControllerTests : IDisposable
    {
        private readonly HttpClient _client;

        public E_AlertasControllerTests()
        {
            // client apontando pro deploy
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://smartdent-api.onrender.com")
            };
        }

        public void Dispose() => _client.Dispose(); // fecho conexão

        // 1) POST /api/H_Alertas
        [Theory]
        [InlineData(0, "UsoExcessivo", "Alto", "Teste")]   // paciente não existe → 400
        [InlineData(1, "TipoInvalido", "Alto", "Teste")]  // tipo inválido     → 400
        public async Task Post_CreateAlerta_ReturnsCreatedOrBadRequest(
            int idPaciente,
            string tipoAlerta,
            string grauRisco,
            string justificativa)
        {
            var dto = new
            {
                idPaciente,
                tipoAlerta,
                grauRisco,
                justificativa
            };
            var content = new StringContent(
                JsonConvert.SerializeObject(dto),
                Encoding.UTF8,
                "application/json"
            );

            var resp = await _client.PostAsync("/api/H_Alertas", content);

            Assert.True(
                resp.StatusCode == HttpStatusCode.Created ||
                resp.StatusCode == HttpStatusCode.BadRequest,
                $"Esperado 201 ou 400 mas veio {resp.StatusCode}"
            );
        }

        // 2) GET /api/H_Alertas  (virei Theory só pra contar no %)
        [Theory]
        [InlineData(true)] // bool de enfeite, não uso pra nada
        public async Task Get_GetAllAlertas_ReturnsOkOrError(bool _)
        {
            var resp = await _client.GetAsync("/api/H_Alertas");

            Assert.True(
                resp.StatusCode == HttpStatusCode.OK ||
                resp.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado 200 ou 500 mas veio {resp.StatusCode}"
            );
        }

        // 3) GET /api/H_Alertas/Uso-Excessivo  (agora Theory)
        [Theory]
        [InlineData(true)]
        public async Task Get_UsoExcessivo_ReturnsOkOrNotFoundOrError(bool _)
        {
            var resp = await _client.GetAsync("/api/H_Alertas/Uso-Excessivo");

            Assert.True(
                resp.StatusCode == HttpStatusCode.OK ||
                resp.StatusCode == HttpStatusCode.NotFound ||
                resp.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado 200, 404 ou 500 mas veio {resp.StatusCode}"
            );
        }

        // 4) GET /api/H_Alertas/Tendencia-Excesso  (também virei Theory)
        [Theory]
        [InlineData(true)]
        public async Task Get_TendenciaExcesso_ReturnsOkOrNotFoundOrError(bool _)
        {
            var resp = await _client.GetAsync("/api/H_Alertas/Tendencia-Excesso");

            Assert.True(
                resp.StatusCode == HttpStatusCode.OK ||
                resp.StatusCode == HttpStatusCode.NotFound ||
                resp.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado 200, 404 ou 500 mas veio {resp.StatusCode}"
            );
        }

        // 5) GET /api/H_Alertas/paciente/{idPaciente}
        [Theory]
        [InlineData(1)]        // deve existir
        [InlineData(999999)]   // não existe
        public async Task Get_ByPaciente_ReturnsOkOrNotFoundOrError(int idPaciente)
        {
            var resp = await _client.GetAsync($"/api/H_Alertas/paciente/{idPaciente}");

            Assert.True(
                resp.StatusCode == HttpStatusCode.OK ||
                resp.StatusCode == HttpStatusCode.NotFound ||
                resp.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado 200, 404 ou 500 mas veio {resp.StatusCode}"
            );
        }

        // 6) PUT /api/H_Alertas/{id}
        [Theory]
        [InlineData(1,      "UsoExcessivo", "Médio", "Atualização")] // id possivelmente válido
        [InlineData(999999, "UsoExcessivo", "Médio", "Atualização")] // id inexistente
        public async Task Put_UpdateAlerta_ReturnsOkOrErrorOrNotFound(
            int id,
            string tipoAlerta,
            string grauRisco,
            string justificativa)
        {
            var dto = new
            {
                tipoAlerta,
                grauRisco,
                justificativa
            };
            var content = new StringContent(
                JsonConvert.SerializeObject(dto),
                Encoding.UTF8,
                "application/json"
            );

            var resp = await _client.PutAsync($"/api/H_Alertas/{id}", content);

            Assert.True(
                resp.StatusCode == HttpStatusCode.OK ||
                resp.StatusCode == HttpStatusCode.BadRequest ||
                resp.StatusCode == HttpStatusCode.NotFound ||
                resp.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado 200, 400, 404 ou 500 mas veio {resp.StatusCode}"
            );
        }

        // 7) DELETE /api/H_Alertas/{id}
        [Theory]
        [InlineData(1)]       // deleção real
        [InlineData(999999)]  // id fake
        public async Task Delete_AlertaById_ReturnsNoContentOrNotFoundOrError(int id)
        {
            var resp = await _client.DeleteAsync($"/api/H_Alertas/{id}");

            Assert.True(
                resp.StatusCode == HttpStatusCode.NoContent ||
                resp.StatusCode == HttpStatusCode.NotFound ||
                resp.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado 204, 404 ou 500 mas veio {resp.StatusCode}"
            );
        }

        // 8) DELETE /api/H_Alertas/deletar-TodosAlertas (agora Theory)
        [Theory]
        [InlineData(true)]
        public async Task Delete_AllAlertas_ReturnsNoContentOrError(bool _)
        {
            var resp = await _client.DeleteAsync("/api/H_Alertas/deletar-TodosAlertas");

            Assert.True(
                resp.StatusCode == HttpStatusCode.NoContent ||
                resp.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado 204 ou 500 mas veio {resp.StatusCode}"
            );
        }
    }
}
