using System;
using System.Linq;

namespace DynamoDev.Stats
{
    public class InputsViewModel : NotificationObject
    {
        private ViewLoadedParams readyParams;
        private string nodeCount;
        private string wireCount;
        private string inputNodes;

        /// <summary>
        /// Displays the count of the nodes in the workspace that are marked as input
        /// </summary>
        public string InputNodes { get; set; }

        /// <summary>
        /// Displays the count of the nodes in the workspace
        /// </summary>
        public string NodeCount { get; set; }

        /// <summary>
        /// Displays the count of the wires in the workspace
        /// </summary>
        public string WireCount { get; set; }

        public InputsViewModel(ReadyParams p)
        {
            this.readyParams = p as ViewLoadedParams;
            Recompute();

            this.readyParams.CurrentWorkspaceModel.NodeAdded += OnNodeCountChange;
            this.readyParams.CurrentWorkspaceModel.NodeRemoved += OnNodeCountChange;
            this.readyParams.CurrentWorkspaceModel.ConnectorAdded += OnWireCountChange;
            this.readyParams.CurrentWorkspaceModel.ConnectorDeleted += OnWireCountChange;
            this.readyParams.CurrentWorkspaceModel.NodeAdded += OnNodesUpdate;
            this.readyParams.CurrentWorkspaceModel.NodeRemoved += OnNodesUpdate;
            this.readyParams.CurrentWorkspaceChanged += OnWorkspaceChange;
        }

        public void Recompute()
        {
            var nodes = this.readyParams.CurrentWorkspaceModel.Nodes
                .Where(x => x.IsInput)
                .Select(x => x.Name)
                .ToList();
            this.InputNodes = nodes.Join(Environment.NewLine);

            this.NodeCount = this.readyParams.CurrentWorkspaceModel.Nodes
                .Count()
                .ToString();

            this.wireCount = this.readyParams.CurrentWorkspaceModel.Connectors
                .Count()
                .ToString();

            RaisePropertyChanged(nameof(NodeCount), nameof(WireCount), nameof(InputNodes));
        }

        #region ChangeProperty events

        private void OnNodeCountChange(NodeModel obj)
        {
            Recompute();
        }

        private void OnWireCountChange(Dynamo.Graph.Connectors.ConnectorModel obj)
        {
            Recompute();
        }

        private void OnNodesUpdate(NodeModel obj)
        {
            Recompute();
        }

        private void OnWorkspaceChange(Dynamo.Graph.Workspaces.IWorkspaceModel obj)
        {
            Recompute();
        }

        #endregion

        public void Dispose()
        {
            this.readyParams.CurrentWorkspaceModel.NodeAdded -= OnNodeCountChange;
            this.readyParams.CurrentWorkspaceModel.NodeRemoved -= OnNodeCountChange;
            this.readyParams.CurrentWorkspaceModel.ConnectorAdded += OnWireCountChange;
            this.readyParams.CurrentWorkspaceModel.ConnectorDeleted += OnWireCountChange;
            this.readyParams.CurrentWorkspaceModel.NodeAdded += OnNodesUpdate;
            this.readyParams.CurrentWorkspaceModel.NodeRemoved += OnNodesUpdate;
            this.readyParams.CurrentWorkspaceChanged += OnWorkspaceChange;
        }
    }
}
