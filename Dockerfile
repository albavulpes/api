FROM microsoft/powershell AS build-env
WORKDIR /app

# Copy everything
COPY . ./
