#!/bin/bash

# Deleting current files in kOS.Safe.Core...
mkdir temp
cp -a "./kOS.Safe.Core/kOS.Safe.Core.csproj" "./temp/kOS.Safe.Core.csproj"
rm -rf "./kOS.Safe.Core/*.*"
cp -a "./temp/kOS.Safe.Core.csproj" "./kOS.Safe.Core/kOS.Safe.Core.csproj"
rm -rf "./temp" 

# Generating a new kOS.Safe.Core...
cp -a "./kOS/src/kOS.Safe/." "./kOS.Safe.Core/"
rm -rf "./kOS.Safe.Core/bin"
rm -rf "./kOS.Safe.Core/obj"
rm -rf "./kOS.Safe.Core/Properties/AssemblyInfo.cs"
rm -rf "./kOS.Safe.Core/kOS.Safe.csproj"