using Dynamo.Wpf.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HelloDynamo
{
  /// <summary>
  /// Dynamo View Extension that can control both the Dynamo application and its UI (menus, view, canvas, nodes).
  /// </summary>
  public class ViewExtensionExample : IViewExtension
  {
    public string UniqueId => "5E85F38F-0A19-4F24-9E18-96845764780C";
    public string Name => "Hello Dynamo View Extension";

    private MenuItem extensionMenu;
    private ViewLoadedParams viewLoadedParams;
    
    private NodeTracker _nodeTracker = null;

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
      viewLoadedParams = vlp;
      //instanciating the window and setting the datacontext to bind it to the viewmodel
      var viewModel = new NodeTrackerViewModel(viewLoadedParams);
      _nodeTracker = new NodeTracker
      {
        Owner = viewLoadedParams.DynamoWindow,
        DataContext = viewModel
      };

      MakeMenuItems();
    }

    /// <summary>
    /// Adds custom menu items to the Dynamo menu
    /// </summary>
    public void MakeMenuItems()
    {
      extensionMenu = new MenuItem { Header = "AU Workshop" };

      var sayHelloMenuItem = new MenuItem { Header = "Say Hello" };
      sayHelloMenuItem.Click += (sender, args) =>
      {
         MessageBox.Show("Hello " + Environment.UserName);
      };
      //new menu item for our node tracker
      var nodeTrackerMenuItem = new MenuItem { Header = "Node Tracker" };
      nodeTrackerMenuItem.Click += (sender, args) =>
      {
        _nodeTracker.Show();
      };

      extensionMenu.Items.Add(sayHelloMenuItem);
      extensionMenu.Items.Add(nodeTrackerMenuItem);
      viewLoadedParams.dynamoMenu.Items.Add(extensionMenu);
    }

    /// <summary>
    /// Method that is called when the host Dynamo application is closed.
    /// </summary>
    public void Shutdown() { }

    public void Dispose() { }
  }
}
