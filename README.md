![Descrição banner](https://github.com/user-attachments/assets/acf148aa-b44a-4ebd-9085-9ce4f31ecaf0)

# 🚀 SmartDent Solutions – API de Monitoramento Odontológico

## 1️⃣ Descrição da Solução

O projeto **SmartDent Solutions** é uma API RESTful desenvolvida em .NET 8 com foco em monitoramento de sinistros odontológicos para operadoras de saúde.

A solução realiza o cadastro e gerenciamento de pacientes, consultas e procedimentos odontológicos, com persistência em banco SQL na nuvem (Azure SQL Database).

A aplicação está **integrada em uma esteira CI/CD no Azure DevOps**, automatizando build, teste, publicação e deploy direto para o Azure App Service.

---

## 2️⃣ Desenho da Pipeline (CI/CD) – Visão Geral e Etapas

> Resumo Visual:
> 

![Captura de tela 2025-05-22 201150.png](Captura_de_tela_2025-05-22_201150.png)

## 🛠️ Pipeline CI/CD – Como Funciona

- **Commit no GitHub:** Toda vez que alguém faz push no repositório, o Azure DevOps começa a pipeline automática.
- **Pipeline CI:**
    - Restaura as dependências do projeto (.NET Restore)
    - Faz o build do projeto (compila)
    - Roda os testes automáticos
    - Se falhar, para tudo e avisa o erro
    - Se passar, gera o artefato para deploy
- **Pipeline CD:**
    - Pega o artefato gerado e faz deploy no Azure Web App
    - API fica disponível na nuvem
    - (Opcional) Pode exigir aprovação manual antes de ir para produção
    - Se aprovado, finaliza o deploy em produção

**Resumindo:**

O fluxo garante que só código testado e aprovado chega na produção. Qualquer erro para a pipeline, e o deploy não acontece até ser corrigido

---

## 3️⃣ Configuração das Pipelines no Azure DevOps

- **CI Pipeline:**
    
    Configurada em `azure-pipelines.yml` para build e publicação.
    
- **CD Pipeline:**
    
    Integrada para deploy contínuo no Azure App Service.
    
- **Configuração automática do Banco de Dados:**
    
    Scripts e migrations aplicados na publicação.
    
---

##

## 2️⃣ Estrutura do Projeto

### 📂 Estrutura de Pastas e Arquivos
O projeto segue uma organização em camadas, garantindo separação de responsabilidades.

- **📁 Controllers/** → Contém os endpoints HTTP (ex.: PacientesController.cs, ConsultasController.cs).
- **📁 Models/** → Define as entidades do banco de dados (Paciente.cs, Consulta.cs, etc.).
- **📁 Repositories/** → Responsável pelo acesso e manipulação dos dados (PacienteRepository.cs).
- **📁 Interfaces/** → Define os contratos dos repositórios (IPacienteRepository.cs).
- **📁 DTOs/** → Contém os objetos de transferência de dados (PacienteCreateDTO, ConsultaUpdateDTO).
- **📁 Data/** → Configuração do banco (ApplicationDbContext.cs, DataSeeder.cs).
- **📄 Program.cs** → Configurações gerais do projeto, como injeção de dependência e Swagger.

### 💡 **Justificativa:**
Essa estrutura segue boas práticas de separação de responsabilidades, garantindo um código modular e fácil de manter.

---

### 🗄️ Modelo de Tabelas no Oracle
A estrutura do banco foi desenhada para refletir corretamente as relações do domínio:

#### 📝 Tabelas principais:

| Tabela       | Descrição                                                                 |
|--------------|---------------------------------------------------------------------------|
| Pacientes    | Contém dados do paciente (Nome, CPF, Data de Nascimento, etc.).           |
| Consultas    | Relacionada ao paciente (via IdPaciente), armazenando dados da consulta.  |
| Procedimentos| Associada a uma consulta (IdConsulta), armazenando o tipo de procedimento realizado. |
| Alertas      | Associada ao paciente (IdPaciente), armazenando notificações de risco da IA.|

#### 🔗 Relacionamentos:

- 1 Paciente → várias Consultas.
- 1 Consulta → 1 Procedimento.
- 1 Paciente → vários Alertas (alertas independentes, mas vinculados ao paciente).

### 💡 **Justificativa:**
Esse layout garante eficiência nas consultas e mantém a consistência dos dados com regras de exclusão em cascata (Delete Cascade).

---

## 4️⃣ Exemplos de Testes JSON para Cada Endpoint

### 📌 POST /api/pacientes)
```json
{
  "nomeCompleto": "João Silva",
  "cpf": "12345678900",
  "dataNascimento": "19850615",
  "email": "joao@email.com",
  "telefone": "11999999999",
  "endereco": "Rua A, 123, São Paulo",
  "planoOdontologico": "Maximum White",
  "empresa": "Empresa X"
}

 ```

### 📌 PUT /api/pacientes/{id})
```json
{
  "nomeCompleto": "Aline Ferreira",
  "email": "aline.ferreira@exemplo.com",
  "telefone": "11699704506",
  "endereco": "Rua Exemplo, 379, Bairro 5, Ituverava",
  "planoOdontologico": "Master",
  "empresa": "Centauro"
}

 ```

### 📌 POST /api/consultas)
```json
{
  "idPaciente": 20820516,
  "dataConsulta": "051220251000"
}

 ```

### 📌 PUT /api/D_Consultas/{id})
```json
{
  "dataConsulta": "300720231400",
  "status": "Realizada"
}

 ```

### 📌 POST /api/E_Procedimentos
```json
{
  "idConsulta": 116930515,
  "tipoProcedimento": "Consulta odontológica geral",
  "descricao": "Avaliação geral"
}


 ```

### 📌 PUT /api/E_Procedimentos{id})
```json
{
  "tipoProcedimento": "Restauração em resina composta",
  "descricao": "Troca de restauração antiga"
}

 ```

### 📌 POST /api/alertas)
```json
{
  "idPaciente": 20820516,
  "tipoAlerta": "UsoExcessivo",
  "grauRisco": "99",
  "justificativa": "Paciente realizou 10 consultas no último mês."
}

 ```

### 📌 PUT /api/H_Alertas//{id})
```json
{
  "tipoAlerta": "Uso Moderado com Tendência a Excesso",
  "grauRisco": "45%",
  "justificativa": "Paciente manteve consultas frequentes, mas não excessivas."
}

 ```

---

## 🧑‍💻 **Equipe de Desenvolvimento**

- **Macauly Vivaldo da Silva** – *Frontend & UX/UI, IA & Backend*  
- **Daniel Bezerra da Silva Melo** – *Mobile Developer & Infraestrutura DevOps (Deploy)*  
- **Gustavo Rocha Caxias** – *Banco de Dados*  
