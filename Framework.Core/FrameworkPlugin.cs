using System.Collections.Generic;
using Autofac;
using Autofac.Integration.Mvc;
using EvilDuck.Framework.Core;
using EvilDuck.Framework.Core.Logging;
using EvilDuck.Framework.Core.Modularity;
using EvilDuck.Framework.Core.Storage.WebServer;

[assembly: AssemblyPluginDescriptor(typeof(FrameworkPlugin))]
namespace EvilDuck.Framework.Core
{
    public class FrameworkPlugin : Plugin
    {
        public override IEnumerable<Module> GetModules(EnvironmentType environmentType)
        {
            yield return new AutofacWebTypesModule();
            yield return new InProcObjectStorageModule();
            yield return new LoggingModule();

        }
    }
}