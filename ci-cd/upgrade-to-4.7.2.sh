#!/bin/bash

cp "$(dirname $0)/upgrade-files/kOS.Safe.csproj" "$(dirname $0)/../kOS/src/kOS.Safe/kOS.Safe.csproj"
cp "$(dirname $0)/upgrade-files/packages.config" "$(dirname $0)/../kOS/src/kOS.Safe/packages.config"
cp "$(dirname $0)/upgrade-files/Resources.Designer.cs" "$(dirname $0)/../kOS/src/kOS.Safe/Properties/Resources.Designer.cs"