# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official .NET SDK as a build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Dota.Auth.csproj", "Dota.Auth.csproj"]
RUN dotnet restore "Dota.Auth.csproj"
COPY . .
WORKDIR "."
RUN dotnet build "Dota.Auth.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "Dota.Auth.csproj" -c Release -o /app/publish

# Use the base image to run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dota.Auth.dll"]