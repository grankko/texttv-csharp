#!/bin/bash

set -e

# Publish the application
dotnet publish src/TextTv.Cli/TextTv.Cli.csproj -c Release -r linux-x64 -p:PublishSingleFile=true -p:AssemblyName=texttv --self-contained -o publish

# Check if appsettings.json exists in the publish directory
if [ ! -f "publish/appsettings.json" ]; then
    # Check if source appsettings.json exists
    if [ -f "src/TextTv.Cli/appsettings.json" ]; then
        read -p "Found appsettings.json in source directory. Would you like to use these settings? (y/n): " choice
        case "$choice" in 
          y|Y ) 
            echo "Copying your existing settings file..."
            cp src/TextTv.Cli/appsettings.json publish/appsettings.json
            echo "Settings copied successfully."
            ;;
          * ) 
            echo "Using template settings instead..."
            cp src/TextTv.Cli/appsettings.example.json publish/appsettings.json
            echo "Created appsettings.json from template. Please update it with valid API key and settings for the application to function properly."
            ;;
        esac
    else
        echo "No appsettings.json found in source or publish directory. Creating one from example..."
        cp src/TextTv.Cli/appsettings.example.json publish/appsettings.json
        echo "Created appsettings.json from template. Please update it with valid API key and settings for the application to function properly."
    fi
else
    echo "Existing appsettings.json found in publish directory, keeping it intact."
fi