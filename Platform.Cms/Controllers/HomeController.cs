using System.Web;
using System.Web.Mvc;

namespace EvilDuck.Platform.Cms.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var a = HttpContext.GetOwinContext();
            return View();
        }
    }
}
