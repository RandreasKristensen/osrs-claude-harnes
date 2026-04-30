# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["services/monitor/src/Monitor.Web/Monitor.Web.csproj", "Monitor.Web/"]
RUN dotnet restore "Monitor.Web/Monitor.Web.csproj"

# Copy the remaining source code
COPY ["services/monitor/src/Monitor.Web/", "Monitor.Web/"]
WORKDIR "/src/Monitor.Web"
RUN dotnet build "Monitor.Web.csproj" -c Release -o /app/build

# Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS publish
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "Monitor.Web.dll"]
EXPOSE 8080
