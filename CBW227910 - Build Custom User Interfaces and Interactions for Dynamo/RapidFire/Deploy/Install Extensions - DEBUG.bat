rem del /q "C:\Users\erudisaile\source\repos\Dynamo\bin\AnyCPU\Debug\extensions\RapidFire_ExtensionDefinition.xml"
del /q "C:\Users\erudisaile\source\repos\Dynamo\bin\AnyCPU\Debug\viewExtensions\RapidFire_ViewExtensionDefinition.xml"
rem del /q "C:\Users\erudisaile\source\repos\Dynamo\bin\AnyCPU\Debug\extensions\RapidFire.*"
del /q "C:\Users\erudisaile\source\repos\Dynamo\bin\AnyCPU\Debug\viewExtensions\RapidFire.*"

rem xcopy /y /q "%~dp0RapidFire_ExtensionDefinition.xml" "C:\Users\erudisaile\source\repos\Dynamo\bin\AnyCPU\Debug\extensions"
xcopy /y /q "%~dp0RapidFire_ViewExtensionDefinition.xml" "C:\Users\erudisaile\source\repos\Dynamo\bin\AnyCPU\Debug\viewExtensions"

rem xcopy /y /q "%~dp0RapidFire.dll" "C:\Users\erudisaile\source\repos\Dynamo\bin\AnyCPU\Debug\extensions"
xcopy /y /q "%~dp0RapidFire.dll" "C:\Users\erudisaile\source\repos\Dynamo\bin\AnyCPU\Debug\viewExtensions"

pause
exit