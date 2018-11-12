using Nancy.Hosting.Self;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLibrary
{
    public class WebServer
    {
        private const string DEFAULT_URL_BASE = "http://localhost:1234";
        private NancyHost server;

        public WebServer()
        {
            var serverConfig = new HostConfiguration();
            serverConfig.UrlReservations.CreateAutomatically = true;

            server = new NancyHost(serverConfig, new Uri(DEFAULT_URL_BASE));
        }

        public void Start()
        {
            server.Start();
            Process.Start("http://localhost:1234");
            Console.WriteLine("Server has now started." + Environment.NewLine);
        }

        public void Stop()
        {
            server.Stop();
            Console.WriteLine("Server has now stopped." + Environment.NewLine);
        }
    }
}
