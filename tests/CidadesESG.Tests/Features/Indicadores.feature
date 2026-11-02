# language: pt
Funcionalidade: Gerenciamento de indicadores ESG
  Como gestor de uma cidade inteligente
  Quero cadastrar, consultar, atualizar e remover indicadores ESG
  Para acompanhar metricas de sustentabilidade com integridade e rastreabilidade

  Cenario: Cadastrar um indicador ESG valido
    Dado que possuo dados validos de um indicador ESG
    Quando envio uma requisicao POST para cadastrar o indicador
    Entao a API deve retornar o status 201
    E o corpo deve conter o indicador cadastrado
    E o corpo deve respeitar o JSON Schema de indicador

  Cenario: Rejeitar cadastro de indicador ESG invalido
    Dado que possuo dados invalidos de um indicador ESG
    Quando envio uma requisicao POST para cadastrar o indicador
    Entao a API deve retornar o status 400

  Cenario: Consultar um indicador ESG existente
    Dado que existe um indicador ESG cadastrado
    Quando consulto esse indicador pelo identificador
    Entao a API deve retornar o status 200
    E o corpo deve respeitar o JSON Schema de indicador

  Cenario: Consultar um indicador ESG inexistente
    Dado que nao existe um indicador com identificador 999999
    Quando consulto o indicador de identificador 999999
    Entao a API deve retornar o status 404

  Cenario: Atualizar e remover um indicador ESG
    Dado que existe um indicador ESG cadastrado
    Quando atualizo esse indicador com dados validos
    Entao a API deve retornar o status 204
    Quando removo esse indicador
    Entao a API deve retornar o status 204
    E uma nova consulta ao indicador deve retornar 404
