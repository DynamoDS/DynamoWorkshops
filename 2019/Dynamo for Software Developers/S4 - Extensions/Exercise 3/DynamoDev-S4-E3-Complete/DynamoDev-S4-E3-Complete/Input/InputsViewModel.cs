using Dynamo.Core;
using Dynamo.Extensions;
using Dynamo.Graph.Nodes;
using Dynamo.Wpf.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DynamoDev.Stats
{
    public class InputsViewModel : NotificationObject
    {
        private ViewLoadedParams readyParams;

        /// <summary>
        /// The input node names and their current values
        /// </summary>
        public Dictionary<string, string> InputNodes { get; set; }

        /// <summary>
        /// Displays the count of the nodes in the workspace that are marked as input
        /// </summary>
        public string InputNodesNames { get; set; }

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
            this.ResetStats();

            this.readyParams = p as ViewLoadedParams;
            Recompute();

            this.readyParams.CurrentWorkspaceModel.NodeAdded += OnNodeCountChange;
            this.readyParams.CurrentWorkspaceModel.NodeRemoved += OnNodeCountChange;
            this.readyParams.CurrentWorkspaceModel.ConnectorAdded += OnWireCountChange;
            this.readyParams.CurrentWorkspaceModel.ConnectorDeleted += OnWireCountChange;
            this.readyParams.CurrentWorkspaceChanged += OnWorkspaceChange;
            this.readyParams.CurrentWorkspaceCleared += OnWorkspaceChange;
        }
        public void Recompute()
        {
            this.ResetStats();
            var nodes = this.readyParams.CurrentWorkspaceModel.Nodes
                .Where(x => x.IsSetAsInput)
                .ToList();

            foreach (var node in nodes)
            {
                var value = node.CachedValue?.Data?.ToString() ?? "N/A";
                if (!this.InputNodes.ContainsKey(node.Name)) this.InputNodes.Add(node.Name, value);
            }

            this.NodeCount = this.readyParams.CurrentWorkspaceModel.Nodes
                .Count()
                .ToString();

            this.WireCount = this.readyParams.CurrentWorkspaceModel.Connectors
                .Count()
                .ToString();

            RaisePropertyChanged(
                nameof(this.NodeCount),
                nameof(this.WireCount),
                nameof(this.InputNodes)
                );
        }

        private void ResetStats()
        {
            this.InputNodes = new Dictionary<string, string>();
            this.NodeCount = 0.ToString();
            this.WireCount = 0.ToString();
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
            this.readyParams.CurrentWorkspaceChanged += OnWorkspaceChange;
        }
    }
}
