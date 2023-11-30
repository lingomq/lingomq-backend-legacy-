@echo off

dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p password && dotnet dev-certs https --trust
docker-compose -f .\docker-compose.windows.dev.yaml --verbose up --build
