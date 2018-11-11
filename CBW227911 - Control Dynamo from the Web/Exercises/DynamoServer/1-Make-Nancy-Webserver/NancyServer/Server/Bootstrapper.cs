using Nancy;
using Nancy.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NancyServer
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private const string _password = "dynamo";

        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get { return new DiagnosticsConfiguration { Password = _password }; }
        }
    }
}
