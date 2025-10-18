#!/bin/bash

API_URL=${AUTOREST_API:-"https://localhost:7077"}
API_EMAIL=${AUTOREST_EMAIL:-"matheo@gmail.com"}
API_PASSWORD=${AUTOREST_PASSWORD:-"SenhaForte123!"}

mkdir -p swagger

echo "Trying to authenticate with API at $API_URL..."

# Perform login to get access token
LOGIN_RESPONSE=$(curl -s -k -X POST "$API_URL/api/Auth/login" \
    -H "Content-Type: application/json" \
    -d "{\"email\":\"$API_EMAIL\",\"password\":\"$API_PASSWORD\"}")

if [ $? -ne 0 ]; then
    echo "Warning: Could not authenticate with API. Trying to download swagger without authentication..."
    ACCESS_TOKEN=""
else
    # Extract access token from response
    ACCESS_TOKEN=$(echo "$LOGIN_RESPONSE" | grep -o '"accessToken":"[^"]*"' | sed 's/"accessToken":"\(.*\)"/\1/')
    
    if [ -n "$ACCESS_TOKEN" ]; then
        echo "Authentication successful!"
    else
        echo "Warning: Could not extract access token. Trying to download swagger without authentication..."
        ACCESS_TOKEN=""
    fi
fi

echo "Downloading swagger from $API_URL..."

# Download swagger with or without authentication header
if [ -n "$ACCESS_TOKEN" ]; then
    curl -s -k -f "$API_URL/swagger/v1/swagger.json" \
        -H "Authorization: Bearer $ACCESS_TOKEN" \
        -o swagger/api-spec.json
else
    curl -s -k -f "$API_URL/swagger/v1/swagger.json" -o swagger/api-spec.json
fi

if [ $? -ne 0 ]; then
    echo "Error downloading swagger from $API_URL"
    exit 1
fi

echo "Swagger downloaded successfully!"

# Fix security scheme for AutoRest compatibility (convert OpenAPI 3.0 http Bearer to apiKey format)
echo "Converting security scheme for AutoRest compatibility..."

# Use node to fix the swagger.json
node -e "
const fs = require('fs');
const swagger = JSON.parse(fs.readFileSync('swagger/api-spec.json', 'utf8'));

// Fix Bearer security scheme to be compatible with AutoRest
if (swagger.components && swagger.components.securitySchemes && swagger.components.securitySchemes.Bearer) {
    swagger.components.securitySchemes.Bearer = {
        type: 'apiKey',
        description: 'JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"',
        name: 'Authorization',
        in: 'header'
    };
}

// Add unique operationIds to all operations
if (swagger.paths) {
    Object.keys(swagger.paths).forEach(path => {
        const pathItem = swagger.paths[path];
        Object.keys(pathItem).forEach(method => {
            if (['get', 'post', 'put', 'delete', 'patch', 'options', 'head'].includes(method)) {
                const operation = pathItem[method];
                if (!operation.operationId) {
                    // Generate operationId from path and method
                    // e.g., /api/Auth/login -> AuthLogin, /api/Users/{id} -> UsersById
                    const segments = path.split('/').filter(s => s && s !== 'api');
                    let operationId = segments.map((seg, idx) => {
                        if (seg.startsWith('{')) {
                            return 'By' + seg.replace(/[{}]/g, '').charAt(0).toUpperCase() + 
                                   seg.replace(/[{}]/g, '').slice(1);
                        }
                        return seg.charAt(0).toUpperCase() + seg.slice(1);
                    }).join('');
                    
                    // Add method prefix for clarity
                    const methodPrefix = method.charAt(0).toUpperCase() + method.slice(1);
                    operation.operationId = operationId + methodPrefix;
                }
            }
        });
    });
}

fs.writeFileSync('swagger/api-spec.json', JSON.stringify(swagger, null, 2));
console.log('Security scheme converted successfully!');
"

if [ $? -ne 0 ]; then
    echo "Error converting swagger schema"
    exit 1
fi

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

echo "Client generated successfully!"

# Fix .js extensions in imports for TypeScript/Next.js compatibility
echo "Fixing import extensions for Next.js..."

node -e "
const fs = require('fs');
const path = require('path');

function fixImports(dir) {
  const files = fs.readdirSync(dir);
  
  files.forEach(file => {
    const fullPath = path.join(dir, file);
    const stat = fs.statSync(fullPath);
    
    if (stat.isDirectory()) {
      fixImports(fullPath);
    } else if (file.endsWith('.ts')) {
      let content = fs.readFileSync(fullPath, 'utf8');
      
      // Remove .js extensions from relative imports
      content = content.replace(/from ['\"](\.[^'\"]*?)\.js['\"]/g, 'from \'\$1\'');
      
      fs.writeFileSync(fullPath, content, 'utf8');
    }
  });
}

fixImports('src/api/src');
console.log('Import extensions fixed successfully!');
"

if [ $? -ne 0 ]; then
    echo "Warning: Could not fix import extensions automatically"
fi

echo "Client generated successfully in src/api/"
