# Exercise 1 - Basic extensions in Dynamo

## Make an extension

Add `ExtensionExample.cs` file


** Implement an interface **

```cs
    public class ExtensionExample : IExtension
```

** Usings **

```cs
using Dynamo.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
```

** Properties **

```cs
        private ViewLoadedParams viewLoadedParams;
```

** References **

```cs
        private DynamoViewModel dynamoViewModel => this.viewLoadedParams.DynamoWindow.DataContext as DynamoViewModel;
```
(aka The ViewModel hack)

** Message boxes **

```cs
MessageBox.Show("Extension is starting up!");
```

## Make a menu item

Let's add some custom `menus` to Dynamo's user interface using our `view extension`.

_Note : you can only do this from a `view extension` as `extensions` have no references to Dynamo UI._

Best to make a property for our menu, add this to the view extension class :

```cs
        private MenuItem extensionMenu;
```

We could do all the below in the `Loaded` method, but it's better to encapsulate the menu-making logic in a separate method.

Add this method to the class : 

```cs
        public void MakeMenuItems()
        {
            // do stuff here
        }
```

Let's now create a completely new top-level new menu item
```cs
            this.extensionMenu = new MenuItem { Header = "DynamoDev Workshop" };
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

Also add a more fun Hack menu
```cs
            var hackMenuItem = new MenuItem { Header = "Hack !" };
            hackMenuItem.Click += (sender, args) =>
            {
                Process.Start("https://giphy.com/gifs/siliconvalleyhbo-2fQ1Gq3KOpvNs4NTmu");
            };
```

We add our submenu to the menu
```cs
            this.extensionMenu.Items.Add(sayHelloMenuItem);
            this.extensionMenu.Items.Add(hackMenuItem);
```

And then we add the entire menu to Dynamo :
```cs
            this.viewLoadedParams.dynamoMenu.Items.Add(this.extensionMenu);
```

Notice we needed the `viewLoadedParams` reference to be able to do this.

Finally, call the menu-making method from the `Loaded` method :
```cs
            MakeMenuItems();
```

## Respond to events

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

### Your first event handler

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

### Handle node changes

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

### Link these events to Dynamo

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

### Register & deregister events

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

# Exercise 2 - The ViewModel

## Create a base ViewModel class and hook it up to Dynamo
Create a class called `InputViewModel` that inherits from `NotificationObject`.

To make our ViewModel useful, let's add some properties to it

```cs
        public string InputNodesNames { get; set; }
        public string NodeCount { get; set; }
        public string WireCount { get; set; }
```

We need to initialise these data fields, so add a constructor and initialise with default/empty values.

```cs
            this.InputNodes = new Dictionary<string, string>();
            this.NodeCount = 0.ToString();
            this.WireCount = 0.ToString();
```

This data will be coming from Dynamo, so we need to pass in information from our extension to this ViewModel. To do this, add the same `ReadyParams` our extension receives to the constructor.

Our ViewModel will be reacting to Dynamo events, so let's make some event handlers :

```cs
        private void OnNodeCountChange(NodeModel obj)
        {
            // code goes here
        }


        private void OnWireCountChange(Dynamo.Graph.Connectors.ConnectorModel obj)
        {
            // code goes here
        }

        private void OnWorkspaceChange(Dynamo.Graph.Workspaces.IWorkspaceModel obj)
        {
            // code goes here
        }
```

We now of course need to link these event handlers to the events. 
Link the first one :
```cs
this.readyParams.CurrentWorkspaceModel.NodeAdded += OnNodeCountChange;
```
And then the rest
```cs
            this.readyParams.CurrentWorkspaceModel.NodeAdded += OnNodeCountChange;
            this.readyParams.CurrentWorkspaceModel.NodeRemoved += OnNodeCountChange;
            this.readyParams.CurrentWorkspaceModel.ConnectorAdded += OnWireCountChange;
            this.readyParams.CurrentWorkspaceModel.ConnectorDeleted += OnWireCountChange;
            this.readyParams.CurrentWorkspaceChanged += OnWorkspaceChange;
            this.readyParams.CurrentWorkspaceCleared += OnWorkspaceChange;
```
When you register to events, also remember to un-register.
Add this to the `Dispose` method
```cs
            this.readyParams.CurrentWorkspaceModel.NodeAdded -= OnNodeCountChange;
            this.readyParams.CurrentWorkspaceModel.NodeRemoved -= OnNodeCountChange;
            this.readyParams.CurrentWorkspaceModel.ConnectorAdded += OnWireCountChange;
            this.readyParams.CurrentWorkspaceModel.ConnectorDeleted += OnWireCountChange;
            this.readyParams.CurrentWorkspaceChanged += OnWorkspaceChange;
