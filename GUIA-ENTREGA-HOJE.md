# Guia final de entrega - Cidades ESG Inteligentes

## O que já está no pacote

- Código-fonte .NET 8 completo.
- API REST ESG com CRUD, Swagger e /health.
- Testes automatizados xUnit.
- Dockerfile multi-stage.
- Docker Compose com staging e produção, rede, variáveis e volumes persistentes SQLite.
- GitHub Actions com build, testes, validação Docker, staging e produção.
- README.md.
- Documentação PDF e PPT.
- Checklist no README e PDF.

## Última etapa que depende da sua conta/máquina

Os prints exigidos pelo professor precisam ser reais. Para obtê-los:

1. Crie um repositório no GitHub e envie o conteúdo desta pasta para a branch `main`.
2. Abra a aba **Actions** e aguarde o workflow **CI-CD Cidades ESG Inteligentes**.
3. Capture a tela mostrando os jobs verdes: **Build e testes**, **Deploy - Staging** e **Deploy - Production**.
4. Com Docker Desktop aberto, na pasta do projeto execute:
   `docker compose up --build -d`
5. Capture `docker compose ps`.
6. Abra `http://localhost:8081/health` e `http://localhost:8080/health` e capture as telas.
7. Abra `http://localhost:8080/swagger` e capture os endpoints.
8. Coloque as imagens em `docs/evidencias/` e, se desejar, substitua os placeholders da apresentação/PDF.

## Conferência antes do upload

Não marque “prints reais” como concluído se não tiver executado. Todo o restante está estruturado para atender à rubrica.
