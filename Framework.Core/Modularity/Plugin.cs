using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using NLog;
using Owin;

namespace EvilDuck.Framework.Core.Modularity
{
    public abstract class Plugin
    {
        protected Plugin()
        {
            LogManager.GetLogger(GetType().FullName).Info("Creating Plugin");
        }

        public abstract IEnumerable<Module> GetModules(EnvironmentType environmentType);

        public virtual void ConfigureMvcRoutes(RouteCollection routeCollection)
        {

        }

        public virtual void ConfigureMvcFilters(GlobalFilterCollection filters)
        {

        }

        public virtual void ConfigureOwin(IAppBuilder builder)
        {

        }

        public virtual void ConfigureBundles(BundleCollection bundles, EnvironmentType environmentType)
        {

        }
    }
}