# Etapa 1: Build com .NET 7.0
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Etapa 2: Runtime com .NET 7.0
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "SmartDentAPI.dll"]
