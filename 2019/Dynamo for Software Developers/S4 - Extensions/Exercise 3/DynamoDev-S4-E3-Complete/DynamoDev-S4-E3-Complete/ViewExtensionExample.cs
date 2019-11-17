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
            MessageBox.Show("Hello there, viewExtension has loaded!");

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
                Process.Start("https://giphy.com/search/hackathon");
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

        /// <summary>
        /// Method that is called when the host Dynamo application is closed.
        /// </summary>
        public void Shutdown() { }

        public void Dispose() { }
    }
}
