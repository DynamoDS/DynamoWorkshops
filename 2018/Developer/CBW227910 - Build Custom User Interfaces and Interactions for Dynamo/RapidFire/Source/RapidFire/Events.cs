using Dynamo.Controls;
using Dynamo.Extensions;
using Dynamo.Graph.Nodes;
using Dynamo.ViewModels;
using Dynamo.Views;
using Dynamo.Wpf.Extensions;
using System.Windows;
using System.Windows.Input;

namespace RapidFire
{
    public static class Events
    {
        private static ViewLoadedParams DynamoReadyParams;
        private static RapidFire RapidFireInstance;

        /// <summary>
        /// Registers custom events to be triggered when something happens in Dynamo.
        /// </summary>
        /// <param name="dynamoReadyParams">Reference to the Dynamo extension ready parameters.</param>
        public static void RegisterEventHandlers(ViewLoadedParams dynamoReadyParams, RapidFire rapidFire)
        {
            DynamoView view = dynamoReadyParams.DynamoWindow as DynamoView;
            view.KeyDown += rapidFire.View_KeyDown;
            view.KeyUp += rapidFire.View_KeyUp;

            dynamoReadyParams.CurrentWorkspaceChanged += OnCurrentWorkspaceChanged;
            dynamoReadyParams.CurrentWorkspaceModel.NodeAdded += OnNodeAdded;
            dynamoReadyParams.CurrentWorkspaceModel.NodeRemoved += OnNodeRemoved;

            // keep a reference to the parameters supplied at startup
            // so we can unregister our event handlers later
            DynamoReadyParams = dynamoReadyParams;
            RapidFireInstance = rapidFire;
        }

        /// <summary>
        /// Removes our custom events from Dynamo.
        /// </summary>
        public static void UnregisterEventHandlers()
        {
            DynamoView view = DynamoReadyParams.DynamoWindow as DynamoView;
            view.KeyDown -= RapidFireInstance.View_KeyDown;
            view.KeyUp -= RapidFireInstance.View_KeyUp;
            
            DynamoReadyParams.CurrentWorkspaceChanged -= OnCurrentWorkspaceChanged;
            DynamoReadyParams.CurrentWorkspaceModel.NodeAdded -= OnNodeAdded;
            DynamoReadyParams.CurrentWorkspaceModel.NodeRemoved -= OnNodeRemoved;
        }

        /// <summary>
        /// Event triggered whenever a Dynamo Workspace (file) is changed.
        /// </summary>
        /// <param name="obj">The current Dynamo workspace</param>
        private static void OnCurrentWorkspaceChanged(Dynamo.Graph.Workspaces.IWorkspaceModel obj)
        {
            //MessageBox.Show($"Congratulations on opening the {obj.Name} workspace!");
        }

        /// <summary>
        /// Event triggered whenever a new node is added to the Dynamo canvas.
        /// </summary>
        /// <param name="node">The node that was added.</param>
        private static void OnNodeAdded(NodeModel node)
        {
            //MessageBox.Show($"You just added the {node.Name} node with Id {node.GUID} to the canvas.");
        }

        /// <summary>
        /// Event triggered whenever a node is removed from the Dynamo canvas.
        /// </summary>
        /// <param name="node">The node that was removed.</param>
        private static void OnNodeRemoved(NodeModel node)
        {
            //MessageBox.Show($"You just removed the {node.Name} node with Id {node.GUID} from the canvas.");
        }
    }
}
