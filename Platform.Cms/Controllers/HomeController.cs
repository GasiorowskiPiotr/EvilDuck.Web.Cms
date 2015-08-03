using System.Web.Mvc;

namespace EvilDuck.Platform.Cms.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
