# Exercise 1 - Basic extensions in Dynamo




# Exercise 2 - 



# Exercise 3 - UI

NOTE
> Windows has a filepath length limit which causes NuGet restore to fail when the source code is placed in a folder structure that is too deep. We've avoided the usual structure in this exercise to avoid causing this issue.

## Make an Input ViewModel 

Make a new class called `InputsViewModel` that reponds to changes :
```cs
public class InputsViewModel : NotificationObject
```

Add these private fields :

```cs
        private ViewLoadedParams readyParams;
        private string activeNodeCount;
        private string activeWireCount;
```

Now we can expose some graph stats in public properties :

```cs
        //Displays the count of the nodes in the workspace
        public string ActiveNodeCount => readyParams.CurrentWorkspaceModel.Nodes.Count().ToString();

        //Displays the count of the wires in the workspace
        public string ActiveWireCount => readyParams.CurrentWorkspaceModel.Connectors.Count().ToString();
```

We tie this to Dynamo data through its constructor :

```cs
        public InputsViewModel(ReadyParams p)
        {
            readyParams = p as ViewLoadedParams;
            readyParams.CurrentWorkspaceModel.NodeAdded += OnNodeCountChange;
            readyParams.CurrentWorkspaceModel.NodeRemoved += OnNodeCountChange;
            readyParams.CurrentWorkspaceModel.ConnectorAdded += OnWireCountChange;
            readyParams.CurrentWorkspaceModel.ConnectorDeleted += OnWireCountChange;
            readyParams.CurrentWorkspaceModel.NodeAdded += OnNodesUpdate;
            readyParams.CurrentWorkspaceModel.NodeRemoved += OnNodesUpdate;
            readyParams.CurrentWorkspaceChanged += OnWorkspaceChange;
        }
```

Remember to de-register events in the `Dispose` method

```cs
        public void Dispose()
        {
            readyParams.CurrentWorkspaceModel.NodeAdded -= OnNodeCountChange;
            readyParams.CurrentWorkspaceModel.NodeRemoved -= OnNodeCountChange;
            readyParams.CurrentWorkspaceModel.ConnectorAdded += OnWireCountChange;
            readyParams.CurrentWorkspaceModel.ConnectorDeleted += OnWireCountChange;
            readyParams.CurrentWorkspaceModel.NodeAdded += OnNodesUpdate;
            readyParams.CurrentWorkspaceModel.NodeRemoved += OnNodesUpdate;
            readyParams.CurrentWorkspaceChanged += OnWorkspaceChange;
        }
```

All that's left is to make sure our properties respond to changes in Dynamo data, so make Dynamo events trigger an update to our UI through these event handlers :

```cs
        private void OnNodeCountChange(NodeModel obj)
        {
            RaisePropertyChanged("NodeCount");
        }

        private void OnWireCountChange(Dynamo.Graph.Connectors.ConnectorModel obj)
        {
            RaisePropertyChanged("WireCount");
        }

        private void OnNodesUpdate(NodeModel obj)
        {
            RaisePropertyChanged("NodeCount");
        }

        private void OnWorkspaceChange(Dynamo.Graph.Workspaces.IWorkspaceModel obj)
        {
            RaisePropertyChanged("NodeCount", "WireCount");
        }
```

## Make a Stats window

Make a new `Stats` folder in project, then add a new `WPF Window` called `InputsWindow.xaml`.

Update the XAML definition so its title & width are more appropriate
```xml
             mc:Ignorable="d" d:DesignWidth="300"
            Width="180" Title="Graph Totals" SizeToContent="Height" BorderThickness="0" Background="White">
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
            <TextBlock Grid.Row="0" Grid.Column="0" Margin="0,0,0,0" Padding="5" TextWrapping="Wrap" TextAlignment="Center" Text="Node Count" FontFamily="Arial" FontWeight="Bold" FontSize="14" Background="#8d9ca9" Foreground="#f2f3f4" VerticalAlignment="Center"/>
            <TextBlock Grid.Row="0" Grid.Column="1" Margin="0,0,0,0" Padding="5" TextWrapping="Wrap" TextAlignment="Center" Text="Connector Count" FontFamily="Arial" FontWeight="Bold" FontSize="14" Background="#8d9ca9" Foreground="#f2f3f4" VerticalAlignment="Center"/>
            <TextBlock Grid.Row="1" Grid.Column="0" Margin="0,0,0,0" Text="{Binding ActiveNodeCount}" Padding="10" TextAlignment="Center" FontFamily="Arial" FontWeight="Bold" FontSize="20"  Background="White" Foreground="#13344e"/>
            <TextBlock Grid.Row="1" Grid.Column="1" Margin="0,0,0,0" Text="{Binding ActiveWireCount}" Padding="10" TextAlignment="Center" FontFamily="Arial" FontWeight="Bold" FontSize="20" Background="White" Foreground="#13344e"/>
        </Grid>
    </StackPanel>
```

Pay close attention the the active text fields, which are bound to our `InputsViewModel` :
```cs
Text="{Binding WireCount}"
```

## Trigger InputsWindow from menu

Now let's add a submenu that will trigger this window.

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

Inside it, initialise a `InputsViewModel`

```cs
                var viewModel = new InputsViewModel(viewLoadedParams);
```

Then make a new `InputsWindow` and tie it to our `InputsViewModel` and `DynamoWindow`.

```cs
                var window = new InputsWindow
                {
                    // Set the data context for the main grid in the window.
                    MainGrid = { DataContext = viewModel },

                    // Set the owner of the window to the Dynamo window.
                    Owner = viewLoadedParams.DynamoWindow
                };
                window.Left = window.Owner.Left + 400;
                window.Top = window.Owner.Top + 200;
                window.Show();
```

Finally, add this new submenu item to the menu:

```cs
            extensionMenu.Items.Add(inputsMenuItem);
```