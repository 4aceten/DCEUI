#!/bin/bash

# Navigate to the parent directory
cd ..

echo "Starting publishing process with single-file binaries for all platforms..."

# Common Publish Settings for Single Binary
PUBLISH_OPTIONS="/p:PublishSingleFile=true \
                 /p:IncludeNativeLibrariesForSelfExtract=false \
                 /p:PublishTrimmed=true \
                 /p:TrimMode=Link \
                 /p:UseAppHost=true \
                 --self-contained true"

# Publish for macOS (Intel and Apple Silicon)
echo "Publishing for macOS (Intel and Apple Silicon)..."
dotnet publish -c Release -r osx-x64 $PUBLISH_OPTIONS
dotnet publish -c Release -r osx-arm64 $PUBLISH_OPTIONS

# Publish for Linux (x64 and arm64)
echo "Publishing for Linux (x64 and arm64)..."
dotnet publish -c Release -r linux-x64 $PUBLISH_OPTIONS
dotnet publish -c Release -r linux-arm64 $PUBLISH_OPTIONS

# Publish for Windows (x64 and arm64)
echo "Publishing for Windows (x64 and arm64)..."
dotnet publish -c Release -r win-x64 $PUBLISH_OPTIONS
dotnet publish -c Release -r win-arm64 $PUBLISH_OPTIONS

echo "Publishing completed for all platforms with single binaries."
