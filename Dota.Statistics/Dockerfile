FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY Common/Dota.Common/ Common/Dota.Common/
COPY Domain/Mongo.API/ Domain/Mongo.API/
COPY Domain/Mongo.Statistics/ Domain/Mongo.Statistics/
COPY Dota.Statistics/ Dota.Statistics/
RUN dotnet restore Dota.Statistics/Dota.Statistics.csproj
RUN dotnet build Dota.Statistics/Dota.Statistics.csproj -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "Dota.Statistics.dll"]
