FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

# Copy everything
COPY . ./
