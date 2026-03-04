FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src
COPY ["src/Armdl.Integration.Service/Armdl.Integration.Service.csproj", "Armdl.Integration.Service/"]
COPY ["src/Armdl.Integration.Authentication/Armdl.Integration.Authentication.csproj", "Armdl.Integration.Authentication/"]

RUN dotnet restore "Armdl.Integration.Service/Armdl.Integration.Service.csproj"
RUN dotnet restore "Armdl.Integration.Authentication/Armdl.Integration.Authentication.csproj"

COPY src/ .

RUN dotnet publish "Armdl.Integration.Service/Armdl.Integration.Service.csproj" -c Release -o /app --no-restore
RUN dotnet publish "Armdl.Integration.Authentication/Armdl.Integration.Authentication.csproj" -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/runtime-deps:10.0 AS final

WORKDIR /app
COPY --from=build /app .

ENTRYPOINT touch 1.txt && tail -f 1.txt
