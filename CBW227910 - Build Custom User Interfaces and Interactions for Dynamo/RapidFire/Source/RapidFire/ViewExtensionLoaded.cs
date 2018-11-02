using Dynamo.Controls;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RapidFire
{
    public class ViewExtensionLoaded : IViewExtension
    {
        public string UniqueId => "C46362AA-B570-43FE-B684-B0EC45E816DC";

        public string Name => "Rapid Fire";

        public void Loaded(ViewLoadedParams p)
        {
            // initiaite a RapidFire instance that needs a DynamoView so it can add nodes
            RapidFire rF = new RapidFire(p.DynamoWindow as DynamoView);

            // read the currently saved keyboard shortcuts
            var stcts = Serialization.ReadFile(Serialization.ShortcutFilePath);
            rF.Shortcuts = new HashSet<Shortcut>( stcts );

            // add a Menu Item for editing the Keyboard Shortcuts
            MenuItem MI = new MenuItem();
            MI.Header = "Rapid Fire";
            MI.Click += (o, e) =>
            {

            };
            p.AddMenuItem(MenuBarType.View, MI);

            // Register all of the even handlers
            Events.RegisterEventHandlers(p, rF);
        }

        public void Startup(ViewStartupParams p) { }

        public void Shutdown()
        {
            Events.UnregisterEventHandlers();
        }

        public void Dispose() { }
    }
}
