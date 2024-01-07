# Common build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the CSPROJ files
COPY src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done
COPY Directory.Build.props Directory.Build.props

# Restore dependencies for Presentation.Api
RUN dotnet restore "src/Presentation.Api/Presentation.Api.csproj"

# Restore dependencies for Consumer.Service
RUN dotnet restore "src/Consumer.Service/Consumer.Service.csproj"

# Copy the project files
COPY . .

# Build and publish Presentation.Api
FROM build-env AS publish-api
RUN dotnet publish "src/Presentation.Api/Presentation.Api.csproj" -c Release -o out

# Build and publish Consumer.Service
FROM build-env AS publish-service
RUN dotnet publish "src/Consumer.Service/Consumer.Service.csproj" -c Release -o out

# Runtime image for Presentation.Api
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime-api
WORKDIR /app
COPY --from=publish-api /app/out .
ENV ASPNETCORE_URLS=http://+:9882
ENTRYPOINT ["dotnet", "Presentation.Api.dll"]

# Runtime image for Consumer.Service
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime-service
WORKDIR /app
COPY --from=publish-service /app/out .
ENTRYPOINT ["dotnet", "Consumer.Service.dll"]