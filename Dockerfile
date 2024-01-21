# # Multi-stage Dockerfile for UserService and FriendshipService

# # Build and publish stage for UserService
# FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS user_service_build_publish
# ARG BUILD_CONFIGURATION=Release
# WORKDIR /src

# COPY ["Microservices/UserService/UserService.csproj", "Microservices/UserService/"]
# COPY ["RabbitMQ/Rabbit.csproj", "RabbitMQ/"]
# RUN dotnet restore "RabbitMQ/Rabbit.csproj"
# RUN dotnet restore "Microservices/UserService/UserService.csproj"
# COPY . .
# RUN dotnet build "Microservices/UserService/UserService.csproj" -c $BUILD_CONFIGURATION -o /app/build
# RUN dotnet publish "Microservices/UserService/UserService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# # Build and publish stage for FriendshipService
# FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS friendship_service_build_publish
# ARG BUILD_CONFIGURATION=Release
# WORKDIR /src

# COPY ["Microservices/FriendshipService/FriendshipService.csproj", "Microservices/FriendshipService/"]
# COPY ["RabbitMQ/Rabbit.csproj", "RabbitMQ/"]
# RUN dotnet restore "Microservices/FriendshipService/FriendshipService.csproj"
# RUN dotnet restore "RabbitMQ/Rabbit.csproj"
# COPY . .
# RUN dotnet build "Microservices/FriendshipService/FriendshipService.csproj" -c $BUILD_CONFIGURATION -o /app/build
# RUN dotnet publish "Microservices/FriendshipService/FriendshipService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# # Runtime stage
# FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
# USER app
# WORKDIR /app
# EXPOSE 8080

# # Copy published artifacts from UserService and FriendshipService
# COPY --from=user_service_build_publish /app/publish ./UserService
# COPY --from=friendship_service_build_publish /app/publish ./FriendshipService

# # Install supervisor
# USER root
# RUN apt-get update && \
#     mkdir -p /var/lib/apt/lists/partial && \
#     chown -R app:app /var/lib/apt/lists/partial && \
#     apt-get install -y supervisor && \
#     apt-get clean && \
#     rm -rf /var/lib/apt/lists/*

# USER app

# # Create supervisor configuration
# COPY supervisord.conf /etc/supervisor/conf.d/supervisord.conf

# # Entry point for supervisord
# CMD ["/usr/bin/supervisord", "-c", "/etc/supervisor/conf.d/supervisord.conf"]



#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Dockerfile for USERSERVICE
#
## Build stage
 #FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
 #ARG BUILD_CONFIGURATION=Release
 #WORKDIR /src
#
 #COPY ["Microservices/UserService/UserService.csproj", "UserService/"]
 #COPY ["RabbitMQ/*.csproj", "RabbitMQ/"]
 #RUN dotnet restore "UserService/UserService.csproj"
 #RUN dotnet restore "RabbitMQ/Rabbit.csproj"
#
## COPY . .
#
## # Build UserService
 #RUN dotnet build "Microservices/UserService/UserService.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
## # Build RabbitMQ project
 #WORKDIR "/src/RabbitMQ"
 #RUN dotnet build "Rabbit.csproj" -c $BUILD_CONFIGURATION -o /app/build-rabbit
#
## # Publish stage
 #FROM build AS publish
 #ARG BUILD_CONFIGURATION=Release
 #WORKDIR "/src"
 #RUN dotnet publish "Microservices/UserService/UserService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
## # Final stage
 #FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
 #USER app
 #WORKDIR /app
 #EXPOSE 8080
#
## # Copy published artifacts from UserService
 #COPY --from=publish /app/publish .
#
## # Add a specific tag to differentiate between builds
 #ARG SERVICE_VERSION=latest
 #ENV SERVICE_VERSION=${SERVICE_VERSION}
#
## # Entry point for UserService
 #ENTRYPOINT ["dotnet", "UserService.dll"]




# FRIENDSHIP SERVICE
# Build stage
#FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#
#COPY ["Microservices/FriendshipService/FriendshipService.csproj", "FriendshipService/"]
#COPY ["RabbitMQ/*.csproj", "RabbitMQ/"]
#RUN dotnet restore "FriendshipService/FriendshipService.csproj"
#RUN dotnet restore "RabbitMQ/Rabbit.csproj"
#
#COPY . .
#
## Build FriendshipService
#RUN dotnet build "Microservices/FriendshipService/FriendshipService.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
## Build RabbitMQ project
#WORKDIR "/src/RabbitMQ"
#RUN dotnet build "Rabbit.csproj" -c $BUILD_CONFIGURATION -o /app/build-rabbit
#
## Publish stage
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#WORKDIR "/src"
#RUN dotnet publish "Microservices/FriendshipService/FriendshipService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
## Final stage
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
#USER app
#WORKDIR /app
#EXPOSE 8080
#
## Copy published artifacts from FriendshipService
#COPY --from=publish /app/publish .
#
## Add a specific tag to differentiate between builds
#ARG SERVICE_VERSION=latest
#ENV SERVICE_VERSION=${SERVICE_VERSION}
#
## Entry point for FriendshipService
#ENTRYPOINT ["dotnet", "FriendshipService.dll"]



#
#
## USER SERVICE DOCKERFILE
#
## Build stage
#FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#
#COPY ["Microservices/UserService/UserService.csproj", "UserService/"]
#COPY ["RabbitMQ/*.csproj", "RabbitMQ/"]
#RUN dotnet restore "UserService/UserService.csproj"
#RUN dotnet restore "RabbitMQ/Rabbit.csproj"
#
#COPY . .
#
## Build FriendshipService
#RUN dotnet build "Microservices/UserService/UserService.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
## Build RabbitMQ project
#WORKDIR "/src/RabbitMQ"
#RUN dotnet build "Rabbit.csproj" -c $BUILD_CONFIGURATION -o /app/build-rabbit
#
## Publish stage
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#WORKDIR "/src"
#RUN dotnet publish "Microservices/UserService/UserService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
## Final stage
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
#USER app
#WORKDIR /app
#EXPOSE 8080
#
## Copy published artifacts from FriendshipService
#COPY --from=publish /app/publish .
#
## Add a specific tag to differentiate between builds
#ARG SERVICE_VERSION=latest
#ENV SERVICE_VERSION=${SERVICE_VERSION}
#
## Entry point for FriendshipService
#ENTRYPOINT ["dotnet", "UserService.dll"]
#



## User DB
#
#FROM mcr.microsoft.com/mssql/server:2022-latest
#
## Set environment variables
#ENV SA_PASSWORD="abcDEF123" \
    #ACCEPT_EULA="Y"
#
## Copy your data into the image
#COPY ./Microservices/UserService/data /var/opt/mssql/data
#COPY ./Microservices/UserService/log /var/mssql/log
#COPY ./Microservices/UserService/secrets /var/opt/mssql/secrets


# Friendship DB

FROM mcr.microsoft.com/mssql/server:2022-latest

# Set environment variables
ENV SA_PASSWORD="test@123" \
    ACCEPT_EULA="Y"

# Copy your data into the image
COPY ./Microservices/FriendshipService/data /var/opt/mssql/data
COPY ./Microservices/FriendshipService/log /var/mssql/log
COPY ./Microservices/FriendshipService/secrets /var/opt/mssql/secret