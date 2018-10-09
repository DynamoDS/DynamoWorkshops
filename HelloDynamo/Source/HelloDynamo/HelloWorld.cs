using Dynamo.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HelloDynamo
{
    public class HelloWorld : IExtension
    {
        public string UniqueId => "3B234622-43B7-4EA8-86DA-54FB390BE29E";

        public string Name => "Hello World";

        public void Dispose() { }

        public void Ready(ReadyParams sp)
        {
            MessageBox.Show("Extension is ready!");
            //todo implement a meaningful and easy to access event 
            sp.CurrentWorkspaceChanged += Sp_CurrentWorkspaceChanged;
        }

        private void Sp_CurrentWorkspaceChanged(Dynamo.Graph.Workspaces.IWorkspaceModel obj)
        {
            MessageBox.Show($"Congratulations on opening{obj.Name}");
        }

        public void Shutdown() { }

        public void Startup(StartupParams sp) { }
    }
}
