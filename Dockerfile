# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copia todos os arquivos do repositório para dentro do contêiner
COPY . ./

# Restaura as dependências e publica em modo Release
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

# Copia a pasta de build da etapa anterior
COPY --from=build /app/out .

# Expõe a porta 8080 (Render costuma mapear essa porta)
EXPOSE 8080

# Comando de inicialização da aplicação
ENTRYPOINT ["dotnet", "SmartDentAPI.dll"]
