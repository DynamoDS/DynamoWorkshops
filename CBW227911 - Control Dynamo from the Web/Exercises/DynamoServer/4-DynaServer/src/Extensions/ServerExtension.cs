using Dynamo.Extensions;
using System;
using System.Windows;

namespace DynaServer.Extensions
{
    public class ServerExtension : IExtension
    {
        public string UniqueId => "EA3501CF-64AE-4246-8837-EFF7DF7F7067";

        public string Name => "DynaServer Extension";

        public void Startup(StartupParams sp) { }

        public void Ready(ReadyParams rp)
        {
            // do something here
        }

        public void Shutdown()
        {
            // do something here
        }

        public void Dispose() { }
    }
}
