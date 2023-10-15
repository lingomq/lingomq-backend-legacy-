FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Topics/Topics.Api/Topics.Api.csproj", "Services/Topics/Topics.Api/"]
COPY ["Services/Topics/Topics.BusinessLayer/Topics.BusinessLayer.csproj", "Services/Topics/Topics.BusinessLayer/"]
COPY ["Libs/Responses/Responses.csproj", "Libs/Responses/"]
COPY ["Services/EventBus/EventBus.csproj", "Services/EventBus/"]
COPY ["Services/Topics/Topics.DomainLayer/Topics.DomainLayer.csproj", "Services/Topics/Topics.DomainLayer/"]
RUN dotnet restore "Services/Topics/Topics.Api/Topics.Api.csproj"
COPY . .
WORKDIR "/src/Services/Topics/Topics.Api"
RUN dotnet build "Topics.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Topics.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Topics.Api.dll"]