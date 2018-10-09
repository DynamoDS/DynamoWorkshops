del /q "C:\Program Files\Dynamo\Dynamo Core\1.3\extensions\HelloDynamo_ExtensionDefinition.xml"
del /q "C:\Program Files\Dynamo\Dynamo Core\1.3\viewExtensions\HelloDynamo_ViewExtensionDefinition.xml"
del /q "C:\Program Files\Dynamo\Dynamo Core\1.3\extensions\HelloDynamo.*"
del /q "C:\Program Files\Dynamo\Dynamo Core\1.3\viewExtensions\HelloDynamo.*"

xcopy /y /q "%~dp0HelloDynamo_ExtensionDefinition.xml" "C:\Program Files\Dynamo\Dynamo Core\1.3\extensions"
xcopy /y /q "%~dp0HelloDynamo_ViewExtensionDefinition.xml" "C:\Program Files\Dynamo\Dynamo Core\1.3\viewExtensions"

xcopy /y /q "%~dp0HelloDynamo.dll" "C:\Program Files\Dynamo\Dynamo Core\1.3\extensions"
xcopy /y /q "%~dp0HelloDynamo.dll" "C:\Program Files\Dynamo\Dynamo Core\1.3\viewExtensions"

pause
exit
