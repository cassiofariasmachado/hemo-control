FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY src/HemoControl/HemoControl.csproj ./src/HemoControl/
COPY test/HemoControl.Test/HemoControl.Test.csproj ./test/HemoControl.Test/
COPY HemoControl.sln .

RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
WORKDIR /app
COPY --from=build /app/out .

# Heroku specific config for run aspnet webapi
CMD ASPNETCORE_URLS=http://*:$PORT dotnet HemoControl.dll