# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar arquivos de projetos (sem a solution)
COPY Stefanini.Presentation.API/Stefanini.Presentation.API.csproj Stefanini.Presentation.API/
COPY Stefanini.Application/Stefanini.Application.csproj Stefanini.Application/
COPY Stefanini.Domain/Stefanini.Domain.csproj Stefanini.Domain/

# Restaurar dependências (apenas do projeto API, não da solution inteira)
RUN dotnet restore "Stefanini.Presentation.API/Stefanini.Presentation.API.csproj"

# Copiar todo o código fonte
COPY . .

# Build do projeto API
WORKDIR "/src/Stefanini.Presentation.API"
RUN dotnet build "Stefanini.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Stefanini.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Configurar para usar a porta do Railway
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Stefanini.Presentation.API.dll"]

# Force rebuild 2024-10-18