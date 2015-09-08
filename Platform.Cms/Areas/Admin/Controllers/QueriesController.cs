using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EvilDuck.Framework.Core.DataAccess;
using EvilDuck.Framework.Core.Web;
using EvilDuck.Framework.Core.Web.Mvc;
using EvilDuck.Platform.Cms.Areas.Admin.Models.Queries;
using EvilDuck.Platform.Core.DataAccess;
using EvilDuck.Platform.Core.DataFramework.Repositories;
using EvilDuck.Platform.Entities.DataFramework;

namespace EvilDuck.Platform.Cms.Areas.Admin.Controllers
{
    public class QueriesController : MvcCrudController<PlatformDomainContext, QueriesRepository, Query, int>
    {
        // GET: Admin/Queries
        public QueriesController(QueriesRepository repository, IUnitOfWork<PlatformDomainContext> unitOfWork) : base(repository, unitOfWork)
        {
        }

        public async Task<ActionResult> Index(QueryModel query)
        {
            var result = await GetItemsAsync(query);
            ViewBag.QueryModel = result.QueryModel;
            ViewBag.AllCount = result.AllCount;

            var items = result.Entities.Select(e => new QueryListViewModel(e)).ToList();
            if (Request.IsAjaxRequest())
                return PartialView(items);
            return View(items);
        }

        public ActionResult Add()
        {
            return View(new AddQueryViewModel());
        }

        [HttpPost]
        public ActionResult Add(AddQueryViewModel vm)
        {
            Query query;
            if (CreateFrom(vm, out query))
            {
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> Remove(int id)
        {
            await this.RemoveAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int id)
        {
            return View(await PrepareEditorViewModel<EditQueryViewModel>(id));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditQueryViewModel vm)
        {
            var query = await UpdateFromAsync(vm.Id, vm);
            if (query == null)
            {
                return View(vm);
            }
            return RedirectToAction("Index");
        }


    }
}