#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/HowToDevelop.HealthFood.WebApp/HowToDevelop.HealthFood.WebApp.csproj", "src/HowToDevelop.HealthFood.WebApp/"]
COPY ["src/HowToDevelop.HealthFood.Models/HowToDevelop.HealthFood.Dominio.csproj", "src/HowToDevelop.HealthFood.Models/"]
COPY ["src/HowToDevelop.Core/HowToDevelop.Core.csproj", "src/HowToDevelop.Core/"]
RUN dotnet restore "src/HowToDevelop.HealthFood.WebApp/HowToDevelop.HealthFood.WebApp.csproj"
COPY . .
WORKDIR "/src/src/HowToDevelop.HealthFood.WebApp"
RUN dotnet build "HowToDevelop.HealthFood.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HowToDevelop.HealthFood.WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HowToDevelop.HealthFood.WebApp.dll"]