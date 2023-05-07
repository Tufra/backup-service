#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Backup Management Service/Backup Management Service.csproj", "Backup Management Service/"]
RUN dotnet restore "Backup Management Service/Backup Management Service.csproj"
COPY . .
WORKDIR "/src/Backup Management Service"
RUN dotnet build "Backup Management Service.csproj" -c Development -o /app/build

FROM build AS publish
RUN dotnet publish "Backup Management Service.csproj" -c Development -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Backup Management Service.dll"]