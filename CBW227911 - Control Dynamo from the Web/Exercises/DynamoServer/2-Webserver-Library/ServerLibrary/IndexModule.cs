using Nancy;
using System;

namespace NancyServer
{
    public class IndexModule : NancyModule
    {
        public IndexModule() : base("/")
        {
            Get["/"] = x => {
                return "Hello, the server is now running. </br>" +
                "It is now " + DateTime.Now.ToLongTimeString();
            };
        }
    }
}
