using System.Web.Http;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using EvilDuck.Platform.Cms;
using EvilDuck.Platform.Core.Security;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace EvilDuck.Platform.Cms
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            var dependencyResolver =
                (AutofacWebApiDependencyResolver)GlobalConfiguration.Configuration.DependencyResolver;
            app.UseAutofacMiddleware(dependencyResolver.Container);
            ConfigureAuth(app);
        }
    }
}
