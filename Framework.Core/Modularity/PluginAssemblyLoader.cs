using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EvilDuck.Framework.Core.Configuration;
using NLog;

namespace EvilDuck.Framework.Core.Modularity
{
    public class PluginAssemblyLoader
    {
        private static Logger _logger;

        private static IList<Plugin> _plugins = new List<Plugin>();
        private static object _lock = new object();

        static PluginAssemblyLoader()
        {
            _logger = LogManager.GetLogger(typeof(PluginAssemblyLoader).FullName);
        }

        public static Assembly[] LoadPluginAssemblies()
        {
            var config = TypedConfiguration.GetSection<GeneralSettingsSection>(GeneralSettingsSection.SectionName);
            var assms = config.PluginAssemblies;

            if (_logger.IsInfoEnabled)
            {
                _logger.Info("Loading plugin assemblies: {0}", assms);
            }

            var asmArr = assms.Split(',');

            return asmArr.Select(Assembly.Load).ToArray();
        }

        public static Type[] LoadPluginTypes(Assembly[] assemblies)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("Loading assemblies: {0}", String.Join(",", assemblies.Select(a => a.FullName)));
            }
            return assemblies.Select(assembly => assembly.GetCustomAttribute<AssemblyPluginDescriptorAttribute>()).Select(attr => attr.PluginType).ToArray();
        }

        public static Plugin[] InstantiatePlugins(Type[] types)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("Instantiating plugins: {0}", String.Join(",", types.Select(a => a.FullName)));
            }
            return types.Select(Activator.CreateInstance).Cast<Plugin>().ToArray();
        }

        public static Plugin[] GetPlugins()
        {
            if (_plugins.Count == 0)
            {
                lock (_lock)
                {
                    if (_plugins.Count == 0)
                    {
                        _plugins = InstantiatePlugins(LoadPluginTypes(LoadPluginAssemblies()));
                    }
                }
            }
            return _plugins.ToArray();
        }
    }
}