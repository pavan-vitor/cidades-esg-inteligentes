# Projeto - Cidades ESG Inteligentes

**Integrante:** Vitor Pavan  
**Stack principal:** C# / .NET 8 / ASP.NET Core Web API  
**Tema:** ESG, cidades inteligentes e monitoramento de indicadores sustentáveis.

## Visão geral

O projeto **Cidades ESG Inteligentes** disponibiliza uma API REST para registrar e consultar indicadores ambientais e urbanos, como consumo de energia, reciclagem e áreas verdes. A entrega foi adaptada para um fluxo DevOps, com build, testes, containerização e implantação automatizada em ambientes de staging e produção.

Endpoints principais:

- `GET /health` - health check da aplicação.
- `GET /api/indicadores` - lista indicadores ESG.
- `GET /api/indicadores/{id}` - consulta um indicador.
- `POST /api/indicadores` - cria um indicador.
- `PUT /api/indicadores/{id}` - atualiza um indicador.
- `DELETE /api/indicadores/{id}` - remove um indicador.
- `/swagger` - documentação interativa da API.

## Como executar localmente com Docker

### Pré-requisitos

- Docker Desktop com Docker Compose.
- Opcionalmente, .NET SDK 8 para execução sem Docker.

### Subindo staging e produção

1. Copie `.env.example` para `.env`.
2. No terminal, na raiz do projeto, execute:

```bash
docker compose up --build -d
```

3. Valide os ambientes:

- Staging: `http://localhost:8081/health`
- Produção: `http://localhost:8080/health`
- Swagger staging: `http://localhost:8081/swagger`
- Swagger produção: `http://localhost:8080/swagger`

4. Para visualizar os containers:

```bash
docker compose ps
```

5. Para encerrar:

```bash
docker compose down
```

Para remover também os volumes:

```bash
docker compose down -v
```

## Pipeline CI/CD

Foi utilizado **GitHub Actions**, configurado em `.github/workflows/ci-cd.yml`.

O fluxo possui três jobs:

1. **Build e testes**
   - checkout do repositório;
   - configuração do .NET 8;
   - `dotnet restore`;
   - `dotnet build`;
   - `dotnet test`;
   - validação do build da imagem Docker.

2. **Deploy - Staging**
   - executado após o sucesso de build/testes em push para `main`;
   - build do serviço `api-staging`;
   - inicialização do container;
   - smoke test em `/health`;
   - registro do estado dos containers.

3. **Deploy - Production**
   - depende do sucesso de staging;
   - utiliza o Environment `production` do GitHub;
   - build e inicialização do serviço de produção;
   - smoke test em `/health`.

> Para uma demonstração acadêmica reproduzível, staging e produção são ambientes containerizados separados, isolados por porta, variável de ambiente e volume. No GitHub Actions, esses deploys são efêmeros no runner e servem como validação automatizada. Em produção real, os mesmos jobs podem apontar para Azure App Service, Azure Container Apps, Kubernetes ou servidor remoto persistente.

## Containerização

O `Dockerfile` usa **multi-stage build**:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/CidadesESG.Api/CidadesESG.Api.csproj", "src/CidadesESG.Api/"]
RUN dotnet restore "src/CidadesESG.Api/CidadesESG.Api.csproj"
COPY . .
WORKDIR "/src/src/CidadesESG.Api"
RUN dotnet publish "CidadesESG.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "CidadesESG.Api.dll"]
```

A estratégia reduz a imagem final porque o SDK fica somente na etapa de compilação e a execução utiliza a imagem ASP.NET Runtime.

### Docker Compose

O `docker-compose.yml` utiliza:

- dois serviços: staging e produção;
- variáveis de ambiente;
- volumes nomeados para persistência do banco SQLite;
- rede bridge própria `esg-network`;
- portas distintas;
- política `restart: unless-stopped`.

## Prints do funcionamento

Inclua nesta seção, antes da entrega, as evidências reais geradas durante a execução:

1. GitHub Actions - job **Build e testes** verde.
2. GitHub Actions - job **Deploy - Staging** verde.
3. GitHub Actions - job **Deploy - Production** verde.
4. `docker compose ps` mostrando os containers.
5. Navegador em `http://localhost:8081/health`.
6. Navegador em `http://localhost:8080/health`.
7. Swagger exibindo `GET /api/indicadores`.

Salve os arquivos em `docs/evidencias/`.

## Tecnologias utilizadas

- C#;
- .NET 8;
- ASP.NET Core Web API;
- Entity Framework Core + SQLite;
- xUnit;
- Swagger / OpenAPI;
- Docker;
- Docker Compose;
- GitHub Actions;
- Git / GitHub.

## Testes

Execução local:

```bash
dotnet restore CidadesESG.sln
dotnet build CidadesESG.sln -c Release
dotnet test CidadesESG.sln -c Release
```

Os testes automatizados validam o endpoint de health check e a consulta de indicadores.

## Desafios encontrados e soluções

**Padronização entre ambientes:** staging e produção precisam executar o mesmo artefato com configurações diferentes. A solução foi utilizar variáveis de ambiente e portas distintas no Docker Compose.

**Reprodutibilidade:** para reduzir diferenças entre máquinas, o Dockerfile empacota as dependências e o runtime necessários. O SQLite fica persistido em volume Docker separado por ambiente.

**Qualidade antes do deploy:** o pipeline impede o avanço para staging caso build ou testes falhem. Produção depende do sucesso do job de staging.

**Evidências:** o endpoint `/health`, os smoke tests e `docker compose ps` facilitam a comprovação do funcionamento.

## Checklist de entrega

| Item | OK |
|---|:---:|
| Projeto compactado em .ZIP com estrutura organizada | ☑ |
| Dockerfile funcional | ☑ |
| docker-compose.yml ou arquivos Kubernetes | ☑ |
| Pipeline com etapas de build, teste e deploy | ☑ |
| README.md com instruções | ☑ |
| Documentação técnica em PDF/PPT | ☑ |
| Deploy configurado para staging e produção | ☑ |
| Prints reais de execução anexados antes da entrega | ☐ |

> **Atenção:** o último item deve ser marcado somente depois de executar o projeto e o workflow e inserir prints reais. Não entregue evidências simuladas como se fossem execuções reais.
