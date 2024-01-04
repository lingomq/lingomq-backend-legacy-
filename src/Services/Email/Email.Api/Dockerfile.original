FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/Email/Email.Api/Email.Api.csproj", "Services/Email/Email.Api/"]
COPY ["Services/Email/Email.BusinessLayer/Email.BusinessLayer.csproj", "Services/Email/Email.BusinessLayer/"]
COPY ["Services/EventBus/EventBus.csproj", "Services/EventBus/"]
RUN dotnet restore "Services/Email/Email.Api/Email.Api.csproj"
COPY . .
WORKDIR "/src/Services/Email/Email.Api"
RUN dotnet build "Email.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Email.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Email.Api.dll"]