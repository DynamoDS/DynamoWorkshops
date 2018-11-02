del /q "C:\Program Files\Dynamo\Dynamo Core\2\viewExtensions\Unfancify_ViewExtensionDefinition.xml"
del /q "C:\Program Files\Dynamo\Dynamo Core\2\viewExtensions\Unfancify.*"

xcopy /y /q "%~dp0Unfancify_ViewExtensionDefinition.xml" "C:\Program Files\Dynamo\Dynamo Core\2\viewExtensions"

xcopy /y /q "%~dp0Unfancify.dll" "C:\Program Files\Dynamo\Dynamo Core\2\viewExtensions"

pause
exit
