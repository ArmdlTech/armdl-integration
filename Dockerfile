# Первый этап - установка SDK для .NET Core 3.1
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build31

# Копируем исходники
WORKDIR /src
COPY ["src/Armdl.Integration.Service/Armdl.Integration.Service.csproj", "./"]
COPY ["src/Armdl.Integration.Authentication/Armdl.Integration.Authentication.csproj", "./"]

# Восстанавливаем зависимости обоих проектов
RUN dotnet restore Armdl.Integration.Service.csproj
RUN dotnet publish Armdl.Integration.Service.csproj -c Release -o out --no-restore


FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build50
WORKDIR /src
COPY ["src/Armdl.Integration.Service/Armdl.Integration.Service.csproj", "./"]
COPY ["src/Armdl.Integration.Authentication/Armdl.Integration.Authentication.csproj", "./"]

RUN dotnet restore Armdl.Integration.Authentication.csproj
RUN dotnet publish Armdl.Integration.Authentication.csproj -c Release -o out --no-restore

# Переход на финальный слой для публикации собранных артефактов
FROM mcr.microsoft.com/dotnet/runtime-deps:5.0 AS final

WORKDIR /app
COPY --from=build31 /src/out .
COPY --from=build50 /src/out .

ENTRYPOINT touch 1.txt && tail -f 1.txt
