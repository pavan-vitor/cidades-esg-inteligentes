# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/CidadesESG.Api/CidadesESG.Api.csproj", "src/CidadesESG.Api/"]
RUN dotnet restore "src/CidadesESG.Api/CidadesESG.Api.csproj"

COPY . .
WORKDIR "/src/src/CidadesESG.Api"
RUN dotnet publish "CidadesESG.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: runtime enxuto
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "CidadesESG.Api.dll"]
