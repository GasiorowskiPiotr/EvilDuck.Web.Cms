using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using EvilDuck.Framework.Core;

namespace EvilDuck.Platform.Cms
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            FrameworkStarter.Start();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
