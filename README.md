# 🦷 SmartDentAlertas - Plataforma de Monitoramento Odontológico

## 📋 Visão Geral

**SmartDentAlertas** é um sistema desenvolvido para monitoramento de consultas odontológicas, visando a prevenção de **sinistros** e **fraudes de uso excessivo de serviços/consultas por paciente**. Este sistema é voltado para o acompanhamento e análise do comportamento dos pacientes, utilizando uma lógica de simulação de tecnologia avançada de **Inteligência Artificial (IA)**, exemplificando como funcionará no projeto final quando utilizarmos **Machine Learning**.

Este projeto é uma demonstração simples e preliminar de como o sistema final funcionará. A ideia é fornecer uma visão geral de como a **IA** pode ser integrada para monitorar consultas e evitar abusos, porém, nesta versão demo, não há implementação real de **Machine Learning**, apenas uma simulação de lógica que, no futuro, será aprimorada.

## 🎯 Objetivo do Projeto

Demonstrar como o monitoramento e os alertas funcionarão na prática. O objetivo é mostrar de forma simples e clara como o uso de **IA** e **Machine Learning** poderá ser integrado no futuro, trazendo maior precisão e eficiência.

## 🚀 Funcionalidades Principais

- **📅 Marcar Consulta**: Permite ao paciente agendar uma nova consulta.
- **✅ Confirmar Consulta**: Exibe a confirmação dos detalhes de uma consulta agendada.
- **📋 Listar Pacientes**: Mostra uma lista de todos os pacientes cadastrados na plataforma.
- **📊 Análise IA**: Utiliza IA para identificar possíveis **usos excessivos dos serviços** odontológicos e **gerar alertas**.
- **🛑 Limpar Alertas**: Remove alertas pendentes do sistema.

```
## 📁 Estrutura de Pastas e Arquivos

Abaixo está a estrutura do projeto **SmartDentAlertas** e a descrição dos arquivos e pastas mais importantes:

## Controllers
Os controladores são responsáveis por gerenciar as operações e funcionalidades específicas da aplicação.

- **HomeController.cs**  
  Controla a página inicial e a página de erro.

- **PacienteController.cs**  
  Gerencia operações relacionadas aos pacientes.

- **FuncionarioController.cs**  
  Gerencia a exibição de funcionalidades para funcionários.

## Models
Os modelos representam a estrutura dos dados usados pelo sistema.

- **Paciente.cs**  
  Modela os pacientes do sistema.

- **Alertas.cs**  
  Modela os alertas gerados pelo sistema.

- **Consulta.cs**  
  Modela as consultas odontológicas.

- **ErrorViewModel.cs**  
  Modela informações de erros.

## Views
As views são responsáveis pela interface do usuário.

### Home
- **Index.cshtml**  
  Página inicial.

- **Error.cshtml**  
  Página de erro.

### Paciente
- **MarcarConsulta.cshtml**  
  Formulário para marcar uma consulta.

- **Confirmacao.cshtml**  
  Página de confirmação da consulta.

- **ListaPacientes.cshtml**  
  Lista todos os pacientes.

### Alertas
- **ListaAlertas.cshtml**  
  Lista os alertas gerados pelo sistema.

## wwwroot
Arquivos estáticos usados pela aplicação, como CSS, JavaScript e bibliotecas externas.

- **css/**  
  Arquivos CSS customizados.

- **js/**  
  Arquivos JavaScript customizados.

- **lib/**  
  Bibliotecas externas, como Bootstrap e jQuery.

## Arquivos Principais
Configurações e inicialização do aplicativo.

- **appsettings.json**  
  Configurações da aplicação, incluindo a string de conexão com o banco de dados.

- **Program.cs**  
  Inicializa e configura o aplicativo.
```

## 🔧 Tecnologias Utilizadas

- **ASP.NET Core MVC**: Framework utilizado para a construção do backend e das páginas web.
- **Entity Framework Core**: Utilizado para manipulação do banco de dados **Oracle**.
- **Oracle Database**: Banco de dados relacional utilizado para armazenar as informações dos pacientes, consultas e alertas.
- **Bootstrap**: Framework CSS para tornar a interface amigável e responsiva.
- **JavaScript/jQuery**: Utilizado para tornar a aplicação mais dinâmica e interativa.

