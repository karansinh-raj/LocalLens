FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LocalLens.WebApi/LocalLens.WebApi.csproj", "LocalLens.WebApi/"]
RUN dotnet restore "./LocalLens.WebApi/./LocalLens.WebApi.csproj"
COPY . .
WORKDIR "/src/LocalLens.WebApi"
RUN dotnet build "./LocalLens.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LocalLens.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LocalLens.WebApi.dll"]