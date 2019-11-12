using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Nancy;

namespace DynamoServer.Server
{
    /// <summary>
    /// This module only handles requests that target the /json endpoints.
    /// </summary>
    public class JsonModule : NancyModule
    {
        public JsonModule() : base("/json")
        {
            // would capture routes like /hello/nancy sent as a GET request
            Get["/json"] = parameters => {
                var feeds = new string[] { "foo", "bar" };
                return Response.AsJson(feeds);
            };
        }
    }
}
