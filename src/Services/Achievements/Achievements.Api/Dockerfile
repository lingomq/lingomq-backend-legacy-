FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Achievements/Achievements.Api/Achievements.Api.csproj", "Services/Achievements/Achievements.Api/"]
COPY ["Services/Achievements/Achievements.BusinessLayer/Achievements.BusinessLayer.csproj", "Services/Achievements/Achievements.BusinessLayer/"]
COPY ["Libs/Responses/Responses.csproj", "Libs/Responses/"]
COPY ["Services/EventBus/EventBus.csproj", "Services/EventBus/"]
COPY ["Services/Achievements/Achievements.DomainLayer/Achievements.DomainLayer.csproj", "Services/Achievements/Achievements.DomainLayer/"]
RUN dotnet restore "Services/Achievements/Achievements.Api/Achievements.Api.csproj"
COPY . .
WORKDIR "/src/Services/Achievements/Achievements.Api"
RUN dotnet build "Achievements.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Achievements.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Achievements.Api.dll"]