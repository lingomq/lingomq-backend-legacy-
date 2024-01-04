FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Finances/Finances.Api/Finances.Api.csproj", "Services/Finances/Finances.Api/"]
COPY ["Services/Finances/Finances.BusinessLayer/Finances.BusinessLayer.csproj", "Services/Finances/Finances.BusinessLayer/"]
COPY ["Libs/Responses/Responses.csproj", "Libs/Responses/"]
COPY ["Services/EventBus/EventBus.csproj", "Services/EventBus/"]
COPY ["Services/Finances/Finances.DomainLayer/Finances.DomainLayer.csproj", "Services/Finances/Finances.DomainLayer/"]
RUN dotnet restore "Services/Finances/Finances.Api/Finances.Api.csproj"
COPY . .
WORKDIR "/src/Services/Finances/Finances.Api"
RUN dotnet build "Finances.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Finances.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Finances.Api.dll"]