using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using CoreNodeModels.Input;
using Dynamo.Core;
using Dynamo.Extensions;
using Dynamo.Graph;
using Dynamo.Models;
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
    /// Use auto-layout?
    /// </summary>
    public bool DoAutoLayout { get; set; } = true;

    /// <summary>
    /// Ungroup all groups?
    /// </summary>
    public bool UngroupAll { get; set; } = true;

    /// <summary>
    /// Delete all text notes?
    /// </summary>
    public bool DeleteTextNotes { get; set; } = true;

    /// <summary>
    /// Disable geometry preview for all nodes?
    /// </summary>
    public bool DisableGeometryPreview { get; set; } = true;

    /// <summary>
    /// Group prefixes that should be ignored.
    /// </summary>
    public string IgnoreGroupPrefixes { get; set; } = "";

    /// <summary>
    /// Text note prefixes that should be ignored.
    /// </summary>
    public string IgnoreTextNotePrefixes { get; set; } = "";

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
    /// Method that gets called when the user has selected a directory and clicked OK.
    /// </summary>
    public void OnBatchUnfancifyClicked(string directoryPath)
    {
      // Reset the message in the UI
      UnfancifyMsg = "";
      // Read directory contents
      var graphs = System.IO.Directory.EnumerateFiles(directoryPath);
      // We'll need a counter here because the above enumeration doesn't have a Count property
      // and we'll want to let the user know how many fiels we have processed in the end.
      var graphCount = 0;
      // Cycle through all files found in the directory
      foreach (var graph in graphs)
      {
        var ext = System.IO.Path.GetExtension(graph);
        // We're only interested in *.dyn files
        if (ext == ".dyn")
        {
          // Open the graph
          viewModel.OpenCommand.Execute(graph);
          // Set the graph run type to manual mode (otherwise some graphs might auto-execute at this point)
          viewModel.CurrentSpaceViewModel.RunSettingsViewModel.Model.RunType = RunType.Manual;
          // Call our main method
          UnfancifyGraph();
          // Save the graph
          viewModel.SaveAsCommand.Execute(graph);
          // Close it
          viewModel.CloseHomeWorkspaceCommand.Execute(null);
          // Increment our counter
          graphCount += 1;
          // Update the message in the UI
          UnfancifyMsg += "Unfancified " + graph + "\n";
        }
      }
      // Write a summary to the UI
      UnfancifyMsg += "Unfancified " + graphCount.ToString() + " graphs...";
    }

    /// <summary>
    /// Main method of our tool that unfancifies a graph.
    /// Actions depend on settings in UI.
    /// </summary>
    public void UnfancifyGraph()
    {
      // Create a list for storing guids of groups, nodes and text notes that we want to keep
      var stuffToKeep = new List<string>();

      // Identify all groups to keep/ungroup
      if (UngroupAll)
      {
        // Make sure that no groups are currently selected
        GeneralUtils.ClearSelection();
        // Create a whitelist of prefixes for group titles from what was entered by the user in the UI
        var groupIgnoreList = IgnoreGroupPrefixes.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        // Cycle through all groups in the graph
        foreach (var anno in viewModel.CurrentSpaceViewModel.Annotations)
        {
          // Cycle through the whitelist
          foreach (var ignoreTerm in groupIgnoreList)
          {
            // Identify keepers (group and its contents)
            if (anno.AnnotationText.StartsWith(ignoreTerm) && !stuffToKeep.Contains(anno.AnnotationModel.GUID.ToString()))
            {
              stuffToKeep.Add(anno.AnnotationModel.GUID.ToString());
              // Identify all nodes and text notes within those groups
              foreach (var element in anno.Nodes) { stuffToKeep.Add(element.GUID.ToString()); }
            }
          }
          // Add all obsolete groups to selection
          if (!stuffToKeep.Contains(anno.AnnotationModel.GUID.ToString())) { viewModel.AddToSelectionCommand.Execute(anno.AnnotationModel); }
        }
        // Ungroup all obsolete groups
        viewModel.UngroupAnnotationCommand.Execute(null);
      }

      // Identify all text notes to keep/delete
      if (DeleteTextNotes)
      {
        // Make sure that no text notes are currently selected
        GeneralUtils.ClearSelection();
        // Create a whitelist of prefixes for text note content from what was entered by the user in the UI
        var textNoteIgnoreList = IgnoreTextNotePrefixes.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        // Cycle through all text notes in the graph
        foreach (var note in viewModel.Model.CurrentWorkspace.Notes)
        {
          // Cycle through the whitelist
          foreach (var ignoreTerm in textNoteIgnoreList)
          {
            // Identify keepers
            if (note.Text.StartsWith(ignoreTerm) && !stuffToKeep.Contains(note.GUID.ToString()))
            {
              stuffToKeep.Add(note.GUID.ToString());
            }
          }
          // Add all obsolete text notes to selection
          if (!stuffToKeep.Contains(note.GUID.ToString())) { viewModel.AddToSelectionCommand.Execute(note); }
        }
        // Delete all obsolete text notes
        viewModel.DeleteCommand.Execute(null);
      }

      // Process nodes before we call node to code
      // Make sure that no nodes are currently selected
      GeneralUtils.ClearSelection();
      // We need this part to circumnavigate two minor node-to-code bugs
      // So this is actually a good example for how you can fix Dynamo issues for yourself without having to touch DynamoCore code
      foreach (var node in viewModel.Model.CurrentWorkspace.Nodes)
      {
        // We're not interested in keepers (i.e. nodes that will not be run through node-to-code)
        if (!stuffToKeep.Contains(node.GUID.ToString()))
        {
          // Pre-processing of string input nodes only
          // Temporary fix for https://github.com/DynamoDS/Dynamo/issues/9117 (Escape backslashes in string nodes)
          // Temporary fix for https://github.com/DynamoDS/Dynamo/issues/9120 (Escape double quotes in string nodes)
          if (node.GetType() == typeof(StringInput))
          {
            // Cast NodeModel to StringInput
            var inputNode = (StringInput)node;
            // Get the current value of the input node
            var nodeVal = inputNode.Value;
            // Escape backslahes and double quotes
            nodeVal = nodeVal.Replace("\\", "\\\\").Replace("\"", "\\\"");
            // Update the input node's value
            var updateVal = new UpdateValueParams("Value", nodeVal);
            node.UpdateValue(updateVal);
          }
          // Add each node to the current selection
          viewModel.AddToSelectionCommand.Execute(node);
        }
      }
      // Call node to code
      viewModel.CurrentSpaceViewModel.NodeToCodeCommand.Execute(null);

      // Turn off geometry preview
      if (DisableGeometryPreview)
      {
        foreach (var node in viewModel.CurrentSpaceViewModel.Nodes)
        {
          if (node.IsVisible) { node.ToggleIsVisibleCommand.Execute(null); }
        }
      }

      // Auto layout
      if (DoAutoLayout)
      {
        // Make sure nothing is selected
        GeneralUtils.ClearSelection();
        // Here we'll need to call the auto-layout via Dynamo's Dispatcher
        // Otherwise the auto layout command will not yet be aware of the size 
        // of the code blocks generated by node to code and our graph will look less pretty.
        dynWindow.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
        {
          viewModel.CurrentSpaceViewModel.GraphAutoLayoutCommand.Execute(null);
        }));
      }
    }
  }
}
