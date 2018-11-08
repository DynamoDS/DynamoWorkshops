    /// <summary>
    /// Delete all watch nodes?
    /// </summary>
    public bool DeleteWatchNodes { get; set; } = true;





      // Process remaining nodes
      if (DisableGeometryPreview || DeleteWatchNodes)
      {
        // Make sure no nodes are selected
        GeneralUtils.ClearSelection();
        foreach (var node in viewModel.CurrentSpaceViewModel.Nodes)
        {
          // Turn off geometry preview
          if (DisableGeometryPreview)
          {
            if (node.IsVisible) { node.ToggleIsVisibleCommand.Execute(null); }
          }
          // Identify Watch nodes
          if (DeleteWatchNodes)
          {
            var nodeType = node.NodeModel.GetType().ToString();
            // We're only targeting Watch, Watch 3D & Watch Image nodes here
            if (nodeType == "CoreNodeModels.Watch" || nodeType == "Watch3DNodeModels.Watch3D" || nodeType == "CoreNodeModels.WatchImageCore")
            {
              // Only add downstream nodes to selection
              // i.e. nodes that have no other nodes connected to their outports
              if (node.NodeModel.OutputNodes.Count == 0) { viewModel.AddToSelectionCommand.Execute(node.NodeModel); }
            }
          }
        }
        if (DeleteWatchNodes)
        {
          // We need to hold off on deleting the Watch nodes until here
          // in order to not modify the collection of nodes while we're still cycling through it.
          viewModel.DeleteCommand.Execute(null);
        }
      }