using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using EvilDuck.Framework.Core.Configuration;
using EvilDuck.Framework.Core.Modularity;
using EvilDuck.Framework.Core.Web.Api;
using EvilDuck.Framework.Core.Web.Mvc;
using NLog;

namespace EvilDuck.Framework.Core
{
    public static class FrameworkStarter
    {
        public static void Start()
        {
            var log = LogManager.GetLogger(typeof (FrameworkStarter).FullName);

            if (log.IsInfoEnabled)
            {
                log.Info("Initializing FRAMEWORK.");
            }

            if (log.IsInfoEnabled)
            {
                log.Info("Getting configured plugins.");
            }
            var plugins = PluginAssemblyLoader.GetPlugins();
            if (log.IsInfoEnabled)
            {
                log.Info("Plugins loaded.");
                log.Info("Creating component container.");
            }

            var container = ModularityBootstrap.Start(plugins);
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            if (log.IsInfoEnabled)
            {
                log.Info("Component container created and injected to WebAPI / MVC.");
                log.Info("Configuring MVC / WebAPI.");
            }

            GlobalConfiguration.Configure(WebApiConfig.Configure);
            ControllerBuilder.Current.SetControllerFactory(new EvilDuckControllerFactory());
            AreaRegistration.RegisterAllAreas();

            if (log.IsInfoEnabled)
            {
                log.Info("WebAPI / MVC configured.");
                log.Info("Loading plugin specific settings.");
            }

            var config = TypedConfiguration.GetSection<GeneralSettingsSection>(GeneralSettingsSection.SectionName);
            foreach (var plugin in plugins)
            {
                var pluginName = plugin.GetType().FullName;
                if (log.IsDebugEnabled)
                {
                    log.Debug("Configuring MVC filters for plugin: {0}", pluginName);
                }
                plugin.ConfigureMvcFilters(GlobalFilters.Filters);
                if (log.IsDebugEnabled)
                {
                    log.Debug("Configuring MVC routes for plugin: {0}", pluginName);
                }
                plugin.ConfigureMvcRoutes(RouteTable.Routes);
                if (log.IsDebugEnabled)
                {
                    log.Debug("Configuring Bundles for plugin: {0}", pluginName);
                }
                plugin.ConfigureBundles(BundleTable.Bundles, config.EnvironmentType);
            }

            if (log.IsInfoEnabled)
            {
                log.Info("FRAMEWORK started.");
            }
        }
    }
}