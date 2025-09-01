# backend/TodoApi/Dockerfile
#################################################
# 1) Build stage - compiles & publishes app
#################################################
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj and restore first (layer caching)
COPY ["TodoApi.csproj", "./"]
RUN dotnet restore "./TodoApi.csproj"

# copy rest of the project
COPY . .

# publish the app into /app/publish (Release)
RUN dotnet publish "TodoApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

#################################################
# 2) Runtime stage - small runtime image to run app
#################################################
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# set the URL the app listens on
ENV ASPNETCORE_URLS=http://+:5000

# copy published files from build stage
COPY --from=build /app/publish .

# expose the listening port
EXPOSE 5000

# run the app
ENTRYPOINT ["dotnet", "TodoApi.dll"]
