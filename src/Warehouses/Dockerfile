FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore "BrewUp.Warehouses.Rest/BrewUp.Warehouses.Rest.csproj"
COPY . .
WORKDIR "/src/BrewUp.Warehouses.Rest"
RUN dotnet build "BrewUp.Warehouses.Rest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BrewUp.Warehouses.Rest.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BrewUp.Warehouses.Rest.dll"]