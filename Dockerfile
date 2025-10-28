# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all .csproj files and restore dependencies
COPY ["AssetManagement.UI/AssetManagement.UI.csproj", "AssetManagement.UI/"]
COPY ["AssetManagement.BusinessLogic/AssetManagement.BusinessLogic.csproj", "AssetManagement.BusinessLogic/"]
COPY ["AssetManagement.DataAccess/AssetManagement.DataAccess.csproj", "AssetManagement.DataAccess/"]
COPY ["AssetManagement.Core/AssetManagement.Core.csproj", "AssetManagement.Core/"]
RUN dotnet restore "AssetManagement.UI/AssetManagement.UI.csproj"

# Copy the rest of the source code
COPY . .
WORKDIR "/src/AssetManagement.UI"
RUN dotnet build "AssetManagement.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AssetManagement.UI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AssetManagement.UI.dll"]