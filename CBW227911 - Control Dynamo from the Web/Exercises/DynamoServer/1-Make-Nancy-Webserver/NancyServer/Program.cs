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
        static void Main(string[] args)
        {
            var server = new WebServer();

            // start the server
            server.Start();

            Process.Start("http://localhost:1234");

            Console.WriteLine("Press any key to terminate server");
            Console.ReadKey();

        }
    }
}
