del /q "C:\Program Files\Dynamo\Dynamo Core\2\viewExtensions\RapidFire_ViewExtensionDefinition.xml"
del /q "C:\Program Files\Dynamo\Dynamo Core\2\viewExtensions\RapidFire.*"

xcopy /y /q "%~dp0RapidFire_ViewExtensionDefinition.xml" "C:\Program Files\Dynamo\Dynamo Core\2\viewExtensions"
xcopy /y /q "%~dp0RapidFire.dll" "C:\Program Files\Dynamo\Dynamo Core\2\viewExtensions"

pause
exit
