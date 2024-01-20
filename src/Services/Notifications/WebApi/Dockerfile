#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Notifications/WebApi/WebApi.csproj", "Services/Notifications/WebApi/"]
COPY ["Services/Notifications/Application/Application.csproj", "Services/Notifications/Application/"]
COPY ["Services/EventBus/EventBus.csproj", "Services/EventBus/"]
COPY ["Services/Notifications/DataAccess/Notifications.DataAccess.Dapper/DataAccess.Dapper.csproj", "Services/Notifications/DataAccess/Notifications.DataAccess.Dapper/"]
COPY ["Services/Notifications/Domain/Domain.csproj", "Services/Notifications/Domain/"]
COPY ["Libs/Responses/Responses.csproj", "Libs/Responses/"]
RUN dotnet restore "./Services/Notifications/WebApi/./WebApi.csproj"
COPY . .
WORKDIR "/src/Services/Notifications/WebApi"
RUN dotnet build "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]