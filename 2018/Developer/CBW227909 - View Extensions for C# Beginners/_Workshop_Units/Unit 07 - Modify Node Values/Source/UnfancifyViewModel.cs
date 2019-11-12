using System;
using System.Windows;
using System.Windows.Input;
using Dynamo.Core;
using Dynamo.Extensions;
using Dynamo.UI.Commands;
using Dynamo.ViewModels;

namespace Unfancify
{
  /// <summary>
  /// The view model of our tool.
  /// </summary>
  class UnfancifyViewModel : NotificationObject, IDisposable
  {
    private ReadyParams readyParams;
    private DynamoViewModel viewModel;
    private Window dynWindow;
    private string unfancifyMsg = "";
    public ICommand UnfancifyCurrentGraph { get; set; }

    /// <summary>
    /// The constructor of our view model.
    /// </summary>
    /// <param name="p">ReadyParams: Application-level handles provided to an extension when Dynamo has started and is ready for interaction</param>
    /// <param name="vm">The Dynamo view model: We'll need this for most of what we do below</param>
    /// <param name="dw">The Dynamo window: We'll need this for auto-layout to work properly</param>
    public UnfancifyViewModel(ReadyParams p, DynamoViewModel vm, Window dw)
    {
      // Hold references to the constructor arguments to be used later
      readyParams = p;
      viewModel = vm;
      dynWindow = dw;
      // The Unfancify Current Graph button is bound to this command
      UnfancifyCurrentGraph = new DelegateCommand(OnUnfancifyCurrentClicked);
    }

    /// <summary>
    /// Method that is called for freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose() { }

    /// <summary>
    /// Ungroup all groups?
    /// </summary>
    public bool UngroupAll { get; set; } = true;

    /// <summary>
    /// Text message that appears below the buttons.
    /// It is updated by some of the methods in this view model.
    /// </summary>
    public string UnfancifyMsg
    {
      get { return unfancifyMsg; }
      set
      {
        unfancifyMsg = value;
        // Notify the UI that the value has changed
        RaisePropertyChanged("UnfancifyMsg");
      }
    }

    /// <summary>
    /// Method that gets called when the user clicks on the Unfancify Current Graph button.
    /// </summary>
    public void OnUnfancifyCurrentClicked(object obj)
    {
      // Reset the message in the UI
      UnfancifyMsg = "";
      // Call our main method
      UnfancifyGraph();
      // Change the message in the UI
      UnfancifyMsg = "Current graph successfully unfancified!";
    }

    /// <summary>
    /// Main method of our tool that unfancifies a graph.
    /// Actions depend on settings in UI.
    /// </summary>
    public void UnfancifyGraph()
    {
      // Identify all groups to keep/ungroup
      if (UngroupAll)
      {
        // Cycle through all groups in the graph
        foreach (var anno in viewModel.CurrentSpaceViewModel.Annotations)
        {
          viewModel.AddToSelectionCommand.Execute(anno.AnnotationModel);
        }
        // Ungroup all obsolete groups
        viewModel.UngroupAnnotationCommand.Execute(null);
      }

      // Select all nodes
      viewModel.SelectAllCommand.Execute(null);
      // Call node to code
      viewModel.CurrentSpaceViewModel.NodeToCodeCommand.Execute(null);
    }
  }
}
