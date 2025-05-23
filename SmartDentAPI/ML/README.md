# Modelo de Análise de Acompanhamento

## Visão Geral

Este módulo implementa um modelo de machine learning utilizando ML.NET para analisar o histórico de consultas dos pacientes e identificar aqueles que necessitam de acompanhamento especializado. O modelo foca em duas características principais:

1. **Consultas não realizadas:** Identifica padrões de faltas sem justificativa prévia
2. **Consultas agendadas expiradas:** Detecta consultas que deveriam ter ocorrido, mas cujo status não foi atualizado

## Arquivos

- `DadosAnaliseAcompanhamento.cs`: Define as classes para os dados de entrada e saída do modelo
- `ServicoAnaliseAcompanhamento.cs`: Implementa o serviço que treina o modelo e realiza as análises

## Como Funciona

O modelo utiliza algoritmo de árvore de decisão (FastTree) para classificar pacientes com base em diversas características:

- Idade do paciente
- Número total de consultas
- Número de consultas não realizadas (faltas)
- Número de consultas agendadas expiradas
- Percentual de faltas em relação ao total
- Tempo (em dias) desde a última consulta realizada

Após a análise, o modelo fornece:
- Predicado sobre a necessidade de acompanhamento
- Nível de confiança da predição
- Justificativa detalhada com os motivos específicos
- Lista de consultas problemáticas identificadas

## Integração com a API

O modelo pode ser consumido de duas formas através da API:

1. **Endpoint GET** (`/api/AnaliseAcompanhamento/paciente/{idPaciente}`):
   - Busca os dados do paciente e suas consultas do banco de dados
   - Realiza a análise e retorna o resultado completo

2. **Endpoint POST** (`/api/AnaliseAcompanhamento/analisar`):
   - Permite enviar dados de paciente e consultas diretamente
   - Útil para integração com outros sistemas

## Treinamento do Modelo

O modelo pode ser treinado através do endpoint (`/api/AnaliseAcompanhamento/treinar-modelo`), que utiliza:

- Dados reais de pacientes e consultas do banco
- Dados sintéticos gerados para melhorar o desempenho do modelo
- Regras heurísticas para rotulagem dos dados de treinamento

O modelo treinado é salvo em arquivo para uso posterior, permitindo sua reutilização sem necessidade de retreinamento a cada execução.

## Exemplo de Uso

Para analisar um paciente específico:

```
GET /api/AnaliseAcompanhamento/paciente/12345
```

Para enviar dados diretamente:

```json
POST /api/AnaliseAcompanhamento/analisar
{
  "paciente": {
    "idPaciente": 12345,
    "nomeCompleto": "João Silva",
    "dataNascimento": "01011990",
    ...
  },
  "consultas": [
    {
      "idConsulta": 1001,
      "idPaciente": 12345,
      "dataConsulta": "010120231500",
      "status": "Realizada"
    },
    {
      "idConsulta": 1002,
      "idPaciente": 12345,
      "dataConsulta": "010220231500",
      "status": "Não realizada"
    }
  ]
}
```

## Resposta da API

```json
{
  "idPaciente": 12345,
  "necessitaAcompanhamento": true,
  "probabilidade": 0.85,
  "justificativa": "O paciente necessita de acompanhamento especializado pelos seguintes motivos: O paciente faltou a 2 consultas sem justificativa prévia; O percentual de faltas é de 25.0%, considerado alto.",
  "detalhesConsultas": [
    "Consulta ID 1002 marcada para 01/02/2023 15:00 não foi realizada",
    "Última consulta realizada: ID 1001 em 01/01/2023 15:00"
  ],
  "numConsultasNaoRealizadas": 2,
  "numConsultasAgendadasExpiradas": 0,
  "dataUltimaConsulta": "2023-01-01T15:00:00"
}
``` 