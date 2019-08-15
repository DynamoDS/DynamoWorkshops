using Dynamo.Extensions;
using Dynamo.Graph.Nodes;
using System.Windows;

namespace HelloDynamo
{
    public static class Events
    {
        private static ReadyParams DynamoReadyParams;

        /// <summary>
        /// Registers custom events to be triggered when something happens in Dynamo.
        /// </summary>
        /// <param name="dynamoReadyParams">Reference to the Dynamo extension ready parameters.</param>
        public static void RegisterEventHandlers(ReadyParams dynamoReadyParams)
        {
            dynamoReadyParams.CurrentWorkspaceChanged += OnCurrentWorkspaceChanged;
            dynamoReadyParams.CurrentWorkspaceModel.NodeAdded += OnNodeAdded;
            dynamoReadyParams.CurrentWorkspaceModel.NodeRemoved += OnNodeRemoved;

            // keep a reference to the parameters supplied at startup
            // so we can unregister our event handlers later
            DynamoReadyParams = dynamoReadyParams;
        }

        /// <summary>
        /// Removes our custom events from Dynamo.
        /// </summary>
        public static void UnregisterEventHandlers()
        {
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
            MessageBox.Show($"Congratulations on opening the {obj.Name} workspace!");
        }

        /// <summary>
        /// Event triggered whenever a new node is added to the Dynamo canvas.
        /// </summary>
        /// <param name="node">The node that was added.</param>
        private static void OnNodeAdded(NodeModel node)
        {
            MessageBox.Show($"You just added the {node.Name} node with Id {node.GUID} to the canvas.");
        }

        /// <summary>
        /// Event triggered whenever a node is removed from the Dynamo canvas.
        /// </summary>
        /// <param name="node">The node that was removed.</param>
        private static void OnNodeRemoved(NodeModel node)
        {
            MessageBox.Show($"You just removed the {node.Name} node with Id {node.GUID} from the canvas.");
        }
    }
}
