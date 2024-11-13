# Imagen base para ASP.NET en Linux
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Imagen SDK para construir el proyecto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el archivo .csproj desde la carpeta correcta
COPY Dashboard-Backend/Dashboard-Backend.csproj Dashboard-Backend/

# Restaurar dependencias
WORKDIR /src/Dashboard-Backend
RUN dotnet restore

# Copiar todo el código fuente desde el contexto de construcción
COPY . .

# Establecer directorio de trabajo y compilar la aplicación
RUN dotnet build "Dashboard-Backend.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "Dashboard-Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagen final para ejecutar la aplicación
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dashboard-Backend.dll"]
