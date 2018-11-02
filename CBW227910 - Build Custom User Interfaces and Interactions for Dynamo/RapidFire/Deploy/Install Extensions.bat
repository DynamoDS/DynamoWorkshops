rem del /q "C:\Program Files\Dynamo\Dynamo Core\2\extensions\RapidFire_ExtensionDefinition.xml"
del /q "C:\Program Files\Dynamo\Dynamo Core\2\viewExtensions\RapidFire_ViewExtensionDefinition.xml"
rem del /q "C:\Program Files\Dynamo\Dynamo Core\2\extensions\RapidFire.*"
del /q "C:\Program Files\Dynamo\Dynamo Core\2\viewExtensions\RapidFire.*"

rem xcopy /y /q "%~dp0RapidFire_ExtensionDefinition.xml" "C:\Program Files\Dynamo\Dynamo Core\2\extensions"
xcopy /y /q "%~dp0RapidFire_ViewExtensionDefinition.xml" "C:\Program Files\Dynamo\Dynamo Core\2\viewExtensions"
rem xcopy /y /q "%~dp0RapidFire.dll" "C:\Program Files\Dynamo\Dynamo Core\2\extensions"
xcopy /y /q "%~dp0RapidFire.dll" "C:\Program Files\Dynamo\Dynamo Core\2\viewExtensions"

pause
exit
