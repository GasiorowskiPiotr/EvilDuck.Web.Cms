using System.Web.Mvc;

namespace EvilDuck.Platform.Cms.Areas.Admin.Controllers
{
    public class SystemParametersController : Controller
    {
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View();
        }
    }
}