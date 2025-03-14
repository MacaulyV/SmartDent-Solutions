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

- **Frontend & Mobile (Java Spring MVC & React Native)**
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

## 🤔 **Considerações Finais e Problemas Enfrentados**

Durante a Sprint 3 do projeto, foi necessário adaptar e reformular diversas partes da solução para atender tanto às novas exigências quanto à evolução das entregas. Esse processo envolveu a reestruturação do pipeline de análise de uso odontológico, principalmente na integração entre a API em C# e o módulo de IA. A seguir, destacam-se os principais pontos e desafios:

---

### ⚙️ **Adaptação do Modelo de Análise**

**Evolução do Conceito:**  
Inicialmente, a abordagem adotada era puramente heurística, baseada em regras pré-definidas (como a quantidade de consultas realizadas e intervalos entre elas) para classificar o risco do paciente.

**Hibridização da Lógica:**  
Após revisarmos os requisitos e entendermos melhor as necessidades do projeto, optamos por uma abordagem híbrida. Essa nova estratégia combina a predição de um modelo treinado (usando técnicas de Machine Learning, como RandomForest) com a geração de justificativas por meio de templates. Essa combinação permitiu capturar nuances dos dados históricos e, ao mesmo tempo, fornecer respostas em linguagem natural e detalhadas para cada paciente.

---

### 🔍 **Problemas com o Tratamento dos Dados**

**Formato dos Dados:**  
Um dos grandes desafios foi lidar com a variedade de formatos presentes no JSON de entrada, especialmente no que tange aos valores monetários (ex.: "R$ 860,00") e datas. Foi necessário desenvolver funções específicas para limpar e converter esses dados (por exemplo, converter o custo para número e formatar as datas para o padrão %d/%m/%Y %H:%M).

**Extração de Features:**  
A definição das features corretas para treinar o modelo também exigiu ajustes. Tivemos que alinhar as colunas do CSV de treinamento com os dados extraídos do JSON, garantindo que contagens por categoria (como "count_ConsultaseDiagnóstico" ou "count_PrevençãoeProfilaxia") fossem calculadas de forma consistente.

---

### 🔗 **Integração entre Módulos**

**Conexão com a API em C#:**  
O desenvolvimento da API da IA teve como objetivo final ser integrada com a API em C#. Durante o desenvolvimento, surgiram desafios relativos à padronização do formato de entrada e saída dos dados, bem como à comunicação entre os módulos, que foram resolvidos através da utilização do FastAPI e de esquemas Pydantic para garantir a consistência dos dados.

**Testes Locais e Validação:**  
Foram realizados extensos testes locais (utilizando o Swagger UI do FastAPI e scripts de teste) para validar a lógica de inferência e garantir que tanto a entrada de um único paciente quanto de múltiplos fossem processadas corretamente.

---

## ✅ **Conclusão**

A Sprint 3 foi marcada por uma significativa evolução no desenvolvimento do projeto, onde desafios de integração, padronização de dados e a definição de uma abordagem híbrida foram superados. Essa nova estratégia não só permitiu capturar padrões mais sutis nos dados por meio do modelo treinado, como também garantiu a geração de justificativas detalhadas e naturalizadas, proporcionando uma análise robusta e de fácil entendimento para os usuários finais.

Apesar das dificuldades iniciais, as lições aprendidas durante essa fase fortaleceram o projeto, preparando-o para a próxima etapa de integração com das APIs com ás interfaces.

---

## 🧠 **Arquitetura de IA**

Na SmartDent Solutions, optamos por usar FastAPI (Python) como camada de serviço de IA, onde rodamos o modelo de Machine Learning. Esse modelo foi construído em Scikit-learn, e usamos um Random Forest porque ele lida bem com diferentes tipos de dados (como número de consultas, custo total, histórico de procedimentos) e oferece resultados interpretáveis.

A razão para escolher essa arquitetura é que a API de IA fica independente do restante do sistema (ou seja, separada do backend .NET e do front-end Java/Mobile). Assim, quando a gente precisa atualizar o modelo ou adicionar alguma lógica de análise nova, não mexemos no código do backend principal. Isso deixa tudo mais modular e facilita o deploy de forma independente—no caso, a API de IA está sendo hospedada no Render.

---

## ⚙️ **Implementação na Prática**

No repositório, há uma pasta específica (chamada `api/`) que contém os scripts de treinamento e o código da API em FastAPI. A gente treina o modelo localmente (ou num ambiente de dados), salva o arquivo `.joblib`, e a API carrega esse modelo quando inicia. Sempre que o backend .NET recebe alguma informação de um paciente para ser analisada, ele faz uma requisição POST para o endpoint do FastAPI, que então processa os dados, aplica o modelo e retorna um rótulo de risco (por exemplo, **UsoExcessivo**) mais uma justificativa.

---

### 🗂 **Base de Dados Usada**

Para o treinamento e teste do modelo, nós ultilizamos dados sintéticos que refletem cenários de uso odontológico (quantidade de consultas, custo, status de cada consulta, tipo de procedimento, etc.). A ideia é simular comportamentos de pacientes abusando ou não do convênio, pra conseguirmos treinar a IA a distinguir entre uso normal e uso excessivo. Esses dados foram gerados num script Python que cria registros aleatórios com diferentes padrões de frequência e custo. Assim, a IA aprende com uma variedade de cenários que representam bem o que acontece no dia a dia de um plano odontológico.

