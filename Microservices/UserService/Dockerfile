#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["UserService.csproj", "."]
COPY ["RabbitMQ/Rabbit.csproj", "RabbitMQ/"]
RUN dotnet restore "UserService.csproj"
RUN dotnet restore "RabbitMQ/Rabbit.csproj"

COPY . .

WORKDIR "/src/UserService"
RUN dotnet build "UserService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Build the RabbitMQ project
WORKDIR "/src/RabbitMQ"
RUN dotnet build "Rabbit.csproj" -c $BUILD_CONFIGURATION -o /app/build-rabbit

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UserService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserService.dll"]