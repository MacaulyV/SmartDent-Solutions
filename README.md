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

## 🎬 Demonstração em Vídeo

[![Assista à demonstração do projeto](https://img.youtube.com/vi/py1fePdV5dE/0.jpg)](https://youtu.be/py1fePdV5dE)

> **Atenção, professor:**
>
> O vídeo acima mostra um panorama geral do funcionamento do projeto, com explicações diretas e sem aprofundar em detalhes técnicos.
> 
> **Dica:** Recomendo assistir o vídeo em velocidade 1.5x para tornar a apresentação mais dinâmica principalmente por que o video tem 15 minutos.
>
> **Recomendo fortemente que leia o conteúdo deste documento** para entender as implementações técnicas, decisões de arquitetura e integrações do sistema. O vídeo serve como apoio visual, mas a parte técnica está toda explicada aqui!

---

## 📈 **Evolução do Projeto**

Nesta seção, destacamos a evolução do projeto em relação à Sprint anterior e à entrega final da disciplina **Disruptive Architectures: IoT, IOB & Generative IA**, resumindo as principais decisões, desafios e avanços de cada etapa.

---

### 🔄 **Recapitulação da Sprint 3 – Pontos-Chave da Entrega Anterior**

Na Sprint 3, o principal foco foi consolidar a arquitetura do projeto e integrar o primeiro modelo de IA ao fluxo real do SmartDent Solutions. Foi uma entrega fundamental pra garantir que todas as partes da solução conversassem entre si, e pra validar, na prática, como o modelo de IA se comportava com dados reais (mesmo que sintéticos).

---

### ✅ **O que foi entregue e validado:**

- 🏗️ Estrutura definitiva da arquitetura, com integração entre backend (.NET), API de IA (FastAPI/Python), banco Oracle e parte da interface web/mobile já desenvolvida.
- 🤖 Primeira versão do modelo de Machine Learning (Random Forest), treinado em dados sintéticos que simulam diferentes padrões de uso de pacientes no setor odontológico.
- 🔁 Pipeline de inferência pronto: backend envia dados dos pacientes para a IA e recebe de volta o grau de risco, justificativa e nível de confiança da análise feita pelo modelo.
- 🧪 API da IA testada e validada para diferentes cenários, garantindo entradas e saídas corretas via endpoints.
- 🚧 Versão beta do sistema já funcional para análise de múltiplos pacientes.

---

## ⚠️ **Principais Desafios e Limitações Encontradas**

---

### 1️⃣ **Dados Sintéticos e Limitações de Realismo**

Por não termos acesso ao formato real dos dados utilizados pela OdontoPrev, tivemos que criar um **dataset sintético** — ou seja, simular dados plausíveis, baseados em como acreditamos que as informações dos pacientes são organizadas na prática. O processo envolveu converter registros detalhados de pacientes em formato JSON para um CSV próprio para o treinamento do modelo. Só que isso trouxe duas limitações sérias:

- **Desbalanceamento das classes:**
    
    O conjunto de dados gerado ficou com muitos exemplos das classes “Nenhum Risco” e “Uso Moderado”, poucos para “Potencial de Uso” e quase nenhum para “Uso Excessivo”.
    
    Resultado: o modelo ficou ótimo para identificar os cenários mais comuns, mas fraco justamente nas situações mais raras — que, na prática, são as mais críticas.
    
- **Falta de realismo dos dados sintéticos:**
    
    Como nosso script não conseguia simular perfeitamente a variedade e complexidade dos casos reais, muitos exemplos gerados eram “parecidos demais” dentro de cada classe, sem nuances suficientes.
    
    Isso levou a dois problemas:
    
    - O modelo ficou “viciado” em padrões simples, dificultando a distinção entre, por exemplo, um paciente de uso moderado e um sem risco.
    - Exemplos mais complexos, próximos do que seria o mundo real (jsons com muitos detalhes ou situações no limite entre categorias), muitas vezes eram classificados de forma incoerente, gerando justificativas pouco convincentes.

Além disso, essa limitação ajudou a inflar a acurácia nos testes (chegando a 99,5% na Sprint 3), mas essa métrica acabou sendo ilusória, já que não reflete a capacidade do modelo de generalizar para situações de verdade — especialmente nos casos que realmente importam, como abuso de uso do convênio.

> Resumindo:
> 
> 
> O uso de dados sintéticos foi essencial para viabilizar a prova de conceito, mas ficou claro que o modelo só é “esperto” quando o cenário é fácil ou repetitivo. Quando testado com dados mais complexos ou próximos da realidade, ele erra justamente onde deveria acertar — um ponto crítico que se tornou prioridade de melhoria para a Sprint 4.
> 

---

### 2️⃣ **Dificuldade com Casos Extremos (Uso Excessivo)**

O maior gargalo do modelo, já percebido logo na Sprint 3, foi justamente na hora de lidar com os **casos raros e extremos** — que, na prática, são os mais importantes.

Por ter poucos exemplos no treinamento, o modelo falhava feio quando precisava identificar pacientes que realmente abusavam do convênio.

- Na maioria das vezes, só conseguia acertar quando o abuso era muito “na cara” (tipo um caso totalmente absurdo).
- Se o padrão era mais sutil ou exigia mais inteligência para identificar o risco, ele errava ou gerava justificativas incoerentes.
- Além disso, essa dificuldade tornava o modelo “cego” para situações **de fronteira** — como um paciente que está no limite entre potencial de abuso e uso excessivo, ou casos em que pequenas variações nos dados mudam totalmente o contexto do risco.

> Resumo:
> 
> 
> O modelo acabou falhando justamente nas situações que mais exigiam inteligência e precisão — onde realmente importa para o negócio. Isso deixou claro que o próximo passo obrigatório seria enriquecer os dados, corrigir o desbalanceamento das classes e investir em estratégias para tornar o modelo mais robusto e assertivo em cenários complexos.
> 

---

### 3️⃣ **Baixa Confiança nas Respostas do Modelo (Necessidade de Calibragem)**

Um dos desafios mais visíveis no final da Sprint 3 foi a **baixa confiança nas respostas do modelo**, principalmente nas predições de maior risco, como os casos de “Uso Excessivo”. Mesmo quando o modelo acertava a classe, muitas vezes a probabilidade atribuída era baixa — sinalizando que a própria IA não estava “segura” do seu chute.

Esse problema tem dois impactos principais:

- ❗ **Dificulta confiar na decisão automatizada**, já que o próprio modelo está inseguro sobre o resultado.
- ❓ **Prejudica a explicabilidade**: se a confiança é baixa, fica difícil justificar para o usuário (ou gestor) por que determinado paciente foi classificado como risco alto ou moderado.

Grande parte disso vem do desbalanceamento dos dados e da falta de exemplos complexos durante o treinamento. Sem calibragem adequada, a distribuição das probabilidades fica distorcida e o modelo pode tanto subestimar quanto superestimar o risco real do paciente.

Por isso, ficou claro que era fundamental investir em **técnicas de calibragem** — como Platt Scaling ou Isotonic Regression — para ajustar as probabilidades e entregar resultados mais confiáveis, transparentes e úteis na prática.

---

### 🚀  **Sprint 4 – Evolução e Refinamento da Solução de IA**

Nesta sprint, o foco foi aprimorar o modelo de IA com base nos desafios identificados anteriormente, trazendo mais precisão, realismo e confiança para as análises. O objetivo agora é tornar a solução realmente robusta e alinhada às demandas do mundo real.

### 1️⃣ 📝 Mudança de Dataset: CSV → JSON → NDJSON no Pipeline de Treinamento

**Contexto e Motivação:**

Após a dúvida levantada no feedback do professor sobre treinar modelos diretamente com JSON, testamos na prática a diferença de usar JSON (e depois NDJSON) em vez do tradicional CSV — buscando mais flexibilidade, performance e facilidade para escalar o volume de dados.

**O que foi feito:**

- Pesquisamos e testamos a ingestão de dados em JSON puro, percebendo que o pandas lida bem com ambos os formatos, mas o JSON traz mais flexibilidade para estruturas ricas (listas, objetos aninhados).
- Reescrevemos o pipeline: da geração dos dados sintéticos até o treinamento do modelo, tudo agora usa `.json` — eliminando conversões desnecessárias para CSV.
- Pra escalar o número de pacientes, adotamos **NDJSON** (um JSON por linha), porque:
    - **Exemplo real:** 1.000 registros de pacientes em JSON comum ocupavam cerca de 70 MB; se tentássemos 100 mil nesse formato, seriam absurdos 7 GB de espaço!
    - Com NDJSON, 100 mil registros ocuparam só 68 MB — ou seja, ficou **100 vezes mais leve** e muito mais eficiente de processar.
    - Além disso, processar dados linha a linha ficou muito mais rápido, sem travar a máquina e evitando a lentidão do modelo para manipular grandes listas ou colunas complexas.

**Por que JSON/NDJSON?**

- **Flexibilidade:** Guarda informações complexas/aninhadas sem precisar “achatar” tudo em colunas fixas.
- **Menos perda de informação:** Permite salvar históricos completos e detalhes sem inventar colunas artificiais.
- **Performance:** NDJSON facilita leitura linha a linha, reduz uso de RAM e acelera processamento de grandes volumes.

**Como foi feito:**

- Ajustamos scripts pra ler `.json` e `.ndjson` direto.
- Pipeline agora lê paciente por paciente (ex: lotes de 10 mil), sem estourar memória.
- Código ficou compatível com ambos os formatos, pronto pra crescer junto com o projeto.

**Impactos práticos:**

- Conseguimos treinar o modelo com 100x mais dados, sem travar a máquina.
- Pipeline mais limpo, flexível e robusto.
- Ganho enorme de performance, menos espaço em disco e carregamento muito mais rápido — mesmo com grandes volumes de dados.

---

### 2️⃣ 🧩 Melhoria do Dataset: Dados Mais Realistas, Precisos e Balanceados

---

**🔎 O problema:**

A verdade é que nosso modelo só parecia bom porque o dataset era fácil demais. Quando começamos a testar com JSONs realmente complexos, ficou claro: em situações de uso excessivo, ele errava feio. E não é surpresa — com só 1.000 exemplos no dataset antigo, menos de 10% eram dessa classe. Ou seja, a IA teve pouco contato com o tipo de caso que mais interessa pra aprender.

---

**📉 O efeito do desbalanceamento:**

Com tudo desbalanceado e cheio de padrão óbvio, a acurácia batia 99,5%. Mas isso só mascarava os pontos fracos do modelo. Ele acertava “de olhos fechados” as classes comuns e se perdia nas raras ou intermediárias.

---

**🔨 O que a gente fez pra resolver:**

- Primeiro, balanceamos o dataset: garantimos 25% pra cada classe (nenhum risco, uso moderado, tendência a excesso, uso excessivo).
- Só com esse ajuste, a acurácia já despencou pra 67% — mostrando que o modelo nunca tinha aprendido de verdade a diferenciar as classes, só decorava o padrão dominante.
- Depois, a gente foi fundo nas melhorias:
    - Deixamos o histórico dos pacientes mais realista, ou seja, com dados mais lógicos e próximos da realidade,
    - Trabalhamos intervalos e tipos de procedimentos variados,
    - Simulamos agendamentos e cancelamentos de verdade,
    - Eliminamos informação inútil — ficou só o que realmente faz diferença pro modelo.
- Caprichamos nos padrões pra forçar a IA a lidar com cenários realmente desafiadores e complexos.

---

**⚡ A virada real:**

Foi só depois dessas melhorias que a acurácia disparou pra 98.91%. Agora, a acurácia faz sentido: o modelo consegue distinguir bem, de verdade, a diferença entre as classes.

![Captura de tela 2025-05-17 195741.png](Captura_de_tela_2025-05-17_195741.png)

---

**🚦 Testando pra valer:**

A gente desafiou a IA com exemplos difíceis, muito mais próximos do mundo real, e ela finalmente passou a acertar e diferenciar bem as quatro classes.

Parou de confundir pacientes de uso moderado com nenhum risco e, principalmente, melhorou a detecção de uso excessivo — que era o nosso maior problema antes.

---

**💡 Resumo real:**

Antes, o modelo só “acertava” exemplos básicos de JSON. Agora, ele realmente aprendeu a lidar com dados realistas, complexos e balanceados.

Mesmo não estando perfeito, a evolução é visível — e o modelo notavelmente melhorou após essa segunda mudança.

---

### 3️⃣ 📏 Calibragem na Confiança do Modelo

---

**⚠️ O problema:**

Desde a Sprint 3, a gente notava que, quanto mais crítico o caso, menor era a confiança da IA.

Os alertas de “Uso Excessivo” — que são justamente onde o erro custa caro — vinham quase sempre com confiança baixa, às vezes até abaixo de 50%. Isso acontecia porque esses casos eram os menos representados no treino, então o modelo não tinha base sólida pra garantir sua confiança.

---

**🤨 Por que confiança importa de verdade?**

- Não basta só prever risco ou não; a gente precisa saber o quanto a IA tá segura da própria resposta.
- Confiança baixa serve de alerta: mostra que o modelo pode estar “chutando”, e aí é papel do analista olhar mais de perto, buscar mais informações ou revisar o caso.
- Sem uma confiança calibrada, todo o processo perde transparência e a automação fica perigosa.

---

**🛠️ O que a gente fez pra resolver:**

- **Calibragem real:**
    
    Usamos o CalibratedClassifierCV do scikit-learn com Isotonic Regression pra deixar as probabilidades do modelo mais próximas do cenário real.
    
- **Temperature scaling (T=0.8):**
    
    Ajustamos o vetor de probabilidades pra deixar as respostas mais “afiadas”:
    
    - Quando a IA tem certeza (casos óbvios, tipo uso excessivo escancarado ou moderado clássico), a confiança pode passar de 90%.
    - Quando pega um caso realmente complexo — na fronteira entre tendência a excesso e uso excessivo, por exemplo —, a confiança fica ali perto de 50%, sinalizando pro humano ficar atento.
    - Esse equilíbrio entre confiança alta e dúvida honesta é o que realmente faz diferença no dia a dia da OdontoPrev, otimizando o tempo dos colaboradores e evitando análise desnecessária nos casos fáceis.

---

**🚦 O impacto real:**

- **Antes:** Tinha muito caso com confiança baixa, de 30% a 70%, inclusive em situações críticas — dava zero segurança pra confiar no modelo.
- **Agora:**
    - Casos “na cara” batem 70–90% de confiança, deixando claro que ali não precisa perder tempo revisando.
    - Casos ambíguos ou complexos ficam em 45–55%, mostrando que exigem uma análise humana mais criteriosa.
    - A distribuição da confiança ficou bem mais coerente, ajudando tanto a identificar os casos urgentes quanto a filtrar o que realmente precisa de atenção.

---

**💡 Resumo direto:**

Com essas mudanças, a confiança virou de fato um reflexo do que o modelo sente: não é mais só um número decorativo, mas sim um indicador honesto se a IA está confiante ou só “no achismo”.

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

---

## 🎯 Conclusão Final

Os objetivos desta sprint estão planejados de forma estratégica para garantir que a **SmartDent Solutions** funcione de forma mais eficiente, segura e escalável. A integração dos modelos de IA será realizada de forma fluida, consolidando uma solução que otimiza custos, melhora a qualidade dos serviços e proporciona uma experiência superior para os beneficiários e operadores da **OdontoPrev**.

---

### 🏷 Deploys Disponíveis

- **API de IA (FastAPI)**
    
    ([https://smartdent-ai.onrender.com/docs](https://smartdent-ai.onrender.com/docs))
    
- **API Principal (C# .NET)**
    
    [https://smartdentapi.fly.dev/api-docs](https://smartdentapi.fly.dev/api-docs)
    

---

## 🧑‍💻 **Equipe de Desenvolvimento**

- **Macauly Vivaldo da Silva** – *Frontend & UX/UI, IA & Backend*
- **Daniel Bezerra da Silva Melo** – Testing *& Infraestrutura DevOps (Deploy)*
- **Gustavo Rocha Caxias** – *Banco de Dados*
