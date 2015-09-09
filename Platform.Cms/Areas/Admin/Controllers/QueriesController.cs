using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EvilDuck.Framework.Core.DataAccess;
using EvilDuck.Framework.Core.Web;
using EvilDuck.Framework.Core.Web.Mvc;
using EvilDuck.Platform.Cms.Areas.Admin.Models.Queries;
using EvilDuck.Platform.Core.DataAccess;
using EvilDuck.Platform.Core.DataFramework.Logic;
using EvilDuck.Platform.Core.DataFramework.Repositories;
using EvilDuck.Platform.Entities.DataFramework;

namespace EvilDuck.Platform.Cms.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class QueriesController : MvcCrudController<PlatformDomainContext, QueriesRepository, Query, int>
    {
        private readonly QueryComponentFactory _queryComponentFactory;
        // GET: Admin/Queries
        public QueriesController(QueriesRepository repository, IUnitOfWork<PlatformDomainContext> unitOfWork, QueryComponentFactory queryComponentFactory) : base(repository, unitOfWork)
        {
            _queryComponentFactory = queryComponentFactory;
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


        public ActionResult Validate(int id)
        {
            var query = GetItem(id);

            var vm = new ValidateQueryViewModel();
            vm.QueryId = id;

            if (query.QueryParams != null)
            {
                var parameterNames = query.QueryParams.Split('|').Select(e => e.Replace("@", String.Empty));

                foreach (var parameterName in parameterNames)
                {
                    vm.AddParameter(parameterName);
                    vm.ParameterNames.Add(parameterName);
                }
            }

            return PartialView(vm);
        }

        [HttpPost]
        public ActionResult Validate(FormCollection vm)
        {
            if (!String.IsNullOrEmpty(vm["QueryId"]))
            {
                var queryId = int.Parse(vm["QueryId"]);

                var queryComponent = _queryComponentFactory.CreateQueryComponent(queryId);

                var parameters = vm.AllKeys.Where(k => k.StartsWith("Parameters.")).ToDictionary(k => k.Replace("Parameters.", String.Empty), k => (object)vm[k]);

                QueryResult queryResult;
                var result = queryComponent.Test(out queryResult, parameters);

                return PartialView("QueryValidationResult", new QueryValidationResultViewModel()
                {
                    Data = queryResult,
                    IsSuccess = result.IsSuccess,
                    Message = result.Message
                });
            }
            

            return new EmptyResult();
        }
    }
}