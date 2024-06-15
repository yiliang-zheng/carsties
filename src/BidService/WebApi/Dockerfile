#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

#copy all .csproj files and restore as distince layers. 
#Use of the same COPY command for every dockerfile to take advantage of docker caching
COPY ["Carsties.sln", "Carsties.sln"]
#Auction service
COPY src/AuctionService/WebApi/WebApi.csproj src/AuctionService/WebApi/
COPY ["src/AuctionService/Application/Application.csproj", "src/AuctionService/Application/"]
COPY ["src/AuctionService/Infrastructure/Infrastructure.csproj", "src/AuctionService/Infrastructure/"]
COPY ["src/AuctionService/Domain/Domain.csproj", "src/AuctionService/Domain/"]
#Common
COPY ["src/Common/Shared.Constants/Shared.Constants.csproj", "src/Common/Shared.Constants/"]
COPY ["src/Common/Shared.Domain/Shared.Domain.csproj", "src/Common/Shared.Domain/"]
COPY ["src/Common/Shared.Extensions/Shared.Extensions.csproj", "src/Common/Shared.Extensions"]
#Gateway
COPY ["src/GatewayService/Web/Web.csproj", "src/GatewayService/Web/"]
#Identity Server 
COPY ["src/IdentityService/Web/Web.csproj", "src/IdentityService/Web/"]
COPY ["src/IdentityService/Application/Application.csproj", "src/IdentityService/Application/"]
COPY ["src/IdentityService/Infrastructure/Infrastructure.csproj", "src/IdentityService/Infrastructure/"]
COPY ["src/IdentityService/Domain/Domain.csproj", "src/IdentityService/Domain/"]
#Search Service
COPY ["src/SearchService/WebApi/WebApi.csproj", "src/SearchService/WebApi/"]
#State machine service
COPY ["src/StateMachine/SagaOrchestration/SagaOrchestration.csproj", "src/StateMachine/SagaOrchestration/"]
#Bid Service
COPY ["src/BidService/Application/Application.csproj", "src/BidService/Application/"]
COPY ["src/BidService/Infrastructure/Infrastructure.csproj", "src/BidService/Infrastructure/"]
COPY ["src/BidService/Domain/Domain.csproj", "src/BidService/Domain/"]
COPY ["src/BidService/WebApi/WebApi.csproj", "src/BidService/WebApi/"]

#Restore package deps
RUN dotnet restore "Carsties.sln"

#Copy the app folders
COPY ["src/BidService/Application", "src/BidService/Application"]
COPY ["src/BidService/Infrastructure", "src/BidService/Infrastructure"]
COPY ["src/BidService/Domain", "src/BidService/Domain"]
COPY ["src/BidService/WebApi", "src/BidService/WebApi"]
COPY ["src/Common/Shared.Constants", "src/Common/Shared.Constants"]
COPY ["src/Common/Shared.Domain", "src/Common/Shared.Domain"]
COPY ["src/Common/Shared.Extensions", "src/Common/Shared.Extensions"]
WORKDIR "/src/src/BidService/WebApi"
RUN dotnet build "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish --no-build /p:UseAppHost=false /p:OutDir=/app/build

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]