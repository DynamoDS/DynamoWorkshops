    /// <summary>
    /// Disable geometry preview for all nodes?
    /// </summary>
    public bool DisableGeometryPreview { get; set; } = true;





      // Turn off geometry preview
      if (DisableGeometryPreview)
      {
        foreach (var node in viewModel.CurrentSpaceViewModel.Nodes)
        {
          if (node.IsVisible) { node.ToggleIsVisibleCommand.Execute(null); }
        }
      }