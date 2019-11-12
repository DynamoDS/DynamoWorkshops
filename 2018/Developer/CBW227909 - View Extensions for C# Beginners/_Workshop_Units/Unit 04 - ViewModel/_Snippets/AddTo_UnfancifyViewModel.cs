    /// <summary>
    /// Text message that appears below the buttons.
    /// It is updated by some of the methods in this view model.
    /// </summary>
    public string UnfancifyMsg { get; set; } = "";

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
      // Select all nodes
      viewModel.SelectAllCommand.Execute(null);
      // Call node to code
      viewModel.CurrentSpaceViewModel.NodeToCodeCommand.Execute(null);
    }