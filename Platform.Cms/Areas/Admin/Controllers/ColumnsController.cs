using System.Collections.Generic;
using System.Data;
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
    public class ColumnsController : MvcCrudController<PlatformDomainContext, TablesRepository, Table, int>
    {
        private readonly PlatformDomainContext _domainContext;

        public ColumnsController(PlatformDomainContext domainContext, TablesRepository repository, IUnitOfWork<PlatformDomainContext> unitOfWork) : base(repository, unitOfWork)
        {
            _domainContext = domainContext;
        }

        public ActionResult AddColumn(int id)
        {
            if (Request.IsAjaxRequest())
            {
                var vm = new CreateColumnViewModel(id);
                vm.UseContext(_domainContext);

                return PartialView(vm);
            }
            return new EmptyResult();
        }

        [HttpPost]
        public async Task<ActionResult> AddColumn(CreateColumnViewModel vm)
        {
            UnitOfWork.SetUser(User.Identity);

            Table table;
            vm.Validate(ModelState);
            if (!ModelState.IsValid)
            {
                table = await GetItemAsync(vm.TableId);
                var items = table.Columns.Select(c => new ColumnsListViewModel(c, vm.TableId)).ToList();

                ViewBag.Errors = ModelState.SelectMany(e => e.Value.Errors).Select(e => e.ErrorMessage);

                return PartialView("GetColumns", new ListResult<ColumnsListViewModel>(items, items.Count(), new QueryModel()));
            }

            using (var tx = UnitOfWork.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                table = await GetItemAsync(vm.TableId);
                var column = new Column();
                
                vm.FillEntity(column);

                UnitOfWork.Add(column);

                table.Columns.Add(column);

                await UnitOfWork.SaveChangesAsync();

                table = await GetItemAsync(vm.TableId);
                var items = table.Columns.Select(c => new ColumnsListViewModel(c, vm.TableId)).ToList();

                tx.Commit();

                return PartialView("GetColumns", new ListResult<ColumnsListViewModel>(items, items.Count(), new QueryModel()));
            }

            
        }

        public async Task<ActionResult> GetColumns(int id)
        {
            var table = await GetItemAsync(id);
            var items = table.Columns.Select(c => new ColumnsListViewModel(c, id)).ToList();

            return PartialView(new ListResult<ColumnsListViewModel>(items, items.Count(), new QueryModel()));
        }

        public async Task<ActionResult> Remove(int id, int tableid)
        {
            UnitOfWork.SetUser(User.Identity);

            using (var tx = UnitOfWork.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                var table = await GetItemAsync(tableid);
                var column = table.Columns.SingleOrDefault(c => c.Id == id);

                if (column != null)
                {
                    table.Columns.Remove(column);

                    UnitOfWork.Delete(column);

                    await UnitOfWork.SaveChangesAsync();
                }
                else
                {
                    ViewBag.Errors = new List<string> {"Nie można odnaleźć kolumny o id: " + id};
                }
                
                table = await GetItemAsync(tableid);
                var items = table.Columns.Select(c => new ColumnsListViewModel(c, tableid)).ToList();

                tx.Commit();

                return PartialView("GetColumns", new ListResult<ColumnsListViewModel>(items, items.Count(), new QueryModel()));
            }
        }
    }
}