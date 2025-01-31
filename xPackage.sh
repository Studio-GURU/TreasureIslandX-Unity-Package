#!/bin/bash
CY='\033[1;33m'
CR='\033[0;31m'
NC='\033[0m'
#------------------------------------------------------------------------------------------------------------------------------------------------------------------------#
# 1. 파라미터 확인
if [ -z "$1" ]; then
    echo "사용법: $0 <versionName>"
    exit 1
fi
# 2. 입력된 versionName 저장
versionName="$1"
#------------------------------------------------------------------------------------------------------------------------------------------------------------------------#
dotnet build ../../TreasureIsland.Addon.Unity.Source/Assembly-Script-Android.csproj --configuration Release
dotnet build ../../TreasureIsland.Addon.Unity.Source/Assembly-Script-iOS.csproj --configuration Release
# PLUGINS
cp -af ../../TreasureIsland.Addon.Unity.Source/Assets/Plugins .
cp -f ../../TreasureIsland.Addon.Unity.Source/Assets/Plugins.meta .
# SCRIPTS
mkdir -p Scripts/Android
mkdir -p Scripts/iOS
cp -f ../../TreasureIsland.Addon.Unity.Source/Assets/Scripts.meta .
cp -f ../../TreasureIsland.Addon.Unity.Source/Temp/bin/Release/TreasureIsland.Package.Android.dll Scripts/Android
cp -f ../../TreasureIsland.Addon.Unity.Source/Temp/bin/Release/TreasureIsland.Package.iOS.dll Scripts/iOS
#------------------------------------------------------------------------------------------------------------------------------------------------------------------------#
echo ">>> ... PACKAGE FILE CREATE >>>"
sed "s/\[#VERSION\]/$versionName/g" ../../TreasureIsland.Addon.Unity.Source/Packages/package.json > package.json
echo ">>> ... PACKAGE COMPLETE >>>"
#------------------------------------------------------------------------------------------------------------------------------------------------------------------------#