# Cidades ESG Inteligentes - Testes Automatizados BDD

Projeto academico em C# / .NET 8 que implementa uma API REST para indicadores ESG de cidades inteligentes. Esta versao evolui o projeto anterior com testes automatizados de API, BDD em Gherkin e testes de contrato com JSON Schema.

## Objetivo ESG

A API registra indicadores de sustentabilidade urbana (por exemplo energia renovavel e reciclagem). A automacao de testes contribui para governanca e compliance ao validar integridade, rastreabilidade, comportamento esperado e contrato das respostas da API.

## Stack

- .NET 8 / ASP.NET Core Web API
- Entity Framework Core + SQLite
- xUnit + WebApplicationFactory
- Reqnroll (BDD compativel com Gherkin/Cucumber para .NET)
- JsonSchema.Net para testes de contrato JSON Schema
- Docker / Docker Compose
- GitHub Actions (CI/CD)

## APIs cobertas

| Metodo | Endpoint | Validacoes automatizadas |
|---|---|---|
| GET | `/health` | 200, JSON, JSON Schema |
| GET | `/api/indicadores` | 200, lista JSON, JSON Schema |
| GET | `/api/indicadores/{id}` | 200/404, JSON, JSON Schema |
| POST | `/api/indicadores` | 201/400, corpo JSON, JSON Schema |
| PUT | `/api/indicadores/{id}` | 204/404 e persistencia da alteracao |
| DELETE | `/api/indicadores/{id}` | 204/404 e confirmacao da exclusao |

## Cenarios BDD

O arquivo `tests/CidadesESG.Tests/Features/Indicadores.feature` contem cinco cenarios executaveis:

1. cadastro valido (caminho feliz);
2. cadastro invalido (cenario negativo);
3. consulta de indicador existente;
4. consulta de indicador inexistente (404);
5. atualizacao e remocao do indicador.

Os steps estao em `tests/CidadesESG.Tests/StepDefinitions/IndicadoresSteps.cs`.

## JSON Schema

Os contratos ficam em `tests/CidadesESG.Tests/Schemas/`:

- `health.schema.json`
- `indicador.schema.json`
- `indicadores-list.schema.json`

Os testes carregam os arquivos de schema e validam as respostas reais usando JsonSchema.Net.

## Pre-requisitos

- .NET SDK 8.x
- Git
- Docker Desktop (opcional para executar a aplicacao em container)

## Executar os testes localmente

Na raiz do projeto:

```bash
dotnet restore CidadesESG.sln
dotnet build CidadesESG.sln --configuration Release
dotnet test CidadesESG.sln --configuration Release --logger "console;verbosity=normal"
```

Para gerar um arquivo TRX como evidencia:

```bash
mkdir -p TestResults
dotnet test CidadesESG.sln --configuration Release --logger "trx;LogFileName=resultados.trx" --results-directory TestResults
```

## Executar a API localmente

```bash
dotnet run --project src/CidadesESG.Api/CidadesESG.Api.csproj
```

Acesse o Swagger no endereco informado pelo terminal, acrescentando `/swagger`.

## Executar com Docker

```bash
docker compose up -d --build api-staging
```

Health check de staging: `http://localhost:8081/health`.

Para encerrar:

```bash
docker compose down
```

## CI/CD

O workflow `.github/workflows/ci-cd.yml` executa automaticamente restore, build, todos os testes xUnit/BDD, gera resultado TRX, valida a imagem Docker e, em push para `main`, executa os ambientes de staging e production com smoke test em `/health`.

## Evidencias recomendadas para a entrega

1. terminal com `dotnet test` e todos os testes aprovados;
2. GitHub Actions com o job `Build e testes` verde;
3. detalhe do passo `Testes automatizados`;
4. arquivo `.feature` aberto mostrando os cenarios Gherkin;
5. pasta `Schemas` mostrando os contratos JSON Schema.

## Estrutura principal

```text
src/CidadesESG.Api/
tests/CidadesESG.Tests/
  ApiTests/
  Features/
  Infrastructure/
  Schemas/
  StepDefinitions/
.github/workflows/ci-cd.yml
Dockerfile
docker-compose.yml
README.md
```

## Autor

Vitor Pavan
