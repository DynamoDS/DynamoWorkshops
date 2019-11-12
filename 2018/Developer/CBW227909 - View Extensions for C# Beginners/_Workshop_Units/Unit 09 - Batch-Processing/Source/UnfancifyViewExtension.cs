using System.Windows.Controls;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;

namespace Unfancify
{
  /// <summary>
  /// Dynamo View Extension that can control both the Dynamo application and its UI (menus, view, canvas, nodes).
  /// </summary>
  public class UnfancifyViewExtension : IViewExtension
  {
    // Make sure to generate a new guid for your tool
    // e.g. here: https://www.guidgenerator.com
    public string UniqueId => "d28ef462-3463-44c3-821c-35390c35c544";
    public string Name => "Unfancify";

    private MenuItem extensionMenu;
    private ViewLoadedParams viewLoadedParams;
    private DynamoViewModel dynamoViewModel => viewLoadedParams.DynamoWindow.DataContext as DynamoViewModel;

    /// <summary>
    /// Method that is called when Dynamo starts, but is not yet ready to be used.
    /// </summary>
    /// <param name="vsp">Parameters that provide references to Dynamo settings, version and extension manager.</param>
    public void Startup(ViewStartupParams vsp) { }

    /// <summary>
    /// Method that is called when Dynamo has finished loading and the UI is ready to be interacted with.
    /// </summary>
    /// <param name="vlp">
    /// Parameters that provide references to Dynamo commands, settings, events and
    /// Dynamo UI items like menus or the background preview. This object is supplied by Dynamo itself.
    /// </param>
    public void Loaded(ViewLoadedParams vlp)
    {
      // Hold a reference to the Dynamo params to be used later
      viewLoadedParams = vlp;
      // Add custom menu items to Dynamo's UI
      MakeMenuItems();
    }

    /// <summary>
    /// Adds custom menu items to the Dynamo menu.
    /// </summary>
    public void MakeMenuItems()
    {
      // Create a completely top-level new menu item
      extensionMenu = new MenuItem { Header = "AU Workshop" };

      // Create a new sub-menu item for our tool
      var unfancifyMenuItem = new MenuItem { Header = "Unfancify" };
      // Add a tool tip to our menu item
      unfancifyMenuItem.ToolTip = new ToolTip { Content = "Simplify your graph..." };
      // Define what happens when our sub-menu item is clicked
      unfancifyMenuItem.Click += (sender, args) =>
      {
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
      };
      // Add our sub-menu item to our top-level menu item
      extensionMenu.Items.Add(unfancifyMenuItem);

      // Add our top-level menu to the Dynamo menu
      viewLoadedParams.dynamoMenu.Items.Add(extensionMenu);
    }

    /// <summary>
    /// Method that is called when the host Dynamo application is closed.
    /// </summary>
    public void Shutdown() { }

    /// <summary>
    /// Method that is called for freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose() { }
  }
}
