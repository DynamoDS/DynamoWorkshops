using ServerLibrary;
using System;

namespace NancyServer
{
    public class Program
    {

        static void Main(string[] args)
        {
            // start the server
            var server = new WebServer();
            server.Start();

            Console.WriteLine("Press any key to terminate server");
            Console.ReadKey();
            server.Stop();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
