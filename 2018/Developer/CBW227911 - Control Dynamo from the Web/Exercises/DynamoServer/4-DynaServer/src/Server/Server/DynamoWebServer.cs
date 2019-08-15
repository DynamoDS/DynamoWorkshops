using Nancy.Hosting.Self;
using System;
using System.Diagnostics;

namespace DynaServer.Server
{
    public class DynamoWebServer : IDisposable
    {
        private const string DEFAULT_URL_BASE = "http://localhost:1234";

        private NancyHost server;
        private HostConfiguration serverConfig;

        public string UrlBase { get; private set; }
        public bool IsRunning { get; private set; }
        public bool FailedToStart { get; private set; }


        public DynamoWebServer()
        {
            UrlBase = DEFAULT_URL_BASE;

            serverConfig = new HostConfiguration();
            serverConfig.UrlReservations.CreateAutomatically = true;
            var bootstrapper = new Bootstrapper();

            server = new NancyHost(bootstrapper, serverConfig, new Uri(UrlBase));

            // set initial state
            IsRunning = false;
            FailedToStart = false;
        }

        public DynamoWebServer(string urlbase) : base()
        {
            if (!string.IsNullOrWhiteSpace(urlbase)) UrlBase = urlbase;
        }

        public void Start()
        {
            Console.WriteLine("Starting web service on " + UrlBase);
            try
            {
                server.Start();
                IsRunning = true;
                Process.Start(UrlBase);
            }
            catch (Exception)
            {
                IsRunning = false;
                FailedToStart = false;
            }
        }

        public void Stop()
        {
            if (!this.IsRunning) return;

            Console.WriteLine("Stopping web service on " + UrlBase);

            server.Stop();
            IsRunning = false;
        }

        public void Dispose()
        {
            server.Dispose();
        }
    }

}
