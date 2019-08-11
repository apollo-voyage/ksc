#!/bin/bash

bash ./gen-kos-safe-core.sh
dotnet publish -r win-x64 -c Release -o ../win-x64
dotnet publish -r win-x86 -c Release -o ../win-x86
dotnet publish -r linux-x64 -c Release -o ../linux-x64
dotnet publish -r osx-x64 -c Release -o ../osx-x64