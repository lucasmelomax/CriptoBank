
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["CriptoBank.API/CriptoBank.API.csproj", "CriptoBank.API/"]
COPY ["CriptoBank.Worker/CriptoBank.Worker.csproj", "CriptoBank.Worker/"]
COPY ["CriptoBank.Application/CriptoBank.Application.csproj", "CriptoBank.Application/"]
COPY ["CriptoBank.Infrastructure/CriptoBank.Infrastructure.csproj", "CriptoBank.Infrastructure/"]
COPY ["CriptoBank.Domain/CriptoBank.Domain.csproj", "CriptoBank.Domain/"]

RUN dotnet restore "CriptoBank.API/CriptoBank.API.csproj"

COPY . .
RUN dotnet publish "CriptoBank.API/CriptoBank.API.csproj" -c Release -o /app/publish/api
RUN dotnet publish "CriptoBank.Worker/CriptoBank.Worker.csproj" -c Release -o /app/publish/worker

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final-api
WORKDIR /app
COPY --from=build /app/publish/api .
ENTRYPOINT ["dotnet", "CriptoBank.API.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final-worker
WORKDIR /app
COPY --from=build /app/publish/worker .
ENTRYPOINT ["dotnet", "CriptoBank.Worker.dll"]