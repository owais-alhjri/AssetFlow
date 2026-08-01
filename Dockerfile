# ---- Stage 1: build ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

COPY AssetFlow-backend/src/ ./src/

RUN dotnet restore src/AssetFlow.API/AssetFlow.API.csproj
RUN dotnet publish src/AssetFlow.API/AssetFlow.API.csproj \
    -c Release -o /app/publish --no-restore

# ---- Stage 2: runtime ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

# Render injects the port as $PORT. ENV can't expand variables at runtime,
# so use the shell form; `exec` keeps dotnet as PID 1 so it receives SIGTERM.
ENTRYPOINT ["sh", "-c", "exec dotnet AssetFlow.API.dll --urls http://+:${PORT:-8080}"]