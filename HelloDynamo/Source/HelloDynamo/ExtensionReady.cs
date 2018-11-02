using Dynamo.Extensions;
using System.Windows;

namespace HelloDynamo
{
  public class ExtensionReady : IExtension
  {
    public string UniqueId => "3B234622-43B7-4EA8-86DA-54FB390BE29E";

    public string Name => "Hello World";

    public void Dispose() { }

    public void Ready(ReadyParams rp)
    {
      MessageBox.Show("Extension is ready!");
    }

    public void Shutdown() { }

    public void Startup(StartupParams sp) { }
    
  }
}
