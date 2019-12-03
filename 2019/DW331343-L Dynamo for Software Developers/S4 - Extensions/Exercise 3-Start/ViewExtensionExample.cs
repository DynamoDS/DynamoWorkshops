using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using DynamoDev.Stats;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace DynamoDev.Extensions
{
    /// <summary>
    /// Dynamo View Extension that can control both the Dynamo application and its UI (menus, view, canvas, nodes).
    /// </summary>
    public class ViewExtensionExample : IViewExtension
    {
        public string UniqueId => "5E85F38F-0A19-4F24-9E18-96845764780C";
        public string Name => "DynamoDev View Extension";

        private InputsViewModel vm;
        private MenuItem extensionMenu;
        private ViewLoadedParams viewLoadedParams;
        private DynamoViewModel dynamoViewModel => this.viewLoadedParams.DynamoWindow.DataContext as DynamoViewModel;

        /// <summary>
        /// Method that is called when Dynamo starts, but is not yet ready to be used.
        /// </summary>
        /// <param name="vsp">Parameters that provide references to Dynamo settings, version and extension manager.</param>
        public void Startup(ViewStartupParams vsp) { }

        /// <summary>
        /// Method that is called when Dynamo has finished loading and the UI is ready to be interacted with.
        /// </summary>
        /// <param name="vlp">
        /// Parameters that provide references to Dynamo commands, settings, events and
        /// Dynamo UI items like menus or the background preview. This object is supplied by Dynamo itself.
        /// </param>
        public void Loaded(ViewLoadedParams vlp)
        {
            // hold a reference to the Dynamo params to be used later
            this.viewLoadedParams = vlp;

            // we can now add custom menu items to Dynamo's UI
            MakeMenuItems();
        }

        /// <summary>
        /// Adds custom menu items to the Dynamo menu
        /// </summary>
        public void MakeMenuItems()
        {
            // let's now create a completely top-level new menu item
            this.extensionMenu = new MenuItem { Header = "DynamoDev Workshop" };

            // and now we add a new sub-menu item that says hello when clicked
            var sayHelloMenuItem = new MenuItem { Header = "Say Hello" };
            sayHelloMenuItem.Click += (sender, args) =>
            {
                MessageBox.Show("Hello " + Environment.UserName);
            };

            // and a submenu item that shows the window
            var inputsMenuItem = new MenuItem { Header = "Show Input Stats" };
            inputsMenuItem.Click += (sender, args) => MakeAndShowInputWindow();

            // now make a hackathon worthy menu item
            var hackMenuItem = new MenuItem { Header = "Hack !" };
            hackMenuItem.Click += (sender, args) =>
            {
                Process.Start("https://giphy.com/gifs/siliconvalleyhbo-2fQ1Gq3KOpvNs4NTmu");
            };

            // add all menu items to menu
            this.extensionMenu.Items.Add(sayHelloMenuItem);
            this.extensionMenu.Items.Add(hackMenuItem);
            this.extensionMenu.Items.Add(inputsMenuItem);

            // finally, we need to add our menu to Dynamo
            this.viewLoadedParams.dynamoMenu.Items.Add(this.extensionMenu);
        }

        private void MakeAndShowInputWindow()
        {
            vm = new InputsViewModel(this.viewLoadedParams);

            MessageBox.Show(
                $"There are {vm.NodeCount} total nodes & {vm.WireCount} wires," + Environment.NewLine +
                $"{vm.InputNodes.Count} of which are marked as inputs : " + Environment.NewLine +
                String.Join(Environment.NewLine, vm.InputNodesNames)
                );

            // Associate ViewModel with Window
        }

        /// <summary>
        /// Method that is called when the host Dynamo application is closed.
        /// </summary>
        public void Shutdown() { }

        public void Dispose() { }
    }
}
