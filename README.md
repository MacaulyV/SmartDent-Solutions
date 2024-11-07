# 🦷 SmartDent Central Controller

## 📝 Descrição do Projeto

O **SmartDent Central Controller** é uma aplicação de gerenciamento odontológico desenvolvida em **Java** e **Spring Boot**. O projeto visa criar uma API REST que permite realizar operações CRUD para gerenciar os dados de pacientes e empresas associadas, incluindo a integração com a **API ViaCep** para facilitar o preenchimento de dados de endereço.

## 📊 Utilidade do Projeto

O **SmartDent Central Controller** é uma parte fundamental do ecossistema da **SmartDent Solutions**. Ele atua como um middleware que conecta o sistema da **OdontoPrev**, o site ou aplicativo que os pacientes utilizam para marcar consultas, aos dados que serão exibidos na futura plataforma **SmartDent**.

Este sistema serve para controlar, gerenciar e organizar os dados que posteriormente serão integrados e exibidos na **SmartDent**, uma plataforma mais completa que ajudará a **OdontoPrev** a ter controle total dos dados dos pacientes, tratar e prevenir sinistros, e usar **IA** para análise de dados. A dashboard utilizada aqui é uma ferramenta de controle interno, separada da futura plataforma **SmartDent**, que será usada pelos funcionários da **OdontoPrev**.

### 📌 Dados Controlados

- **Pacientes**: Nome, contato, dados pessoais e histórico de consultas.
- **Empresas**: Informações sobre as empresas vinculadas aos pacientes, como nome, endereço, etc.
- **Endereços**: Utilizando a integração com a **API ViaCep** para preenchimento automático, facilitando o cadastro.

Os dados são visíveis ao sistema por meio da **dashboard**, onde o usuário pode realizar operações como cadastro de novos pacientes, atualizações de informações e verificação de histórico. Nesta fase inicial do projeto, estamos utilizando as tabelas de **Pacientes**, **Endereços** e **Empresas** para criar a estrutura de gerenciamento de dados necessária. A interface da dashboard (HTML, CSS, JavaScript) consome a API REST para exibir e gerenciar as informações de forma intuitiva e eficiente. Esta solução centraliza o controle dos dados e facilita a integração do **back-end** com o **front-end**, oferecendo uma experiência de gerenciamento ágil e segura.

O projeto do **SmartDent Central Controller** atualmente está em uma versão alfa, o que significa que ainda passará por diversas melhorias. Futuramente, planejamos incluir dados mais complexos que estarão disponíveis na plataforma principal, e esses dados serão essenciais para os modelos de **IA**, permitindo análises preditivas e a prevenção de sinistros de maneira eficaz.

## 👥 Integrantes do Grupo e Responsabilidades

- **🔹 Macauly**: Responsável pelo desenvolvimento do **back-end** com Java e Spring Boot, integração com banco de dados **Oracle**, **API ViaCep**, e também pela criação do **front-end** da dashboard, usando HTML, CSS e JavaScript, integrando com a API desenvolvida.
- **🔹 Daniel**: Auxilia na pesquisa sobre a arquitetura do projeto, comunica-se com Gustavo para definir os dados e tabelas que devem ser utilizados a cada melhoria, e repassa informações para Macauly implementar e melhorar.
- **🔹 Gustavo**: Responsável pelo gerenciamento de todo o banco de dados, incluindo as tabelas mais avançadas que serão utilizadas nas próximas versões do projeto. Também colabora com Daniel para decidir quais dados e tabelas devem ser usados no projeto conforme ele evolui.

> **Nota**: Esta explicação refere-se apenas ao projeto **SmartDent Central Controller** e não ao contexto geral do projeto completo da **SmartDent Solutions**.

## 🗓️ Cronograma de Desenvolvimento

Para o desenvolvimento do projeto, as atividades foram divididas da seguinte maneira:

