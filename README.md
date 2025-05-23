![Descrição banner](https://github.com/user-attachments/assets/acf148aa-b44a-4ebd-9085-9ce4f31ecaf0)





---

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

## 🧠 **Arquitetura de IA**

Na SmartDent Solutions, optamos por usar FastAPI (Python) como camada de serviço de IA, onde rodamos o modelo de Machine Learning. Esse modelo foi construído em Scikit-learn, e usamos um Random Forest porque ele lida bem com diferentes tipos de dados (como número de consultas, custo total, histórico de procedimentos) e oferece resultados interpretáveis.

A razão para escolher essa arquitetura é que a API de IA fica independente do restante do sistema (ou seja, separada do backend .NET e do front-end Java/Mobile). Assim, quando a gente precisa atualizar o modelo ou adicionar alguma lógica de análise nova, não mexemos no código do backend principal. Isso deixa tudo mais modular e facilita o deploy de forma independente—no caso, a API de IA está sendo hospedada no Render.

---

## ⚙️ **Implementação na Prática**

No repositório, há uma pasta específica (chamada api/) que contém os scripts de treinamento e o código da API em FastAPI. A gente treinou o modelo localmente, salvou o arquivo .joblib, e a API carrega esse modelo quando inicia. Sempre que o backend .NET recebe alguma informação de um paciente para ser analisada, ele faz uma requisição POST para o endpoint do FastAPI, que então processa os dados, aplica o modelo e retorna um rótulo de risco (por exemplo, UsoExcessivo) mais uma justificativa.

---

### 🗂 **Base de Dados Usada Para Treinamento**

Para o treinamento e teste do modelo, nós ultilizamos dados sintéticos que refletem cenários de uso odontológico (quantidade de consultas, custo, status de cada consulta, tipo de procedimento, etc.). A ideia é simular comportamentos de pacientes abusando ou não do convênio, pra conseguirmos treinar a IA a distinguir entre uso normal e uso excessivo. Esses dados foram gerados num script Python que cria registros aleatórios com diferentes padrões de frequência e custo. Assim, a IA aprende com uma variedade de cenários que representam bem o que acontece no dia a dia de um plano odontológico.

#### Por que dados sintéticos?
Porque no momento não temos acesso a dados reais. Mesmo assim, essa base sintética é suficiente para a prova de conceito e pra demonstrar como a IA seria integrada no fluxo real da Odontoprev.

---

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

## ⚙️ Aspectos Técnicos
Quando o modelo é treinado (**por exemplo, utilizando RandomForest**), ele aprende a identificar padrões nos dados e utiliza o método **predict_proba()** para calcular a probabilidade de cada classe ser a correta.

🔹 **Como funciona?**
- O modelo gera **probabilidades** para cada possível classificação.
- No caso do **RandomForest**, essas probabilidades são calculadas a partir dos votos de diversas árvores de decisão.
- A **maior probabilidade** é usada como a predição final.

🔹 **Exemplo:** Se a maior probabilidade calculada for **90%**, esse será o nível de **confiança da predição**.

Esse processo garante que o modelo **não apenas classifique um paciente**, mas também comunique **o quão seguro** ele está na sua decisão, tornando a análise **mais transparente**.

---

## 🔥 Desafios e Melhorias
Atualmente, o modelo está funcionando bem, mas alguns pontos críticos precisam ser observados:

### ⚠️ Confiança em Casos de Alto Risco
- Quanto maior o nível de alerta, menor tende a ser a **confiança da predição**.
- Isso acontece porque **casos extremos** (como **"Uso Excessivo"**) são **menos frequentes** no conjunto de treinamento.
- Como há **menos exemplos desse tipo**, o modelo tem mais dificuldade em identificar padrões consistentes, resultando em **uma confiança inferior a 50%** em muitos casos.

### 🎯 Melhoria na Precisão para Casos Extremos
**Problema:** O modelo tem menos dados representativos para **casos raros**, reduzindo sua capacidade de fazer previsões confiáveis nesses cenários.

**Solução:**
✅ Ajustar os **hiperparâmetros** do modelo.  
✅ Melhorar o **balanceamento dos dados** para garantir que casos extremos tenham uma **representação justa** no treinamento.  
✅ Aplicar **técnicas de calibragem de probabilidade**, que ajudam a tornar as previsões mais confiáveis.  

🚀 **Essas melhorias serão priorizadas na próxima sprint.**

---

## 📈 Precisão Atual do Modelo
Nos testes realizados, o modelo foi treinado com **1.000 dados de pacientes**, cada um contendo entre **10 e 20 consultas/procedimentos**. O resultado foi uma **acurácia de 99,1%**, um valor alto.

🔹 **No entanto, vale destacar que:**
✅ A quantidade de **dados influencia diretamente a acurácia**.  
✅ Quanto **maior o volume e a qualidade** dos dados reais utilizados, **mais confiável será o modelo**.  
⚠️ No momento, os dados usados **não foram otimizados ao máximo** para refletirem casos **100% reais**. Isso significa que a **acurácia ainda não pode ser considerada definitiva**.

---

## 🏁 Conclusão
O modelo está apresentando **bons resultados**, mas ainda há espaço para melhorias, especialmente em:

📌 **Aumentar a confiança** das predições em casos extremos.  
📌 **Refinar o balanceamento** dos dados para tornar as análises mais precisas.  
📌 **Implementar ajustes finos** nos hiperparâmetros e calibragem das probabilidades.  

🚀 **Essas melhorias serão o foco da próxima sprint para garantir que o modelo continue evoluindo e se tornando mais robusto!**

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

### 🏷 Deploys Disponíveis

- **API de IA (FastAPI)**  
  (https://smartdent-ai.onrender.com/docs)

- **API Principal (C# .NET)**  
  (https://smartdent-api.onrender.com/swagger)

As APIs estão hospedadas no Render e, devido às limitações da versão gratuita, podem entrar em modo de suspensão quando inativas. Ao receber a primeira requisição, elas podem levar entre 1 a 2 minutos para serem reativadas antes de processar novas chamadas.

⚠ **Importante:** Caso vá testar os Endpoints de IA via API .NET, primeiro acesse a URL da FastAPI diretamente (https://smartdent-ai.onrender.com/docs) e aguarde cerca de 1 minuto para garantir que ela esteja ativa. Isso evitará erros de requisição ao chamá-la via API .NET.

---

## 🧑‍💻 **Equipe de Desenvolvimento**

- **Macauly Vivaldo da Silva** – *Frontend & UX/UI, IA & Backend*  
- **Daniel Bezerra da Silva Melo** – *Mobile Developer & Infraestrutura DevOps (Deploy)*  
- **Gustavo Rocha Caxias** – *Banco de Dados*  
