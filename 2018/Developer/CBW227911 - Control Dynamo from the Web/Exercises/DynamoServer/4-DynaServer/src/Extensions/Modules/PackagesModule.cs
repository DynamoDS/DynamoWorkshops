using Dynamo.Extensions;
using DynaServer.Extensions;
using Nancy;
using System.Collections.Generic;
using System.Linq;

namespace DynaServer.Server
{
    /// <summary>
    /// Module to handle packages in Dynamo, allowing you to list them or install new packages or remove existing ones.
    /// </summary>
    public class PackagesModule : NancyModule
    {
        public PackagesModule() : base("/Packages")
        {
            Get["/"] = GetPackages;
            Get["/Install/{packagename}"] = InstallPackage;
            Get["/Remove/{packagename}"] = RemovePackage;
        }

        /// <summary>
        /// Get the list of currently installed packages in Dynamo.
        /// </summary>
        /// <param name="parameters">Not used, can specify null.</param>
        /// <returns>HTML result summary.</returns>
        public dynamic GetPackages(dynamic parameters)
        {
            string html = "";
            var packageNames = new List<string>();
            var uniqueLibs = new HashSet<string>();


            ServerViewExtension.RunInContext(() =>
            {
                var nsm = ServerViewExtension.dynamoViewModel.Model.SearchModel;
                List<Dynamo.Search.SearchElements.NodeSearchElement> nodes = nsm.SearchEntries.ToList();

                var libs = nodes
                    .Where(x => x.Categories.Count > 0)
                    .Select(x => x.Categories.FirstOrDefault())
                    .ToList();

                libs.Sort();
                uniqueLibs = new HashSet<string>(libs);

                packageNames = ServerViewExtension.dynamoViewModel.Model.ExtensionManager.Extensions
                    .Select(x => x.Name)
                    .ToList();
            });

            html = "<h2>Currently installed pacakges : </h2>" +
                 "<ul>";

            foreach (var item in uniqueLibs)
            {
                html += "<li>" + item + "</li>";
            }
            html += "</ul></br>";

            html += "<h2>Currently installed extensions : </h2>" +
                   "<ul>";

            foreach (var item in packageNames)
            {
                html += "<li>" + item + "</li>";
            }
            html += "</ul></br>";
            html += "<em>Note : only Extensions are listed above, does not include ViewExtensions.";

            return Response.AsText(html, "text/html");
        }

        /// <summary>
        /// Installs the specified package in Dynamo, using the Package Manager.
        /// </summary>
        /// <param name="parameters">The name of the package to install.</param>
        /// <returns>HTML result summary.</returns>
        public dynamic InstallPackage(dynamic parameters)
        {
            string packageName = parameters.packagename;
            if (string.IsNullOrWhiteSpace(packageName)) return "Supplied package name was invalid or empty.";

            string result = "";
            int packageCountBefore = 0, packageCountAfter = 0;

            ServerViewExtension.RunInContext(() =>
            {
                try
                {
                    // TODO : implement package install
                    result = "To be implemented.";
                }
                catch (System.Exception e)
                {
                    result = "Something went wrong, error : " + e.Message;
                }
            });

            if (packageCountAfter > packageCountBefore) result = $"Installed {packageCountBefore - packageCountAfter} packages from Dynamo.";
            else result = "Did not install any package to Dynamo.";

            return Response.AsText(result, "text/html");
        }

        /// <summary>
        /// Remove the specified package from Dynamo, using the Package Manager.
        /// </summary>
        /// <param name="parameters">The name of the package to remove.</param>
        /// <returns>HTML result summary.</returns>
        public dynamic RemovePackage(dynamic parameters)
        {
            string html = "";
            int packageCountBefore = 0, packageCountAfter = 0;

            ServerViewExtension.RunInContext(() =>
            {
                // TODO : implement package removal
            }
            );

            if (packageCountAfter < packageCountBefore) html = $"Removed {packageCountBefore - packageCountAfter} packages from Dynamo.";
            else html = "Did not remove any packages from Dynamo.";

            return Response.AsText(html, "text/html");
        }
    }
}
