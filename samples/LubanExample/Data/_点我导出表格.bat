set LUBAN_DLL=.\Luban\Luban.dll

dotnet %LUBAN_DLL% ^
    --conf .\luban.conf ^
    -t client ^
    -c cs-bin ^
    -d bin ^
    -x outputCodeDir=..\LubanExampleUnity\Assets\Scripts\Game\Configs\Tables ^
    -x outputDataDir=..\LubanExampleUnity\Assets\Res\Configs\Tables

pause