using Dynamo.Controls;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HelloDynamo
{
    public class ViewExtensionLoaded : IViewExtension
    {
        public string UniqueId => "5E85F38F-0A19-4F24-9E18-96845764780C";

        public string Name => "Hello Dynamo";

        public void Loaded(ViewLoadedParams p)
        {
            MessageBox.Show("ViewExtension has loaded!");
        }

        public void Dispose() { }

        public void Shutdown() { }

        public void Startup(ViewStartupParams p) { }
    }
}
