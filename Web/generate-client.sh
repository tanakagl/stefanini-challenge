#!/bin/bash

API_URL=${AUTOREST_API:-"https://localhost:7077"}

mkdir -p swagger

echo Trying to download swagger from $API_URL, check if the API is running...

curl -s -f "$API_URL/swagger/v1/swagger.json" -o swagger/api-spec.json

if [ $? -ne 0 ]; then
    echo "Error downloading swagger from $API_URL"
    exit 1
fi

echo "Swagger downloaded successfully!"

if ! command -v autorest &> /dev/null; then
    echo "Installing autorest..."
    npm install -g autorest
fi

echo "Generating TypeScript client..."

autorest \
    --input-file=swagger/api-spec.json \
    --typescript \
    --output-folder=src/api \
    --add-credentials \
    --override-client-name=ApiClient

if [ $? -ne 0 ]; then
    echo "Error generating client"
    exit 1
fi

echo "Client generated successfully in src/api/"