Macauly ficou responsável por configurar o banco de dados, desenvolver a API e integrar os serviços externos, como a API ViaCep. Essa etapa começou em 1º de novembro de 2024 e foi concluída em 10 de novembro de 2024.

Daniel assumiu a tarefa de criar a dashboard, focando no front-end e conectando-a à API criada por Macauly. Ele trabalhou nessa parte entre 4 e 15 de novembro de 2024.

Gustavo cuidou dos testes e da validação da API. Sua etapa começou em 10 de novembro e terminou em 17 de novembro de 2024, garantindo que todas as funcionalidades estavam operando corretamente e que os dados estavam sendo processados de maneira eficaz.

## 🔧 Instruções para Rodar a Aplicação

### 🚀 Configuração do Ambiente

1. **Certifique-se de que o Java 17 está instalado.**

2. **Instale o Maven** para gerenciar as dependências do projeto.

3. **Configure o banco de dados Oracle** e ajuste o arquivo `application.properties` para incluir as credenciais corretas.

### 📥 Clonar o Repositório

```bash
git clone -b Back-end https://github.com/MacaulyV/SmartDent-Solutions.git
```
### 📥 Clonar o Repositório
```bash
mvn spring-boot:run
```

# SmartDent Central Controller

<div align="center">
## 🏗️ Arquitetura Geral

