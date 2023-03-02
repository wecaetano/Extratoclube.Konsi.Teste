#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Extratoclube.Konsi.Api/Extratoclube.Konsi.Api.csproj", "Extratoclube.Konsi.Api/"]
RUN dotnet restore "Extratoclube.Konsi.Api/Extratoclube.Konsi.Api.csproj"
COPY . .
WORKDIR "/src/Extratoclube.Konsi.Api"
RUN dotnet build "Extratoclube.Konsi.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Extratoclube.Konsi.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENV SELENIUM_GRID_URL=http://selenium:4444/wd/hub
ENTRYPOINT ["dotnet", "Extratoclube.Konsi.Api.dll"]