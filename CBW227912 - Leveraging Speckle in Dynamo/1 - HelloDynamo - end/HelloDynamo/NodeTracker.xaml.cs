using System.Windows;

namespace HelloDynamo
{
  /// <summary>
  /// Tracks and displays events for node/connector added/removed 
  /// </summary>
  public partial class NodeTracker : Window
  {
    public NodeTracker()
    {
      InitializeComponent();
      this.Closing += NodeTracker_Closing;
    }

    private void NodeTracker_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      //hide window instead of closing it, so we keep tracking in the background
      e.Cancel = true;
      this.Hide();
    }
  }
}
