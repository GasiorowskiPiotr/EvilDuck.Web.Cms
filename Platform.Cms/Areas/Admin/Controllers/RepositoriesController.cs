using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EvilDuck.Framework.Core.DataAccess;
using EvilDuck.Framework.Core.Web;
using EvilDuck.Framework.Core.Web.Mvc;
using EvilDuck.Platform.Cms.Areas.Admin.Models.Repositories;
using EvilDuck.Platform.Core.DataAccess;
using EvilDuck.Platform.Core.DataFramework.Repositories;
using EvilDuck.Platform.Entities.DataFramework;

namespace EvilDuck.Platform.Cms.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RepositoriesController : MvcCrudController<PlatformDomainContext, RepositoriesRepository, Repository, int>
    {
        private readonly QueriesRepository _queriesRepository;
        // GET: Admin/Repositories
        public RepositoriesController(RepositoriesRepository repository, IUnitOfWork<PlatformDomainContext> unitOfWork, QueriesRepository queriesRepository) : base(repository, unitOfWork)
        {
            _queriesRepository = queriesRepository;
        }

        public async Task<ActionResult> Index(QueryModel query)
        {
            var result = await GetItemsAsync(query);
            ViewBag.QueryModel = result.QueryModel;
            ViewBag.AllCount = result.AllCount;

            var items = result.Entities.Select(e => new RepositoriesListViewModel(e)).ToList();
            if (Request.IsAjaxRequest())
                return PartialView(items);
            return View(items);
        }

        public async Task<ActionResult> Add()
        {
            var vm = await PrepareCreatorViewModel<CreateRepositoryViewModel>();
            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> Add(CreateRepositoryViewModel vm)
        {
            if (await CreateFromAsync(vm) != null)
            {
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> Remove(int id)
        {
            await RemoveAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int id)
        {
            var vm = await PrepareEditorViewModel<EditRepositoryViewModel>(id);
            return View(vm);
        }

        protected override void ViewModelToEntity<TViewModel>(TViewModel viewModel, Repository entity)
        {
            base.ViewModelToEntity(viewModel, entity);
            var createViewModel = viewModel as CreateRepositoryViewModel;
            if (createViewModel != null)
            {
                entity.DeleteQuery = createViewModel.DeleteQuery == 0 ? null : _queriesRepository.GetByKey(createViewModel.DeleteQuery);
                entity.InsertQuery = createViewModel.InsertQuery == 0 ? null : _queriesRepository.GetByKey(createViewModel.InsertQuery);
                entity.UpdateQuery = createViewModel.UpdateQuery == 0 ? null : _queriesRepository.GetByKey(createViewModel.UpdateQuery);
                entity.SelectQueries = createViewModel.SelectQueries == null || !createViewModel.SelectQueries.Any() ? null : _queriesRepository.GetManyByKey(createViewModel.SelectQueries);
                return;
            }

            var editorViewModel = viewModel as EditRepositoryViewModel;
            if (editorViewModel != null)
            {
                entity.DeleteQuery = editorViewModel.DeleteQuery == 0 ? null : _queriesRepository.GetByKey(editorViewModel.DeleteQuery);
                entity.InsertQuery = editorViewModel.InsertQuery == 0 ? null : _queriesRepository.GetByKey(editorViewModel.InsertQuery);
                entity.UpdateQuery = editorViewModel.UpdateQuery == 0 ? null : _queriesRepository.GetByKey(editorViewModel.UpdateQuery);
                entity.SelectQueries = editorViewModel.SelectQueries == null || !editorViewModel.SelectQueries.Any() ? null : _queriesRepository.GetManyByKey(editorViewModel.SelectQueries);
            }
        }

        
    }
}