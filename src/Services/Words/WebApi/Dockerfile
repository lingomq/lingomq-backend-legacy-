#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Words/Words.WebApi/Api.csproj", "Services/Words/Words.WebApi/"]
COPY ["Services/Words/Words.Application/Application.csproj", "Services/Words/Words.Application/"]
COPY ["Services/EventBus/EventBus.csproj", "Services/EventBus/"]
COPY ["Services/Words/DataAccess/DataAccess.Dapper/DataAccess.Dapper.csproj", "Services/Words/DataAccess/DataAccess.Dapper/"]
COPY ["Services/Words/Words.Domain/Domain.csproj", "Services/Words/Words.Domain/"]
COPY ["Libs/Responses/Responses.csproj", "Libs/Responses/"]
RUN dotnet restore "./Services/Words/Words.WebApi/./Api.csproj"
COPY . .
WORKDIR "/src/Services/Words/Words.WebApi"
RUN dotnet build "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]