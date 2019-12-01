#!/bin/bash

bash ./gen-kos-safe-core.sh
cd ./kOS.Cli
dotnet publish -r win-x64 -c Release -o ../win-x64 /p:UseAppHost=true /p:PublishSingleFile=true /p:PublishTrimmed=true
dotnet publish -r win-x86 -c Release -o ../win-x86 /p:UseAppHost=true /p:PublishSingleFile=true /p:PublishTrimmed=true
dotnet publish -r linux-x64 -c Release -o ../linux-x64 /p:UseAppHost=true /p:PublishSingleFile=true /p:PublishTrimmed=true
dotnet publish -r osx-x64 -c Release -o ../osx-x64 /p:UseAppHost=true /p:PublishSingleFile=true /p:PublishTrimmed=true