```

## Hook it up to Dynamo

Let's add a method to the ViewExtension called `MakeAndShowInputWindow`

In it, we first make a new `InputViewModel` :
```cs
            vm = new InputsViewModel(this.viewLoadedParams);
```

Then we display its current status in a message box :
```cs
            MessageBox.Show(
                $"There are {vm.NodeCount} total nodes & {vm.WireCount} wires," + Environment.NewLine +
                $"{vm.InputNodes.Count} of which are marked as inputs : " + Environment.NewLine +
                String.Join(Environment.NewLine, vm.InputNodesNames)
                );

            // Associate ViewModel with Window
```

## Keep track of Dynamo stats

Our stats will only ever show 0 at the minute, we now need to actually do something, so let's add a method called `Recompute` where we can keep track of Dynamo stats every time something changes.

Let's record the number of nodes on canvas first
```cs
            this.NodeCount = this.readyParams.CurrentWorkspaceModel.Nodes
                .Count()
                .ToString();
```

And then the number of wires (connectors)
```cs
            this.WireCount = this.readyParams.CurrentWorkspaceModel.Connectors
                .Count()
                .ToString();
```

To keep track of the nodes that are inputs, we need two steps : gather them and extract their names into a single list of nodes.
Gather input nodes
```cs
            var nodes = this.readyParams.CurrentWorkspaceModel.Nodes
                .Where(x => x.IsSetAsInput)
                .ToList();
```
Record names
```cs
            this.InputNodesNames = String.Join(Environment.NewLine,nodes.Select(x => x.Name));
```

Try it out !

We also want to display the input nodes in a table, with the node name and its current value, so let's add a dictionary to hold that data :

```cs
        public Dictionary<string, string> InputNodes { get; set; }
```

Then in `Recompute` method, after collecting all input nodes, add each of them to the dictionary, taking note of node name & current value (if any)

```cs
            foreach (var node in nodes)
            {
                var value = node.CachedValue?.Data?.ToString() ?? "N/A";
                if (!this.InputNodes.ContainsKey(node.Name)) this.InputNodes.Add(node.Name, value);
            }
```

Finally, make sure we let the ViewModel know we've changed some values, add this to the end of `Recompute.`
```cs
            RaisePropertyChanged(
                nameof(this.NodeCount),
                nameof(this.WireCount),
                nameof(this.InputNodes)
                );
```

## Trigger recomputes

To update our data when something in Dynamo changes, call our recompute method from the event handlers
```cs
            Recompute();
```


## Reset values

Our counters might not work correctly, so we will need to add a method to reset them for each event. Move the lines below from constructor to a new method.
```cs
        private void ResetStats()
        {
            this.InputNodes = new Dictionary<string, string>();
            this.NodeCount = 0.ToString();
            this.WireCount = 0.ToString();
        }
```
Then call this method at the start of `Recompute`.

# Exercise 3 - UI

NOTE
> Windows has a filepath length limit which causes NuGet restore to fail when the source code is placed in a folder structure that is too deep. We've avoided the usual structure in this exercise to avoid causing this issue.

## Make a Stats window

Add a new `WPF Window` to the `Input` folder, called `InputsWindow.xaml`.

Update the XAML definition so its title & width are more appropriate
```xml
            xmlns:local="clr-namespace:DynamoDev.Stats"
             mc:Ignorable="d"
            Width="250" Title="Graph Inputs" SizeToContent="Height" BorderThickness="0" Background="White" Height="300">
```

Then add a `StackPanel` with a 2x2 grid :

```xml
    <StackPanel Name="MainGrid" 
          HorizontalAlignment="Stretch"
          VerticalAlignment="Stretch">
        <Grid>
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="1*" />
                <ColumnDefinition Width="1*" />
            </Grid.ColumnDefinitions>
            <Grid.RowDefinitions>
                <RowDefinition Height="2*" />
                <RowDefinition Height="2*" />
                <RowDefinition Height="2*" />
                <RowDefinition Height="2*" />
            </Grid.RowDefinitions>
        </Grid>
    </StackPanel>
