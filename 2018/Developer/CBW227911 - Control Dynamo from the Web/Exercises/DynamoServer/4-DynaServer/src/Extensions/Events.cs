using Dynamo.Graph.Nodes;
using System;
using System.Windows;

namespace DynaServer.Extensions
{
    public static class Events
    {
        public static void RegisterEventHandlers()
        {
            ServerViewExtension.viewLoadedParams.CurrentWorkspaceChanged += OnCurrentWorkspaceChanged;
            ServerViewExtension.viewLoadedParams.CurrentWorkspaceModel.NodeAdded += OnNodeAdded;
            ServerViewExtension.viewLoadedParams.CurrentWorkspaceModel.NodeRemoved += OnNodeRemoved;
        }

        public static void UnregisterEventHandlers()
        {
            ServerViewExtension.viewLoadedParams.CurrentWorkspaceChanged -= OnCurrentWorkspaceChanged;
            ServerViewExtension.viewLoadedParams.CurrentWorkspaceModel.NodeAdded -= OnNodeAdded;
            ServerViewExtension.viewLoadedParams.CurrentWorkspaceModel.NodeRemoved -= OnNodeRemoved;
        }

        private static void OnCurrentWorkspaceChanged(Dynamo.Graph.Workspaces.IWorkspaceModel obj)
        {
            // do something here
        }

        private static void OnNodeAdded(NodeModel node)
        {
            // do something here
        }

        private static void OnNodeRemoved(NodeModel node)
        {
            // do something here
        }

        public static async void OnServerStartAsync(object sender, RoutedEventArgs e)
        {
            await ServerViewExtension.StartServerAsync();
        }

        public static async void OnServerStop(object sender, RoutedEventArgs e)
        {
            await ServerViewExtension.StopServerAsync();
        }

        public static void OnCheckServerStatus(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(ServerViewExtension.GetServerStatus());
        }
    }
}
