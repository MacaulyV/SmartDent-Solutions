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

## 📈 **Evolução entre as Entregas**

A seguir, detalhamos a evolução do projeto ao longo das Sprints, destacando as principais decisões, desafios e avanços realizados em cada etapa.

---

### 🚀 **Sprint 1 – Planejamento e Definição da Arquitetura**

Na primeira sprint, o foco foi discutir e definir a solução em equipe. Principais atividades realizadas:

- Definição da arquitetura geral do sistema e das tecnologias a serem utilizadas.  
- Discussão sobre a integração dos modelos de IA na plataforma.  
- Decisão inicial de que haveriam três modelos diferentes de IA, cada um responsável por uma funcionalidade específica.  
- Sem desenvolvimento prático, apenas planejamento e estruturação da ideia.

---

### 🛠 **Sprint 2 – Início do Desenvolvimento e Primeiros Testes**

Nesta fase, começamos a implementação das primeiras partes do projeto. Principais marcos:

- Desenvolvimento inicial da API em C# (.NET) e Java, testando qual abordagem seria mais eficiente.  
- A princípio, o plano era construir todo o sistema em Java (Backend e Frontend), mas mudamos essa decisão durante a sprint seguinte.  
- Primeiro protótipo do modelo de IA para detecção de uso excessivo, porém ainda sem treinamento real e sem integração via API.  
- Estruturação da API Fast ainda incompleta, pois precisávamos finalizar a base do sistema para entender como o modelo de IA receberia requisições HTTP e retornaria análises.

---

### 🔄 **Sprint 3 – Definição Final e Integração das Soluções**

Neste estágio, consolidamos a arquitetura e fizemos ajustes importantes:

- Decisão definitiva sobre a estrutura do sistema, garantindo alinhamento com a implementação real.  
- Revisão do plano original de IA: em vez de três modelos distintos, passamos a utilizar apenas dois:  
  - Modelo único para análise de padrões de uso e acompanhamento dos pacientes, garantindo maior eficiência e integração entre essas funcionalidades.  
  - Modelo separado para o aplicativo mobile, que funciona de maneira complementar, ajudando os pacientes a evitar alertas de uso excessivo e oferecendo recomendações personalizadas.  
- Finalização da estrutura de dados no Oracle.  
- API Central (.NET) praticamente concluída, com endpoints definidos e prontos para integração.  
- Treinamento e integração do modelo de IA via FastAPI, permitindo comunicação entre a IA e a API Central em C#.  
- Início do desenvolvimento do aplicativo mobile e da interface do sistema, que consumirá as APIs, com previsão de conclusão até Sprint 4.

---

## 🎥 **Demonstração e Apresentação**

### 🏷 Deploys Disponíveis

- **API de IA (FastAPI)**  
  [https://smartdent-ai.onrender.com/docs](#)  

- **API Principal (C#)**  
  [https://smartdent-ai.onrender.com/docs](#)  

---

## 🧑‍💻 **Equipe de Desenvolvimento**

- **Macauly Vivaldo da Silva** – *Frontend & UX/UI, IA & Backend*  
- **Daniel Bezerra da Silva Melo** – *Mobile Developer & Infraestrutura DevOps (Deploy)*  
- **Gustavo Rocha Caxias** – *Banco de Dados*  

---

## Main - README.md

```markdown
# SmartDent Solutions - Main

## Objetivo
Este repositório é o ponto central da versão final da solução SmartDent. Após a conclusão de todas
as integrações, testes e validações, a branch `Main` contém a versão estável e pronta para o deploy
da plataforma SmartDent, unificando Mobile, Front-end, Back-end, e Modelos de IA.

2. Siga as instruções específicas de cada subprojeto (Back-end, Front-end, Mobile, Models-IA) para
configurar o ambiente.

## Estrutura do Código
- `/back-end`: Código do sistema de gerenciamento central
- `/front-end`: Interface para visualização e interação dos dados
- `/mobile`: Aplicativo para beneficiários da OdontoPrev
- `/models-ia`: Modelos de IA integrados na plataforma

## Regras de Commit
- Commits devem ser feitos na branch `Main` somente após o Pull Request ser revisado e aprovado,
  garantindo que o código na branch `Main` esteja sempre estável e pronto para o deploy.
