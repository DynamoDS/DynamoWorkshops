using System.Windows;

namespace HelloDynamo
{
    /// <summary>
    /// Dynamo View Extension that can control both the Dynamo application and its UI (menus, view, canvas, nodes).
    /// </summary>
    public class ViewExtensionExample : IViewExtension
    {
        public string UniqueId => "5E85F38F-0A19-4F24-9E18-96845764780C";
        public string Name => "Hello Dynamo View Extension";

        /// <summary>
        /// Method that is called when Dynamo starts, but is not yet ready to be used.
        /// </summary>
        /// <param name="vsp">Parameters that provide references to Dynamo settings, version and extension manager.</param>
        public void Startup(ViewStartupParams vsp)
        {

        }

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

            // hold references here to the Dynamo params to be used later
        }

        /// <summary>
        /// Method that is called when the host Dynamo application is closed.
        /// </summary>
        public void Shutdown() { }

        public void Dispose() { }
    }
}
