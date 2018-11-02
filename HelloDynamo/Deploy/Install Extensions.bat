del /q "C:\Program Files\Dynamo\Dynamo Core\2\extensions\HelloDynamo_ExtensionDefinition.xml"
del /q "C:\Program Files\Dynamo\Dynamo Core\2\viewExtensions\HelloDynamo_ViewExtensionDefinition.xml"
del /q "C:\Program Files\Dynamo\Dynamo Core\2\extensions\HelloDynamo.*"
del /q "C:\Program Files\Dynamo\Dynamo Core\2\viewExtensions\HelloDynamo.*"

xcopy /y /q "%~dp0HelloDynamo_ExtensionDefinition.xml" "C:\Program Files\Dynamo\Dynamo Core\2\extensions"
xcopy /y /q "%~dp0HelloDynamo_ViewExtensionDefinition.xml" "C:\Program Files\Dynamo\Dynamo Core\2\viewExtensions"

xcopy /y /q "%~dp0HelloDynamo.dll" "C:\Program Files\Dynamo\Dynamo Core\2\extensions"
xcopy /y /q "%~dp0HelloDynamo.dll" "C:\Program Files\Dynamo\Dynamo Core\2\viewExtensions"

pause
exit
