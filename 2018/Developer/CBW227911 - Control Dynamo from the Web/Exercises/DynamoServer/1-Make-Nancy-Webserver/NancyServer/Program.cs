using Nancy.Hosting.Self;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NancyServer
{
    public class Program
    {
        private const string DEFAULT_URL_BASE = "http://localhost:1234";

        static void Main(string[] args)
        {
            var serverConfig = new HostConfiguration();
            serverConfig.UrlReservations.CreateAutomatically = true;

            var server = new NancyHost(serverConfig, new Uri(DEFAULT_URL_BASE));

            // start the server
            server.Start();

            Process.Start("http://localhost:1234");

            Console.WriteLine("Press any key to terminate server");
            Console.ReadKey();
            server.Stop();
            Console.WriteLine("Server has now stopped" + Environment.NewLine);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
