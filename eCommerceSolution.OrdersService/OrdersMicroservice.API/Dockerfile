#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OrdersMicroservice.API/OrdersMicroservice.API.csproj", "OrdersMicroservice.API/"]
RUN dotnet restore "./OrdersMicroservice.API/OrdersMicroservice.API.csproj"
COPY . .
WORKDIR "/src/OrdersMicroservice.API"
RUN dotnet build "./OrdersMicroservice.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OrdersMicroservice.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV MONGODB_HOST=localhost
ENV MONGODB_PORT=27017
ENV MONGODB_DATABASE=OrdersDatabase
ENV UsersMicroserviceName=localhost
ENV UsersMicroservicePort=9090
ENV ProductsMicroserviceName=localhost
ENV ProductsMicroservicePort=8080
ENTRYPOINT ["dotnet", "OrdersMicroservice.API.dll"]