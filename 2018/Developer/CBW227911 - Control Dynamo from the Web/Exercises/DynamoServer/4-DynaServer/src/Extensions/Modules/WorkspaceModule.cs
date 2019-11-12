using Dynamo.Models;
using DynaServer.Extensions;
using Nancy;
using System;
using System.IO;

namespace DynaServer.Server
{
    /// <summary>
    /// Module that handles interactions with the Dynamo workspace itself, allowing you to open, save, close, run graphs and more.
    /// </summary>
    public class WorkspaceModule : NancyModule
    {
        public const string DYNAMO_FILE_EXTENSION = ".DYN";

        public WorkspaceModule() : base("/Workspace")
        {
            Get["/"] = CurrentFile;
            Get["/Current"] = CurrentFile;

            Get["/Open/{filepath}"] = OpenFile;
            Get["/Close"] = CloseFile;
            Get["/Run"] = RunGraph;
            Get["/Save"] = SaveFile;
        }

        /// <summary>
        /// Instruct Dynamo to open the specified file, must have the .dyn extension.
        /// If any file is currently open, it closes it before opening the new one.
        /// </summary>
        /// <param name="parameters">Requires a property called "filepath", which specified the filepath to load from.</param>
        /// <returns>HTML result summary.</returns>
        public dynamic OpenFile(dynamic parameters)
        {
            if (string.IsNullOrWhiteSpace(parameters.filepath)) return "Empty filepath supplied";

            string result = "";
            string file = parameters.filepath;
            if (!file.ToUpper().EndsWith(DYNAMO_FILE_EXTENSION)) return "File does not have the correct extension (.DYN)";
            if (!File.Exists(file)) return 404;

            ServerViewExtension.RunInContext(() =>
            {
                var vm = ServerViewExtension.dynamoViewModel;

                vm.CloseHomeWorkspaceCommand.Execute(null);
                vm.OpenCommand.Execute(file);
                result = ServerViewExtension.viewLoadedParams.CurrentWorkspaceModel.FileName;
            });

            return Response.AsText("Opened file : " + result, "text/html");
        }

        /// <summary>
        /// Asks Dynamo what the path & name of the currently open file is.
        /// </summary>
        /// <param name="parameters">Not used, can specify null.</param>
        /// <returns>HTML result summary.</returns>
        public dynamic CurrentFile(dynamic parameters)
        {
            string filePath = "";
            string fileName = "";
            ServerViewExtension.RunInContext(() =>
            {
                filePath = ServerViewExtension.viewLoadedParams.CurrentWorkspaceModel.FileName;
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    filePath = "File has not been saved yet";
                    fileName = "Home - Default Name";
                }
                else
                {
                    int ind = filePath.LastIndexOf("\\");
                    fileName = filePath.Remove(0, (ind + 1));
                }
            }
            );

            var html = @"<h3>File Path of Current File</h3>" + filePath + "</br>" +  @"<h3>File Name of Current File</h3>" + fileName;

            return Response.AsText(html, "text/html");
        }

        /// <summary>
        /// Uploads a graph (.dyn) file to Dynamo and instruct Dynamo to open it.
        /// </summary>
        /// <param name="parameters">Not used, can specify null.</param>
        /// <returns>HTML result summary.</returns>
        public dynamic UploadFile(dynamic parameters)
        {
            string file = "";
            ServerViewExtension.RunInContext(() =>
            {
                file = ServerViewExtension.viewLoadedParams.CurrentWorkspaceModel.FileName;
            }
            );

            var html = @"<h3>Currently open file</h3></br>" + file;

            return Response.AsText(html, "text/html");
        }

        /// <summary>
        /// Saves the currently open Dynamo file.
        /// </summary>
        /// <param name="parameters">Not used, can specify null.</param>
        /// <returns>HTML result summary.</returns>
        public dynamic SaveFile(dynamic parameters)
        {
            string file = "";
            string result = "";

            ServerViewExtension.RunInContext(() =>
            {
                file = ServerViewExtension.viewLoadedParams.CurrentWorkspaceModel.FileName;
                if (string.IsNullOrWhiteSpace(file)) result = "No file is open, did not save anything.";
                else
                {
                    ServerViewExtension.DynamoLogger.Log("Saving " + file);
                    ServerViewExtension.dynamoViewModel.SaveCommand.Execute(null);
                    result = "Successfully Saved file : " + file;
                }
            }
            );

            return result;
        }

        /// <summary>
        /// Closes the currently open Dynamo file, if any.
        /// </summary>
        /// <param name="parameters">Not used, can specify null.</param>
        /// <returns>HTML result summary.</returns>
        public dynamic CloseFile(dynamic parameters)
        {
            string file = "";

            ServerViewExtension.RunInContext(() =>
            {
                file = ServerViewExtension.viewLoadedParams.CurrentWorkspaceModel.FileName;
                ServerViewExtension.DynamoLogger.Log("Closed " + file);
                ServerViewExtension.dynamoViewModel.CloseHomeWorkspaceCommand.Execute(null);
            }
            );

            return "Closed file : " + file;
        }

        /// <summary>
        /// Forces Dynamo to run the currently open graph.
        /// </summary>
        /// <param name="parameters">Not used, can specify null.</param>
        /// <returns>HTML result summary.</returns>
        public dynamic RunGraph(dynamic parameters)
        {
            ServerViewExtension.RunInContext(() =>
            {
                var vm = ServerViewExtension.dynamoViewModel;
                vm.CurrentSpaceViewModel.RunSettingsViewModel.Model.RunType = RunType.Manual;
                vm.Model.ForceRun();
            });

            return "ran";
        }
    }
}
