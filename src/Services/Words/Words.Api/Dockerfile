FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Words/Words.Api/Words.Api.csproj", "Services/Words/Words.Api/"]
COPY ["Services/Words/Words.BusinessLayer/Words.BusinessLayer.csproj", "Services/Words/Words.BusinessLayer/"]
COPY ["Libs/Responses/Responses.csproj", "Libs/Responses/"]
COPY ["Services/EventBus/EventBus.csproj", "Services/EventBus/"]
COPY ["Services/Words/Words.DomainLayer/Words.DomainLayer.csproj", "Services/Words/Words.DomainLayer/"]
RUN dotnet restore "Services/Words/Words.Api/Words.Api.csproj"
COPY . .
WORKDIR "/src/Services/Words/Words.Api"
RUN dotnet build "Words.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Words.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Words.Api.dll"]