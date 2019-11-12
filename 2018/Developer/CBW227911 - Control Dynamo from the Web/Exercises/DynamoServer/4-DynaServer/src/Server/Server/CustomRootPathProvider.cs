using Nancy;
using System;
using System.IO;

namespace DynaServer.Server
{
    public class CustomRootPathProvider : IRootPathProvider
    {
        public string GetRootPath()
        {
            return 
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + 
                @"\Dynamo\Dynamo Core\2.0\packages\DynaServer";
        }
    }
}
