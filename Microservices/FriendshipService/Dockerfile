# This assumes the directory structure where FriendshipService.csproj and RabbitMQ.csproj are in separate folders
# FriendshipService Dockerfile

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the solution file and restore dependencies for both projects
COPY ["FriendshipService.csproj", "."]
COPY ["RabbitMQ/Rabbit.csproj", "RabbitMQ/"]
RUN dotnet restore "FriendshipService/FriendshipService.csproj"
RUN dotnet restore "RabbitMQ/Rabbit.csproj"

# Copy the rest of the solution
COPY . .

# Build the FriendshipService project
WORKDIR "/src/FriendshipService"
RUN dotnet build "FriendshipService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Build the RabbitMQ project
WORKDIR "/src/RabbitMQ"
RUN dotnet build "Rabbit.csproj" -c $BUILD_CONFIGURATION -o /app/build-rabbit

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src/FriendshipService"
RUN dotnet publish "FriendshipService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FriendshipService.dll"]