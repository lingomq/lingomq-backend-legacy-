FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Notifications/Notifications.Api/Notifications.Api.csproj", "Services/Notifications/Notifications.Api/"]
COPY ["Services/Notifications/Notifications.BusinessLayer/Notifications.BusinessLayer.csproj", "Services/Notifications/Notifications.BusinessLayer/"]
COPY ["Libs/Responses/Responses.csproj", "Libs/Responses/"]
COPY ["Services/EventBus/EventBus.csproj", "Services/EventBus/"]
COPY ["Services/Notifications/Notifications.DomainLayer/Notifications.DomainLayer.csproj", "Services/Notifications/Notifications.DomainLayer/"]
RUN dotnet restore "Services/Notifications/Notifications.Api/Notifications.Api.csproj"
COPY . .
WORKDIR "/src/Services/Notifications/Notifications.Api"
RUN dotnet build "Notifications.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Notifications.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Notifications.Api.dll"]