using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.IO;
using System.Reflection;
using System.Linq;

namespace SmartDentAPI.Config
{
    /// <summary>
    /// Configurações do Swagger para a API SmartDent
    /// </summary>
    public static class SwaggerConfig
    {
        /// <summary>
        /// Adiciona e configura o Swagger para a API
        /// </summary>
        /// <param name="services">Coleção de serviços da aplicação</param>
        public static void AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // Informações básicas da API
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SmartDent API",
                    Version = "v1",
                    Description = @"API para gerenciamento de consultório odontológico, incluindo:
                    - Cadastro e gerenciamento de pacientes
                    - Agendamento e controle de consultas
                    - Registro de procedimentos odontológicos
                    - Sistema de alertas e monitoramento
                    - Integração com planos odontológicos",
                    Contact = new OpenApiContact
                    {
                        Name = "Equipe SmartDent",
                        Email = "suporte@smartdent.com.br",
                        Url = new Uri("https://smartdent.com.br/contato")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Licença SmartDent",
                        Url = new Uri("https://smartdent.com.br/licenca")
                    }
                });

                // Caminho para o arquivo XML de documentação
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                
                // Incluir comentários XML para Swagger
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
                
                // Configurações para exibir exemplos nas requisições
                c.EnableAnnotations();
                
                // Adicionar exemplos programaticamente
                SwaggerExamplesConfig.ConfigureSwaggerExamples(c);
                
                // Usar apenas o valor do atributo SwaggerTag para agrupamento
                c.TagActionsBy(api => {
                    if (api.ActionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor controllerActionDescriptor)
                    {
                        var tagAttributes = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(SwaggerTagAttribute), true);
                        if (tagAttributes.Any())
                        {
                            var tagAttribute = (SwaggerTagAttribute)tagAttributes.First();
                            return new[] { tagAttribute.Description };
                        }
                    }
                    return new[] { api.ActionDescriptor.RouteValues["controller"] };
                });
                
                // Organizar endpoints: AnaliseAcompanhamento por último, demais por ordem alfabética
                c.OrderActionsBy(apiDesc => {
                    string controller = apiDesc.ActionDescriptor.RouteValues["controller"];
                    string tag = "";
                    
                    if (apiDesc.ActionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor controllerActionDescriptor)
                    {
                        var tagAttributes = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(SwaggerTagAttribute), true);
                        if (tagAttributes.Any())
                        {
                            var tagAttribute = (SwaggerTagAttribute)tagAttributes.First();
                            tag = tagAttribute.Description;
                        }
                    }
                    
                    // Se for AnaliseAcompanhamento, adicionar prefixo "Z_" para que fique por último
                    if (tag == "AnaliseAcompanhamento" || controller == "AnaliseAcompanhamento")
                    {
                        return $"Z_{controller}_{apiDesc.RelativePath}";
                    }
                    
                    return $"{controller}_{apiDesc.RelativePath}";
                });
                
                // Ordenar as tags (controllers) de forma personalizada
                c.DocumentFilter<SwaggerTagsSorter>();
                
                // Agrupar endpoints por tag (tipo de recurso)
                c.DocInclusionPredicate((docName, apiDesc) => true);
            });
        }

        /// <summary>
        /// Configura a interface do Swagger
        /// </summary>
        /// <param name="app">Aplicação ASP.NET Core</param>
        public static void UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            // Habilitar o middleware do Swagger
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api-docs/{documentName}/swagger.json";
            });

            // Configurar a UI do Swagger
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/api-docs/v1/swagger.json", "SmartDent API v1");
                c.RoutePrefix = "api-docs";
                
                // Personalização da interface
                c.DocumentTitle = "Documentação SmartDent API";
                c.DefaultModelsExpandDepth(-1); // Oculta a seção de modelos (schemas)
                c.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Example);
                c.DisplayRequestDuration();
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                c.EnableDeepLinking();
                c.EnableFilter();
                c.ShowExtensions();
                
                // Customização CSS
                c.InjectStylesheet("/swagger-ui/custom.css");
            });
        }
    }
} 