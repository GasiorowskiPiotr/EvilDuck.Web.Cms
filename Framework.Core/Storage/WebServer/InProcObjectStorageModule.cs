using Autofac;

namespace EvilDuck.Framework.Core.Storage.WebServer
{
    public class InProcObjectStorageModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InProcObjectStorage>().As<IObjectStorage>().InstancePerRequest();
        }
    }
}