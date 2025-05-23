![Descrição banner](https://github.com/user-attachments/assets/acf148aa-b44a-4ebd-9085-9ce4f31ecaf0)

# 🦷 **SmartDent Solutions**

### 🤖 **IA Aplicada à Odontologia**

## 📖 **Sobre o Projeto**

O **SmartDent Solutions** é uma plataforma baseada em Inteligência Artificial desenvolvida para a **OdontoPrev**, com o objetivo de **identificar e prevenir sinistros odontológicos**. O projeto visa reduzir custos e melhorar a experiência dos beneficiários, utilizando IA para analisar padrões de uso e comportamento dos pacientes.

### 🎯 **Objetivo da Solução**
- Detectar **uso excessivo** dos serviços odontológicos.  
- Monitorar o **acompanhamento dos pacientes** e identificar ausências.  
- **Reduzir custos operacionais** da operadora de planos odontológicos.  
- Melhorar a **qualidade do serviço** prestado aos clientes.

---

## 🏗 **Visão Geral da Arquitetura da Solução**

A arquitetura do SmartDent Solutions foi projetada para garantir integração eficiente entre seus diversos módulos, proporcionando escalabilidade e um fluxo de dados otimizado. O sistema é composto por:

- **Frontend & Mobile (Java Spring MVC, React, React Native)**
- **Backend (.NET - C#)**
- **API de IA (FastAPI + Scikit-learn)**
- **Banco de Dados (Oracle)**
- **Infraestrutura e Deploy (Render, Azure Cloud, Docker, CI/CD)**

Cada componente desempenha um papel essencial na operação do sistema, conforme detalhado abaixo.

---

### 🔹 **1. Frontend & Mobile**

#### Frontend (Java Spring MVC + React & JavaScript)

Responsável por oferecer uma interface interativa e intuitiva para operadores e funcionários da OdontoPrev. Suas principais funcionalidades incluem:

- Exibição estruturada dos dados dos pacientes, incluindo gráficos, tabelas e dashboards interativos.  
- Monitoramento detalhado do estado dos pacientes, auxiliando na detecção de padrões.  
- Integração com a IA, permitindo análises preditivas e geração de relatórios inteligentes.  

#### Aplicativo Mobile (React Native)

Projetado para que os beneficiários acompanhem, em tempo real, informações como:

- Histórico de consultas e procedimentos.  
- Gastos acumulados no plano odontológico.  
- Recomendações da IA para otimizar o uso do convênio e evitar alertas de uso excessivo.

---

### 🔹 **2. Backend (.NET - C#)**

Atua como ponte central do sistema, sendo responsável por:

- Expor endpoints REST para comunicação com o Frontend e o Mobile.  
- Capturar, processar e armazenar informações dos usuários e pacientes.  
- Realizar chamadas para a API de IA, enviando os dados necessários para análise.  
- Aplicar as regras de negócio específicas da OdontoPrev e gerenciar o fluxo de dados.

---

### 🔹 **3. API de IA (FastAPI + Scikit-learn)**

A API de IA tem um papel fundamental na análise dos dados dos pacientes. Suas funções incluem:

- Pré-processamento e inferência das informações (histórico de consultas, custos, etc.).  
- Carregamento do modelo de Machine Learning (Random Forest) para avaliação de risco.  
- Classificação dos pacientes com base no uso do convênio (ex.: Uso Moderado, Uso Excessivo).  
- Geração de justificativas textuais explicando o motivo da classificação.

Inicialmente, a API de IA está hospedada no Render, permitindo acesso pelo Backend .NET via HTTP.

---

### 🔹 **4. Banco de Dados (Oracle)**

Responsável pelo armazenamento centralizado de todas as informações do sistema, incluindo:

- Dados dos pacientes e seus históricos de consultas e procedimentos.  
- Parâmetros de negócio relevantes para a OdontoPrev.  
- Logs de análises e alertas gerados pela IA, garantindo rastreabilidade e auditoria.

---

### 🔹 **5. Infraestrutura e Deploy**

#### Deploy Inicial

Atualmente, a API de IA está sendo hospedada no Render, permitindo testes e ajustes iniciais.

#### Fase Final de Deploy

O plano é migrar toda a infraestrutura para a Azure Cloud, utilizando:

- Docker para containerização dos serviços.  
- Repositórios e pipelines CI/CD para automação de deploys e atualizações.

---

## 🏗 **Diagrama da Arquitetura**

![Descrição banner](https://github.com/user-attachments/assets/2f15db1a-d6bf-44f3-a7d1-da50122ae3fe)

### Descrição Lógica do Fluxo da Arquitetura

1. **Interação do Usuário:**  
   Usuários acessam o sistema através do frontend web ou do aplicativo móvel.

2. **Comunicação com a API Central:**  
   Tanto o frontend quanto o mobile enviam requisições HTTP para a API central implementada em .NET, que processa as informações e aplica as regras de negócio.

3. **Chamada à API de IA:**  
   Quando necessário, a API central realiza chamadas à API de IA (desenvolvida com FastAPI e Scikit-learn) para obter análises de risco e justificativas.

4. **Persistência dos Dados:**  
   Os dados processados, incluindo logs e análises, são armazenados no banco de dados Oracle para garantir rastreabilidade e integridade.

5. **Infraestrutura e Deploy:**  
   Toda a solução é implantada em uma infraestrutura robusta como (Azure Cloud, Docker, CI/CD), garantindo escalabilidade, facilidade de manutenção e atualizações contínuas.

> **Nota Importante:**  
> Esta visão da arquitetura representa o planejamento final previsto para a Sprint 4 e não reflete completamente a implementação atual.

---

## 📌 **Documentação da API Central do Projeto**

## 1️⃣ Arquitetura

### 🏛️ Por que escolhemos a arquitetura monolítica?
Optamos por uma arquitetura monolítica principalmente pela simplicidade de desenvolvimento e manutenção nesta etapa do projeto. Como temos um único serviço principal que gerencia o fluxo de Pacientes, Consultas, Procedimentos e Alertas, manter tudo em um só lugar facilita o desenvolvimento sem precisar lidar com a complexidade de múltiplos serviços, orquestração e rede interna.

Além disso, como o projeto ainda está em **Fase Beta,** essa abordagem torna o trabalho mais prático, pois:
- ✅ Permite que todos os componentes (controllers, repositórios, modelos) compartilhem as mesmas dependências.
- ✅ Evita a necessidade de gerenciar múltiplas aplicações separadas.
- ✅ Facilita a colaboração e manutenção.

### 🔄 Comparação com microservices
Se tivesse escolhido microservices, cada funcionalidade (Pacientes, Consultas, etc.) seria um serviço independente, com seu próprio banco e comunicação via HTTP ou mensageria.

#### 🟢 Vantagens de microservices:
- ✔️ Melhor escalabilidade (cada serviço pode ser escalado individualmente).
- ✔️ Maior isolamento de falhas (se um serviço cair, o restante continua funcionando).
- ✔️ Possibilidade de usar diferentes tecnologias para cada serviço.

#### 🔴 Desvantagens para este projeto:
- ❌ Aumento da complexidade: seria necessário gerenciar múltiplas aplicações, configurar API Gateway, Discovery Service, etc.
- ❌ Mais esforço para monitoramento, logs e versionamento.
- ❌ Overhead desnecessário para um projeto como esse.

Conclusão: No momento, a arquitetura monolítica é a melhor escolha, pois mantém o desenvolvimento ágil e organizado, sem sobrecarga desnecessária.

---

## 2️⃣ Estrutura do Projeto

### 📂 Estrutura de Pastas e Arquivos
O projeto segue uma organização em camadas, garantindo separação de responsabilidades.

- **📁 Controllers/** → Contém os endpoints HTTP (ex.: PacientesController.cs, ConsultasController.cs).
- **📁 Models/** → Define as entidades do banco de dados (Paciente.cs, Consulta.cs, etc.).
- **📁 Repositories/** → Responsável pelo acesso e manipulação dos dados (PacienteRepository.cs).
- **📁 Interfaces/** → Define os contratos dos repositórios (IPacienteRepository.cs).
- **📁 DTOs/** → Contém os objetos de transferência de dados (PacienteCreateDTO, ConsultaUpdateDTO).
- **📁 Data/** → Configuração do banco (ApplicationDbContext.cs, DataSeeder.cs).
- **📄 Program.cs** → Configurações gerais do projeto, como injeção de dependência e Swagger.

### 💡 **Justificativa:**
Essa estrutura segue boas práticas de separação de responsabilidades, garantindo um código modular e fácil de manter.

---

### 🗄️ Modelo de Tabelas no Oracle
A estrutura do banco foi desenhada para refletir corretamente as relações do domínio:

#### 📝 Tabelas principais:

| Tabela       | Descrição                                                                 |
|--------------|---------------------------------------------------------------------------|
| Pacientes    | Contém dados do paciente (Nome, CPF, Data de Nascimento, etc.).           |
| Consultas    | Relacionada ao paciente (via IdPaciente), armazenando dados da consulta.  |
| Procedimentos| Associada a uma consulta (IdConsulta), armazenando o tipo de procedimento realizado. |
| Alertas      | Associada ao paciente (IdPaciente), armazenando notificações de risco da IA.|

#### 🔗 Relacionamentos:

- 1 Paciente → várias Consultas.
- 1 Consulta → 1 Procedimento.
- 1 Paciente → vários Alertas (alertas independentes, mas vinculados ao paciente).

### 💡 **Justificativa:**
Esse layout garante eficiência nas consultas e mantém a consistência dos dados com regras de exclusão em cascata (Delete Cascade).

---

## 3️⃣ Design Patterns Utilizados

- **📌 Repository Pattern** → Utilizado para abstrair o acesso ao banco, facilitando manutenção e testes.
- **📌 Factory Pattern (uso menor)** → Utilizado em partes do código, como IHttpClientFactory, para melhorar a criação de instâncias reutilizáveis.
- **📌 Arquitetura em Camadas** → Não é um pattern formal, mas uma boa prática para organização do código.

### 💡 **Justificativa:**
O uso desses padrões garante que o código fique mais modular, reutilizável e testável, evitando acoplamento excessivo.

---

## 4️⃣ Exemplos de Testes JSON para Cada Endpoint

### 📌 POST /api/pacientes)
```json
{
  "nome": "João Silva",
  "cpf": "123.456.789-00",
  "dataNascimento": "1985-06-15",
  "email": "joao@email.com",
  "telefone": "(11) 99999-9999",
  "endereco": "Rua A, 123, São Paulo",
  "plano": "Plano Premium",
  "empresa": "Empresa X"
}

 ```

### 📌 PUT /api/pacientes/{id})
```json
{
  "nomeCompleto": "Bruno Costa",
  "email": "bruno.costa@exemplo.com",
  "telefone": "11101935544",
  "endereco": "Rua Exemplo, 52, Bairro 7, Itapeva",
  "planoOdontologico": "Premium",
  "empresa": "Individual"
}

 ```

### 📌 POST /api/consultas)
```json
{
  "idPaciente": 20820516,
  "dataConsulta": "051220251000"
}

 ```

### 📌 PUT /api/D_Consultas/{id})
```json
{
  "dataConsulta": "300720231400",
  "status": "Realizada"
}

 ```

### 📌 POST /api/E_Procedimentos
```json
{
  "idConsulta": 116930515,
  "tipoProcedimento": "Consulta odontológica geral",
  "descricao": "Avaliação geral"
}


 ```

### 📌 PUT /api/E_Procedimentos{id})
```json
{
  "tipoProcedimento": "Restauração em resina composta",
  "descricao": "Troca de restauração antiga"
}

 ```

### 📌 POST /api/alertas)
```json
{
  "idPaciente": 20820516,
  "tipoAlerta": "UsoExcessivo",
  "grauRisco": "99",
  "justificativa": "Paciente realizou 10 consultas no último mês."
}

 ```

### 📌 PUT /api/H_Alertas//{id})
```json
{
  "tipoAlerta": "Uso Moderado com Tendência a Excesso",
  "grauRisco": "45%",
  "justificativa": "Paciente manteve consultas frequentes, mas não excessivas."
}

 ```

---

### 🔗 **Integração entre Módulos**

**Conexão com a API em C#:**  
O desenvolvimento da API da IA teve como objetivo final ser integrada com a API em C#. Durante o desenvolvimento, surgiram desafios relativos à padronização do formato de entrada e saída dos dados, bem como à comunicação entre os módulos, que foram resolvidos através da utilização do FastAPI e de esquemas Pydantic para garantir a consistência dos dados.

**Testes Locais e Validação:**  
Foram realizados extensos testes locais (utilizando o Swagger UI do FastAPI e scripts de teste) para validar a lógica de inferência e garantir que tanto a entrada de um único paciente quanto de múltiplos fossem processadas corretamente.

---

## 🧠 **Arquitetura de IA**

Na SmartDent Solutions, optamos por usar FastAPI (Python) como camada de serviço de IA, onde rodamos o modelo de Machine Learning. Esse modelo foi construído em Scikit-learn, e usamos um Random Forest porque ele lida bem com diferentes tipos de dados (como número de consultas, custo total, histórico de procedimentos) e oferece resultados interpretáveis.

A razão para escolher essa arquitetura é que a API de IA fica independente do restante do sistema (ou seja, separada do backend .NET e do front-end Java/Mobile). Assim, quando a gente precisa atualizar o modelo ou adicionar alguma lógica de análise nova, não mexemos no código do backend principal. Isso deixa tudo mais modular e facilita o deploy de forma independente—no caso, a API de IA está sendo hospedada no Render.

---

## ⚙️ **Implementação na Prática**

No repositório na **branch: Models-IA**, há uma pasta específica (chamada `api/`) que contém os scripts de treinamento e o código da API em FastAPI. A gente treinou o modelo localmente, salvou o arquivo `.joblib`, e a API carrega esse modelo quando inicia. Sempre que o backend .NET recebe alguma informação de um paciente para ser analisada, ele faz uma requisição POST para o endpoint do FastAPI, que então processa os dados, aplica o modelo e retorna um rótulo de risco (por exemplo, **UsoExcessivo**) mais uma justificativa.

---

### 🗂 **Base de Dados Usada Para Treinamento**

Para o treinamento e teste do modelo, nós ultilizamos dados sintéticos que refletem cenários de uso odontológico (quantidade de consultas, custo, status de cada consulta, tipo de procedimento, etc.). A ideia é simular comportamentos de pacientes abusando ou não do convênio, pra conseguirmos treinar a IA a distinguir entre uso normal e uso excessivo. Esses dados foram gerados num script Python que cria registros aleatórios com diferentes padrões de frequência e custo. Assim, a IA aprende com uma variedade de cenários que representam bem o que acontece no dia a dia de um plano odontológico.

#### Por que dados sintéticos?
Porque no momento não temos acesso a dados reais. Mesmo assim, essa base sintética é suficiente para a prova de conceito e pra demonstrar como a IA seria integrada no fluxo real da Odontoprev.

---

### 📋 Exemplo de Teste em JSON para IA

Abaixo, um exemplo de payload **(não real)** que pode ser enviado para a **API da IA**, demonstrando um formato esperado para análise:

 ```json
{
  "idPaciente": 248247482,
  "nomeCompleto": "Eduardo Rocha",
  "cpf": "343.919.106-22",
  "dataNascimento": "04/11/1981",
  "email": "eduardo.rocha@exemplo.com",
  "telefone": "(11) 17255-2789",
  "endereco": "Rua Exemplo, 378, Bairro 1, Guaratinguetá",
  "planoOdontologico": "Bem Estar Orto",
  "empresa": "Individual",
  "numConsultas": 4,
  "gastoTotal": "R$ 870,00",
  "consultas": [
    {
      "idConsulta": 974258209,
      "dataConsulta": "01/05/2024 10:30",
      "status": "Realizada",
      "procedimento": {
        "idProcedimento": 857706282,
        "tipoProcedimento": "Instrução de higiene bucal",
        "descricao": null,
        "custo": "R$ 70,00"
      }
    },
    {
      "idConsulta": 581313226,
      "dataConsulta": "27/05/2024 16:59",
      "status": "Realizada",
      "procedimento": {
        "idProcedimento": 934230516,
        "tipoProcedimento": "Cirurgia periodontal",
        "descricao": null,
        "custo": "R$ 600,00"
      }
    },
    {
      "idConsulta": 44468131,
      "dataConsulta": "18/07/2024 11:52",
      "status": "Realizada",
      "procedimento": {
        "idProcedimento": 523901337,
        "tipoProcedimento": "Tratamento restaurador em dentes de leite",
        "descricao": null,
        "custo": "R$ 200,00"
      }
    },
    {
      "idConsulta": 982321214,
      "dataConsulta": "19/07/2024 12:00",
      "status": "Agendada",
      "procedimento": {
        "idProcedimento": 441760667,
        "tipoProcedimento": "Instrução de higiene bucal",
        "descricao": null,
        "custo": "R$ 70,00"
      }
    }
  ]
}

 ```
---

### 💡 Modelos de Respostas Geradas pela IA

 ```json
{
  "idPaciente": 248247482,
  "nomePaciente": "Eduardo Rocha",
  "tipoAlerta": "Uso Moderado",
  "grauRisco": "44%",
  "justificativa": "Ao analisar os atendimentos entre 01/05/2024 e 18/07/2024, verifiquei que o paciente teve 3 consultas com um gasto acumulado de R$ 870.00 e intervalos de 38.5 dias. Embora haja repetições, como (nenhuma repetição de procedimentos), elas não ultrapassam os limites normais, indicando um uso moderado.",
  "totalConsultas": 3,
  "gastoTotal": "R$ 870,00",
  "dataAnalise": "14/03/2025 03:53",
  "modelo_utilizado": true,
  "confiança": 0.5
}

 ```
---

## 📊 Detalhes Importantes sobre o Modelo

### ⏳ Janela de Análise (Período de 365 dias)
O modelo analisa apenas as consultas realizadas dentro de um período de **365 dias** a partir da última consulta agendada ou realizada do paciente. Essa abordagem torna as análises mais eficientes, permitindo que a IA trabalhe com **dados mais recentes e relevantes**, sem se sobrecarregar com informações antigas que podem não ser mais úteis para o contexto atual do paciente.

---

### 💰 Cálculo do Gasto Total
O modelo considera no cálculo do gasto total **apenas as consultas e procedimentos efetivamente realizados**. Consultas canceladas ou ainda agendadas **não entram nessa soma** e, portanto, não influenciam a justificativa final da IA. Isso evita **distorções nos dados financeiros** do paciente.

---

### 🚦 Status de Retorno do Modelo
Além de calcular o grau de risco, a justificativa e o nível de alerta, o modelo também fornece um valor essencial:

#### 📌 Valor de Confiança
Esse valor indica **o quão "certa" a IA está em relação à predição feita**. Em outras palavras, além de classificar um paciente (por exemplo, como **"Uso Moderado"** ou **"Uso Excessivo"**), o modelo também atribui **uma probabilidade à sua decisão**, baseada nos padrões aprendidos durante o treinamento.

🔹 **Exemplo:** Se o modelo atribui **90% de confiança** a uma predição, isso significa que, com base nos dados históricos, ele acredita fortemente que essa classificação está correta.

Essa medida é essencial porque **ajuda os usuários a entenderem o nível de segurança da decisão**. Quando a confiança é baixa, isso pode indicar a necessidade de uma revisão ou de mais informações para validar a análise.

---

## 🏆 Sprint 4: Direcionamento Estratégico e Próximos Passos

A quarta sprint do **SmartDent Solutions** tem como foco o refinamento e a integração de todas as camadas do projeto, garantindo um sistema mais robusto, seguro e alinhado com as necessidades da **OdontoPrev**. Abaixo, destacamos os principais objetivos e ações planejadas para esta fase.

---

### 🔎 Refinamento e Integração dos Modelos de IA

- Aprimorar os modelos preditivos para garantir maior precisão na identificação de padrões de uso excessivo, acompanhamento de pacientes e confiabilidade dos resultados.
- Integrar definitivamente os modelos com a plataforma principal, tornando a IA interativa e funcional na interface. Isso inclui a conexão com os demais endpoints da **API Central em C#**.

### 🔗 Aprimoramento da Arquitetura e Integração dos Módulos

- Revisar a arquitetura em camadas para garantir uma comunicação eficiente entre backend, IA e as interfaces web e mobile.
- Implementar testes de integração que assegurem um fluxo de dados consistente e confiável entre todos os componentes do sistema.

### 🔒 Segurança e Atualização da Documentação

- Implementar medidas de segurança avançadas, incluindo **JWT para autenticação**, reforçando a proteção dos dados dos usuários.
- Atualizar a documentação do projeto, incluindo **diagramas de integração e fluxos de dados**, para facilitar a compreensão e manutenção futura da solução.

### ⚡ Otimização de Performance e Coleta de Feedback

- Analisar o desempenho da plataforma para identificar possíveis gargalos e propor melhorias que suportem um volume maior de acessos simultâneos.
- Realizar testes com usuários para coletar feedback sobre a experiência de uso, promovendo ajustes na interface e usabilidade conforme necessário.

### 🚀 Preparação para o Deploy Final e Continuidade do Projeto

- Consolidar todas as integrações e configurar um **ambiente de staging** para testes finais antes da implantação em produção.
- Estabelecer um plano de **monitoramento pós-deploy**, incluindo métricas de desempenho e relatórios periódicos para aprimoramento contínuo da solução.

---

## 🎯 Conclusão Final

Os objetivos desta sprint estão planejados de forma estratégica para garantir que a **SmartDent Solutions** funcione de forma mais eficiente, segura e escalável. A integração dos modelos de IA será realizada de forma fluida, consolidando uma solução que otimiza custos, melhora a qualidade dos serviços e proporciona uma experiência superior para os beneficiários e operadores da **OdontoPrev**.

---

## 🎥 **Demonstração e Apresentação**

### 🏷 Deploys Disponíveis

- **API de IA (FastAPI)**  
  [https://smartdent-ai.onrender.com/docs](#)

- **API Principal (C#)**  
  [https://smartdent-ai.onrender.com/docs](#)

Ambas as APIs estão hospedadas no **Render** e, devido às limitações da versão gratuita, podem levar alguns segundos ou até cerca de um minuto para iniciar após a primeira chamada. Esse tempo de espera ocorre porque, quando inativas, as APIs entram em modo de suspensão e precisam ser reativadas antes de processar qualquer requisição.

⚠ **Importante:** Ao realizar o primeiro teste, aguarde entre **1 a 2 minutos** para que a API seja iniciada. Após esse tempo inicial, as requisições subsequentes serão processadas de forma instantânea e sem atrasos.

### 🔎 Recomendações de Teste

Para **testar a aplicação de forma mais simplificada**, recomenda-se utilizar os **endpoints já disponibilizados no deploy** acima. Você também pode aproveitar que **os dados necessários** (pacientes, consultas, etc.) **já estão gerados** para realizar chamadas JSON de exemplo em cada endpoint — conforme foi demonstrado anteriormente na seção de exemplos de testes. Assim, você pode enviar requisições diretamente às URLs em produção, sem precisar configurar ou rodar o ambiente local. 

Isso facilita bastante a verificação do funcionamento dos endpoints e a experimentação dos cenários de CRUD, análise de IA e geração de alertas, pois o banco já contém registros suficientes para ilustrar cada caso de uso.

---

## 🧑‍💻 **Equipe de Desenvolvimento**

- **Macauly Vivaldo da Silva** – *Frontend & UX/UI, IA & Backend*  
- **Daniel Bezerra da Silva Melo** – *Mobile Developer & Infraestrutura DevOps (Deploy)*  
- **Gustavo Rocha Caxias** – *Banco de Dados*  
