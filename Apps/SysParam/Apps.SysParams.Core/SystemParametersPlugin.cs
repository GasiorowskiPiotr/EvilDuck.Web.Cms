using System.Collections.Generic;
using Autofac;
using EvilDuck.Framework.Core;
using EvilDuck.Framework.Core.Modularity;

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