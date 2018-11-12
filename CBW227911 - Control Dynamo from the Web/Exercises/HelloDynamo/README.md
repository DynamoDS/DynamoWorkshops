
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


# Exercise 2 -Extension snippets

**Implement an interface**

```cs
    public class ExtensionExample : IExtension
```


**Interface properties**

```cs
        public string UniqueId => "3B234622-43B7-4EA8-86DA-54FB390BE29E";

        public string Name => "Hello Dynamo Extension";
```


**Interface methods**

```cs
        public void Startup(StartupParams sp) {
            
        }

        public void Ready(ReadyParams rp)
        {
            
        }
```

And two more methods : 
```cs
        public void Shutdown()
        {

        }

        public void Dispose() { }
```


**Usings**

```cs
using Dynamo.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
```

** Message boxes **

```cs
MessageBox.Show("Extension is starting up!");
```


# Exercise 3 - View Extension snippets

**Implement Interface**

```cs
    public class ViewExtensionExample : IViewExtension
```

**Interface properties**

```cs
        public string UniqueId => "5E85F38F-0A19-4F24-9E18-96845764780C";
        public string Name => "Hello Dynamo View Extension";
```

**Interface methods**

```cs
        public void Startup(ViewStartupParams vsp)
        {

        }

        public void Loaded(ViewLoadedParams vlp)
        {
            MessageBox.Show("Hello there, viewExtension has loaded!");
        }

```

And two more methods, just like before : 
```cs
        public void Shutdown()
        {

        }

        public void Dispose() { }
```


**Usings**

```cs
using Dynamo.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
```

**Message boxes**

```cs
MessageBox.Show("Extension is starting up!");
```


# Exercise 4 - References to Dynamo objects, snippets

## Properties

```cs
        private ViewLoadedParams viewLoadedParams;
        private DynamoViewModel dynamoViewModel;
        private DynamoModel dynamoModel;
```

## Retain references

In `Loaded` method, add

**ViewLoadedParams**

```cs
            viewLoadedParams = vlp;
```

**DynamoViewModel**
```cs
dynamoViewModel = vlp.DynamoWindow.DataContext as DynamoViewModel;
```

**DynamoModel**

Let's use the DynamoViewModel reference we've already set.
```cs
dynamoModel = dynamoViewModel.Model as DynamoModel;
```

Now we can use these references in our `extension` or `view extension`.


# Exercise 5 - Menus

Let's add some custom `menus` to Dynamo's user interface using our `view extension`.

_Note : you can only do this from a `view extension` as `extensions` have no references to Dynamo UI._

## Property

Best to make a property for our menu, add this to the view extension class :

```cs
        private MenuItem extensionMenu;
```

## Method

We could do all the below in the `Loaded` method, but it's better to encapsulate the menu-making logic in a separate method.

Add this method to the class : 

```cs
        public void MakeMenuItems()
        {
            // do stuff here
        }
```

Let's now create a completely top-level new menu item
```cs
            extensionMenu = new MenuItem { Header = "AU Workshop" };
```

And now we make a new sub-menu item
```cs
            var sayHelloMenuItem = new MenuItem { Header = "Say Hello" };
```

Let's add behaviour to this submenu item so that it says hello when clicked
```cs
            sayHelloMenuItem.Click += (sender, args) =>
            {
                MessageBox.Show("Hello " + Environment.UserName);
            };
```

We add our submenu to the menu
```cs
            extensionMenu.Items.Add(sayHelloMenuItem);
```

And then we add the entire menu to Dynamo :
```cs
            viewLoadedParams.dynamoMenu.Items.Add(extensionMenu);
```

Notice we needed the `viewLoadedParams` reference to be able to do this.

Finally, call the menu-making method from the `Loaded` method :
```cs
            MakeMenuItems();
```


# Exercise 6 - Events snippets

## Event handler class

Make a new class called `Events` and make it `static`

```cs
    public static class Events
    {

    }
```

Then keep a reference to Dynamo inside this class, so add a `private` `static` property :

```cs
        private static ReadyParams DynamoReadyParams;
```

## Your first event handler

Let's make a new method that handles workspace changes, so it fires when you open a new Dynamo file.

Add this method :

```cs
        private static void OnCurrentWorkspaceChanged()
        {
        }

```

We need to know what kind of object `Dynamo` will send us when this fires, so peek it from the `Dynamo.Graph.Workspaces` class, you'll see we get a `Dynamo.Graph.Workspaces.IWorkspaceModel` object, so let's add that as a parameter to our method.

```cs
        private static void OnCurrentWorkspaceChanged(Dynamo.Graph.Workspaces.IWorkspaceModel obj)
```

Now we can use the object Dynamo gives to do something, so let's just pop open a `MessageBox` when it fires, add this to the method :
```cs
            MessageBox.Show($"Congratulations on opening the {obj.Name} workspace!");
```

## Node changes

Let's do the same for nodes, add this method : 
```cs
        private static void OnNodeAdded(NodeModel node)
        {
            MessageBox.Show();
        }
```

Let's populate the content with details about the node that was added, add this inside the `.Show(...)` method call.

```cs
$"You just added the {node.Name} node with Id {node.GUID} to the canvas."
```

We could do the same for node removals :
```cs
        private static void OnNodeRemoved(NodeModel node)
        {
            MessageBox.Show($"You just removed the {node.Name} node with Id {node.GUID} from the canvas.");
        }
```

## Link these events to Dynamo

To link them, we need to add our event handlers to Dynamo's events, let's make a method that does this.

Make a new method :
```cs
        public static void RegisterEventHandlers()
```

We somehow need to interact with Dynamo, so let's specify we need the Dynamo `ReadyParams` as input so we can keep hold of whatever references we need.

Your method is now :
```cs
        public static void RegisterEventHandlers(ReadyParams dynamoReadyParams
        {

        }
```

Keep a reference to these params so we can use them later :
```cs
            DynamoReadyParams = dynamoReadyParams;
```

And now let's link our events to Dynamo :

```cs
            dynamoReadyParams.CurrentWorkspaceChanged += OnCurrentWorkspaceChanged;
            dynamoReadyParams.CurrentWorkspaceModel.NodeAdded += OnNodeAdded;
            dynamoReadyParams.CurrentWorkspaceModel.NodeRemoved += OnNodeRemoved;
```

Good practice is to unlink our events from Dynamo when our extension shuts down, so let's make a method that will allow us to do that.

Notice this is just the reverse to adding our handlers.

```cs
        public static void UnregisterEventHandlers()
        {
            DynamoReadyParams.CurrentWorkspaceChanged -= OnCurrentWorkspaceChanged;
            DynamoReadyParams.CurrentWorkspaceModel.NodeAdded -= OnNodeAdded;
            DynamoReadyParams.CurrentWorkspaceModel.NodeRemoved -= OnNodeRemoved;
        }
```




## Register & deregister events

We have our handlers and ways to register them to Dynamo events now, but when does this get used ?

Well, let's use these methods when the extension starts up

in `ExtensionExample.cs` :

```cs
        public void Ready(ReadyParams rp)
        {
            // we can register our own events that will be triggered when specific things happen in Dynamo
            // a reference to the ReadyParams is needed to do this, so we pass it on
            Events.RegisterEventHandlers(rp);
        }
```

De-register them when the extension is shut down

```cs
        public void Shutdown()
        {
            Events.UnregisterEventHandlers();
        }
```

**Notice**
Because our class is `static`, we don't need to make an instance of it, so we don't need to keep a reference of an `events` object, simplifying how we handle events overall.


And that's it ! Your events are now linked and unlinked when your extension is loaded/unloaded by Dynamo.
