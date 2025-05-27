![Descrição banner](https://github.com/user-attachments/assets/acf148aa-b44a-4ebd-9085-9ce4f31ecaf0)

# SmartDent Solutions – Documentação Final

## 🤖 Sobre o Projeto

O **SmartDent Solutions** é um sistema que usa IA pra ajudar operadoras odontológicas a não perderem dinheiro com uso exagerado dos serviços, acompanhar pacientes de perto e melhorar a experiência de quem usa o convênio. Tudo focado em automação inteligente: identificar padrões esquisitos, gerar alertas quando detecta riscos e recomendar ações.

---

## 🎯 **Objetivo**

- **Detectar uso fora do normal** dos serviços.
- **Avisar sobre pacientes sumidos** ou que podem abandonar o tratamento.
- **Reduzir custo**, sem ferrar quem realmente precisa do serviço.
- **Melhorar atendimento**, porque ninguém merece ficar no escuro sobre o próprio histórico.

---

## 📋 Visão Geral das Entregas da Sprint 4

Nesta Sprint, o projeto **SmartDent Solutions** evoluiu em quatro pontos principais:

1. **Documentação com Swagger:**
    
    Implementação e organização completa da documentação interativa dos endpoints via Swagger.
    
2. **Integração com ML.NET:**
    
    Desenvolvimento e integração do modelo de IA para análise preditiva de acompanhamento de pacientes usando ML.NET.
    
3. **Serviço Externo Integrado + Clean Code:**
    
    Consumo de um modelo de IA externo via API (implementado desde a Sprint 3), agora seguindo práticas sólidas de Clean Code.
    
4. **Testes Unitários Automatizados:**
    
    Criação de uma branch dedicada para testes (`api-tests`), onde todos os endpoints da API foram testados automaticamente.
    

---

## 1️⃣ **Documentação com Swagger**

### 🛠️ **Como foi implementado?**

- **Swashbuckle.AspNetCore:**
    
    Usamos essa lib pra gerar a documentação Swagger da API. Ela mapeia todos os controllers, actions e modelos pra interface visual do Swagger UI.
    
- **Comentários XML no Código:**
    
    Todos os endpoints receberam comentários explicativos direto no código (`/// <summary> ... </summary>`), que aparecem automaticamente no Swagger.
    
- **Exemplos de Requests/Responses:**
    
    Configuramos exemplos práticos (ex: corpo de requisições POST/PUT), pra quem for testar já ver os formatos esperados.
    
- **Personalização Visual:**
    
    Alteramos o CSS do Swagger UI (`wwwroot/swagger-ui/custom.css`) pra deixar a cara da doc mais alinhada com o projeto.
    
- **Organização por Controladores:**
    
    Cada funcionalidade está agrupada: Pacientes, Consultas, Procedimentos, Alertas etc, facilitando achar e testar cada parte.
    

### 🔍 **Como funciona na prática?**

- Qualquer dev acessa [https://smartdentapi.fly.dev/api-docs](https://smartdentapi.fly.dev/api-docs), vê todos endpoints, parâmetros, exemplos de resposta, testa direto na interface e já entende o funcionamento sem depender de manual.
- O time de negócio consegue enxergar o que a API oferece sem precisar de conhecimento técnico.

---

## 2️⃣ **Integração com ML.NET**

### 🧠 **Como foi implementado?**

- **Projeto separado de IA na solução:**
    
    Criamos uma pasta ML com dois arquivos principais:
    
    - `DadosAnaliseAcompanhamento.cs` — Define os dados de entrada, saída e estrutura dos dados analisados.
    - `ServicoAnaliseAcompanhamento.cs` — Lógica da IA: treino do modelo, análises, explicação dos resultados e sugestão de ações.
- **Treinamento com dados reais/simulados:**
    
    O modelo de regressão logística do ML.NET foi treinado com um dataset gerado pelo sistema (consultas, faltas, tempo entre consultas, etc).
    
- **Injeção de dependência:**
    
    O serviço de IA foi injetado via DI, podendo ser chamado de qualquer controller que precise analisar pacientes.
    
- **Resultados explicativos:**
    
    A resposta da IA inclui se precisa de acompanhamento, nível de alerta, justificativa e sugestão de ação, tudo pronto pra ser consumido pela aplicação ou exibido pro usuário.
    

### 🦾 **Como funciona na prática?**

- O backend chama a IA sempre que precisa analisar o risco de abandono ou uso exagerado por paciente.
- O resultado detalhado já volta pronto pro frontend exibir, sem precisar de processamento extra.
- O modelo pode ser re-treinado fácil quando o banco crescer ou mudar.

---

## 3️⃣ **Serviço Externo Integrado + Clean Code**

### 🌐 **Integração com Serviço Externo de IA (desde a Sprint 3)**

- **Integração já existente:**
    
    Desde a Sprint 3, nossa API já consome um modelo de IA externo através de uma API REST dedicada.
    
    O backend envia dados dos pacientes para esse serviço externo, recebe a análise pronta (ex: predição de risco, justificativas, sugestões) e repassa para a aplicação e/ou exibe para o usuário.
    
- **Por que isso conta como serviço externo?**
    
    O modelo de IA não roda local, mas sim numa API separada (FastAPI Python hospedada externamente), garantindo escalabilidade, independência tecnológica e flexibilidade pra atualizar ou trocar o modelo sem mexer no sistema principal.
    

### 💡 **Como foi estruturado? (Clean Code na prática)**

- **Cliente HTTP dedicado:**
    
    Criamos uma classe só pra consumir o serviço externo, isolando essa integração do resto do código.
    
- **Interface de abstração:**
    
    Implementamos uma interface para o serviço de análise, permitindo trocar a fonte dos dados (modelo local ou externo) sem alterar o código dos controllers.
    
- **Separação clara de camadas:**
    
    Toda lógica de chamada, tratamento de resposta e erro fica centralizada no client, mantendo controllers e services limpos.
    
- **Tratamento de falhas:**
    
    Sempre que a API externa falha, retornamos mensagens claras e registramos logs, evitando queda geral do sistema.
    

### 🤝 **Na prática:**

- Ao pedir uma análise de paciente, nossa API faz uma requisição HTTP para o serviço externo, recebe a resposta estruturada e retorna direto pro frontend, tudo via código desacoplado e testável.
- Isso permite evoluir o modelo, mudar para outro serviço externo no futuro, ou até rodar local, só trocando a implementação da interface — **sem refatorar todo o sistema**.

---

## 4️⃣ **Testes Unitários Automatizados**

### 🧪 **Como foi implementado?**

- **Branch dedicada (`api-tests`):**
    
    Criamos uma branch só pra testes automatizados, mantendo separado do código principal.
    
- **Framework xUnit:**
    
    Usamos xUnit pra escrever e rodar todos os testes nos controllers e serviços.
    
- **Cobertura de endpoints:**
    
    Todos endpoints principais (GET, POST, PUT, DELETE) foram testados, incluindo casos de erro e sucesso.
    
- **Mock de dependências:**
    
    Utilizamos mocks pra simular repositórios e serviços, garantindo que os testes focassem só na lógica dos endpoints.
    
- **CI/CD pronto pra rodar testes:**
    
    Configuração da pipeline pra rodar os testes automaticamente antes do deploy.
    

### 🧑‍🔬 **Como funciona na prática?**

- Qualquer alteração de código dispara os testes automaticamente.
- Erros ou falhas nos endpoints já aparecem antes de subir pra produção, garantindo qualidade e segurança.
- Facilita manutenção e evita bug besta em produção.

---

## 📁 **Principais Arquivos & Organização do Projeto**

A estrutura do projeto foi pensada pra garantir separação de responsabilidades, facilitar manutenção e seguir boas práticas de arquitetura moderna. Cada pasta/caminho tem um papel específico:

- **Config/**
    
    ⚙️ Arquivos de configuração do sistema (settings, middlewares customizados, etc).
    
- **Controllers/**
    
    🚦 Controladores responsáveis pelos endpoints da API. Toda entrada HTTP começa aqui (ex: PacientesController, ConsultasController).
    
- **DTOs/**
    
    📦 Objetos de transferência de dados usados para trafegar informações entre camadas da aplicação, sem expor entidades do banco diretamente.
    
- **Data/**
    
    🗄️ Configuração do banco de dados, migrations e possíveis seeders (ex: ApplicationDbContext, inicialização do banco).
    
- **Interfaces/Repository/**
    
    🔌 Contratos que definem como os repositórios devem funcionar (ex: IPacienteRepository). Permite trocar implementação sem quebrar a aplicação.
    
- **ML/**
    
    🧠 Toda a lógica da inteligência artificial: definição dos dados analisados, serviço de análise, modelos de machine learning, etc.
    
- **Models/**
    
    📝 Entidades do domínio representando tabelas do banco (Paciente, Consulta, etc).
    
- **Repositories/**
    
    💾 Implementações reais dos repositórios que conversam com o banco (PacienteRepository, ConsultaRepository, etc).
    
- **wwwroot/swagger-ui/**
    
    🖥️ Arquivos estáticos do Swagger UI (customizações visuais da documentação interativa).
    

---

- **Dockerfile**
    
    🐳 Define como a aplicação será empacotada e executada via Docker para deploy.
    
- **Program.cs**
    
    🚀 Ponto de entrada da aplicação, onde tudo é inicializado (injeção de dependência, Swagger, etc).
    
- **SmartDentAPI.csproj**
    
    🛠️ Arquivo de configuração do projeto .NET.
    
- **appsettings.json / appsettings.Development.json**
    
    ⚙️ Configurações de ambiente (conexão com banco, secrets, etc).
    
- **fly.toml**
    
    ☁️ Configuração de deploy para o Fly.io.
    
- **qodana.yaml**
    
    🔬 Configuração de análise de qualidade de código com Qodana.
    

### **ML/**

- `DadosAnaliseAcompanhamento.cs` – Define o que é analisado (features, resultado…)
- `ServicoAnaliseAcompanhamento.cs` – Lógica toda da IA (treino, análise, geração de explicações e sugestões).

---

## 🗄️ **Banco de Dados (SQL Server Azure)**

| Tabela | Função |
| --- | --- |
| Pacientes | Dados básicos do paciente (nome, CPF, nascimento, etc.) |
| Consultas | Consulta feita pelo paciente (data, status, vinculada ao paciente) |
| Procedimentos | Tipo de procedimento feito em cada consulta |
| Alertas | Notificações de risco geradas pela IA |

---

## 🌐 **Endpoints do modelo feito com ML.NET**

- **`GET /api/AnaliseAcompanhamento/individuais`**
    
    Retorna todos os pacientes individuais ordenados por urgência.
    
- **`GET /api/AnaliseAcompanhamento/paciente/{idPaciente}`**
    
    Analisa um paciente específico.
    
- **`GET /api/AnaliseAcompanhamento/empresa?empresa=NomeDaEmpresa`**
    
    Analisa todos os pacientes de uma empresa.
    
- **`POST /api/AnaliseAcompanhamento/treinar-modelo`**
    
    Retreina o modelo com os dados atuais.
    
- **`POST /api/AnaliseAcompanhamento/analisar`**
    
    Permite analisar dados enviados direto no corpo da requisição.
    

### **Formato do Retorno**

```json
json
CopyEdit
{
  "idPaciente": 123,
  "nomePaciente": "Maria Silva",
  "necessitaAcompanhamento": true,
  "nivelAlerta": "Crítico",
  "justificativa": "...",
  "sugestaoAcao": "...",
  "detalhesConsultas": [
    "Consulta ID 456 marcada para 15/03/2023 10:30 não foi realizada"
  ]
}

```

---

## 🚀 **Deploys Disponíveis**

- **API Principal (C#):**
    
    [https://smartdentapi.fly.dev/api-docs](https://smartdentapi.fly.dev/api-docs) (Swagger aberto)
    
- **API IA (FastAPI Python):**
    
    [https://smartdent-ai.onrender.com/docs](https://smartdent-ai.onrender.com/docs)
    

---

## 🧩 **Dependências & Ferramentas**

- **ML.NET**
- **Swashbuckle.AspNetCore (Swagger)**
- **ASP.NET Core 9.0**
- **Banco SQLServer na Azure**

---

## 📖 **Documentação Swagger**

- **Acesso:** [https://smartdentapi.fly.dev/api-docs](https://smartdentapi.fly.dev/api-docs)
- Swagger cobre todos endpoints, exemplos de request/response, explicação dos modelos e códigos de retorno.
- Mantido automaticamente por comentários XML, atributos `[SwaggerOperation]`, `[SwaggerResponse]`, e exemplos customizados.

---

## 👨‍💻 **Equipe**

- **Macauly Vivaldo da Silva** — Front, UX/UI, IA, Backend
- **Daniel Bezerra da Silva Melo** — Mobile, DevOps, Deploy
- **Gustavo Rocha Caxias** — Banco de Dados

---

## 🎬 **Conclusão**

Projeto 100% funcional, com IA real treinada em dados sintéticos e estrutura pronta pra crescer (ou integrar com base real). Entregue com deploy público, documentação transparente e código limpo.
