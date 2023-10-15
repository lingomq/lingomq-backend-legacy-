FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Identity/Identity.Api/Identity.Api.csproj", "Services/Identity/Identity.Api/"]
COPY ["Libs/Cryptography/Cryptography.csproj", "Libs/Cryptography/"]
COPY ["Services/Identity/Identity.BusinessLayer/Identity.BusinessLayer.csproj", "Services/Identity/Identity.BusinessLayer/"]
COPY ["Libs/Responses/Responses.csproj", "Libs/Responses/"]
COPY ["Services/EventBus/EventBus.csproj", "Services/EventBus/"]
COPY ["Services/Identity/Identity.DomainLayer/Identity.DomainLayer.csproj", "Services/Identity/Identity.DomainLayer/"]
RUN dotnet restore "Services/Identity/Identity.Api/Identity.Api.csproj"
COPY . .
WORKDIR "/src/Services/Identity/Identity.Api"
RUN dotnet build "Identity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.Api.dll"]