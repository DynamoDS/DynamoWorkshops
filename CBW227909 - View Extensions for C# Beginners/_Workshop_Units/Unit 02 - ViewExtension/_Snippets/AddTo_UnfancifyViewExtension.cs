        // Instantiate the view model of our tool
        var viewModel = new UnfancifyViewModel(viewLoadedParams, dynamoViewModel, viewLoadedParams.DynamoWindow);
        // Create the window for our UI
        var window = new UnfancifyWindow
        {
          // Set our view model as the DataContext of the main panel of our UI
          unfancifyPanel = { DataContext = viewModel },
          // Set our window as a child of the Dynamo window
          Owner = viewLoadedParams.DynamoWindow
        };
        // Define the start position of our window
        window.Left = window.Owner.Left + 400;
        window.Top = window.Owner.Top + 200;
        // Display our window
        window.Show();