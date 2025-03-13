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

### ✅ **Conclusão**

A Sprint 3 foi marcada por uma significativa evolução no desenvolvimento do projeto, onde desafios de integração, padronização de dados e a definição de uma abordagem híbrida foram superados. Essa nova estratégia não só permitiu capturar padrões mais sutis nos dados por meio do modelo treinado, como também garantiu a geração de justificativas detalhadas e naturalizadas, proporcionando uma análise robusta e de fácil entendimento para os usuários finais.

Apesar das dificuldades iniciais, as lições aprendidas durante essa fase fortaleceram o projeto, preparando-o para a próxima etapa de integração com das APIs com ás interfaces.

## ♻️ Refatoração e Organização do Código

Durante o desenvolvimento da API **SmartDentAI**, a estrutura foi organizada para garantir modularidade, clareza e facilitar futuras manutenções. A separação em diferentes diretórios mantém **treinamento**, **inferência** e **pré-processamento** bem delimitados.

### 📂 Estrutura dos Arquivos
Abaixo está a organização atual do projeto, refletindo a separação de responsabilidades:

- **`api/`**  
  - `main.py`  
    Arquivo principal da API em **FastAPI**, responsável pela inferência do modelo e exposição dos endpoints.

- **`data/`**  
  - `dataset_treino.csv`  
    Base de dados utilizada para treinar o modelo.  
  - `synthetic_patients.json`  
    Dados sintéticos gerados para teste e validação.

- **`model/`**  
  - **`artifacts/`**  
    - `model_rf.joblib`  
      Arquivo do modelo Random Forest salvo após o treinamento.  
  - **`preprocessing/`**  
    - `prepare_dataset.py`  
      Script para limpar e preparar o dataset antes do treinamento.  
  - **`training/`**  
    - `train_model.py`  
      Script responsável por treinar o modelo e salvá-lo em `artifacts/`.

- **`scripts/`**  
  - `generate_synthetic_data.py`  
    Script auxiliar para gerar dados sintéticos de pacientes, ajudando nos testes.

### 📝 Documentação e Logs

Foi fundamental garantir que o comportamento do modelo pudesse ser monitorado:

- Adicionamos **logs detalhados** para indicar quando o modelo foi carregado corretamente e para relatar possíveis falhas.
- Incluímos o campo **"modelo_utilizado"** nas respostas da API, permitindo identificar de forma clara se a predição foi feita pelo modelo treinado.

Essas medidas facilitam identificar rapidamente qualquer problema na inferência e manter o modelo operando corretamente em produção.

---

## 🧠 **Arquitetura de IA**

No SmartDent Solutions, optamos por usar FastAPI (Python) como camada de serviço de IA, onde rodamos o modelo de Machine Learning. Esse modelo foi construído em Scikit-learn, e usamos um Random Forest porque ele lida bem com diferentes tipos de dados (como número de consultas, custo total, histórico de procedimentos) e oferece resultados interpretáveis.

A razão para escolher essa arquitetura é que a API de IA fica independente do restante do sistema (ou seja, separada do backend .NET e do front-end Java/Mobile). Assim, quando a gente precisa atualizar o modelo ou adicionar alguma lógica de análise nova, não mexemos no código do backend principal. Isso deixa tudo mais modular e facilita o deploy de forma independente—no caso, a API de IA está sendo hospedada no Render.

---

## ⚙️ **Implementação na Prática**

No repositório, há uma pasta específica (chamada algo como `api/` ou `IA/`) que contém os scripts de treinamento e o código da API em FastAPI. A gente treina o modelo localmente (ou num ambiente de dados), salva o arquivo `.joblib`, e a API carrega esse modelo quando inicia. Sempre que o backend .NET recebe alguma informação de um paciente para ser analisada, ele faz uma requisição POST para o endpoint do FastAPI, que então processa os dados, aplica o modelo e retorna um rótulo de risco (por exemplo, "UsoExcessivo") mais uma justificativa.

---

### 🗂 **Base de Dados Usada**

Para o treinamento e teste do modelo, nós estamos trabalhando com dados sintéticos que refletem cenários de uso odontológico (quantidade de consultas, custo, status de cada consulta, tipo de procedimento, etc.). A ideia é simular comportamentos de pacientes abusando ou não do convênio, pra conseguirmos treinar a IA a distinguir entre uso normal e uso excessivo. Esses dados foram gerados num script Python que cria registros aleatórios com diferentes padrões de frequência e custo. Assim, a IA aprende com uma variedade de cenários que representam bem o que acontece no dia a dia de um plano odontológico.

#### Por que dados sintéticos?
Porque no momento não temos acesso a dados reais (que por questões de privacidade e compliance não podem ser usados livremente). Mesmo assim, essa base sintética é suficiente para a prova de conceito e pra demonstrar como a IA seria integrada no fluxo real.

---

## 🎥 **Demonstração e Apresentação**

- *(Inserir link para um vídeo demonstrativo do projeto funcionando.)*  
- *(Adicionar imagens mostrando a API em funcionamento, chamadas ao endpoint e exemplos de resposta.)*

---

## 🧑‍💻 **Equipe de Desenvolvimento**

- **Macauly V.** *(Líder do Projeto, IA & Backend)*  
- **[Nome do Integrante]** *(Mobile Developer)*  
- **[Nome do Integrante]** *(Banco de Dados & Infraestrutura)*  
- **[Nome do Integrante]** *(Frontend & UX/UI)*
