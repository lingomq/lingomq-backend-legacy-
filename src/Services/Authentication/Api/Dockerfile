#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Authentication/Api/Api.csproj", "Services/Authentication/Api/"]
COPY ["Services/Authentication/Application/Application.csproj", "Services/Authentication/Application/"]
COPY ["Libs/Cryptography/Cryptography.csproj", "Libs/Cryptography/"]
COPY ["Services/EventBus/EventBus.csproj", "Services/EventBus/"]
COPY ["Services/Authentication/DataAccess/DataAccess.Dapper/DataAccess.Dapper.csproj", "Services/Authentication/DataAccess/DataAccess.Dapper/"]
COPY ["Services/Authentication/Domain/Domain.csproj", "Services/Authentication/Domain/"]
COPY ["Libs/Responses/Responses.csproj", "Libs/Responses/"]
RUN dotnet restore "./Services/Authentication/Api/./Api.csproj"
COPY . .
WORKDIR "/src/Services/Authentication/Api"
RUN dotnet build "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]