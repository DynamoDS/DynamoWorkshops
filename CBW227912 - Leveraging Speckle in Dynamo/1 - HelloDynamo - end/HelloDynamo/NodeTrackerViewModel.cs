using System;
using System.Collections.ObjectModel;
using Dynamo.Core;
using Dynamo.Extensions;

namespace HelloDynamo
{
  public class NodeTrackerViewModel: NotificationObject, IDisposable

  {
    private ReadyParams readyParams;

    private ObservableCollection<string> _actions = new ObservableCollection<string> ();
    public ObservableCollection<string> Actions { get { return _actions; } set { _actions = value; RaisePropertyChanged("Actions"); } }

    public NodeTrackerViewModel(ReadyParams p)
    {
      readyParams = p;
      
      //subscribing to dynamo events
      readyParams.CurrentWorkspaceModel.NodeAdded += CurrentWorkspaceModel_NodeAdded;
      readyParams.CurrentWorkspaceModel.NodeRemoved += CurrentWorkspaceModel_NodeRemoved;
      readyParams.CurrentWorkspaceModel.ConnectorAdded += CurrentWorkspaceModel_ConnectorAdded;
      readyParams.CurrentWorkspaceModel.ConnectorDeleted += CurrentWorkspaceModel_ConnectorDeleted;

      //making sure the binding is updated when elements are added and removed
      Actions.CollectionChanged += Actions_CollectionChanged;
    }

    private void Actions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
      RaisePropertyChanged("Actions");
    }

    private void CurrentWorkspaceModel_ConnectorDeleted(Dynamo.Graph.Connectors.ConnectorModel obj)
    {
      Actions.Add($"Connector between port {obj.Start.Name} and {obj.End.Name} deleted");
    }

    private void CurrentWorkspaceModel_ConnectorAdded(Dynamo.Graph.Connectors.ConnectorModel obj)
    {
      Actions.Add($"Connector between port {obj.Start.Name} and {obj.End.Name} added");
    }

    private void CurrentWorkspaceModel_NodeRemoved(Dynamo.Graph.Nodes.NodeModel obj)
    {
      Actions.Add($"Node {obj.Name} deleted");
    }

    private void CurrentWorkspaceModel_NodeAdded(Dynamo.Graph.Nodes.NodeModel obj)
    {
      Actions.Add($"Node {obj.Name} created");
    }

    public void Dispose()
    {
      //unsubscribing from events
      readyParams.CurrentWorkspaceModel.NodeAdded -= CurrentWorkspaceModel_NodeAdded;
      readyParams.CurrentWorkspaceModel.NodeRemoved -= CurrentWorkspaceModel_NodeRemoved;
      readyParams.CurrentWorkspaceModel.ConnectorAdded -= CurrentWorkspaceModel_ConnectorAdded;
      readyParams.CurrentWorkspaceModel.ConnectorDeleted -= CurrentWorkspaceModel_ConnectorDeleted;
    }

  }
}
