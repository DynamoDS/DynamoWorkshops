
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