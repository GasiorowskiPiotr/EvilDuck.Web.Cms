using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using EvilDuck.Framework.Core;
using EvilDuck.Framework.Core.Modularity;
using EvilDuck.Platform.Core;

namespace EvilDuck.Platform.Cms
{
    public class PlatformPlugin : Plugin
    {
        public override IEnumerable<Module> GetModules(EnvironmentType environmentType)
        {
            yield return new PlatformCoreModule();
            yield return new PlatformCmsModule();
        }

        public override void ConfigureMvcRoutes(RouteCollection routeCollection)
        {
            routeCollection.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routeCollection.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
        }
    }
}