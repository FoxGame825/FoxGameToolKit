tabtoy.exe --csharp_out=.\DataConfig.cs --binary_out=.\DataConfig.bytes --json_out=.\DataConfig.json --go_out=.\DataConfig.go --combinename=DataConfig --lan=zh_cn Buff.xlsx Skill.xlsx SkillEffect.xlsx Modifier.xlsx Unit.xlsx

@IF %ERRORLEVEL% NEQ 0 pause

: ±íË÷Òý
xcopy /y DataConfig.cs  .\output
xcopy /y DataConfig.json  .\output
xcopy /y DataConfig.go  .\output
xcopy /y DataConfig.bytes  .\output

del DataConfig.bytes DataConfig.cs DataConfig.go DataConfig.json DataConfig.proto
pause