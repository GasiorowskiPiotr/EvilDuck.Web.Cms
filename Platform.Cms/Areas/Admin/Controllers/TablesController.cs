using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EvilDuck.Framework.Core.DataAccess;
using EvilDuck.Framework.Core.Web;
using EvilDuck.Framework.Core.Web.Mvc;
using EvilDuck.Platform.Cms.Areas.Admin.Models.Tables;
using EvilDuck.Platform.Core.DataAccess;
using EvilDuck.Platform.Core.DataFramework.Repositories;
using EvilDuck.Platform.Entities.DataFramework;

namespace EvilDuck.Platform.Cms.Areas.Admin.Controllers
{
    [Authorize]
    public class TablesController : MvcCrudController<PlatformDomainContext, TablesRepository, Table,int>
    {
        // GET: Admin/Tables
        public TablesController(TablesRepository repository, IUnitOfWork<PlatformDomainContext> unitOfWork) : base(repository, unitOfWork)
        {
        }

        public async Task<ActionResult> Index(QueryModel queryModel)
        {
            var result = await GetItemsAsync(queryModel);
            ViewBag.QueryModel = result.QueryModel;
            ViewBag.AllCount = result.AllCount;

            var items = result.Entities.Select(e => new TableListViewModel(e)).ToList();
            if (Request.IsAjaxRequest())
                return PartialView(items);
            return View(items);
        }

        protected override IQueryable<Table> PreFilter(IQueryable<Table> query)
        {
            return query.Include(e => e.Columns);
        }

        public ActionResult Add()
        {
            return View(new AddTableViewModel());
        }

        [HttpPost]
        public ActionResult Add(AddTableViewModel vm)
        {
            Table table;
            if (CreateFrom(vm, out table))
            {
                return RedirectToAction("Edit", new {id = table.Id});
            }
            return View(vm);
        }

        public async Task<ActionResult> Edit(int id)
        {
            return View(await PrepareEditorViewModel<EditTableViewModel>(id));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditTableViewModel vm)
        {
            await UpdateFromAsync(vm.Id, vm);
            return View(await PrepareEditorViewModel<EditTableViewModel>(vm.Id));
        }

        


    }
}