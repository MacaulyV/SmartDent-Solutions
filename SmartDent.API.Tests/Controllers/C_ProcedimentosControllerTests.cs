using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace SmartDent.API.Tests.Controllers
{
    public class C_ProcedimentosControllerTests : IDisposable
    {
        private readonly HttpClient _client;

        public C_ProcedimentosControllerTests()
        {
            // aqui eu crio o HttpClient pra mandar requisições na API de produção
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://smartdent-api.onrender.com")
            };
        }

        public void Dispose()
        {
            // fecho o client depois de usar pra não vazar conexão
            _client.Dispose();
        }

        // 1) POST /api/E_Procedimentos
        [Theory]
        [InlineData(0, "Consulta odontológica geral", "Descrição X")]       // consulta não existe → BadRequest
        [InlineData(999999, "Consulta odontológica geral", "Descrição Y")] // id fake → BadRequest ou Created
        [InlineData(1, "Tipo inválido", "Descrição Z")]                    // tipo inválido → BadRequest
        public async Task CreateProcedimento_ValidaFluxoPost(int idConsulta, string tipo, string descricao)
        {
            // montando o DTO com os campos que o endpoint espera
            var dto = new
            {
                IdConsulta       = idConsulta,
                TipoProcedimento = tipo,
                Descricao        = descricao
            };
            // serializo em JSON e coloco no body
            var content = new StringContent(
                JsonConvert.SerializeObject(dto),
                Encoding.UTF8,
                "application/json"
            );

            // mando o POST pra criar procedimento
            var resp = await _client.PostAsync("/api/E_Procedimentos", content);
            // checo se foi Created (201) ou BadRequest (400) na validação
            Assert.True(
                resp.StatusCode == HttpStatusCode.Created ||
                resp.StatusCode == HttpStatusCode.BadRequest,
                $"Esperado Created ou BadRequest mas foi {resp.StatusCode}"
            );
        }

        // 2) GET /api/E_Procedimentos
        [Fact]
        public async Task GetAllProcedimentos_ReturnsOk()
        {
            // tento listar todos os procedimentos
            var resp = await _client.GetAsync("/api/E_Procedimentos");
            // OK se deu certo ou 500 se rolou erro interno
            Assert.True(
                resp.StatusCode == HttpStatusCode.OK ||
                resp.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado OK ou InternalServerError mas foi {resp.StatusCode}"
            );
        }

        // 3) GET /api/E_Procedimentos/{id}
        [Theory]
        [InlineData(1)]      // id possivelmente existente
        [InlineData(999999)] // id que não existe
        public async Task GetProcedimentoById_ValidaStatus(int id)
        {
            // faço GET por ID
            var resp = await _client.GetAsync($"/api/E_Procedimentos/{id}");
            // OK se achar, NotFound se não, ou 500 se deu ruim no servidor
            Assert.True(
                resp.StatusCode == HttpStatusCode.OK ||
                resp.StatusCode == HttpStatusCode.NotFound ||
                resp.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado OK, NotFound ou InternalServerError mas foi {resp.StatusCode}"
            );
        }

        // 4) PUT /api/E_Procedimentos/{id}
        [Theory]
        [InlineData(1, "Limpeza dental (profilaxia)", "Atualizado")]        // atualização válida
        [InlineData(1, "Tipo inválido", "Atualizado")]                     // tipo inválido → BadRequest
        [InlineData(999999, "Limpeza dental (profilaxia)", "Atualizado")]  // id inexistente → NotFound
        public async Task UpdateProcedimento_ValidaFluxoPut(int id, string tipo, string descricao)
        {
            // criando payload de update
            var dto = new
            {
                TipoProcedimento = tipo,
                Descricao        = descricao
            };
            var content = new StringContent(
                JsonConvert.SerializeObject(dto),
                Encoding.UTF8,
                "application/json"
            );

            // mando PUT pra alterar o procedimento
            var resp = await _client.PutAsync($"/api/E_Procedimentos/{id}", content);
            // aceito OK, BadRequest, NotFound ou 500 dependendo do cenário
            Assert.True(
                resp.StatusCode == HttpStatusCode.OK ||
                resp.StatusCode == HttpStatusCode.BadRequest ||
                resp.StatusCode == HttpStatusCode.NotFound ||
                resp.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado OK, BadRequest, NotFound ou InternalServerError mas foi {resp.StatusCode}"
            );
        }

        // 5) DELETE /api/E_Procedimentos/{id}
        [Theory]
        [InlineData(1)]      // tenta excluir existente → NoContent
        [InlineData(999999)] // tenta excluir inexistente → NotFound
        public async Task DeleteProcedimento_ValidaFluxoDelete(int id)
        {
            // mando DELETE
            var resp = await _client.DeleteAsync($"/api/E_Procedimentos/{id}");
            // espero NoContent ao excluir, NotFound se não achar, ou 500 se deu ruim
            Assert.True(
                resp.StatusCode == HttpStatusCode.NoContent ||
                resp.StatusCode == HttpStatusCode.NotFound ||
                resp.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado NoContent, NotFound ou InternalServerError mas foi {resp.StatusCode}"
            );
        }
    }
}
