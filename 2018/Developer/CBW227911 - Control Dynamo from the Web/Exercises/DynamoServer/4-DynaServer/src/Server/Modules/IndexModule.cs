using Nancy;
using System;

namespace DynaServer.Server
{
    public class IndexModule : NancyModule
    {
        public IndexModule() : base("/")
        {
            Get["/"] = x =>
            {
                return Response.AsFile("extra/start.html", "text/html");
            };

            Get["/stop"] = x =>
            {
                return Response.AsFile("extra/stop.html", "text/html");
            };

            Get["/time"] = x => { return "Hello, it is now " + DateTime.Now.ToLongTimeString(); };

            Get["/hello"] = SayHello;

            Get["/hello/{name}"] = parameters =>
            {
                return "Hello back, " + parameters?.name + " ! ";
            };

            Get["/echo/{value:int}"] = Echo;
        }

        private dynamic Echo(dynamic parameters)
        {
            return "This is what you wrote to me : " + parameters.value;
        }

        private dynamic SayHello(dynamic parameters)
        {
            var html = @"
<h1>Hello from Dynamo !</h1><br/><br/>
<p>I don't know your name yet, so why don't you add a '/' followed by your name to the URL ?<br/>
<p>For example, visit '/hello/Dynamo` to tell me your name is Dynamo - though I won't believe you since robots don't have names.</p>";

            return Response.AsText(html, "text/html");
        }

    }
}
