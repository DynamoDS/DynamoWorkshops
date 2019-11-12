    /// <summary>
    /// Method that gets called when the user clicks on the Batch-Unfancify button.
    /// </summary>
    public void SelectSource_Click(object sender, RoutedEventArgs e)
    {
      // A dialog that lets the user select a directory
      var openDialog = new FolderBrowserDialog
      {
        // Include the option to create a new folder on-the-fly
        ShowNewFolderButton = true
      };

      // Only do something if the user leaves the FolderBrowserDialog by clicking OK
      // Otherwise the user just returns to our window
      if (openDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        // Get a refrence to our tool's view model
        var vm = (UnfancifyViewModel)unfancifyPanel.DataContext;
        // Call the batch-unfancify method in our view model, providing the folder chosen by the user
        vm.OnBatchUnfancifyClicked(openDialog.SelectedPath);
      }
    }



using System.Windows.Forms;