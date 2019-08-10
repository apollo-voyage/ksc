#!/bin/bash

cp -a "./kOS/src/kOS.Safe/." "./kOS.Safe.Core/"
rm -rf "./kOS.Safe.Core/bin"
rm -rf "./kOS.Safe.Core/obj"
rm -rf "./kOS.Safe.Core/Properties/AssemblyInfo.cs"
rm -rf "./kOS.Safe.Core/kOS.Safe.csproj"