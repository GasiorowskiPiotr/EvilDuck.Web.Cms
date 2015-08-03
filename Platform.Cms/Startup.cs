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
            ConfigureAuth(app);
        }
    }
}
