using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DynamoServer.Extensions
{
    public static class UI
    {
        public static MenuItem DynamoServerMenu;
        private static MenuItem StartServerMenuItem;
        private static MenuItem StopServerMenuItem;

        static UI()
        {
            // let's now create a completely top-level new menu item
            DynamoServerMenu = new MenuItem { Header = "Dynamo Server" };

            // and now we add a new sub-menu item that says hello when clicked
            StartServerMenuItem = new MenuItem { Header = "Start Server" };
            StopServerMenuItem = new MenuItem { Header = "Stop Server" };

            // register event handlers
            StartServerMenuItem.Click += Events.OnServerStartAsync;
            StopServerMenuItem.Click += Events.OnServerStop;

            DynamoServerMenu.Items.Add(StartServerMenuItem);
            DynamoServerMenu.Items.Add(StopServerMenuItem);
        }
    }
}
