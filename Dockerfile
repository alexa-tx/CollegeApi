# Базовый образ для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем csproj и восстанавливаем зависимости
COPY *.csproj ./
RUN dotnet restore

# Копируем остальные файлы и публикуем
COPY . ./
RUN dotnet publish -c Release -o /app

# Финальный минимальный образ
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

# Запускаем приложение
ENTRYPOINT ["dotnet", "CollegeApi.dll"]