## 📂 Explicação de Componentes

### 🔹 Controllers

Os **Controllers** são responsáveis pela lógica de negócio do sistema. Eles processam as requisições feitas pelo usuário e retornam as views correspondentes.

- **HomeController.cs**: Controla a página inicial e a página de erro.
- **PacienteController.cs**: Gerencia todas as operações relacionadas aos pacientes, incluindo **marcação de consultas**, **atualização** e **análise por IA**.
- **FuncionarioController.cs**: Exibe as funcionalidades disponíveis para funcionários, como **listagem de pacientes**.

### 🔹 Models

Os **Models** representam a camada de dados do projeto. Cada classe modela uma tabela do banco de dados.

- **Paciente.cs**: Representa os pacientes do sistema, com informações como **nome**, **email**, **data da consulta** e **quantidade de consultas acumuladas**.
- **Alertas.cs**: Representa os alertas gerados pelo sistema quando é identificado um uso excessivo dos serviços.
- **Consulta.cs**: Representa uma consulta odontológica.
- **ErrorViewModel.cs**: Representa informações de erros a serem exibidos ao usuário.

### 🔹 Views

As **Views** são responsáveis pela interface do usuário. São utilizadas para exibir formulários, tabelas e mensagens para os usuários finais.

- **MarcarConsulta.cshtml**: Página de formulário para marcar uma nova consulta.
- **Confirmacao.cshtml**: Página de confirmação após a consulta ser marcada.
- **ListaPacientes.cshtml**: Exibe uma lista de todos os pacientes.
- **ListaAlertas.cshtml**: Exibe os alertas gerados pela **IA**.

## ⚙️ Configuração e Uso do Banco de Dados

O sistema utiliza um banco de dados **Oracle** para armazenar as informações dos pacientes, consultas e alertas.

### 🔸 Estrutura do Banco de Dados Oracle

- **Tabela Pacientes** (`PACIENTE`): Contém os dados dos pacientes, incluindo **PacienteID**, **Nome**, **Email**, **DataConsulta**, e **QuantidadeConsultas**.
- **Tabela Alertas** (`ALERTAS`): Contém os alertas gerados pelo sistema, incluindo **AlertaID**, **NomePaciente**, **EmailPaciente**, e **QuantidadeConsultas**.

### 🔸 Contexto de Dados

- **AppDbContext.cs**: Define o contexto de banco de dados utilizado pelo **Entity Framework** para mapear as entidades **Paciente**, e **Alertas**, nas tabelas do banco de dados.

## 🔍 Testes e Validações

O sistema possui validações para garantir que os dados inseridos estejam corretos. Exemplos de validações:

- **Validação de email** e **nome** no cadastro dos pacientes.
- **Data da consulta deve ser futura**.
- **Quantidade de consultas** deve ser um número positivo.

## 🖼️ Layout e Design

O layout do sistema foi desenvolvido utilizando **Bootstrap** e **CSS** customizado, com foco na responsividade e experiência do usuário. O uso de **gradientes**, **efeitos hover** e **transições** foi feito para tornar a aplicação visualmente atraente e moderna.

Desenvolvido por **Macauly**, **Daniel** e **Gustavo**.

## 2. Front-end - README.md

