del /q "C:\Users\erudisaile\source\repos\Dynamo\bin\AnyCPU\Debug\extensions\HelloDynamo_ExtensionDefinition.xml"
del /q "C:\Users\erudisaile\source\repos\Dynamo\bin\AnyCPU\Debug\viewExtensions\HelloDynamo_ViewExtensionDefinition.xml"
del /q "C:\Users\erudisaile\source\repos\Dynamo\bin\AnyCPU\Debug\extensions\hellodynamo.*"
del /q "C:\Users\erudisaile\source\repos\Dynamo\bin\AnyCPU\Debug\viewExtensions\HelloDynamo.*"

xcopy /y /q "%~dp0HelloDynamo_ExtensionDefinition.xml" "C:\Users\erudisaile\source\repos\Dynamo\bin\AnyCPU\Debug\extensions"
xcopy /y /q "%~dp0HelloDynamo_ViewExtensionDefinition.xml" "C:\Users\erudisaile\source\repos\Dynamo\bin\AnyCPU\Debug\viewExtensions"

xcopy /y /q "%~dp0HelloDynamo.dll" "C:\Users\erudisaile\source\repos\Dynamo\bin\AnyCPU\Debug\extensions"
xcopy /y /q "%~dp0HelloDynamo.dll" "C:\Users\erudisaile\source\repos\Dynamo\bin\AnyCPU\Debug\viewExtensions"

pause
exit