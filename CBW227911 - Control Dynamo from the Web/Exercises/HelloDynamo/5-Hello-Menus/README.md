
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