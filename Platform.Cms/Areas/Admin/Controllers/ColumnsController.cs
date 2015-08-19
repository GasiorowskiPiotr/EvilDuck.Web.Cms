using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
        public ColumnsController(TablesRepository repository, IUnitOfWork<PlatformDomainContext> unitOfWork) : base(repository, unitOfWork)
        {
        }

        public ActionResult AddColumn(int id)
        {
            if (Request.IsAjaxRequest())
                return PartialView(new CreateColumnViewModel(id));
            return new EmptyResult();
        }

        [HttpPost]
        public async Task<ActionResult> AddColumn(CreateColumnViewModel vm)
        {
            UnitOfWork.SetUser(User.Identity);

            using (var tx = UnitOfWork.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                var table = await GetItemAsync(vm.TableId);
                var column = new Column();

                vm.FillEntity(column);

                table.Columns.Add(column);

                await UnitOfWork.SaveChangesAsync();
                tx.Commit();
            }

            return null; // TODO
        }

        public async Task<ActionResult> GetColumns(int id)
        {
            var table = await GetItemAsync(id);
            var items = table.Columns.Select(c => new ColumnsListViewModel(c, id)).ToList();

            return PartialView(new ListResult<ColumnsListViewModel>(items, items.Count(), new QueryModel()));
        }
    }
}