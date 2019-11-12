using Dynamo.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dynamo.Extensions;
using Dynamo.Graph.Nodes;
using Dynamo.ViewModels;
using System.Windows;
using Dynamo.Graph;
using Dynamo.Wpf.Extensions;
using Dynamo.Graph.Annotations;
using Dynamo.Graph.Workspaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HelloDynamo.Stats
{
    public class StatsViewModel : NotificationObject
    {
        private ViewLoadedParams readyParams;
        private string activeNodeCount;
        private string activeWireCount;

        //Displays the count of the nodes in the workspace
        public string ActiveNodeCount => readyParams.CurrentWorkspaceModel.Nodes.Count().ToString();

        //Displays the count of the wires in the workspace
        public string ActiveWireCount => readyParams.CurrentWorkspaceModel.Connectors.Count().ToString();

        public StatsViewModel(ReadyParams p)
        {
            readyParams = p as ViewLoadedParams;
            readyParams.CurrentWorkspaceModel.NodeAdded += OnNodeCountChange;
            readyParams.CurrentWorkspaceModel.NodeRemoved += OnNodeCountChange;
            readyParams.CurrentWorkspaceModel.ConnectorAdded += OnWireCountChange;
            readyParams.CurrentWorkspaceModel.ConnectorDeleted += OnWireCountChange;
            readyParams.CurrentWorkspaceModel.NodeAdded += OnNodesUpdate;
            readyParams.CurrentWorkspaceModel.NodeRemoved += OnNodesUpdate;
            readyParams.CurrentWorkspaceChanged += OnWorkspaceChange;
        }

        #region ChangeProperty events

        private void OnNodeCountChange(NodeModel obj)
        {
            RaisePropertyChanged("ActiveNodeCount");
        }

        private void OnWireCountChange(Dynamo.Graph.Connectors.ConnectorModel obj)
        {
            RaisePropertyChanged("ActiveWireCount");
        }

        private void OnNodesUpdate(NodeModel obj)
        {
            RaisePropertyChanged("ActiveNodeCount");
        }

        private void OnWorkspaceChange(Dynamo.Graph.Workspaces.IWorkspaceModel obj)
        {
            RaisePropertyChanged("ActiveNodeCount", "ActiveWireCount");
        }

        #endregion

        public void Dispose()
        {
            readyParams.CurrentWorkspaceModel.NodeAdded -= OnNodeCountChange;
            readyParams.CurrentWorkspaceModel.NodeRemoved -= OnNodeCountChange;
            readyParams.CurrentWorkspaceModel.ConnectorAdded += OnWireCountChange;
            readyParams.CurrentWorkspaceModel.ConnectorDeleted += OnWireCountChange;
            readyParams.CurrentWorkspaceModel.NodeAdded += OnNodesUpdate;
            readyParams.CurrentWorkspaceModel.NodeRemoved += OnNodesUpdate;
            readyParams.CurrentWorkspaceChanged += OnWorkspaceChange;
        }
    }
}
