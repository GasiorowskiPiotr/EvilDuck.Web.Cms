using System.Collections.Generic;
using System.Linq;
using Autofac;
using EvilDuck.Framework.Core.Configuration;
using EvilDuck.Framework.Core.Utils;
using NLog;

namespace EvilDuck.Framework.Core.Modularity
{
    public class ModularityBootstrap
    {
        public static IContainer Start(IEnumerable<Plugin> plugins)
        {
            var config = TypedConfiguration.GetSection<GeneralSettingsSection>(GeneralSettingsSection.SectionName);

            LogManager.GetLogger(typeof(ModularityBootstrap).FullName).Info("Bootstrapping modules for environment: {0}", config.EnvironmentType);

            var cb = new ContainerBuilder();
            plugins.SelectMany(p => p.GetModules(config.EnvironmentType)).Do(m => cb.RegisterModule(m));
            return cb.Build();
        }
    }
}