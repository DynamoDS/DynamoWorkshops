    /// <summary>
    /// Ungroup all groups?
    /// </summary>
    public bool UngroupAll { get; set; } = true;



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
