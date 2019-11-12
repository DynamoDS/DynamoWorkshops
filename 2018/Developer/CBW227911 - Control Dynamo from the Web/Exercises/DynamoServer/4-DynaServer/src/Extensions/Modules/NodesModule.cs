using DynaServer.Extensions;
using Nancy;
using System;
using System.Linq;

namespace DynaServer.Server
{
    /// <summary>
    /// Module to handle basic operations with nodes (get, add, remove) on the Dynamo canvas.
    /// </summary>
    public class NodesModule : NancyModule
    {
        public NodesModule() : base("/Nodes")
        {
            Get["/"] = GetNodes;
            Get["/Clear"] = Clear;
            Get["/Add/{nodename}"] = AddNode;
        }

        /// <summary>
        /// Add a node to the Dynamo canvas.
        /// </summary>
        /// <param name="parameters">The node name, example : List.Map</param>
        /// <returns>HTML result summary.</returns>
        public dynamic AddNode(dynamic parameters)
        {
            string node = parameters.nodename;
            if (string.IsNullOrWhiteSpace(node)) return "Supplied node was empty";

            string result = "";
            var command = new Dynamo.Models.DynamoModel.CreateNodeCommand(Guid.NewGuid().ToString(), node, 100, 100, true, false);
            var nodes = "";
            ServerViewExtension.RunInContext(() =>
            {
                try
                {
                    int countBefore = ServerViewExtension.dynamoViewModel.Model.CurrentWorkspace.Nodes.Count();

                    ServerViewExtension.dynamoViewModel.Model.ExecuteCommand(command);
                    int countAfter = ServerViewExtension.dynamoViewModel.Model.CurrentWorkspace.Nodes.Count();

                    var n = ServerViewExtension.dynamoViewModel.Model.CurrentWorkspace.Nodes.Select(x => "<li>" + x.Name).ToArray();
                    nodes = string.Join("</li> ", n);

                    var nodeCountDiff = countAfter - countBefore;

                    if (nodeCountDiff < 1) result = $"Could not add your {node} node to canvas.";
                    else if (nodeCountDiff == 1) result = $"Added {node} node to the canvas.";
                    else throw new Exception($"Added {nodeCountDiff} nodes to the canvas.");
                }
                catch (System.Exception e)
                {
                    result = "Something went wrong, error : " + e.Message;
                }
            });
            return Response.AsText(result, "text/html");
        }

        /// <summary>
        /// Gets a list of all the nodes currently on the Dynamo canvas.
        /// </summary>
        /// <param name="parameters">Not used, can specify null.</param>
        /// <returns>HTML result summary.</returns>
        public dynamic GetNodes(dynamic parameters)
        {
            var allNodes = ServerViewExtension.viewLoadedParams.CurrentWorkspaceModel.Nodes.Select(x => x.Name).ToList();

            string html = "" +
                "<h2>Current workspace nodes : </h2></br>" +
                "<ul>";

            foreach (var item in allNodes)
            {
                html += "<li>" + item + "</li>";
            }
            html += "</ul>";

            return Response.AsText(html, "text/html");
        }

        /// <summary>
        /// Clears (deletes) everything on the Dynamo canvas, including Nodes, Annotations, etc.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>HTML result summary.</returns>
        public dynamic Clear(dynamic parameters)
        {
            string html = "";
            int nodeCountBefore = 0, nodeCountAfter = 0;

            ServerViewExtension.RunInContext(() =>
            {
                nodeCountBefore = ServerViewExtension.viewLoadedParams.CurrentWorkspaceModel.Nodes.Count();

                var vm = ServerViewExtension.dynamoViewModel;
                vm.Model.ClearCurrentWorkspace();

                nodeCountAfter = ServerViewExtension.viewLoadedParams.CurrentWorkspaceModel.Nodes.Count();
            }
            );

            if (nodeCountAfter < nodeCountBefore) html = $"Cleared {nodeCountBefore - nodeCountAfter} nodes from canvas.";
            else html = "Did not clear any nodes from canvas.";

            return Response.AsText(html, "text/html");
        }
    }
}
