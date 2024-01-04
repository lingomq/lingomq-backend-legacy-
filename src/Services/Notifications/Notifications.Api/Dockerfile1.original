#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Notifications/Notifications.Api/Notifications.Api/Api.csproj", "Services/Notifications/Notifications.Api/Notifications.Api/"]
COPY ["Services/Notifications/DataAccess/Notifications.DataAccess.Dapper/DataAccess.Dapper.csproj", "Services/Notifications/DataAccess/Notifications.DataAccess.Dapper/"]
COPY ["Services/Notifications/Notifications.Domain/Domain.csproj", "Services/Notifications/Notifications.Domain/"]
COPY ["Libs/Responses/Responses.csproj", "Libs/Responses/"]
COPY ["Services/Notifications/Notifications.Application/Application.csproj", "Services/Notifications/Notifications.Application/"]
COPY ["Services/EventBus/EventBus.csproj", "Services/EventBus/"]
RUN dotnet restore "./Services/Notifications/Notifications.Api/Notifications.Api/./Api.csproj"
COPY . .
WORKDIR "/src/Services/Notifications/Notifications.Api/Notifications.Api"
RUN dotnet build "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]