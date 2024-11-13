# Imagen base para ASP.NET en Linux
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Imagen SDK para construir el proyecto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el archivo .csproj desde su ubicación
# Nota: Este asume que el Dockerfile está en la raíz del proyecto
COPY Dashboard-Backend.csproj ./ 

# Restaurar dependencias para todos los proyectos
RUN dotnet restore "Dashboard-Backend.csproj"

# Copiar todo el código fuente
COPY . .

# Establecer directorio de trabajo y compilar la aplicación
WORKDIR "/src"
RUN dotnet build "Dashboard-Backend.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "Dashboard-Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagen final para ejecutar la aplicación
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dashboard-Backend.dll"]
