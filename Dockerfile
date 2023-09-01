FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BankingSystem.Api/BankingSystem.Api.csproj", "BankingSystem.Api/"]
COPY ["BankingSystem.Core/BankingSystem.Core.csproj", "BankingSystem.Core/"]
COPY ["BankingSystem.Models/BankingSystem.Models.csproj", "BankingSystem.Models/"]
COPY ["BankingSystem.Repositories/BankingSystem.Repositories.csproj", "BankingSystem.Repositories/"]
RUN dotnet restore "BankingSystem.Api/BankingSystem.Api.csproj"
COPY . .
WORKDIR "/src/BankingSystem.Api"
RUN dotnet build "BankingSystem.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BankingSystem.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "BankingSystem.Api.dll"]