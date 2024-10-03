![Descrição da Imagem](https://github.com/user-attachments/assets/83cd9ed6-6cbd-4102-ad10-d4f76acab151)
# SmartDent Solutions
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
- **🏆 Aprimoramento do Atendimento:** Compreende as necessidades dos beneficiários, possibilitando ajustes nos planos ou oferta de suporte adicional.

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
   
2. **🦷 Cadastro e Gerenciamento de Dentistas**
   - Facilitar o registro, atualização e exclusão de dados dos profissionais.
   
3. **🛠️ Cadastro e Gerenciamento de Procedimentos**
   - Gerenciar o catálogo de procedimentos odontológicos disponíveis.
   
4. **📅 Agendamento de Consultas**
   - Permitir que pacientes agendem, visualizem e cancelem consultas.
   
5. **📝 Registro de Consultas e Procedimentos Realizados**
   - Documentar cada consulta realizada e os procedimentos executados durante a mesma.
   
6. **🛡️ Monitoramento de Uso Excessivo de Serviços**
   - Identificar e alertar sobre padrões de uso que excedem a média.
   
7. **💰 Detecção de Cobranças Indevidas**
   - Analisar e identificar discrepâncias nos valores cobrados por procedimentos.
   
8. **📉 Monitoramento de Acompanhamento dos Pacientes**
   - Rastrear e notificar sobre faltas a consultas de acompanhamento.
   
9. **⚠️ Geração e Gerenciamento de Alertas**
   - Criar, atualizar e gerenciar alertas gerados pelos modelos de IA.
    
10. **📊 Visualização de Dashboards e Relatórios**
    - Exibir dados analíticos e insights gerados pela IA através de dashboards interativos.
    
11. **📢 Notificações e Comunicações aos Pacientes**
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

## 🗄️ Estrutura do Banco de Dados Usado no Projeto .NET

O projeto utiliza um banco de dados relacional para armazenar informações sobre pacientes, consultas e alertas. Abaixo estão as principais tabelas utilizadas:

### Tabelas Principais

1. **Pacientes**
   - Armazena os dados dos pacientes.
   - **Campos:** `PacienteID`, `Nome`, `DataNascimento`, `Contato`.

2. **Consultas**
   - Registra as consultas realizadas.
   - **Campos:** `ConsultaID`, `PacienteID`, `DataConsulta`, `DescricaoProcedimento`.

3. **Alertas**
   - Armazena alertas gerados quando o número de consultas se desvia da média.
   - **Campos:** `AlertaID`, `PacienteID`, `DataAlerta`, `MotivoAlerta`.

### Relacionamentos
- Um **Paciente** pode ter várias **Consultas**.
- Um **Alerta** está relacionado a um **Paciente** e suas consultas.

---

# 📐 Desenho da Arquitetura do Sistema .NET de Monitoramento de Consultas Odontológicas

## Contexto e Propósito da Arquitetura

Esta arquitetura foi desenvolvida com base no projeto **SmartDent Solutions**, que visa resolver problemas de sinistros odontológicos por meio de **Inteligência Artificial (IA)**. O projeto utiliza o padrão **Clean Architecture** para manter o código desacoplado, modular e de fácil manutenção.

No contexto do projeto **SmartDent**, o sistema foi dividido em camadas que separam as responsabilidades e facilitam a escalabilidade da solução. Para efeitos de implementação prática no **projeto em C# .NET**, focamos na lógica da funcionalidade de **detecção de consultas excessivas**, aplicando a arquitetura limpa na implementação dessa funcionalidade.

---

## 🔄 Integração da Arquitetura no Projeto .NET

A arquitetura implementada segue os mesmos princípios do projeto principal, mas focamos em simular o caso de uso da **detecção de consultas excessivas**. No projeto **.NET**, implementamos o controle de pacientes, consultas e alertas, usando o **Entity Framework Core** para persistência de dados.

A seguir, detalhamos as camadas da arquitetura e como cada uma delas foi estruturada e implementada no **projeto C#**:

---

## 🏗️ Estrutura das Camadas

### 1. **Apresentação (Presentation)**

Esta camada lida com a interface de usuário e a comunicação com os serviços de aplicação. No projeto **.NET**, essa camada é responsável pelos controladores que recebem e processam as requisições do usuário ou da API.

- **Controlador de Pacientes:** Gerencia as requisições relacionadas ao cadastro e listagem de pacientes.
- **Controlador de Alertas:** Gerencia os alertas gerados para pacientes com consultas excessivas.
- **Interface do Usuário:** Exibe os dados e interações para o administrador da OdontoPrev.

---

### 2. **Aplicação (Application)**

A camada de aplicação contém os **Serviços de Aplicação** que coordenam a lógica dos casos de uso, conectando a apresentação ao domínio.

- **ConsultaService:** Responsável por verificar o número de consultas dos pacientes e gerar alertas se necessário.
- **AlertaService:** Lida com a geração e exibição de alertas baseados nos dados de consultas.
- **Casos de Uso:** Implementa a lógica que identifica consultas excessivas e aciona o alerta para a equipe.

---

### 3. **Domínio (Domain)**

A camada de domínio contém as **Entidades** que representam os conceitos de negócio e as **Regras de Negócio** que regem o comportamento do sistema. Nesta parte, focamos nas entidades relacionadas a pacientes, consultas e alertas.

- **Entidades:**
  - **Paciente:** Representa os dados do paciente, como histórico de consultas.
  - **Consulta:** Registra os dados de cada consulta realizada.
  - **Alerta:** Contém informações sobre os alertas gerados quando o limite de consultas é excedido.
  
- **Regras de Negócio:**
  - A regra central é verificar se o paciente excedeu o número de consultas permitidas em um período específico.

---

### 4. **Infraestrutura (Infrastructure)**

Esta camada lida com o acesso ao banco de dados e outras integrações externas. No projeto **.NET**, usamos o **Entity Framework Core** para mapear as entidades e realizar operações CRUD (Create, Read, Update, Delete).

- **PacienteRepository:** Acessa o banco de dados para gerenciar pacientes.
- **ConsultaRepository:** Armazena e consulta os dados de consultas.
- **AlertaRepository:** Gerencia a persistência dos alertas gerados.
- **Entity Framework Core:** Usado para mapear as entidades e sincronizar com o banco de dados.

---

## 🖼️ Diagrama de Arquitetura

O diagrama a seguir mostra a interação entre as camadas da **Clean Architecture** para o caso de uso implementado de **detecção de consultas excessivas**.

![Descrição da Imagem](https://github.com/user-attachments/assets/881c80f8-184c-4d17-b718-271523db6485)

---

## Lógica do diagrama usando a Ferramenta Mermaid

## graph TD
    Apresentação -->|Recebe Requisições| Aplicação
    Aplicação -->|Verifica Limites| Domínio
    Domínio -->|Envia Alerta| Aplicação
    Aplicação -->|Grava Dados| Infraestrutura
    Infraestrutura -->|Banco de Dados| EntityFramework[(EF Core)]
    Aplicação -->|Retorna Dados| Apresentação
    
    subgraph Apresentação
        A1[Controlador de Pacientes]
        A2[Controlador de Alertas]
        A3[Interface do Usuário]
    end

    subgraph Aplicação
        B1[ConsultaService]
        B2[AlertaService]
    end

    subgraph Domínio
        C1[Paciente]
        C2[Consulta]
        C3[Alerta]
        C4[Regra de Limite]
    end

    subgraph Infraestrutura
        D1[PacienteRepository]
        D2[ConsultaRepository]
        D3[AlertaRepository]
    end
    
---
