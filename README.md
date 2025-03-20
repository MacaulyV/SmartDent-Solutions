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

## 📝 Descrição Geral do Aplicativo

O **SmartDent App** é o aplicativo móvel da plataforma SmartDent Solutions, desenvolvido para facilitar a interação dos usuários com os serviços odontológicos. Ele permite o gerenciamento de consultas, acesso ao histórico odontológico e suporte inteligente, garantindo maior transparência e praticidade na utilização do seu convênio odontológico.

---

## 🏗️ Diagrama de Arquitetura

![Diagrama de Pastas](https://github.com/user-attachments/assets/c18fc194-4b46-4673-ace1-73fefb873183)

---

## 📂 Estrutura de Pastas do Projeto

- 📁 **app/** → Diretório principal contendo toda a lógica do aplicativo.

  - 📁 **assets/** → Armazena arquivos estáticos como imagens e animações.
    - 📂 animations/
    - 📂 images/
  - 📁 **components/** → Componentes reutilizáveis que são usados em várias telas.
  - 📁 **config/** → Arquivos de configuração geral (endpoints, parâmetros).
  - 📁 **context/** → Gestão de estados globais da aplicação (autenticação, dados do usuário).
    - 🔐 AuthContext.tsx
    - 👤 UserContext.tsx
  - 📁 **navigation/** → Controle e configuração das rotas da aplicação.
    - 🗺️ AppNavigator.tsx
  - 📁 **screens/** → Telas principais agrupadas logicamente.
    - 🔑 **Auth/** (LoginScreen.tsx, RegisterScreen.tsx)
    - 🌟 **Main/** (MainScreen.tsx, ProfileScreen.tsx & etc)
    - 🎉 **Onboarding/** (ChoiceScreen.tsx, WelcomeScreen.tsx)

- 📁 **services/** → Camada para comunicação com APIs e autenticação.

  - 🌐 api.ts  
    - Configurações do Axios para chamadas à API do SmartDent.
    - Interceptores de requisição e resposta para log e tratamento de erros.
    - Funções específicas para criar e obter pacientes (`createUser`, `getUser`).
  - 🔒 auth.ts  
    - `AuthService` centraliza a lógica de registro e login.  
    - Chama `StorageService` para guardar dados do usuário após a criação ou login.
    - Valida dados no login local (nome, CPF).
  - 💾 storage.ts  
    - `StorageService` utiliza `react-native-encrypted-storage` para manter dados sensíveis seguros no dispositivo.  
    - Salva e recupera objetos do tipo `UserData`, garantindo persistência local.

- 📁 **types/** → Arquivos contendo tipagens TypeScript.

  - 📘 auth.ts  
    - Tipos e interfaces como `UserData`, `LoginCredentials`, `AuthResponse`.

- 📁 **utils/** → Funções utilitárias usadas por toda a aplicação.

- **App.tsx** → Ponto de entrada do aplicativo, renderiza o `AppNavigator`.

---

## 🛠️ Tecnologias e Bibliotecas Utilizadas

- ⚛️ **React Native**: Framework base para desenvolvimento cross-platform de alta performance.
- 🚀 **React Navigation**: Gerenciamento intuitivo das rotas e pilhas de navegação entre telas (Stack Navigator).
- 🎨 **LinearGradient & RadialGradient**: Adicionam gradientes modernos a botões e fundos, realçando a experiência visual.
- 🔍 **Axios**: Biblioteca para consumo de APIs REST, simplificando requisições e interceptores.
- 📦 **react-native-encrypted-storage**: Garante segurança ao armazenar dados sensíveis (token, CPF do usuário).
- ⚡ **Animated & Easing**: Permitem animações detalhadas (fade, scale, slide) e efeitos de clique (press-in/out) com alta fluidez.
- 🏞️ **FastImage** (AnimatedFastImage): Otimiza carregamento de imagens e manipulação de GIFs, crucial para telas animadas como Welcome e Main.
- 🔧 **AuthService** e **StorageService**: Arquitetura de serviços para separar lógica de autenticação e armazenamento local, garantindo coesão e manutenibilidade do código.
- 📝 **TypeScript**: Fornece tipagens seguras (interfaces como `UserData`, `LoginCredentials`), reduzindo erros em tempo de compilação.

---

## 📱 Explicação das Telas do Aplicativo

### 🎉 **WelcomeScreen (Onboarding/WelcomeScreen.tsx)**

**Objetivo da Tela**  
- Apresentar uma tela de boas-vindas repleta de animações e efeitos visuais para causar impacto positivo ao iniciar o app.
- Servir de introdução ao usuário, mostrando o logotipo do SmartDent e algumas informações básicas.

**Elementos e Fluxo Interno**  
1. **Animações de Título e Elementos**:  
   - Usa `Animated` e `Easing` para controlar opacidade, escala e translação do título “Bem-vindo à SmartDent”, além de ícones e texto descritivo.  
   - Possui um GIF (Esfera.gif) e um ícone (IconCapa.png) animados via `AnimatedFastImage`.  
2. **Botão de Avançar**:  
   - Implementado com `BotaoAnimado` usando Animated para efeito de clique (pequena redução de escala ao pressionar).  
   - Ao clicar, navega para `ChoiceScreen` através de `navigation.navigate('Choice')`.  
3. **Responsividade**:  
   - Funções como `normalize(size: number)` ajustam dinamicamente tamanhos de fonte e layout conforme a resolução do dispositivo.  
4. **Bibliotecas**  
   - `react-native-fast-image` (para GIFs e imagens com cache otimizado).  
   - `react-native-linear-gradient` e animações do React Native (fade, scale, translate) para criar transições suaves e destacar o visual.

### 🎉 **ChoiceScreen (Onboarding/ChoiceScreen.tsx)**

**Objetivo da Tela**  
- Permitir ao usuário escolher entre criar conta (`RegisterScreen`) ou fazer login (`LoginScreen`).
- Continuar a experiência de animações e partícula, mantendo a identidade visual desde a tela de boas-vindas.

**Elementos e Fluxo Interno**  
1. **Botões de Navegação**:  
   - “Criar Conta” → chama `navigation.navigate('Register')`.  
   - “Fazer Login” → chama `navigation.navigate('Login')`.  
2. **Animações de Background**:  
   - Partículas subindo na tela, gradientes animados (RadialGradient, LinearGradient).  
   - `Animated.sequence(...)` e `Animated.parallel(...)` para gerenciar a entrada de cada elemento (logo, título, subtítulo, botões).  
3. **Responsividade**:  
   - Ajustes de fonte e layout com base em breakpoints do dispositivo (uso de `useWindowDimensions()` e `normalize()`).
4. **Bibliotecas**  
   - `react-native-radial-gradient` para efeitos de gradiente circular nos botões.  
   - `Animated` + `Easing` para coordenar a entrada sequencial de texto e botões.  

### 📝 **RegisterScreen (Auth/Register.tsx)**

**Objetivo da Tela**  
- Criar novo usuário no sistema, enviando dados obrigatórios à API e armazenando localmente no dispositivo em caso de sucesso.

**Elementos Principais**  
1. **Formulário de Cadastro**:  
   - Campos: nome completo, CPF, email, e opção de empresa.  
   - Gera campos fictícios (ex.: dataNascimento, telefone) se não fornecidos.  
2. **Botão “Cadastrar”**:  
   - Aciona `AuthService.register(...)`, que faz POST em `/api/A_Pacientes`.  
   - Exibe alerta de sucesso e salva local com `StorageService.saveUserData(...)`, ou alerta de erro com a mensagem correspondente.  
3. **Validações de Formulário**:  
   - Nome deve ter >= 3 palavras.  
   - CPF com 11 dígitos formatado.  
   - E-mail no padrão “exemplo@gmail.com”.  
   - Checkbox “Aceitar termos”.  
4. **Alertas**  
   - Modal customizada (usando `Modal` do React Native) para avisar erro (ex.: “Preencha todos os campos!”) ou sucesso (“Cadastro realizado com sucesso!”).
5. **Armazenamento Local**  
   - Em caso de registro bem-sucedido, o app chama `StorageService.saveUserData(...)` para reter dados do usuário, inclusive gerando `deviceId` e `encryptionKey`.  
6. **Bibliotecas**  
   - **Axios**: Posta dados do paciente no endpoint configurado (`createUser`).
   - **Animated**: Efeitos de fade e scale nos campos ao carregar a tela.  

**Fluxo Interno Completo**  
1. Usuário preenche nome completo, CPF, e-mail e (opcional) empresa.  
2. Ao clicar “Cadastrar”, chama `AuthService.register(...)`.  
3. AuthService → `createUser(...)`: POST no endpoint `/api/A_Pacientes`.  
4. Se status 201/200, `StorageService.saveUserData(...)`.  
5. Exibe modal “Cadastro realizado com sucesso!” e, ao fechar, redireciona para `MainScreen`.

### 🔐 **LoginScreen (Auth/LoginScreen.tsx)**

**Objetivo da Tela**  
- Autenticar o usuário no sistema, verificando CPF e nome/email.  
- Se válido, permitir acesso à `MainScreen` (área logada).

**Elementos Principais**  
1. **Campos de Formulário**:  
   - `CampoFormulario` com inputs animados.  
   - Inputs para “Nome completo ou Email” e “CPF” (limitado a 11 dígitos).  
   - Opção “Lembrar-me” (checkbox) que sinaliza se deve armazenar dados localmente.  
2. **Botão “Entrar”**:  
   - Envia as credenciais via `AuthService.login(credentials)`.  
   - Se retorno `success = true`, exibe alerta de sucesso e navega para `MainScreen`.
   - Caso contrário, apresenta modal de erro.  
3. **Validações de Formulário**:  
   - CPF é formatado dinamicamente (`000.000.000-00`) e validado quanto ao número de dígitos.  
   - Nome completo deve ter pelo menos 3 palavras se for loginType=`name`.  
   - Mensagens de erro detalhadas exibidas em `Alert`/Modal.  
4. **Fluxo de Autenticação e Armazenamento**:  
   - `AuthService` verifica se os dados armazenados localmente (`StorageService`) coincidem com as credenciais informadas.  
   - **Sem JWT** neste caso. O login está usando uma validação local ou chamando a API (dependendo da configuração do `authService.login()`).
   - Em caso de sucesso, `AuthService` salva dados no `EncryptedStorage`, permitindo “login offline” em aberturas futuras.  
5. **Bibliotecas**  
   - **Axios**: Envio de requisições à API (caso a API exija, `authService.login()` faz POST).  
   - **react-native-encrypted-storage**: Salvamento de dados.  
   - **Animated** para transições de tela e elementos do formulário.  

**Fluxo Interno Completo**  
1. Usuário digita “Nome” ou “Email” (identifier) e “CPF”.  
2. Verifica formatações (via `handleCpfChange` e `handleNomeChange`).  
3. Ao pressionar “Entrar”, chama `AuthService.getInstance()` → `authService.login(credentials)`.  
4. `AuthService` faz validações locais e/ou consulta a API via `createUser` e `getUser` (dependendo do design).  
5. Se sucessful, `AuthService` chama `StorageService.saveUserData(...)`.  
6. Exibe modal de sucesso e navega para `MainScreen`.  
7. Se erro, exibe modal com mensagem do erro.

### 🏠 **MainScreen (Main/Main.tsx)**

**Objetivo da Tela**  
- Servir de “hub” para funcionalidades centrais (marcar consulta, histórico, chat de suporte) e permitir acesso ao perfil do usuário.

**Elementos Principais**  
1. **DestaqueTopo**: Card animado que exibe breve chamada à ação (“Encontre o que você precisa”) e um botão “Agendar”.  
2. **Grid de Opções** (OpcaoCard):  
   - Cada card representa uma função (Marcar Consulta, Histórico, Chat, Perfil).  
   - Ao clicar, chama métodos como `handleMarcarConsulta` ou `handlePerfil`.  
3. **Partículas Animadas**:  
   - Efeito decorativo de bolinhas subindo no fundo.  
   - Realizado via arrays e `Animated.timing` em loop.  
4. **Bibliotecas**  
   - **AnimatedFastImage** para exibir GIF (Clinica.gif).  
   - **LinearGradient**: Efeitos de gradiente em botões e cartões.
   - **Animated & Easing**: Animações de entrada suave em cada card do grid.  
5. **Fluxo Interno**  
   - Tela monta → inicia animações de background e revelação (`revealAnim`).  
   - Quando o usuário seleciona uma opção, navega para a tela correspondente (ex.: `ProfileScreen`).  

---

# 📌 Detalhes importantes do Cadastro + Login e Solução de Problemas

## 📝 Visão Geral do Cadastro

Nosso formulário de cadastro contém apenas quatro campos preenchidos pelo usuário, enquanto o endpoint POST da API exige oito registros. Criar um formulário extenso tornaria a experiência do usuário cansativa. Para resolver esse problema, implementamos uma lógica que gera automaticamente os dados restantes no formato esperado pela API antes de enviá-los junto com metades dos dados reais fornecidos pelo usuario.

---

## ⚠️ Possíveis Erros ao Cadastrar e Como Resolver

### 1. Erro de Conexão ou Requisição Falhada

Esse erro ocorre porque nossa API está hospedada na plataforma Render (plano gratuito), que suspende a API após alguns minutos de inatividade.

**Solução:**

Antes de cadastrar, abra a API no Swagger e aguarde cerca de um minuto para que ela seja reativada.

Após a API carregar completamente no navegador, o endpoint estará pronto para receber a requisição POST e salvar os dados no banco Oracle.

---

### 2. Erro ao Clicar em "Cadastrar", Mesmo com a API Ativa

Às vezes, o front-end exibe um erro, mas o cadastro pode ter sido enviado corretamente para o back-end.

**Solução:**

Tente clicar no botão "Cadastrar" mais uma ou duas vezes antes de desistir.

Se o erro persistir, verifique se o cadastro foi salvo acessando os endpoints da API.

---

### 3. Erro Contínuo ao Tentar Cadastrar

Se o erro continuar aparecendo repetidamente, o problema pode estar relacionado a um CPF duplicado.

**Solução:**

O sistema impede o cadastro de um CPF já registrado.

Para testar um novo cadastro, altere um dígito do CPF (por exemplo, o último número) e tente novamente.

---

## 🔍 Como Verificar se o Cadastro Foi Salvo

Para conferir se o cadastro foi registrado corretamente no banco de dados da API, acesse um dos seguintes endpoints da API pelo Swagger:

### 🏷 **Deploy da API**

- **API Principal (C# .NET)**  
  (https://smartdent-api.onrender.com/swagger)

Se o campo "Empresa" não foi preenchido no cadastro:

Acesse: `/api/A_Pacientes/GetPacientesByPlanoIndividual`

Se foi cadastrada uma empresa:

Acesse: `/api/A_Pacientes/GetPacientesByEmpresa/{empresa}`

Substitua `{empresa}` pelo nome exato da empresa cadastrada.

---

## 📱 Persistência dos Dados no Aplicativo

Usamos AsyncStorage para armazenar Nome Completo e CPF localmente.

O que isso significa?

Mesmo que o app ou o emulador seja fechado e reaberto, esses dados continuarão salvos, permitindo login automático.

Quando um novo cadastro for realizado, os dados antigos serão substituídos pelos novos.

---

## ♻️ Como Limpar os Dados Salvos no Dispositivo

Se precisar resetar completamente os dados armazenados no aplicativo local, execute o seguinte comando no terminal do repositório do projeto:

```bash
adb shell pm clear com.smartdentapp

```
---

## 🧑‍💻 **Equipe de Desenvolvimento**

- **Macauly Vivaldo da Silva** – *Frontend & UX/UI, IA & Backend*  
- **Daniel Bezerra da Silva Melo** – *Mobile Developer & Infraestrutura DevOps (Deploy)*  
- **Gustavo Rocha Caxias** – *Banco de Dados*  
