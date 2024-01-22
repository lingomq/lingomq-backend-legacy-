#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Identity/WebApi/WebApi.csproj", "Services/Identity/WebApi/"]
COPY ["Services/Identity/Application/Application.csproj", "Services/Identity/Application/"]
COPY ["Libs/Cryptography/Cryptography.csproj", "Libs/Cryptography/"]
COPY ["Services/EventBus/EventBus.csproj", "Services/EventBus/"]
COPY ["Services/Identity/DataAccess/DataAccess.Dapper/DataAccess.Dapper.csproj", "Services/Identity/DataAccess/DataAccess.Dapper/"]
COPY ["Services/Identity/Domain/Domain.csproj", "Services/Identity/Domain/"]
COPY ["Libs/Responses/Responses.csproj", "Libs/Responses/"]
RUN dotnet restore "./Services/Identity/WebApi/./WebApi.csproj"
COPY . .
WORKDIR "/src/Services/Identity/WebApi"
RUN dotnet build "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.WebApi.dll"]