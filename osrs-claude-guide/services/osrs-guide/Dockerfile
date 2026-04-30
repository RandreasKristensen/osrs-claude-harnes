# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["services/osrs-guide/src/OsrsGuide.Web/OsrsGuide.Web.csproj", "OsrsGuide.Web/"]
RUN dotnet restore "OsrsGuide.Web/OsrsGuide.Web.csproj"

# Copy the remaining source code
COPY ["services/osrs-guide/src/OsrsGuide.Web/", "OsrsGuide.Web/"]
WORKDIR "/src/OsrsGuide.Web"
RUN dotnet build "OsrsGuide.Web.csproj" -c Release -o /app/build

# Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS publish
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "OsrsGuide.Web.dll"]
EXPOSE 8080
