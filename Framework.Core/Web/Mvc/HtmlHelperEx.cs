using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EvilDuck.Framework.Core.Web.Mvc
{
    public static class HtmlHelperEx
    {
        public static EvilDuckHelper EvilDuck(this HtmlHelper html)
        {
            return new EvilDuckHelper(html);
        }
    }

    public class EvilDuckHelper
    {
        private readonly HtmlHelper _html;

        public EvilDuckHelper(HtmlHelper html)
        {
            _html = html;
        }

        public string ActiveMenuItem(params string[] possibleControllers)
        {
            if (possibleControllers == null)
                return String.Empty;

            if (possibleControllers.Contains(_html.ViewContext.RouteData.Values["controller"]))
            {
                return "active";
            }
            return String.Empty;
        } 
    }
}
