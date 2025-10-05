# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all .csproj files and restore dependencies
COPY ["Sciforn.AssetManagement.UI/Sciforn.AssetManagement.UI.csproj", "Sciforn.AssetManagement.UI/"]
COPY ["Sciforn.AssetManagement.BusinessLogic/Sciforn.AssetManagement.BusinessLogic.csproj", "Sciforn.AssetManagement.BusinessLogic/"]
COPY ["Sciforn.AssetManagement.DataAccess/Sciforn.AssetManagement.DataAccess.csproj", "Sciforn.AssetManagement.DataAccess/"]
COPY ["Sciforn.AssetManagement.Core/Sciforn.AssetManagement.Core.csproj", "Sciforn.AssetManagement.Core/"]
RUN dotnet restore "Sciforn.AssetManagement.UI/Sciforn.AssetManagement.UI.csproj"

# Copy the rest of the source code
COPY . .
WORKDIR "/src/Sciforn.AssetManagement.UI"
RUN dotnet build "Sciforn.AssetManagement.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sciforn.AssetManagement.UI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sciforn.AssetManagement.UI.dll"]