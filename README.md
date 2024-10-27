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

```markdown
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
