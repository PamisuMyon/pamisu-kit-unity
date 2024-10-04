#!/bin/bash

LUBAN_HELPER_DLL=./LubanHelper/LubanHelper.dll

dotnet $LUBAN_HELPER_DLL updateTables \
    --tablesPath ./Data/__tables__.xlsx \
    --dataPath .

pause