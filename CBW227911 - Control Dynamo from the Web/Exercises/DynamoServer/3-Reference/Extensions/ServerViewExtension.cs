using Dynamo.Logging;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using DynamoServer.Server;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace DynamoServer.Extensions
{
    public class ServerViewExtension : IViewExtension
    {
        public string UniqueId => "5E85F38F-0A19-4F24-9E18-96845764780C";
        public string Name => "Dynamo Server View Extension";
        public string DynamoVersion => dynamoViewModel.Version;
        public string DynamoHostVersion => dynamoViewModel.HostVersion;

        internal static ViewLoadedParams viewLoadedParams = null;
        internal static DynamoViewModel dynamoViewModel => viewLoadedParams.DynamoWindow.DataContext as DynamoViewModel;

        public ServerViewExtension()
        {
            if (Server == null) Server = new DynamoWebServer();
        }

        public void Startup(ViewStartupParams vsp) { }

        public void Loaded(ViewLoadedParams vlp)
        {
            MessageBox.Show($"[ {DateTime.Now} ] : {this.Name} is ready!");

            // hold a reference to the Dynamo params to be used later
            if (viewLoadedParams == null) viewLoadedParams = vlp;

            // register events
            Events.RegisterEventHandlers();
        }

        public static async Task StartServerAsync()
        {
            var message = $"[ {DateTime.Now} ] : Starting server on machine {Environment.MachineName}";

            // start server and continue execution
            await Task.Run(() => {
                // start server here
                });
        }

        public static async Task StopServerAsync()
        {
            // stop server and continue execution
            await Task.Run(() => {
                // stop server here
                });
        }

        public async void Shutdown()
        {
            Events.UnregisterEventHandlers();
        }

        public void Dispose() { }
    }
}
