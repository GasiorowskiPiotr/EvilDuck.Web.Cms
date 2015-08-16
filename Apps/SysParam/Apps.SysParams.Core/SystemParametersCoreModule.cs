using Autofac;
using EvilDuck.Applications.SystemParameters.Core.Controllers;
using EvilDuck.Applications.SystemParameters.Core.DataAccess;
using EvilDuck.Framework.Core.Modularity;

namespace EvilDuck.Applications.SystemParameters.Core
{
    public class SystemParametersCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterDomainContext<SystemParametersDomainContext>();
            builder.RegisterUnitOfWork<SystemParametersUnitOfWork, SystemParametersDomainContext>();
            builder.RegisterType<SystemParametersRepository>().AsSelf().InstancePerRequest();
            builder.RegisterType<SystemParametersController>().AsSelf().InstancePerRequest();
        }
    }
}