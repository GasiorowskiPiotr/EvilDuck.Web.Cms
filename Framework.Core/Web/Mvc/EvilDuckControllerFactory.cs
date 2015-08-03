using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EvilDuck.Framework.Core.Web.Mvc
{
    public class EvilDuckControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                throw new HttpException(404, "not found");
            return (IController)DependencyResolver.Current.GetService(controllerType);
        }

        public static void RegisterSelf()
        {
            ControllerBuilder.Current.SetControllerFactory(new EvilDuckControllerFactory());
        }
    }
}