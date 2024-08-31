# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Final_API/Final_API.csproj", "Final_API/"]
COPY ["Final_Business/Final_Business.csproj", "Final_Business/"]
COPY ["Final_Data/Final_Data.csproj", "Final_Data/"]
COPY ["Final_Core/Final_Core.csproj", "Final_Core/"]
RUN dotnet restore "./Final_API/./Final_API.csproj"
COPY . .
WORKDIR "/src/Final_API"
RUN dotnet build "./Final_API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish image
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Final_API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Copy the certificate into the container
COPY certs/ayazumman.pfx /https/ayazumman.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/ayazumman.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=ayazumman
ENTRYPOINT ["dotnet", "Final_API.dll"]
