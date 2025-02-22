# Projeto de Avaliação para Desenvolvedores

`LEIA COM ATENÇÃO`

## Instruções
**O teste abaixo deverá ser entregue em até 7 dias corridos a partir da data de recebimento deste manual.**

- O código deve ser versionado em um repositório público no Github, e um link deve ser enviado para avaliação após a conclusão.
- Faça o upload deste template para seu repositório e comece a trabalhar a partir dele.
- Leia as instruções cuidadosamente e certifique-se de que todos os requisitos estão sendo atendidos.
- O repositório deve fornecer instruções sobre como configurar, executar e testar o projeto.
- A documentação e a organização geral também serão consideradas na avaliação.

## Caso de Uso
**Você é um desenvolvedor na equipe da DeveloperStore. Agora precisamos implementar os protótipos da API.**

Como trabalhamos com `DDD`, para referenciar entidades de outros domínios, usamos o padrão de `Identidades Externas` com desnormalização das descrições das entidades.

Portanto, você deverá desenvolver uma API (CRUD completo) que gerencie registros de vendas. A API deve ser capaz de informar:

* Número da venda
* Data da venda
* Cliente
* Valor total da venda
* Filial onde a venda foi realizada
* Produtos
* Quantidades
* Preços unitários
* Descontos
* Valor total de cada item
* Cancelada/Não Cancelada

Não é obrigatório, mas seria um diferencial construir o código para a publicação de eventos de:
* VendaCriada
* VendaModificada
* VendaCancelada
* ItemCancelado

Se você desenvolver esse código, **não é necessário** publicá-lo em um Message Broker. Você pode registrar uma mensagem no log da aplicação ou usar qualquer outro meio que considerar mais conveniente.

### Regras de Negócio

* Compras acima de 4 itens idênticos recebem um desconto de 10%
* Compras entre 10 e 20 itens idênticos recebem um desconto de 20%
* Não é possível vender mais de 20 itens idênticos
* Compras abaixo de 4 itens não podem ter desconto

Essas regras de negócio definem os níveis de desconto e as limitações de quantidade:

1. Níveis de Desconto:
   - 4+ itens: 10% de desconto
   - 10-20 itens: 20% de desconto

2. Restrições:
   - Limite máximo: 20 itens por produto
   - Nenhum desconto permitido para quantidades abaixo de 4 itens

## Visão Geral
Esta seção fornece uma visão geral do projeto e das diversas habilidades e competências que ele busca avaliar nos candidatos a desenvolvedor. 

Veja [Visão Geral](/.doc/overview.md)

## Stack Tecnológica
Esta seção lista as principais tecnologias utilizadas no projeto, incluindo os componentes de backend, testes, frontend e banco de dados. 

Veja [Stack Tecnológica](/.doc/tech-stack.md)

## Frameworks
Esta seção descreve os frameworks e bibliotecas que são utilizados no projeto para melhorar a produtividade e a manutenção do desenvolvimento. 

Veja [Frameworks](/.doc/frameworks.md)

<!-- 
## Estrutura da API
Esta seção inclui links para a documentação detalhada dos diferentes recursos da API:
- [API Geral](./docs/general-api.md)
- [API de Produtos](/.doc/products-api.md)
- [API de Carrinhos](/.doc/carts-api.md)
- [API de Usuários](/.doc/users-api.md)
- [API de Autenticação](/.doc/auth-api.md)
-->

## Estrutura do Projeto
Esta seção descreve a estrutura geral e a organização dos arquivos e diretórios do projeto. 

Veja [Estrutura do Projeto](/.doc/project-structure.md)