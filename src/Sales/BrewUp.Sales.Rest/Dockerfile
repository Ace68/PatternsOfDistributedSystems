#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BrewUp.Sales.Rest/BrewUp.Sales.Rest.csproj", "BrewUp.Sales.Rest/"]
COPY ["BrewUp.Sales.Facade/BrewUp.Sales.Facade.csproj", "BrewUp.Sales.Facade/"]
COPY ["BrewUp.Sales.Domain/BrewUp.Sales.Domain.csproj", "BrewUp.Sales.Domain/"]
COPY ["BrewUp.Shared/BrewUp.Shared.csproj", "BrewUp.Shared/"]
COPY ["BrewUp.Sales.ReadModel/BrewUp.Sales.ReadModel.csproj", "BrewUp.Sales.ReadModel/"]
COPY ["BrewUp.Sales.SharedKernel/BrewUp.Sales.SharedKernel.csproj", "BrewUp.Sales.SharedKernel/"]
COPY ["BrewUp.Sales.Infrastructures/BrewUp.Sales.Infrastructures.csproj", "BrewUp.Sales.Infrastructures/"]
COPY ["BrewUp.Sales.Acl/BrewUp.Sales.Acl.csproj", "BrewUp.Sales.Acl/"]
RUN dotnet restore "./BrewUp.Sales.Rest/./BrewUp.Sales.Rest.csproj"
COPY . .
WORKDIR "/src/BrewUp.Sales.Rest"
RUN dotnet build "./BrewUp.Sales.Rest.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BrewUp.Sales.Rest.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BrewUp.Sales.Rest.dll"]