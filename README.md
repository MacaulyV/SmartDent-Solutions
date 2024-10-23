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
- Commits devem ser feitos na branch `Back-end`.
- Certifique-se de seguir os padrões de mensagens de commit para garantir a clareza do histórico.

## Membros Responsável:
   [Gustavo Rocha Caxias] <br>
   [Macauly Vivaldo da Silva]
