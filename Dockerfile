# Use uma imagem oficial do .NET 9 Preview
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /src
COPY ["SmartDentAPI.csproj", "./"]
RUN dotnet restore "SmartDentAPI.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "SmartDentAPI.csproj" -c Release -o /app/build

# Publicação da aplicação
FROM build AS publish
RUN dotnet publish "SmartDentAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Configuração da execução
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartDentAPI.dll"]
