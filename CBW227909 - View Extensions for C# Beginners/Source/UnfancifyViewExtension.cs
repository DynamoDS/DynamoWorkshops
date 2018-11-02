using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Unfancify
{
    /// <summary>
    /// Dynamo View Extension that can control both the Dynamo application and its UI (menus, view, canvas, nodes).
    /// </summary>
    public class UnfancifyViewExtension : IViewExtension
    {
        public string UniqueId => "115faa5e-faf3-47f6-a888-76438463bea6";
        public string Name => "Unfancify";

        private MenuItem extensionMenu;
        private ViewLoadedParams viewLoadedParams;
        private DynamoViewModel dynamoViewModel => viewLoadedParams.DynamoWindow.DataContext as DynamoViewModel;

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
            viewLoadedParams = vlp;

            // we can now add custom menu items to Dynamo's UI
            MakeMenuItems();
        }

        /// <summary>
        /// Adds custom menu items to the Dynamo menu
        /// </summary>
        public void MakeMenuItems()
        {
            // let's now create a completely top-level new menu item
            extensionMenu = new MenuItem { Header = "AU Workshop" };

            // and now we add a new sub-menu item that says hello when clicked
            var sayHelloMenuItem = new MenuItem { Header = "Unfancify" };
            sayHelloMenuItem.Click += (sender, args) =>
            {
                MessageBox.Show("Hello " + Environment.UserName);
            };
            extensionMenu.Items.Add(sayHelloMenuItem);

            // finally, we need to add our menu to Dynamo
            viewLoadedParams.dynamoMenu.Items.Add(extensionMenu);
        }

        /// <summary>
        /// Method that is called when the host Dynamo application is closed.
        /// </summary>
        public void Shutdown() { }

        public void Dispose() { }
    }
}