```markdown
# SmartDent Solutions - Front-end

## Objetivo
Este repositório contém o código do front-end da plataforma SmartDent, que será usado pela equipe
da OdontoPrev para visualizar e interagir com os dados.

## Tecnologias Utilizadas
- React.js
- TypeScript
- CSS/SCSS

## Como Configurar Localmente
1. Clone o repositório:
   ```bash
   git clone --branch Front-end https://github.com/MacaulyV/SmartDent-Solutions.git
   ```
2. Instale as dependências do projeto:
   ```bash
   npm install
   ```
3. Execute o projeto:
   ```bash
   npm start
   ```

## Estrutura do Código
- `/src`: Componentes React e lógica de interface
- `/public`: Arquivos estáticos

## Regras de Commit
- Siga o padrão de commit: `feat:`, `fix:`, `style:`, etc.

## Membros Responsável:
Responsável: [Macauly vivaldo da silva]

![🌟 Descrição da Imagem](https://github.com/user-attachments/assets/83cd9ed6-6cbd-4102-ad10-d4f76acab151)
# 💡 SmartDent Solutions
### *Previna e resolva problemas odontológicos com mais agilidade e precisão.*

---

## 📜 **Descrição Geral**

A **SmartDent Solutions** é uma solução que utiliza **Inteligência Artificial (IA)** para ajudar a **OdontoPrev** a identificar, antecipar e prevenir sinistros odontológicos. Integrada aos sistemas existentes da OdontoPrev, a plataforma oferece análises preditivas detalhadas, permitindo ações proativas que reduzem custos, melhoram a qualidade dos serviços e aumentam a satisfação dos beneficiários.

---

## 🚀 **Principais Funcionalidades**

1. 🛡️ **Análise de Uso Excessivo de Serviços**
2. 💰 **Evitando Cobranças Indevidas**
3. 📉 **Monitoramento de Acompanhamento dos Pacientes**

---

### 🛡️ **1. Análise de Uso Excessivo de Serviços**

**Como Funciona:**  
A IA analisa o histórico de utilização dos conveniados, identificando padrões de uso que desviam significativamente da média. Ao detectar um beneficiário com número excessivo de consultas ou procedimentos em um curto período, a plataforma gera um alerta para a equipe da OdontoPrev.

**Benefícios:**
- **💰 Controle de Custos:** Identifica possíveis abusos ou fraudes, evitando gastos desnecessários.
- **🌟 Aprimoramento do Atendimento:** Compreende as necessidades dos beneficiários, possibilitando ajustes nos planos ou oferta de suporte adicional.

---

### 💰 **2. Evitando Cobranças Indevidas**

**Como Funciona:**  
A IA compara os valores cobrados por procedimentos odontológicos com a média praticada na rede de clínicas e dentistas. Ao identificar tratamentos com custos acima da média, a plataforma gera um alerta detalhado para a OdontoPrev.

**Benefícios:**
- **📈 Controle Financeiro:** Entende melhor os custos associados aos tratamentos, permitindo ajustes necessários.
- **🔧 Ajuste de Planos:** Reavalia e reajusta os benefícios dos planos para manter a viabilidade financeira.

---

### 📉 **3. Monitoramento de Acompanhamento dos Pacientes**

**Como Funciona:**  
A IA rastreia os agendamentos de consultas de acompanhamento. Quando um paciente não comparece a uma consulta agendada e não fornece justificativa, a plataforma notifica a OdontoPrev para possíveis ações.

**Benefícios:**
- **🛡️ Prevenção de Complicações:** Evita agravamentos na saúde do paciente, prevenindo tratamentos mais caros.
- **💸 Redução de Custos:** Minimiza despesas com tratamentos futuros evitáveis.
- **😊 Melhoria da Experiência do Cliente:** Demonstra cuidado e atenção, aumentando a satisfação e confiança na empresa.
  
---

## 📝 **Requisitos Funcionais e Não Funcionais**

### ✅ **Requisitos Funcionais**

1. **📋 Cadastro e Gerenciamento de Pacientes**
   - Permitir o registro, atualização e exclusão de informações dos beneficiários.
   
2. **🛠️ Cadastro e Gerenciamento de Procedimentos**
   - Gerenciar o catálogo de procedimentos odontológicos disponíveis.
   
3. **📅 Agendamento de Consultas**
   - Permitir que pacientes agendem, visualizem e cancelem consultas.
   
4. **📝 Registro de Consultas e Procedimentos Realizados**
   - Documentar cada consulta realizada e os procedimentos executados durante a mesma.
   
5. **🛡️ Monitoramento de Uso Excessivo de Serviços**
   - Identificar e alertar sobre padrões de uso que excedem a média.
   
6. **💰 Detecção de Cobranças Indevidas**
   - Analisar e identificar discrepâncias nos valores cobrados por procedimentos.
   
7. **📉 Monitoramento de Acompanhamento dos Pacientes**
   - Rastrear e notificar sobre faltas a consultas de acompanhamento.
   
8. **⚠️ Geração e Gerenciamento de Alertas**
   - Criar, atualizar e gerenciar alertas gerados pelos modelos de IA.
    
9. **📊 Visualização de Dashboards e Relatórios**
    - Exibir dados analíticos e insights gerados pela IA através de dashboards interativos.
    
10. **📢 Notificações e Comunicações aos Pacientes**
    - Enviar lembretes e comunicações sobre consultas e acompanhamentos.

### 🛠️ **Requisitos Não Funcionais**

1. **⚡ Desempenho**
   - A plataforma deve responder às requisições em menos de 2 segundos.
   
2. **🔒 Segurança**
   - Implementar autenticação e autorização robustas para proteger dados sensíveis.
   
3. **🖥️ Usabilidade**
   - Interface intuitiva e amigável para facilitar o uso por todos os usuários.
   
4. **📈 Escalabilidade**
   - Capacidade de trabalhar com grandes volumes de dados.
   
5. **🛠️ Manutenibilidade**
   - Código modular e bem documentado para facilitar futuras manutenções e atualizações.
   
6. **📦 Compatibilidade**
   - Suporte a múltiplos navegadores e dispositivos, incluindo desktops e mobile.
   
7. **🔐 Privacidade dos Dados**
   - Conformidade com a Lei Geral de Proteção de Dados (LGPD) para garantir a privacidade dos dados dos beneficiários.
   
8. **🕒 Disponibilidade**
    - Manter a plataforma operacional 24/7 com mínimo tempo de inatividade.

---

## 💄 Estrutura do Banco de Dados Usado no Projeto .NET

O projeto utiliza um banco de dados relacional para armazenar informações sobre pacientes, consultas e alertas. Abaixo estão as principais tabelas utilizadas:

### Tabelas Principais

1. **Pacientes**
   - Armazena os dados dos pacientes.
   - **Campos:** `PacienteID`, `Nome`, `Email`, `DataConsulta`, QuantidadeConsulta.
  
2. **Alertas**
   - Armazena alertas gerados quando o número de consultas se desvia da média.
   - **Campos:** `NomePaciente`, `E-mailPaciente`, `QuantidadeConsultasAcumuladas`,.

### Relacionamentos
- Um **📢 Alerta** está relacionado a um **👨️‍⚕️ Paciente** e suas consultas.

---

# 💉 Desenho da Arquitetura do Sistema .NET de Monitoramento de Consultas Odontológicas

## Contexto e Propósito da Arquitetura

Esta arquitetura foi desenvolvida com base no projeto **SmartDent Solutions**, que visa resolver problemas de sinistros odontológicos por meio de **Inteligência Artificial (IA)**. O projeto utiliza o padrão **Clean Architecture** para manter o código desacoplado, modular e de fácil manutenção.

No contexto do projeto **SmartDent**, o sistema foi dividido em camadas que separam as responsabilidades e facilitam a escalabilidade da solução. Para efeitos de implementação prática no **projeto em C# .NET**, focamos na lógica da funcionalidade de **detecção de consultas excessivas**, aplicando a arquitetura limpa na implementação dessa funcionalidade.

## 🔄 Integração da Arquitetura no Projeto .NET

A arquitetura implementada segue os mesmos princípios do projeto principal, mas focamos em apenas por enquanto simular o caso de uso da **detecção de consultas excessivas** de forma simples, pois no projeto final a funcionalidade vai funcionar de forma bem mais profunda e elaborada. 

No projeto **.NET**, implementamos o controle de pacientes, consultas e alertas, usando o **Entity Framework Core** para persistência de dados.

A seguir, detalhamos as camadas da arquitetura e como cada uma delas foi estruturada e implementada no **projeto C#**:

---

## 🏰 Estrutura das Camadas

### 1. **Apresentação (Presentation)**

Esta camada lida com a interface de usuário e a comunicação com os serviços de aplicação. No projeto **.NET**, essa camada é responsável pelos controladores que recebem e processam as requisições do usuário ou da API.

- **👨️‍💻 Controlador de Pacientes:** Gerencia as requisições relacionadas ao cadastro e listagem de pacientes.
- **📢 Controlador de Alertas:** Gerencia os alertas gerados para pacientes com consultas excessivas.
- **💻 Interface do Usuário:** Exibe os dados e interações para o administrador da OdontoPrev.

---

### 2. **Aplicação (Application)**

A camada de aplicação contém os **🛠️ Serviços de Aplicação** que coordenam a lógica dos casos de uso, conectando a apresentação ao domínio.

- **📑 ConsultaService:** Responsável por verificar o número de consultas dos pacientes e gerar alertas se necessário.
- **📢 AlertaService:** Lida com a geração e exibição de alertas baseados nos dados de consultas.
- **📊 Casos de Uso:** Implementa a lógica que identifica consultas excessivas e aciona o alerta para a equipe.

---

### 3. **Domínio (Domain)**

A camada de domínio contém as **📚 Entidades** que representam os conceitos de negócio e as **💸 Regras de Negócio** que regem o comportamento do sistema. Nesta parte, focamos nas entidades relacionadas a pacientes, consultas e alertas.

- **Entidades:**
  - **👨️‍⚕️ Paciente:** Representa os dados do paciente, como histórico de consultas.
  - **🗓️ Consulta:** Registra os dados de cada consulta realizada.
  - **📢 Alerta:** Contém informações sobre os alertas gerados quando o limite de consultas é excedido.
  
- **Regras de Negócio:**
  - 💡 A regra central é verificar se o paciente excedeu o número de consultas permitidas em um período específico.

---

### 4. **Infraestrutura (Infrastructure)**

Esta camada lida com o acesso ao banco de dados e outras integrações externas. No projeto **.NET**, usamos o **Entity Framework Core** para mapear as entidades e realizar operações CRUD (Create, Read, Update, Delete).

- **💻 PacienteRepository:** Acessa o banco de dados para gerenciar pacientes.
- **🗓️ ConsultaRepository:** Armazena e consulta os dados de consultas.
- **📢 AlertaRepository:** Gerencia a persistência dos alertas gerados.
- **📏 Entity Framework Core:** Usado para mapear as entidades e sincronizar com o banco de dados.

---

## 🖼 Diagrama de Arquitetura

O diagrama a seguir mostra a interação entre as camadas da **Clean Architecture** para o caso de uso implementado de **detecção de consultas excessivas**.

<p align="center">
  <img src="https://github.com/user-attachments/assets/881c80f8-184c-4d17-b718-271523db6485" alt="🖼 Descrição da Imagem" />
</p>

---

## 🤖 Lógica do Diagrama

**🏛️ Apresentação** -->|Recebe Requisições| **🚀 Aplicação**  

**🚀 Aplicação** -->|Verifica Limites| **📚 Domínio**  

**📚 Domínio** -->|Envia Alerta| **🚀 Aplicação**  

**🚀 Aplicação** -->|Grava Dados| **🏢 Infraestrutura**  

**🏢 Infraestrutura** -->|Banco de Dados| **📏 EntityFramework**  

**🚀 Aplicação** -->|Retorna Dados| **🏛️ Apresentação**
    
---

---

## 🛠️ Status Atual do Projeto

Ainda não concluímos todas as funcionalidades do projeto, mas continuaremos e finalizaremos na próxima entrega, na segunda fase do desenvolvimento.

### 💻 Funcionalidades Implementadas
- **📅 Agendamento de Consultas**: Concluímos a funcionalidade de agendamento, incluindo o salvamento dos dados no banco de dados Oracle.
- **📈 Exibição dos Dados**: Implementamos a exibição dos dados em uma tabela destinada aos funcionários.

## 🛍️ Próximos Passos - Sprint 2

Na Sprint 2, focaremos em implementar as seguintes melhorias e novas funcionalidades:

1. **📢 Funcionalidade Principal de Alertas**: Conclusão da lógica para a funcionalidade de alertas.
2. **💻 CRUD Completo**: Implementação de um CRUD para controle dos dados dos pacientes na página de funcionários.
3. **🔐 Página de Login**: Criação de uma página de login para simular um cenário real de acesso a informações importantes.
4. **🌸 Melhoria de UI/UX**: Reestilização completa da aplicação, tornando-a mais agradável e intuitiva.
5. **💡 Possíveis Novas Funcionalidades**: Avaliaremos a inclusão de novas funcionalidades para melhorar a experiência do usuário e as capacidades do sistema.

---
