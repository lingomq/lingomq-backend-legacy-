FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ApiGateway/OcelotApiGateway/OcelotApiGateway.csproj", "ApiGateway/OcelotApiGateway/"]
RUN dotnet restore "ApiGateway/OcelotApiGateway/OcelotApiGateway.csproj"
COPY . .
WORKDIR "/src/ApiGateway/OcelotApiGateway"
RUN dotnet build "OcelotApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OcelotApiGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        curl \
        ca-certificates \
        gnupg \
        wget \
    && curl -fsSL https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.asc.gpg \
    && mv microsoft.asc.gpg /etc/apt/trusted.gpg.d/ \
    && wget -q https://packages.microsoft.com/config/debian/10/prod.list \
    && mv prod.list /etc/apt/sources.list.d/microsoft-prod.list \
    && chown root:root /etc/apt/trusted.gpg.d/microsoft.asc.gpg \
    && chown root:root /etc/apt/sources.list.d/microsoft-prod.list \
    && apt-get update \
    && apt-get install -y --no-install-recommends \
        dotnet-sdk-5.0

# Create a Docker volume for the dev-certs directory
RUN mkdir /root/.dotnet-ssl
VOLUME /root/.dotnet-ssl

# Generate a self-signed development certificate
RUN dotnet dev-certs https --clean && dotnet dev-certs https -ep /root/.dotnet-ssl/aspnetapp.pfx -p mypassword
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=mypassword
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/root/.dotnet-ssl/aspnetapp.pfx

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OcelotApiGateway.dll"]