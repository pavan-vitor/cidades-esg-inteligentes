# Guia rapido - evidencias para a atividade de Testes Automatizados

Depois de enviar este projeto ao GitHub:

1. Abra a aba **Actions** e execute/aguarde o workflow `CI-CD Cidades ESG Inteligentes`.
2. Tire um print do resumo mostrando **Build e testes** concluido com sucesso.
3. Entre no job **Build e testes** e tire um print do passo **Testes automatizados**.
4. No computador, execute `dotnet test CidadesESG.sln --configuration Release` e tire um print do resumo final.
5. Abra `tests/CidadesESG.Tests/Features/Indicadores.feature` e tire um print dos cenarios em Gherkin.
6. Abra `tests/CidadesESG.Tests/Schemas/indicador.schema.json` e tire um print do contrato.

O workflow tambem publica o artefato `resultados-testes` com o arquivo TRX da execucao.
