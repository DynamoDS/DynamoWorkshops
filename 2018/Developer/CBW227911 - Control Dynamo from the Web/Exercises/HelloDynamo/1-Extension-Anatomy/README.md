
# Exercise 1 - Anatomy snippets

## Manifest files


**HelloDynamo_ExtensionDefinition.xml**
```xml
<ExtensionDefinition>
  <AssemblyPath>..\bin\HelloDynamo.dll</AssemblyPath>
  <TypeName>HelloDynamo.ExtensionExample</TypeName>
</ExtensionDefinition>
```

**HelloDynamo_ViewExtensionDefinition.xml**
```xml
<ViewExtensionDefinition>
  <AssemblyPath>..\bin\HelloDynamo.dll</AssemblyPath>
  <TypeName>HelloDynamo.ViewExtensionExample</TypeName>
</ViewExtensionDefinition>
```

And if we want to make this into a package, add a `pkg.json` file to solution & paste :

```json
{
  "license": "",
  "file_hash": null,
  "name": "Hello Dynamo",
  "version": "0.0.1",
  "description": "Sample Extension",
  "group": "",
  "keywords": [],
  "dependencies": [],
  "contents": "",
  "engine_version": "2.0.1.5065",
  "engine": "dynamo",
  "engine_metadata": "",
  "site_url": "https://github.com/DynamoDS/DeveloperWorkshop",
  "repository_url": "https://github.com/DynamoDS/DeveloperWorkshop",
  "contains_binaries": true,
  "node_libraries": []
}
```

## Build events

### With post-build script

**Extension**
```shell
echo copying to Dynamo Extension folder
xcopy /y /q "$(TargetDir)*" "C:\Program Files\Dynamo\Dynamo Core\2\extensions"
xcopy /y /q "$(ProjectDir)Resources\*ExtensionDefinition.xml" "C:\Program Files\Dynamo\Dynamo Core\2\extensions"
```

**View Extension**
```shell
echo copying to Dynamo ViewExtensions folder
xcopy /y /q "$(TargetDir)*" "C:\Program Files\Dynamo\Dynamo Core\2\viewExtensions"
xcopy /y /q "$(ProjectDir)Resources\*ViewExtensionDefinition.xml" "C:\Program Files\Dynamo\Dynamo Core\2\viewExtensions
```

### With .csproj action

Add before `</Project>`
```xml
  <Target Name="AfterBuild">

  </Target>
```

Inside there, add some messages first : 
```xml
    <Message Importance="High" Text="++++++++++++++++++++++++++++++++++++++" />
    <Message Importance="High" Text="Started building the Dynamo extension" />
```

Define what we will want to copy
```xml
    <!--Defining folders to copy-->
    <ItemGroup>
      <SourceDlls Include="$(TargetDir)*.dll" />
      <SourcePdbs Include="$(TargetDir)*.pdb" />
      <SourcePkg Include="$(ProjectDir)pkg.json" />
      <SourceExtension Include="$(TargetDir)*ExtensionDefinition.xml" />
    </ItemGroup>
```

Assemble into a new folder
```xml
    <!--Copying to Build Folder-->
    <Copy SourceFiles="@(SourceDlls)" DestinationFolder="$(TargetDir)$(ProjectName)\bin\" />
    <Copy SourceFiles="@(SourcePdbs)" DestinationFolder="$(TargetDir)$(ProjectName)\bin\" />
    <Copy SourceFiles="@(SourcePkg)" DestinationFolder="$(TargetDir)$(ProjectName)" />
    <Copy SourceFiles="@(SourceExtension)" DestinationFolder="$(TargetDir)\$(ProjectName)\extra" />
    <Message Importance="High" Text="++++++++++++++++++++++++++++++++++++++" />
    <Message Importance="High" Text="Built to $(TargetDir)\$(ProjectName)" />
    <ItemGroup>
      <SourcePackage Include="$(TargetDir)\$(ProjectName)\**\*" />
    </ItemGroup>
```

Then deploy to packages folder
```xml
    <PropertyGroup>
      <!--Copy to Dynamo sandbox for testing -->
      <DeployFolder Condition="'$(Configuration)' == 'Debug'">$(AppData)\Dynamo\Dynamo Core\2.0\packages\$(ProjectName)</DeployFolder>
      <!--Copy to Dynamo revit for publishing -->
      <DeployFolder Condition="'$(Configuration)' == 'Release' Or '$(Configuration)' == 'DebugRevit'">$(AppData)\Dynamo\Dynamo Revit\2.0\packages\$(ProjectName)</DeployFolder>
    </PropertyGroup>
    <Copy SourceFiles="@(SourcePackage)" DestinationFolder="$(DeployFolder)\%(RecursiveDir)" />
    <Message Importance="High" Text="++++++++++++++++++++++++++++++++++++++" />
    <Message Importance="High" Text="Deployed to $(DeployFolder)" />
```

Remember, all this is inside `<Target> .... </Target>`

## Start action

Find all `<PropertyGroup>...</PropertyGroup>` that have this condition or similar (anything with `Debug`.)

```xml
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
```

Then add before `</PropertyGroup>` :
```xml
    <StartAction>Program</StartAction>
    <StartProgram>C:\Program Files\Dynamo\Dynamo Core\2\DynamoSandbox.exe</StartProgram>
```