![Arquitetura](https://github.com/user-attachments/assets/b0ae0d0b-bd9e-432b-a19a-1031ec118c3d)
</div>

*Nota:* Este diagrama mostra a arquitetura do SmartDent Central Controller, incluindo como ele se comunica com outros componentes, como o sistema da OdontoPrev, o banco de dados Oracle e a interface da dashboard. Nesta versão inicial, a arquitetura foi planejada para atender aos seguintes pontos:

- **Interface do Usuário - Dashboard:**  
  Os usuários interagem com o sistema através da dashboard, onde realizam operações de cadastro, edição e consulta de dados.

- **API REST - Spring Boot:**  
  Esta camada é responsável por controlar toda a lógica do sistema e se comunicar com os outros componentes, oferecendo operações CRUD e conectando ao banco de dados.

- **Base de Dados - Oracle:**  
  Armazena os dados dos pacientes, empresas e endereços, que são recuperados e manipulados conforme necessário pela API.

- **Serviço Externo - API ViaCep:**  
  Utilizado para obter automaticamente informações de endereço a partir do CEP, facilitando o preenchimento e minimizando erros de entrada de dados.

- **Sistema OdontoPrev:**  
  Conecta-se à nossa API para sincronização de dados e garantir que todas as informações sobre pacientes estejam alinhadas entre os sistemas.

Futuramente, a arquitetura será expandida para incluir novos módulos e funcionalidades mais complexas.

<div align="center">
## 🗺️ Entidade e Relacionamento (ERD)

![ERD](https://github.com/user-attachments/assets/94882307-1898-4754-adb7-0dc9a7c7ed4e)
</div>

*Nota:* Neste diagrama ERD, mostramos todas as tabelas do projeto. Nesta versão alfa, estamos utilizando apenas as tabelas de **Pacientes**, **Endereços** e **Empresas**. A tabela **Empresa** foi adicionada de última hora para atender às necessidades do projeto e, por isso, ainda não está completamente integrada ao diagrama ERD apresentado. Essa integração será aprimorada nas próximas entregas para garantir a consistência do banco de dados.

<div align="center">
  
### Diagrama de Classes

![CLASSES](https://github.com/user-attachments/assets/79f32929-f4b1-436c-986a-01214331c1e0)
</div>

*Nota:* Neste diagrama de classes, apresentamos as três classes principais utilizadas nesta versão alfa do projeto: **Paciente**, **Empresa** e **Endereço**. Essas classes representam as entidades fundamentais para o gerenciamento dos dados no sistema. Cada classe contém atributos essenciais, como `pacienteID`, que atua como identificador único na classe **Paciente**, garantindo a unicidade dos registros. Além disso, o relacionamento entre as classes, como a associação entre **Paciente** e **Endereço** por meio do atributo `pacienteID`, está claramente estabelecido. Essa estrutura inicial permite o armazenamento e a manipulação de informações de forma organizada, e será ampliada nas próximas versões para incluir relacionamentos mais complexos e novas funcionalidades.

## 🎥 Link para o Vídeo de Apresentação

Assista ao vídeo de apresentação do projeto clicando no link abaixo:  
[**Apresentação do Projeto SmartDent**](#)

### 📋 No vídeo, discutimos:

- **Proposta Tecnológica:**  
  Como o SmartDent ajuda a gerenciar os dados odontológicos de forma centralizada.

- **Público-Alvo:**  
  Clínicas odontológicas que desejam melhorar a eficiência no gerenciamento de pacientes e suas informações.

- **Problemas Solucionados:**  
  Redução de erros no cadastro de endereços, melhor controle dos dados de pacientes e otimização dos agendamentos e acompanhamentos.

## 📚 Documentação dos Endpoints da API

### Pacientes

- **GET /pacientes**  
  Retorna a lista de todos os pacientes.

- **POST /pacientes**  
  Cria um novo paciente.

- **GET /pacientes/{id}**  
  Retorna um paciente específico pelo ID.

- **PUT /pacientes/{id}**  
  Atualiza as informações de um paciente existente.

- **DELETE /pacientes/{id}**  
  Deleta um paciente pelo ID.

### Endereços

- **GET /enderecos/cep/{cep}**  
  Este endpoint aceita um CEP como parâmetro e faz uma solicitação à API ViaCep para obter informações detalhadas do endereço, como rua, bairro, cidade e estado. O cliente (como a dashboard) faz uma requisição HTTP para a API do projeto, que então consulta a API ViaCep e retorna os dados recebidos ao cliente.

### Empresas

As informações da empresa são adicionadas automaticamente ao cadastrar um paciente. Quando um novo paciente é inserido, os dados da empresa onde ele trabalha são vinculados ao registro do paciente. Não há endpoints separados para criar ou listar empresas diretamente.

## 📈 Evolução do Projeto

Nesta seção, vamos detalhar os tópicos abordados desde a primeira Sprint e as melhorias feitas nesta segunda entrega:

1. **Compreensão da Arquitetura do Projeto:**  
   Na primeira entrega, estávamos focados em entender como funcionaria toda a arquitetura do projeto e não tínhamos uma base clara sobre o papel do projeto em Java com Spring como sendo o elo central entre o sistema da SmartDent e a plataforma da OdontoPrev. Por isso, na Sprint 1, criamos um sistema simples, sem interface gráfica, que consistia apenas em um CRUD básico para a tabela **Paciente**, com dados limitados e focado em visualizar os endpoints desenvolvidos.

2. **Evolução da Estrutura de Dados:**  
   Nesta segunda entrega, aprimoramos a estrutura, incluindo as tabelas de **Endereços** e **Empresas**, além da integração com a API ViaCep para preenchimento automático dos dados de endereço.

3. **Aprimoramento da API REST:**  
   A API REST foi expandida para incluir novos endpoints que permitiram operações mais detalhadas para cada entidade, como pacientes e endereços. Além disso, realizamos melhorias na lógica interna da API para otimizar o desempenho e reduzir redundâncias.

4. **Dashboard:**  
   A interface do usuário foi ampliada para incluir a exibição dos novos dados de **Endereços** e **Empresas**, proporcionando uma experiência mais rica e eficiente para quem utiliza a plataforma.

Estas melhorias refletem a evolução contínua do projeto, evidenciando os avanços desde a Sprint 1 até a Sprint 2.

## 🔚 Considerações Finais

O projeto **SmartDent Central Controller** visa transformar o gerenciamento de dados da OdontoPrev em uma tarefa eficiente, proporcionando uma solução centralizada que conecta diversas fontes de dados de maneira integrada. Nesta versão inicial, já contamos com recursos como integração com serviços externos, como a **API ViaCep**, para automatizar o preenchimento de informações de endereço e assegurar a consistência dos dados. Futuramente, o projeto será ampliado para incluir funcionalidades mais complexas, utilizando tabelas com dados avançados que servirão de base para modelos preditivos de IA, ajudando a prevenir sinistros, além de uma integração direta com a plataforma principal, que será o front-end do nosso projeto.

## 1. Back-end - README.md

```markdown
# SmartDent Solutions - Back-end

## Objetivo
Este repositório contém o código do sistema de gerenciamento central do back-end da plataforma
SmartDent. Ele gerencia a lógica de negócio e se conecta ao banco de dados Oracle, integrando as
diferentes partes do sistema, como Front-end e Mobile. Além disso, ele controla todos os dados que
são exibidos nas interfaces, como Mobile e Front-end, garantindo que os dados sejam corretamente
processados e disponibilizados aos usuários após passarem pelo banco de dados.

## Tecnologias Utilizadas
- Java 11
- Spring Boot
- Oracle Database
- JPA/Hibernate

## Como Configurar Localmente
1. **Clone o Repositório**:
   - Clone o repositório em sua máquina local:
   ```bash
   git clone --branch Back-end https://github.com/MacaulyV/SmartDent-Solutions.git
   ```

2. **Crie uma Branch de Trabalho**:
   - Para manter a organização do desenvolvimento, crie uma nova branch a partir da branch `Back-end`:
   ```bash
   git checkout -b minha-branch-de-desenvolvimento
   ```
   - **Nota**: Criar uma branch a partir de Back-end gera uma nova cópia baseada no estado atual dessa branch. A nova branch pode ter qualquer nome, permitindo trabalhar de forma isolada, sem impactar Back-end.

3. **Baixar Dependências**:
   - Instale todas as dependências do projeto:
   ```bash
   mvn install
   ```
   - Caso novas dependências sejam adicionadas ao projeto, certifique-se de rodar este comando novamente para garantir que tudo esteja atualizado.

4. **Faça as Alterações no Código**:
   - Utilize o IntelliJ IDEA para realizar as modificações necessárias no código Java e Spring Boot.

5. **Verificar Alterações e Stage**:
   - Após realizar as alterações, verifique o que foi modificado:
   ```bash
   git status
   ```
   - Adicione as alterações para commit:
   ```bash
   git add .
   ```

6. **Commit das Alterações**:
   - Faça o commit das alterações, garantindo que a mensagem de commit seja clara e descritiva:
   ```bash
   git commit -m "feat: descrição clara das alterações realizadas"
   ```

7. **Atualize o Repositório Local**:
   - Antes de enviar suas mudanças para o repositório remoto, certifique-se de atualizar sua branch local com as últimas alterações da branch `Back-end`:
   ```bash
   git pull origin Back-end
   ```
   - Resolva quaisquer conflitos que possam existir.

8. **Push das Alterações**:
   - Envie suas alterações para o repositório remoto:
   ```bash
   git push origin minha-branch-de-desenvolvimento
   ```

9. **Abra um Pull Request (PR)**:
   - No GitHub, abra um Pull Request da sua branch de desenvolvimento para a branch `Back-end`, descrevendo claramente as alterações feitas.

10. **Execute o Sistema**:
    - Para testar o sistema localmente, execute o Spring Boot:
    ```bash
    mvn spring-boot:run
    ```

## Estrutura do Código
- `/src`: Código fonte do sistema
- `/config`: Arquivos de configuração
- `/docs`: Documentação e instruções sobre a integração com outros componentes

## Regras de Commit
- Certifique-se de seguir os padrões de mensagens de commit para garantir a clareza do histórico.

## Membros Responsável:
   [Gustavo Rocha Caxias] <br>
   [Macauly Vivaldo da Silva]
