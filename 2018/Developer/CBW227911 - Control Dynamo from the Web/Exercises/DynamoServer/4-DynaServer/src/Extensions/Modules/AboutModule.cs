using Nancy;
using System.Reflection;

namespace DynaServer.Server
{
    /// <summary>
    /// This module provides details about the currently running DynamoServer and Dynamo itself.
    /// </summary>
    public class AboutModule : NancyModule
    {
        /// <summary>
        /// Get version information for DynamoServer and Dynamo.
        /// </summary>
        public AboutModule() : base("/about")
        {
            Get["/"] = x =>
            {
                var info =
                    "<h2>DynamoServer</h2></br>" +
                    "Assembly : " + Assembly.GetExecutingAssembly().FullName + "</br>" +
                    "Version : " + Assembly.GetExecutingAssembly().GetName().Version + "</br></br>" +
                    "<h2>Dynamo</h2></br>" +
                    "Version : " + Dynamo.Utilities.AssemblyHelper.GetDynamoVersion();

                return Response.AsText(info,"text/html");
            };
        }
    }
}
