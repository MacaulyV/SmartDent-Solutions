# SmartDentApp 🦷

## Descrição do Projeto
SmartDentApp é uma aplicação móvel desenvolvida em React Native para gerenciamento de serviços odontológicos. O aplicativo oferece uma interface moderna e intuitiva para pacientes e profissionais da área odontológica, facilitando o acesso a serviços e informações.

## 🚀 Tecnologias Utilizadas

### Core
- React Native
- TypeScript
- React Navigation
- Axios para requisições HTTP

### UI/UX
- React Native Animated para animações suaves
- React Native Linear Gradient para elementos visuais gradientes
- Componentes customizados para formulários e alertas

### Segurança e Armazenamento
- React Native Encrypted Storage para armazenamento seguro
- Device Info para identificação única de dispositivos
- Validações client-side robustas

### Desenvolvimento
- ESLint para linting de código
- Prettier para formatação consistente
- Jest para testes automatizados
- TypeScript para tipagem estática

## 📱 Funcionalidades Principais

### Sistema de Autenticação
- Registro de usuários com validação de dados
- Login com email/nome + CPF
- Armazenamento seguro de credenciais
- Validação de dispositivo

### Validações de Formulário
- Validação de CPF
- Validação de nome completo (mínimo 3 palavras)
- Validação de formato de email
- Feedback visual imediato

### Interface do Usuário
- Animações suaves de transição
- Feedback visual para ações do usuário
- Alertas personalizados com gradientes
- Formulários responsivos

## 🏗️ Arquitetura do Projeto

### Estrutura de Diretórios
```
SmartDentApp/
├── android/                 # Configurações nativas Android
├── ios/                    # Configurações nativas iOS
├── src/
│   ├── screens/           # Telas da aplicação
│   │   ├── Auth/         # Telas de autenticação
│   │   └── Main/         # Telas principais
│   ├── components/       # Componentes reutilizáveis
│   ├── services/         # Serviços e APIs
│   │   ├── api.ts       # Configuração base da API
│   │   ├── auth.ts      # Serviço de autenticação
│   │   └── storage.ts   # Serviço de armazenamento
│   ├── navigation/       # Configuração de rotas
│   ├── context/         # Contextos React
│   ├── utils/           # Utilitários
│   ├── types/           # Definições de tipos TypeScript
│   └── assets/          # Recursos estáticos
├── __tests__/           # Testes automatizados
└── config/              # Arquivos de configuração
```

### Serviços Principais

#### API Service (api.ts)
- Configuração base do Axios
- Interface para dados do paciente
- Endpoints para criação e consulta de usuários
- Interceptors para logging de requisições e respostas

#### Authentication Service (auth.ts)
- Singleton pattern para gerenciamento de estado
- Registro de novos usuários com validação
- Login com suporte a email ou nome + CPF
- Validação local de credenciais
- Gerenciamento de sessão

#### Storage Service (storage.ts)
- Armazenamento criptografado de dados sensíveis
- Identificação única de dispositivos
- Validação de tempo de sessão
- Limpeza automática de dados expirados

## 🔒 Segurança

### Armazenamento Local
- Criptografia de dados sensíveis
- Validação de dispositivo
- Expiração automática de dados
- Limpeza segura de informações

### Validações
- Sanitização de inputs
- Validação client-side
- Proteção contra inputs maliciosos
- Timeout de sessão

## 💻 Como Executar o Projeto

### Pré-requisitos
- Node.js 18 ou superior
- React Native CLI
- Android Studio (para Android)
- Xcode (para iOS)
- JDK 11

### Instalação
1. Clone o repositório
2. Instale as dependências:
```bash
npm install
```

### Execução
Para Android:
```bash
npm run android
```

Para iOS:
```bash
npm run ios
```

## 🧪 Testes
O projeto inclui testes automatizados usando Jest:
```bash
npm test
```

## 📱 Compatibilidade
- iOS 13.0 ou superior
- Android 6.0 (API 23) ou superior

## 🔄 CI/CD
- ESLint para qualidade de código
- Prettier para formatação consistente
- TypeScript para prevenção de erros
- Testes automatizados

## 👥 Autores
[Nome do Autor]

## 📄 Licença
[Tipo de Licença]

## 🤝 Agradecimentos
- Professores e orientadores
- Contribuidores do projeto
- Comunidade React Native
