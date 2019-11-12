
# Exercise 2 -Extension snippets

** Implement an interface **

```cs
    public class ExtensionExample : IExtension
```


** Interface properties **

```cs
        public string UniqueId => "3B234622-43B7-4EA8-86DA-54FB390BE29E";

        public string Name => "Hello Dynamo Extension";
```


** Interface methods**

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