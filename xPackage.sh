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
cp -af ../../TreasureIsland.Addon.Unity.Source/Assets/Scripts .
cp -af ../../TreasureIsland.Addon.Unity.Source/Assets/Plugins .
cp -f ../../TreasureIsland.Addon.Unity.Source/Assets/Scripts.meta .
cp -f ../../TreasureIsland.Addon.Unity.Source/Assets/Plugins.meta .
#------------------------------------------------------------------------------------------------------------------------------------------------------------------------#
echo ">>> ... PACKAGE FILE CREATE >>>"
sed "s/\[#VERSION\]/$versionName/g" ../../TreasureIsland.Addon.Unity.Source/Packages/package.json > package.json
echo ">>> ... PACKAGE COMPLETE >>>"
#------------------------------------------------------------------------------------------------------------------------------------------------------------------------#
#------------------------------------------------------------------------------------------------------------------------------------------------------------------------#
echo ">>> ... 배포 버전 정보 확인"
# 초기값 설정
inputFile="package.json"
if [ ! -f "$inputFile" ]; then
    echo "오류: $inputFile 파일이 존재하지 않습니다."
    exit 1
fi
# 2. jq(추천) 또는 grep을 사용하여 version 값 추출
if command -v jq &> /dev/null; then
    version=$(jq -r '.version' "$inputFile")
else
    version=$(grep -o '"version": *"[^"]*"' "$inputFile" | awk -F '"' '{print $4}')
fi
# 3. 결과 출력
if [ -n "$version" ]; then
    echo ">>> ... 🔹추출된 버전: $version"
else
    echo ">>> ... 오류: 버전 정보를 찾을 수 없습니다."
    exit 1
fi
#------------------------------------------------------------------------------------------------------------------------------------------------------------------------#
echo "${CY}>>> Git Push > Verion${NC} ${CR}($version)${NC} ${CY}<<<${NC}"
propt1="${CY}>>> Do you want to proceed ?${NC} ..... ${CR}[y/n]   ${NC}"
read -e -p "$(echo ${propt1})" isProceed
if [ $isProceed != "y" ]; then
    echo "${CR}>>> task stopped!${NC}"
    exit 0
fi
#--------------------------------------------------#
echo ">>> git pull..."
git pull
#--------------------------------------------------#
echo ">>> git add..."
git add .
#--------------------------------------------------#
echo ">>> git commit... 'TreasureIsland Unity Package $version Release'"
git commit -m "TreasureIsland Unity Package $version Release"
#--------------------------------------------------#
echo ">>> git push..."
git push
#--------------------------------------------------#
echo ">>> git tagging... (version: $version)"
git tag -a "$version" -m "Release $version"
git push origin "$version"
#--------------------------------------------------#
echo ">>> complete version: $version"
#--------------------------------------------------#