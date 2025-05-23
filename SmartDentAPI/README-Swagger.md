# Documentação Swagger da API SmartDent

## Visão Geral

A API SmartDent possui uma documentação detalhada e interativa criada com Swagger/OpenAPI. Esta documentação permite:

- Explorar todos os endpoints disponíveis
- Testar requisições diretamente através da interface
- Visualizar os modelos de dados e seus exemplos
- Compreender os possíveis códigos de resposta

## Acessando a Documentação

A documentação Swagger está disponível em:

- Ambiente de desenvolvimento: `/swagger`
- Ambiente de produção: `/api-docs`

## Recursos Implementados

1. **Informações Gerais da API**
   - Título, versão e descrição detalhada
   - Informações de contato e licença
   - Visão geral dos recursos disponíveis

2. **Endpoints Organizados por Controladores**
   - Pacientes
   - Consultas
   - Procedimentos
   - Alertas
   - Outros recursos do sistema

3. **Documentação Detalhada para cada Endpoint**
   - Descrição clara da funcionalidade
   - Parâmetros necessários
   - Corpo da requisição (para POST/PUT)
   - Exemplos de requisição
   - Possíveis códigos de resposta e seus significados
   - Exemplos de respostas

4. **Exemplos Pré-configurados**
   - Exemplos de criação para todos os endpoints POST
   - Exemplos de atualização para todos os endpoints PUT
   - Valores realistas para facilitar o teste

5. **Interface Personalizada**
   - Estilo visual alinhado com a identidade da SmartDent
   - Organização lógica dos endpoints
   - Filtragem e busca de endpoints

## Benefícios da Documentação

1. **Para Desenvolvedores:**
   - Compreensão rápida da API
   - Facilidade para integração com outros sistemas
   - Testes de endpoints sem necessidade de ferramentas adicionais

2. **Para Equipe de Negócios:**
   - Visualização dos recursos disponíveis
   - Entendimento das funcionalidades sem conhecimento técnico profundo

3. **Para Suporte:**
   - Referência para troubleshooting
   - Documentação sempre atualizada com o código

## Tecnologias Utilizadas

- Swashbuckle.AspNetCore 6.5.0
- Swashbuckle.AspNetCore.Annotations 6.5.0
- ASP.NET Core 9.0

## Manutenção da Documentação

A documentação é gerada automaticamente a partir:

1. **Comentários XML no código**
   - Descrições em controllers, actions e modelos
   - Anotações de exemplo com a tag `<example>`

2. **Atributos do Swagger**
   - `[SwaggerOperation]` para endpoints
   - `[SwaggerResponse]` para respostas
   - `[SwaggerSchema]` para modelos

3. **Exemplos programáticos**
   - Configurados em `SwaggerExamplesConfig.cs`
   - Associados a operações específicas

## Extensibilidade

A documentação pode ser facilmente expandida:

1. **Adicionar novos exemplos** em `SwaggerExamplesConfig.cs`
2. **Customizar a UI** modificando o arquivo CSS em `wwwroot/swagger-ui/custom.css`
3. **Adicionar novas seções** através da configuração em `SwaggerConfig.cs`

## Melhores Práticas

Ao adicionar novos endpoints à API:

1. **Documente corretamente com comentários XML**
2. **Adicione exemplos realistas** para requisições e respostas
3. **Especifique todos os possíveis códigos de resposta**
4. **Mantenha os grupos (tags) organizados** 