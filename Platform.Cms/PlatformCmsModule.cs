using Autofac;
using Autofac.Integration.Mvc;
using EvilDuck.Framework.Core.Modularity;
using EvilDuck.Platform.Cms;
using EvilDuck.Platform.Cms.Areas.Admin.Controllers;
using EvilDuck.Platform.Core.DataAccess;

[assembly: AssemblyPluginDescriptor(typeof(PlatformPlugin))]

namespace EvilDuck.Platform.Cms
{
    public class PlatformCmsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterControllers(typeof (PlatformCmsModule).Assembly);

            builder.RegisterType<UsersController>().InstancePerRequest();
            builder.RegisterType<RolesController>().InstancePerRequest();
            builder.RegisterType<TablesController>().InstancePerRequest();
            builder.RegisterUnitOfWork<PlatformUnitOfWork, PlatformDomainContext>();
            builder.RegisterDomainContext<PlatformDomainContext>();
        }
    }
}