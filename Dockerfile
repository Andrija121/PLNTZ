# Multi-stage Dockerfile for UserService and FriendshipService

# Build and publish stage for UserService
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS user_service_build_publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Microservices/UserService/UserService.csproj", "Microservices/UserService/"]
COPY ["RabbitMQ/Rabbit.csproj", "RabbitMQ/"]
RUN dotnet restore "RabbitMQ/Rabbit.csproj"
RUN dotnet restore "Microservices/UserService/UserService.csproj"
COPY . .
RUN dotnet build "Microservices/UserService/UserService.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "Microservices/UserService/UserService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Build and publish stage for FriendshipService
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS friendship_service_build_publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Microservices/FriendshipService/FriendshipService.csproj", "Microservices/FriendshipService/"]
COPY ["RabbitMQ/Rabbit.csproj", "RabbitMQ/"]
RUN dotnet restore "Microservices/FriendshipService/FriendshipService.csproj"
RUN dotnet restore "RabbitMQ/Rabbit.csproj"

# Copy the rest of the solution
COPY . .
RUN dotnet build "Microservices/FriendshipService/FriendshipService.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "Microservices/FriendshipService/FriendshipService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
USER app
WORKDIR /app
EXPOSE 8080

# Copy published artifacts from UserService and FriendshipService
COPY --from=user_service_build_publish /app/publish ./UserService
COPY --from=friendship_service_build_publish /app/publish ./FriendshipService

# Entry point for UserService
CMD ["echo", "No specific command specified"]
