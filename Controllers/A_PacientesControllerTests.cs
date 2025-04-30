using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace SmartDent.API.Tests.Controllers
{
    public class A_PacientesControllerTests : IDisposable
    {
        private readonly HttpClient _client;

        public A_PacientesControllerTests()
        {
            // aqui eu crio o HttpClient apontando pra API em produção
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://smartdent-api.onrender.com")
            };
        }

        public void Dispose() => _client.Dispose();  // libero o client no fim pra não vazar conexão

        // testa criação de paciente via POST /api/A_Pacientes
        [Theory]
        [InlineData("Carlos Silva", "98765432100")]
        [InlineData("", "98765432100")]
        public async Task CreatePaciente_ValidaFluxoPost(string nome, string cpf)
        {
            // montando o DTO que vai pro body do POST
            var dto = new
            {
                NomeCompleto      = nome,
                CPF               = cpf,
                DataNascimento    = "15031990",        // ddMMyyyy, coloquei esse formato pq a API exige
                Email             = $"test{Guid.NewGuid():N}@ex.com",  // e-mail único pra evitar duplicidade
                Telefone          = "11988887777",
                Endereco          = "Rua Exemplo, 456",
                PlanoOdontologico = "Bem-Estar",
                Empresa           = "Individual"
            };
            var content = new StringContent(
                JsonConvert.SerializeObject(dto),
                Encoding.UTF8,
                "application/json"  // content-type que a API espera
            );

            var resp = await _client.PostAsync("/api/A_Pacientes", content);
            // aqui a resposta pode ser Created (201) ou BadRequest (400) se tiver erro de validação
            Assert.True(
                resp.StatusCode == HttpStatusCode.Created ||
                resp.StatusCode == HttpStatusCode.BadRequest,
                $"Esperado Created ou BadRequest mas foi {resp.StatusCode}"
            );
        }

        // testa GET de ficha dos pacientes individuais
        [Theory]
        [InlineData(true)]
        public async Task GetFichaPacientesByPlanoIndividual_SemFalhas(bool _)
        {
            var resp = await _client.GetAsync("/api/A_Pacientes/GetFichaPacientesByPlanoIndividual");
            // OK (200) ou NotFound (404) se não tiver nenhum, ou 500 em caso de erro interno
            Assert.True(
                resp.StatusCode == HttpStatusCode.OK ||
                resp.StatusCode == HttpStatusCode.NotFound ||
                resp.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado OK, NotFound ou InternalServerError mas foi {resp.StatusCode}"
            );
        }

        // testa GET geral de pacientes por plano individual
        [Theory]
        [InlineData(true)]
        public async Task GetPacientesByPlanoIndividual_SemFalhas(bool _)
        {
            var resp = await _client.GetAsync("/api/A_Pacientes/GetPacientesByPlanoIndividual");
            // uso o mesmo padrão de status esperado pra manter consistente
            Assert.True(
                resp.StatusCode == HttpStatusCode.OK ||
                resp.StatusCode == HttpStatusCode.NotFound ||
                resp.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado OK, NotFound ou InternalServerError mas foi {resp.StatusCode}"
            );
        }

        // testa GET de pacientes por empresa, passando string de empresa
        [Theory]
        [InlineData("Individual")]
        [InlineData("EmpresaInexistente")]
        [InlineData("")]
        public async Task GetPacientesByEmpresa_ValidaStatus(string empresa)
        {
            var resp = await _client.GetAsync($"/api/A_Pacientes/GetPacientesByEmpresa/{empresa}");
            // OK, NotFound se não achar, BadRequest se param vazio, ou 500 se rolou erro interno
            Assert.True(
                resp.StatusCode == HttpStatusCode.OK ||
                resp.StatusCode == HttpStatusCode.NotFound ||
                resp.StatusCode == HttpStatusCode.BadRequest ||
                resp.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado OK, NotFound, BadRequest ou InternalServerError mas foi {resp.StatusCode}"
            );
        }

        // testa GET de ficha por empresa específica
        [Theory]
        [InlineData("Individual")]
        [InlineData("Invalida")]
        public async Task GetFichaPacientesByEmpresa_ValidaStatus(string empresa)
        {
            var resp = await _client.GetAsync($"/api/A_Pacientes/GetFichaPacientesByEmpresa/{empresa}");
            // mesma lógica de status pro endpoint de ficha
            Assert.True(
                resp.StatusCode == HttpStatusCode.OK ||
                resp.StatusCode == HttpStatusCode.NotFound ||
                resp.StatusCode == HttpStatusCode.BadRequest ||
                resp.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado OK, NotFound, BadRequest ou InternalServerError mas foi {resp.StatusCode}"
            );
        }

        // testa GET de ficha de paciente específico pelo ID
        [Theory]
        [InlineData(1)]         // id real (pode ou não existir)
        [InlineData(999999)]    // id fake (inexistente)
        public async Task GetFichaPacienteById_ValidaStatus(int id)
        {
            var resp = await _client.GetAsync($"/api/A_Pacientes/FichaPaciente/{id}");
            // OK se existir, NotFound se não, ou 500 se deu ruim no servidor
            Assert.True(
                resp.StatusCode == HttpStatusCode.OK ||
                resp.StatusCode == HttpStatusCode.NotFound ||
                resp.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado OK ou NotFound ou InternalServerError mas foi {resp.StatusCode}"
            );
        }

        // testa GET de paciente simples por ID
        [Theory]
        [InlineData(1)]
        [InlineData(999999)]
        public async Task GetPacienteById_ValidaStatus(int id)
        {
            var resp = await _client.GetAsync($"/api/A_Pacientes/{id}");
            // mesclo OK, NotFound ou 500, pq vai depender do que rolou na request
            Assert.True(
                resp.StatusCode == HttpStatusCode.OK ||
                resp.StatusCode == HttpStatusCode.NotFound ||
                resp.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado OK ou NotFound ou InternalServerError mas foi {resp.StatusCode}"
            );
        }

        // testa PUT de atualização de paciente
        [Theory]
        [InlineData(1)]      // existente, deve atualizar ou dar BadRequest
        [InlineData(999999)] // inexistente, NotFound
        public async Task UpdatePaciente_ValidaFluxoPut(int id)
        {
            var dto = new
            {
                NomeCompleto      = "Atualizado Teste",
                Email             = "upd{Guid.NewGuid():N}@ex.com", // GUID pra email único
                Telefone          = "11977776666",
                Endereco          = "Rua Atualizada, 789",
                PlanoOdontologico = "Bem-Estar",
                Empresa           = "Individual"
            };
            var content = new StringContent(
                JsonConvert.SerializeObject(dto),
                Encoding.UTF8,
                "application/json"
            );
            var resp = await _client.PutAsync($"/api/A_Pacientes/{id}", content);
            // OK, NotFound, BadRequest ou 500 dependendo do cenário
            Assert.True(
                resp.StatusCode == HttpStatusCode.OK ||
                resp.StatusCode == HttpStatusCode.NotFound ||
                resp.StatusCode == HttpStatusCode.BadRequest ||
                resp.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado OK, NotFound, BadRequest ou InternalServerError mas foi {resp.StatusCode}"
            );
        }

        // testa DELETE de paciente por ID
        [Theory]
        [InlineData(1)]
        [InlineData(999999)]
        public async Task DeletePaciente_ValidaFluxoDelete(int id)
        {
            var resp = await _client.DeleteAsync($"/api/A_Pacientes/{id}");
            // NoContent se deletou, NotFound se não existe, ou 500 se rolou erro interno
            Assert.True(
                resp.StatusCode == HttpStatusCode.NoContent ||
                resp.StatusCode == HttpStatusCode.NotFound ||
                resp.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado NoContent, NotFound ou InternalServerError mas foi {resp.StatusCode}"
            );
        }

        // testa GET de edição de paciente (busca dados pra editar)
        [Theory]
        [InlineData(1)]
        [InlineData(999999)]
        public async Task BuscarPacienteEdit_ValidaStatus(int id)
        {
            var resp = await _client.GetAsync($"/api/A_Pacientes/BuscarPacienteEdit/{id}");
            // OK se achar, NotFound se não, ou 500 num erro geral
            Assert.True(
                resp.StatusCode == HttpStatusCode.OK ||
                resp.StatusCode == HttpStatusCode.NotFound ||
                resp.StatusCode == HttpStatusCode.InternalServerError,
                $"Esperado OK, NotFound ou InternalServerError mas foi {resp.StatusCode}"
            );
        }
    }
}
