FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["HomeOffCine.Api/HomeOffCine.Api.csproj", "HomeOffCine.Api/"]
COPY ["HomeOffCine.Business/HomeOffCine.Business.csproj", "HomeOffCine.Business/"]
COPY ["HomeOffCine.Infra/HomeOffCine.Infra.csproj", "HomeOffCine.Infra/"]
RUN dotnet restore "HomeOffCine.Api/HomeOffCine.Api.csproj"
COPY . .
WORKDIR "/src/HomeOffCine.Api"
RUN dotnet build "HomeOffCine.Api.csproj" -c Development -o /app/build

FROM build AS publish
RUN dotnet publish "HomeOffCine.Api.csproj" -c Development -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeOffCine.Api.dll"]