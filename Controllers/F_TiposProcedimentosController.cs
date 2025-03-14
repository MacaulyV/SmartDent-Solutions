using Microsoft.AspNetCore.Mvc;
using SmartDentAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SmartDentAPI.Controllers
{
    /// <summary>
    /// Controller responsável por fornecer os tipos de procedimentos odontológicos disponíveis.
    /// </summary>
    /// <remarks>
    /// Este endpoint retorna um dicionário onde cada chave representa uma categoria de procedimentos 
    /// (por exemplo, "Consultas e Diagnóstico", "Prevenção e Profilaxia", etc.) e cada valor é uma lista 
    /// de procedimentos com seus respectivos custos formatados no padrão brasileiro.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class F_TiposProcedimentosController : ControllerBase
    {
        /// <summary>
        /// Método auxiliar que formata um valor decimal como moeda brasileira.
        /// </summary>
        /// <param name="valor">O valor decimal a ser formatado.</param>
        /// <returns>O valor formatado como moeda (ex.: "R$ 150,00").</returns>
        private string FormatCurrency(decimal valor)
        {
            return valor.ToString("C", CultureInfo.GetCultureInfo("pt-BR"));
        }

        /// <summary>
        /// GET: api/TiposProcedimentos
        /// Retorna os tipos de procedimentos organizados por categoria, com os custos formatados.
        /// </summary>
        /// <remarks>
        /// O endpoint constrói um dicionário onde cada chave é uma categoria (por exemplo, "Consultas e Diagnóstico")
        /// e o valor é uma lista de objetos TipoProcedimentoDTO, sendo que o campo Custo é formatado para exibição.
        /// </remarks>
        /// <returns>Um IActionResult com o dicionário de procedimentos formatados.</returns>
        [HttpGet]
        public IActionResult GetTiposProcedimentos()
        {
            var tiposProcedimentos = new Dictionary<string, List<TipoProcedimentoDTO>>
            {
                { "Consultas e Diagnóstico", new List<TipoProcedimentoDTO>
                    {
                        new TipoProcedimentoDTO { TipoProcedimento = "Consulta odontológica geral", Custo = 150m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Avaliação clínica e diagnóstico", Custo = 120m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Consulta para clareamento", Custo = 100m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Consulta para próteses", Custo = 130m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Acompanhamento ortodôntico", Custo = 140m }
                    }
                },
                { "Prevenção e Profilaxia", new List<TipoProcedimentoDTO>
                    {
                        new TipoProcedimentoDTO { TipoProcedimento = "Limpeza dental (profilaxia)", Custo = 120m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Aplicação de flúor", Custo = 80m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Aplicação de selante", Custo = 90m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Instrução de higiene bucal", Custo = 70m }
                    }
                },
                { "Urgência e Emergência 24h", new List<TipoProcedimentoDTO>
                    {
                        new TipoProcedimentoDTO { TipoProcedimento = "Atendimento odontológico de urgência", Custo = 200m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Alívio de dor", Custo = 150m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Drenagem de abscessos", Custo = 250m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Controle de hemorragias", Custo = 180m }
                    }
                },
                { "Radiologia e Exames", new List<TipoProcedimentoDTO>
                    {
                        new TipoProcedimentoDTO { TipoProcedimento = "Radiografia intraoral", Custo = 180m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Radiografia panorâmica", Custo = 300m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Documentação ortodôntica completa", Custo = 400m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Tomografia computadorizada", Custo = 700m }
                    }
                },
                { "Dentística (Tratamentos Restauradores)", new List<TipoProcedimentoDTO>
                    {
                        new TipoProcedimentoDTO { TipoProcedimento = "Restauração em resina composta", Custo = 300m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Restauração em amálgama", Custo = 280m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Troca de restaurações antigas", Custo = 320m }
                    }
                },
                { "Cirurgia Oral e Extrações", new List<TipoProcedimentoDTO>
                    {
                        new TipoProcedimentoDTO { TipoProcedimento = "Extração de dente comum", Custo = 200m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Extração de dente do siso", Custo = 450m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Frenectomia lingual e labial", Custo = 500m }
                    }
                },
                { "Endodontia (Tratamento de Canal)", new List<TipoProcedimentoDTO>
                    {
                        new TipoProcedimentoDTO { TipoProcedimento = "Canal em dentes anteriores", Custo = 550m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Canal em dentes posteriores", Custo = 750m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Retratamento endodôntico", Custo = 800m }
                    }
                },
                { "Periodontia (Tratamento da Gengiva)", new List<TipoProcedimentoDTO>
                    {
                        new TipoProcedimentoDTO { TipoProcedimento = "Tratamento de gengivite", Custo = 350m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Raspagem de tártaro", Custo = 400m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Cirurgia periodontal", Custo = 600m }
                    }
                },
                { "Odontopediatria (Atendimento Infantil)", new List<TipoProcedimentoDTO>
                    {
                        new TipoProcedimentoDTO { TipoProcedimento = "Atendimento odontológico para crianças", Custo = 250m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Aplicação de flúor e selante", Custo = 150m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Tratamento restaurador em dentes de leite", Custo = 200m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Extração de dentes de leite", Custo = 180m }
                    }
                },
                { "Ortodontia (Aparelhos Dentários)", new List<TipoProcedimentoDTO>
                    {
                        new TipoProcedimentoDTO { TipoProcedimento = "Documentação ortodôntica completa", Custo = 400m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Instalação de aparelho fixo metálico", Custo = 1800m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Manutenção mensal do aparelho", Custo = 250m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Retirada do aparelho ortodôntico", Custo = 350m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Mantenedores ortodônticos", Custo = 500m }
                    }
                },
                { "Odontologia Estética", new List<TipoProcedimentoDTO>
                    {
                        new TipoProcedimentoDTO { TipoProcedimento = "Clareamento dental caseiro", Custo = 400m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Clareamento estético em consultório", Custo = 600m }
                    }
                },
                { "Próteses Dentárias", new List<TipoProcedimentoDTO>
                    {
                        new TipoProcedimentoDTO { TipoProcedimento = "Prótese fixa (coroa unitária)", Custo = 2000m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Prótese removível total (dentadura)", Custo = 2500m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Prótese removível parcial", Custo = 2200m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Prótese sobre cerâmica ou resina", Custo = 2800m },
                        new TipoProcedimentoDTO { TipoProcedimento = "Placa de mordida para bruxismo", Custo = 900m }
                    }
                }
            };

            // Projeta o dicionário para que o campo Custo seja uma string formatada no padrão de moeda brasileira.
            var tiposProcedimentosFormatados = tiposProcedimentos.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Select(tp => new
                {
                    tp.TipoProcedimento,
                    Custo = FormatCurrency(tp.Custo)
                }).ToList()
            );

            return Ok(tiposProcedimentosFormatados);
        }
    }
}
