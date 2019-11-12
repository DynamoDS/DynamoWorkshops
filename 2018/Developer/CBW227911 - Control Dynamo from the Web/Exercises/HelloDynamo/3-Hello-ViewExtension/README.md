
# Exercise 3 - View Extension snippets

** Implement Interface**

```cs
    public class ViewExtensionExample : IViewExtension
```

** Interface properties**

```cs
        public string UniqueId => "5E85F38F-0A19-4F24-9E18-96845764780C";
        public string Name => "Hello Dynamo View Extension";
```

** Interface methods **

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

** Message boxes **

```cs
MessageBox.Show("Extension is starting up!");
```