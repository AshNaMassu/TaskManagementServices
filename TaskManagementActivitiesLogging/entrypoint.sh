#!/bin/sh

set -e

echo "Applying EF Core migrations..."
dotnet ef database update --verbose

echo "Starting the application..."
dotnet API.dll