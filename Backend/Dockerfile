# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY Stefanini.sln .
COPY Stefanini.Domain/Stefanini.Domain.csproj Stefanini.Domain/
COPY Stefanini.Application/Stefanini.Application.csproj Stefanini.Application/
COPY Stefanini.Infrastructure/Stefanini.Infrastructure.csproj Stefanini.Infrastructure/
COPY Stefanini.Presentation.API/Stefanini.Presentation.API.csproj Stefanini.Presentation.API/

# Restore dependencies
RUN dotnet restore

# Copy the rest of the code
COPY . .

# Build and publish
WORKDIR /src/Stefanini.Presentation.API
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy published app
COPY --from=build /app/publish .

# Expose port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the app
ENTRYPOINT ["dotnet", "Stefanini.Presentation.API.dll"]

