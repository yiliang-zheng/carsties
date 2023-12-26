#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/AuctionService/WebApi/WebApi.csproj", "src/AuctionService/WebApi/"]
COPY ["src/AuctionService/Application/Application.csproj", "src/AuctionService/Application/"]
COPY ["src/Common/Shared.Constants/Shared.Constants.csproj", "src/Common/Shared.Constants/"]
COPY ["src/AuctionService/Domain/Domain.csproj", "src/AuctionService/Domain/"]
COPY ["src/Common/Shared.Domain/Shared.Domain.csproj", "src/Common/Shared.Domain/"]
COPY ["src/AuctionService/Infrastructure/Infrastructure.csproj", "src/AuctionService/Infrastructure/"]
RUN dotnet restore "./src/AuctionService/WebApi/./WebApi.csproj"
COPY . .
WORKDIR "/src/src/AuctionService/WebApi"
RUN dotnet build "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]