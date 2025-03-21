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

## 🏗 **Diagrama da Arquitetura em Nuvem**

![Descrição banner](https://github.com/user-attachments/assets/a9bd0116-91d0-4f0c-abe2-e4ed01d0d0a5)

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
  "nomeCompleto": "João Silva",
  "cpf": "12345678900",
  "dataNascimento": "19850615",
  "email": "joao@email.com",
  "telefone": "11999999999",
  "endereco": "Rua A, 123, São Paulo",
  "planoOdontologico": "Maximum White",
  "empresa": "Empresa X"
}

 ```

### 📌 PUT /api/pacientes/{id})
```json
{
  "nomeCompleto": "Aline Ferreira",
  "email": "aline.ferreira@exemplo.com",
  "telefone": "11699704506",
  "endereco": "Rua Exemplo, 379, Bairro 5, Ituverava",
  "planoOdontologico": "Master",
  "empresa": "Centauro"
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

## 🎥 **Demonstração e Apresentação**

### 🏷 Deploy Disponível

- **API de IA (FastAPI)**  
  [https://smartdent-ai.onrender.com/docs](#)

- **API Principal (C#)**  
  [https://smartdent-ai.onrender.com/docs](#)

Ambas as APIs estão no **Render** e essa versão delas estão conectadas com o Oracle não na nuvem, e devido às limitações da versão gratuita, podem levar alguns segundos ou até cerca de um minuto para iniciar após a primeira chamada. Esse tempo de espera ocorre porque, quando inativas, as APIs entram em modo de suspensão e precisam ser reativadas antes de processar qualquer requisição.

⚠ **Importante:** Ao realizar o primeiro teste, aguarde entre **1 a 2 minutos** para que a API seja iniciada. Após esse tempo inicial, as requisições subsequentes serão processadas de forma instantânea e sem atrasos.

### 🔎 Recomendações de Teste

Para **testar a aplicação de forma mais simplificada**, recomenda-se utilizar os **endpoints já disponibilizados no deploy** acima. Você também pode aproveitar que **os dados necessários** (pacientes, consultas, etc.) **já estão gerados** para realizar chamadas JSON de exemplo em cada endpoint — conforme foi demonstrado anteriormente na seção de exemplos de testes. Assim, você pode enviar requisições diretamente às URLs em produção, sem precisar configurar ou rodar o ambiente local. 

Isso facilita bastante a verificação do funcionamento dos endpoints e a experimentação dos cenários de CRUD, análise de IA e geração de alertas, pois o banco já contém registros suficientes para ilustrar cada caso de uso.

---

## 🧑‍💻 **Equipe de Desenvolvimento**

- **Macauly Vivaldo da Silva** – *Frontend & UX/UI, IA & Backend*  
- **Daniel Bezerra da Silva Melo** – *Mobile Developer & Infraestrutura DevOps (Deploy)*  
- **Gustavo Rocha Caxias** – *Banco de Dados*  
