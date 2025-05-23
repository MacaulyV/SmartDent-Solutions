using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SmartDentAPI.Config
{
    /// <summary>
    /// Configuração de exemplos para o Swagger
    /// </summary>
    public class SwaggerExamplesConfig
    {
        /// <summary>
        /// Configurar exemplos de requisições e respostas para o Swagger
        /// </summary>
        /// <param name="swaggerOptions">Opções do SwaggerGen</param>
        public static void ConfigureSwaggerExamples(SwaggerGenOptions swaggerOptions)
        {
            // Exemplos para Pacientes
            AddRequestExample(swaggerOptions, "PacienteCreate", @"{
              ""nomeCompleto"": ""Maria Silva"",
              ""cpf"": ""12345678901"",
              ""dataNascimento"": ""01052000"",
              ""email"": ""maria.silva@email.com"",
              ""telefone"": ""11987654321"",
              ""endereco"": ""Rua das Flores, 123 - São Paulo/SP"",
              ""planoOdontologico"": ""Bem-Estar Pró"",
              ""empresa"": ""Individual""
            }");

            AddRequestExample(swaggerOptions, "PacienteUpdate", @"{
              ""nomeCompleto"": ""Maria Silva Souza"",
              ""email"": ""maria.silva.souza@email.com"",
              ""telefone"": ""11987654321"",
              ""endereco"": ""Avenida Paulista, 1500 - São Paulo/SP"",
              ""planoOdontologico"": ""Bem-Estar Orto"",
              ""empresa"": ""Individual""
            }");

            // Exemplos para Consultas
            AddRequestExample(swaggerOptions, "ConsultaCreate", @"{
              ""idPaciente"": 123456,
              ""dataConsulta"": ""010520241430""
            }");

            AddRequestExample(swaggerOptions, "ConsultaUpdate", @"{
              ""dataConsulta"": ""010520241530"",
              ""status"": ""Realizada""
            }");

            // Exemplos para Procedimentos
            AddRequestExample(swaggerOptions, "ProcedimentoCreate", @"{
              ""idConsulta"": 10001,
              ""tipoProcedimento"": ""Restauração em resina composta"",
              ""descricao"": ""Restauração de resina fotopolimerizável em dente 36""
            }");

            AddRequestExample(swaggerOptions, "ProcedimentoUpdate", @"{
              ""tipoProcedimento"": ""Restauração em amálgama"",
              ""descricao"": ""Restauração de amálgama no dente 36""
            }");

            // Exemplos para Alertas
            AddRequestExample(swaggerOptions, "AlertaCreate", @"{
              ""idPaciente"": 123456,
              ""tipoAlerta"": ""Uso Excessivo Identificado"",
              ""grauRisco"": ""59%"",
              ""justificativa"": ""Paciente realizou 5 consultas em menos de 30 dias sem justificativa clínica""
            }");
        }

        /// <summary>
        /// Adicionar um exemplo de requisição para o Swagger
        /// </summary>
        /// <param name="options">Opções de configuração do Swagger</param>
        /// <param name="operationId">ID da operação</param>
        /// <param name="jsonExample">Exemplo em formato JSON</param>
        private static void AddRequestExample(SwaggerGenOptions options, string operationId, string jsonExample)
        {
            options.RequestBodyFilter<ShowSwaggerExamplesFilter>();
        }
    }

    /// <summary>
    /// Filtro para exibir exemplos no Swagger
    /// </summary>
    public class ShowSwaggerExamplesFilter : IRequestBodyFilter
    {
        /// <summary>
        /// Aplicar filtro para exemplos de corpo de requisição
        /// </summary>
        /// <param name="requestBody">Corpo da requisição</param>
        /// <param name="context">Contexto da operação</param>
        public void Apply(OpenApiRequestBody requestBody, RequestBodyFilterContext context)
        {
            var examples = new Dictionary<string, OpenApiExample>();

            // Exemplos são adicionados automaticamente pelos atributos [Example] e [SwaggerSchema]
            // Esta classe pode ser estendida para adicionar exemplos programaticamente

            if (examples.Count > 0)
            {
                foreach (var mediaType in requestBody.Content.Values)
                {
                    mediaType.Examples = examples;
                }
            }
        }
    }
} 