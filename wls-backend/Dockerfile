FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY *.csproj .
RUN dotnet restore

# copy everything else and build app
COPY . .
RUN dotnet publish

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./

ENV TZ=Europe/Vienna
RUN ln -sf /usr/share/zoneinfo/Europe/Vienna /etc/localtime

ENTRYPOINT ["dotnet", "bin/Release/net8.0/wls-backend.dll"]
