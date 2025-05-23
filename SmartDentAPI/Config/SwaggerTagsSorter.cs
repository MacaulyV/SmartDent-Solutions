using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace SmartDentAPI.Config
{
    /// <summary>
    /// Filtro de documento para ordenar as tags (controladores) no Swagger
    /// </summary>
    public class SwaggerTagsSorter : IDocumentFilter
    {
        /// <summary>
        /// Aplica ordenação personalizada às tags do documento Swagger
        /// </summary>
        /// <param name="swaggerDoc">Documento Swagger</param>
        /// <param name="context">Contexto do filtro</param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (swaggerDoc.Tags == null)
            {
                swaggerDoc.Tags = new List<OpenApiTag>();
            }

            // Ordenar as tags colocando AnaliseAcompanhamento por último
            var orderedTags = swaggerDoc.Tags
                .OrderBy(tag => tag.Name == "AnaliseAcompanhamento" ? "Z" + tag.Name : tag.Name)
                .ToList();

            swaggerDoc.Tags = orderedTags;
        }
    }
} 