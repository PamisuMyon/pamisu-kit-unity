#!/bin/bash

LUBAN_DLL=./Luban/Luban.dll

dotnet $LUBAN_DLL \
    -t all \
    -c cs-bin \
    -d bin \
    --conf ./luban.conf \
    -x outputCodeDir=../LubanExampleUnity/Assets/Scripts/Game/Configs/Tables \
    -x outputDataDir=../LubanExampleUnity/Assets/Res/Configs/Tables
