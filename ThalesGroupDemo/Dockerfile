Windows dockerfile proper one

FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 
WORKDIR /App
COPY --from=build /App/out .
# Add this line to copy the PowerShell script
# COPY start.ps1 .
ENV DOTNET_RUNNING_IN_CONTAINER=true
## executing exe file through entrypoint
ENTRYPOINT ["WeatherServiceDemo.exe"]