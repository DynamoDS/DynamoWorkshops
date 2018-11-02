using Dynamo.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HelloDynamo
{
    public class ExtensionReady : IExtension
    {
        public string UniqueId => "3B234622-43B7-4EA8-86DA-54FB390BE29E";

        public string Name => "Hello World";

        public void Dispose() { }

        /// <summary>
        /// Method that is called when Dynamo has finished loading and is ready to be used.
        /// </summary>
        /// <param name="rp">
        /// Parameters that provide references to Dynamo commands, settings and events.
        /// This object is supplied by Dynamo itself.
        /// </param>
        public void Ready(ReadyParams rp)
        {
            MessageBox.Show("Extension is ready!");

            // we can register our own events that will be triggered when specific things happen in Dynamo
            // a reference to the ReadyParams is needed to do this, so we pass it on
            Events.RegisterEventHandlers(rp);
        }

        /// <summary>
        /// Method that is called when the host Dynamo application is closed.
        /// </summary>
        public void Shutdown()
        {
            Events.UnregisterEventHandlers();
        }

        /// <summary>
        /// Method that is called when Dynamo starts, but is not yet ready to be used.
        /// </summary>
        /// <param name="sp">Parameters that provide references to Dynamo settings and version.</param>
        public void Startup(StartupParams sp) { }
    }
}
