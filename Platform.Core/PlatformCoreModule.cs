using System.Collections.Generic;
using System.Web;
using Autofac;
using EvilDuck.Framework.Core;
using EvilDuck.Framework.Core.Modularity;
using EvilDuck.Platform.Core.DataAccess;
using EvilDuck.Platform.Core.Security;
using EvilDuck.Platform.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EvilDuck.Platform.Core
{
    public class PlatformCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterDomainContext<PlatformDomainContext>();
            builder.RegisterType<ApplicationDbContext>().As<ApplicationDbContext>().InstancePerRequest();
            builder.RegisterType<RolesStore>().As<IRoleStore<IdentityRole, string>>().AsSelf().InstancePerRequest();
            builder.RegisterType<UsersStore>().As<IUserStore<ApplicationUser>>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().As<UserManager<ApplicationUser>>().AsSelf().InstancePerRequest();
            builder.RegisterType<RolesManager>().As<RoleManager<IdentityRole>>().AsSelf().InstancePerRequest();

            builder.RegisterType<ApplicationSignInManager>().As<ApplicationSignInManager>().InstancePerRequest();
        }
    }

    
}