#### Por que dados sintéticos?
Porque no momento não temos acesso a dados reais. Mesmo assim, essa base sintética é suficiente para a prova de conceito e pra demonstrar como a IA seria integrada no fluxo real da Odontoprev.

---

### 📋 Exemplo de Teste em JSON para IA

Abaixo, um exemplo de payload **(não real)** que pode ser enviado para a **API da IA**, demonstrando um formato esperado para análise:

 ```json
{
  "idPaciente": 20820516,
  "nomeCompleto": "Bruno Costa",
  "cpf": "607.408.751-30",
  "dataNascimento": "25/04/1997",
  "email": "bruno.costa@exemplo.com",
  "telefone": "(11) 10193-0760",
  "endereco": "Rua Exemplo, 52, Bairro 7, Itapeva",
  "planoOdontologico": "Premium",
  "empresa": "Individual",
  "numConsultas": 8,
  "gastoTotal": "R$ 660,00",
  "consultas": [
    {
      "idConsulta": 310695008,
      "dataConsulta": "26/05/2024 09:57",
      "status": "Não Realizada",
      "procedimento": {
        "idProcedimento": 726567325,
        "tipoProcedimento": "Retirada do aparelho ortodôntico",
        "descricao": null,
        "custo": "R$ 350,00"
      }
    },
    {
      "idConsulta": 258701131,
      "dataConsulta": "02/07/2024 15:09",
      "status": "Realizada",
      "procedimento": {
        "idProcedimento": 316492721,
        "tipoProcedimento": "Avaliação clínica e diagnóstico",
        "descricao": null,
        "custo": "R$ 120,00"
      }
    },
    {
      "idConsulta": 581255255,
      "dataConsulta": "24/08/2024 18:51",
      "status": "Realizada",
      "procedimento": {
        "idProcedimento": 241020345,
        "tipoProcedimento": "Limpeza dental (profilaxia)",
        "descricao": null,
        "custo": "R$ 120,00"
      }
    },
    {
      "idConsulta": 272152776,
      "dataConsulta": "28/09/2024 09:08",
      "status": "Realizada",
      "procedimento": {
        "idProcedimento": 305039687,
        "tipoProcedimento": "Limpeza dental (profilaxia)",
        "descricao": null,
        "custo": "R$ 120,00"
      }
    },
    {
      "idConsulta": 287692216,
      "dataConsulta": "20/10/2024 08:31",
      "status": "Não Realizada",
      "procedimento": {
        "idProcedimento": 174651742,
        "tipoProcedimento": "Aplicação de flúor e selante",
        "descricao": null,
        "custo": "R$ 150,00"
      }
    },
    {
      "idConsulta": 181868504,
      "dataConsulta": "04/01/2025 12:47",
      "status": "Cancelada",
      "procedimento": {
        "idProcedimento": 467280265,
        "tipoProcedimento": "Frenectomia lingual e labial",
        "descricao": null,
        "custo": "R$ 500,00"
      }
    },
    {
      "idConsulta": 79560090,
      "dataConsulta": "27/01/2025 10:24",
      "status": "Realizada",
      "procedimento": {
        "idProcedimento": 979588618,
        "tipoProcedimento": "Restauração em resina composta",
        "descricao": null,
        "custo": "R$ 300,00"
      }
    },
    {
      "idConsulta": 154936160,
      "dataConsulta": "27/04/2025 18:09",
      "status": "Não Realizada",
      "procedimento": {
        "idProcedimento": 765476085,
        "tipoProcedimento": "Clareamento estético em consultório",
        "descricao": null,
        "custo": "R$ 600,00"
      }
    }
  ]
}

 ```
---

### 💡 Modelos de Respostas Geradas pela IA

 ```json
{
  "idPaciente": 912345,
  "nomeCompleto": "Fulano de Tal",
  "cpf": "123.456.789-10",
  "dataNascimento": "01/03/1985",
  "email": "fulano@example.com",
  "telefone": "(11) 91234-5678",
  "endereco": "Rua Exemplo, 123, Bairro, Cidade, Estado",
  "planoOdontologico": "Bem Estar Pró",
  "empresa": "Independente",
  "numConsultas": 3,
  "gastoTotal": "R$ 450,00",
  "consultas": [
    {
      "idConsulta": 1001,
      "dataConsulta": "10/06/2024 14:00",
      "status": "Realizada",
      "procedimento": {
        "idProcedimento": 50001,
        "tipoProcedimento": "Limpeza dental (profilaxia)",
        "descricao": null,
        "custo": "R$ 120,00"
      }
    },
    {
      "idConsulta": 1002,
      "dataConsulta": "15/07/2024 09:30",
      "status": "Agendada",
      "procedimento": {
        "idProcedimento": 50002,
        "tipoProcedimento": "Restauração em resina composta",
        "descricao": null,
        "custo": "R$ 330,00"
      }
    },
    {
      "idConsulta": 1003,
      "dataConsulta": "02/09/2024 11:45",
      "status": "Cancelada",
      "procedimento": {
        "idProcedimento": 50003,
        "tipoProcedimento": "Aplicação de flúor",
        "descricao": null,
        "custo": "R$ 100,00"
      }
    }
  ]
}

 ```
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

### 🎯 Conclusão

Os objetivos desta sprint estão planejados de forma estratégica para garantir que a **SmartDent Solutions** funcione de forma mais eficiente, segura e escalável. A integração dos modelos de IA será realizada de forma fluida, consolidando uma solução que otimiza custos, melhora a qualidade dos serviços e proporciona uma experiência superior para os beneficiários e operadores da **OdontoPrev**.

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
