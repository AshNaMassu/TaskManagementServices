#!/bin/sh

set -e

echo "Applying EF Core migrations..."
dotnet ef database update --verbose --context TaskManagement.Persistence.TaskManagementDbContext --assembly TaskManagement.API

echo "Starting the application..."
dotnet API.dll