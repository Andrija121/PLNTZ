# Multi-stage Dockerfile for UserService and FriendshipService

# Build and publish stage for UserService
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS user_service_build_publish
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
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS friendship_service_build_publish
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
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
USER app
WORKDIR /app
EXPOSE 8080

# Copy published artifacts from UserService and FriendshipService
COPY --from=user_service_build_publish /app/publish ./UserService
COPY --from=friendship_service_build_publish /app/publish ./FriendshipService

# Entry point for UserService
CMD ["echo", "No specific command specified"]


#
## Multi-stage Dockerfile for UserService and FriendshipService
#
## Build stage
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#
## Copy the entire solution
#COPY . .
#
## Restore dependencies and build UserService
#WORKDIR "/src/Microservices/UserService"
#RUN dotnet restore
#RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build-user
#RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish-user /p:UseAppHost=false
#
## Restore dependencies and build FriendshipService
#WORKDIR "/src/Microservices/FriendshipService"
#RUN dotnet restore
#RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build-friendship
#RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish-friendship /p:UseAppHost=false
#
## Runtime stage
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
#USER app
#WORKDIR /app
#EXPOSE 8080
#
## Copy published artifacts from UserService and FriendshipService
#COPY --from=build /app/publish-user ./UserService
#COPY --from=build /app/publish-friendship ./FriendshipService
#
## Entry point for UserService (you can customize this as needed)
#CMD ["echo", "No specific command specified"]
