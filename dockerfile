# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los archivos .csproj y restaurar dependencias
COPY ["Lab10-GonzaloDiaz.Api/Lab10-GonzaloDiaz.Api.csproj", "Lab10-GonzaloDiaz.Api/"]
COPY ["Lab10-GonzaloDiaz.Application/Lab10-GonzaloDiaz.Application.csproj", "Lab10-GonzaloDiaz.Application/"]
COPY ["Lab10-GonzaloDiaz.Domain/Lab10-GonzaloDiaz.Domain.csproj", "Lab10-GonzaloDiaz.Domain/"]
COPY ["Lab10-GonzaloDiaz.Infraestructure/Lab10-GonzaloDiaz.Infraestructure.csproj", "Lab10-GonzaloDiaz.Infraestructure/"]
RUN dotnet restore "Lab10-GonzaloDiaz.Api/Lab10-GonzaloDiaz.Api.csproj"

# Copiar el resto del código y publicar
COPY . .
WORKDIR "/src/Lab10-GonzaloDiaz.Api"
RUN dotnet publish "Lab10-GonzaloDiaz.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final (ejecución)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render asigna el puerto dinámicamente (por defecto suele usar 8080 o 10000)
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Lab10-GonzaloDiaz.Api.dll"]