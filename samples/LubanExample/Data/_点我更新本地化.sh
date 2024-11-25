#!/bin/bash

LUBAN_HELPER_DLL=./LubanHelper/LubanHelper.dll

dotnet $LUBAN_HELPER_DLL updateL10N \
    --l10nPath ./本地化.xlsx \
    --dataPath . \
    --noteColumnSuffix _note \
    --textIdColumnSuffix _text_id \
    --l10nStartId 20001

pause