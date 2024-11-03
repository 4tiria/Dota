FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY Common/Dota.Common/ Common/Dota.Common/
COPY Domain/NoSql/ Domain/NoSql/
COPY Dota.API/ Dota.API/

RUN dotnet restore Dota.API/Dota.API.csproj
RUN dotnet build Dota.API/Dota.API.csproj -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app
COPY --from=build /app/build .

ENTRYPOINT ["dotnet", "Dota.API.dll"]
