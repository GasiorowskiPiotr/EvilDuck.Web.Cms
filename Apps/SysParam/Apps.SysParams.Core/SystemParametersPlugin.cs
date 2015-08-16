using System.Collections.Generic;
using Autofac;
using EvilDuck.Applications.SystemParameters.Core;
using EvilDuck.Framework.Core;
using EvilDuck.Framework.Core.Modularity;

[assembly:AssemblyPluginDescriptor(typeof(SystemParametersPlugin))]
namespace EvilDuck.Applications.SystemParameters.Core
{
    public class SystemParametersPlugin : Plugin
    {
        public override IEnumerable<Module> GetModules(EnvironmentType environmentType)
        {
            yield return new SystemParametersCoreModule();
        }
    }
}