```

Once we have the grid, we can add two `TextBlock`s to it :
```xml
            <TextBlock Grid.Row="0" Grid.Column="0" Margin="0,0,0,0" Padding="15" TextWrapping="Wrap" TextAlignment="Center" Text="Nodes" FontFamily="Arial" FontWeight="Bold" FontSize="14" Background="#8d9ca9" Foreground="#f2f3f4" VerticalAlignment="Center"/>
            <TextBlock Grid.Row="1" Grid.Column="0" Margin="0,0,0,0" Text="{Binding NodeCount}" Padding="10" TextAlignment="Center" FontFamily="Arial" FontWeight="Bold" FontSize="20"  Background="White" Foreground="#13344e"/>
            <TextBlock Grid.Row="0" Grid.Column="1" Margin="0,0,0,0" Padding="15" TextWrapping="Wrap" TextAlignment="Center" Text="Wires" FontFamily="Arial" FontWeight="Bold" FontSize="14" Background="#8d9ca9" Foreground="#f2f3f4" VerticalAlignment="Center"/>
            <TextBlock Grid.Row="1" Grid.Column="1" Margin="0,0,0,0" Text="{Binding WireCount}" Padding="10" TextAlignment="Center" FontFamily="Arial" FontWeight="Bold" FontSize="20"  Background="White" Foreground="#13344e"/>
```

Pay close attention the the active text fields, which are bound to our `InputsViewModel` :
```cs
Text="{Binding WireCount}"
```

## Trigger InputsWindow from menu

Now let's add a submenu that will trigger this window, back in the ViewExtension's menu-making method.

```cs
            // and a submenu item that shows the window
            var inputsMenuItem = new MenuItem { Header = "Show inputs stats" };
```

We need to handle the click event, so add an anonymous method to do so :

```
            inputsMenuItem.Click += (sender, args) =>
            {

            };
```

Then make a new method called `MakeAndShowInputWindow`, where we initialise a new `InputsWindow` and tie it to our `InputsViewModel` and `DynamoWindow`.

```cs
        private void MakeAndShowInputWindow()
        {
            var viewModel = new InputsViewModel(this.viewLoadedParams);
            var window = new InputsWindow
            {
                // Set the data context for the main grid in the window.
                MainGrid = { DataContext = viewModel },

                // Set the owner of the window to the Dynamo window.
                Owner = this.viewLoadedParams.DynamoWindow
            };
            window.Left = window.Owner.Left + 400;
            window.Top = window.Owner.Top + 200;
            window.Show();
        }

```

Finally, call this method in the click handler and add this new submenu item to the menu:

```cs
            extensionMenu.Items.Add(inputsMenuItem);
```

## Display input nodes

Add a new text block so we separate the 2 areas of our window :
```xml
        <TextBlock Margin="0,0,0,0" Padding="5" TextWrapping="Wrap" TextAlignment="Center" Text="Input Nodes" FontFamily="Arial" FontWeight="Bold" FontSize="14" Background="#8d9ca9" Foreground="#f2f3f4" VerticalAlignment="Center"/>
```

(alternative with a grid :)
```xml
        <Grid>
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="1*" />
            </Grid.ColumnDefinitions>
            <Grid.RowDefinitions>
                <RowDefinition Height="2*" />
            </Grid.RowDefinitions>
            <TextBlock Grid.Row="0" Grid.Column="0" Margin="0,0,0,0" Padding="5" TextWrapping="Wrap" TextAlignment="Center" Text="Input Nodes" FontFamily="Arial" FontWeight="Bold" FontSize="14" Background="#8d9ca9" Foreground="#f2f3f4" VerticalAlignment="Center"/>
        </Grid>
```

Then add a data grid at the bottom, before the `StackPanel` closes
```xml
        <DataGrid Grid.Row="1" 
                  ItemsSource="{Binding InputNodes}" 
                  Grid.ColumnSpan="2" Margin="0,0,0.333,-0.333" 
                  AutoGenerateColumns="False" 
                  IsReadOnly="True">
            <DataGrid.Columns>
                <DataGridTextColumn Binding="{Binding Key}" Header="Name" MinWidth="100"/>
                <DataGridTextColumn Binding="{Binding Value}" Header="Value" MinWidth="100"/>
            </DataGrid.Columns>
        </DataGrid>
```

Notice we bound the data source of this `DataGrid` to our dictionary and its columns to the `Key` and `Values` of the dictionary.

You're now ready to rumble !

## Next steps

The current code has several limitations, think about how you could overcome these :

- changing a node to `Is Input` does not trigger the stats window to update
- we could dock the whole window to the sidepanel of Dynamo
- the `DataGrid` could be editable and update the values