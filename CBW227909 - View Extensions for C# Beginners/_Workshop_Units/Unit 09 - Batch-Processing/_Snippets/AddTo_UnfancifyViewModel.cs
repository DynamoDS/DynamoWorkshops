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



using Dynamo.Models;