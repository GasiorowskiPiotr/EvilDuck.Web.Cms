using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using EvilDuck.Framework.Core.DataAccess;
using EvilDuck.Framework.Entities;
using Newtonsoft.Json;
using NLog;

namespace EvilDuck.Framework.Core.Web.Mvc
{
    public abstract class MvcCrudController<TContext, TRepository, TEntity, TKey> : Controller
        where TContext : DomainContext
        where TRepository : EntityRepository<TContext, TEntity, TKey>
        where TEntity : Entity<TKey>, new()
    {
        protected readonly TRepository Repository;
        protected readonly IUnitOfWork<TContext> UnitOfWork;
        protected readonly Logger Logger;

        protected MvcCrudController(TRepository repository, IUnitOfWork<TContext> unitOfWork)
        {
            Repository = repository;
            UnitOfWork = unitOfWork;
            Logger = LogManager.GetLogger(GetType().FullName);
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            UnitOfWork.SetUser(User.Identity);
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Settings = { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }
            };
        }

        protected ListResult<TEntity> GetItems(QueryModel queryModel)
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.Info("Getting items for Query: {0}", queryModel == null ? "[null]" : queryModel.ToString());
            }
            int allCount;
            var query = Repository.AdHocQuery();
            query = PreFilter(query);
            if (queryModel != null)
            {
                query = Filter(query, queryModel.FilterField, queryModel.FilterValue,
                    (FilterOper)Enum.Parse(typeof(FilterOper), queryModel.FilterOper));

                allCount = query.Count();

                query = Order(query, queryModel.OrderBy, (OrderDir)queryModel.OrderDir);
                query = Page(query, queryModel.Take, queryModel.Skip);
            }
            else
            {
                allCount = query.Count();

                query = Order(query, String.Empty, OrderDir.Asc);
                query = Page(query, 20, 0);
            }

            var items = query.ToList();
            if (Logger.IsInfoEnabled)
            {
                Logger.Info("Found {0} / {1} Entities.", items.Count, allCount);
            }

            return new ListResult<TEntity>(items, allCount, queryModel);
        }

        protected async Task<ListResult<TEntity>> GetItemsAsync(QueryModel queryModel)
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.Info("Getting items for Query: {0}", queryModel == null ? "[null]" : queryModel.ToString());
            }
            int allCount;
            var query = Repository.AdHocQuery();
            query = PreFilter(query);
            if (queryModel != null)
            {
                query = Filter(query, queryModel.FilterField, queryModel.FilterValue,
                    (FilterOper)Enum.Parse(typeof(FilterOper), queryModel.FilterOper));

                allCount = await query.CountAsync();

                query = Order(query, queryModel.OrderBy, (OrderDir)queryModel.OrderDir);
                query = Page(query, queryModel.Take, queryModel.Skip);
            }
            else
            {
                allCount = await query.CountAsync();

                query = Order(query, String.Empty, OrderDir.Asc);
                query = Page(query, 20, 0);
            }

            var items = await query.ToListAsync();
            if (Logger.IsInfoEnabled)
            {
                Logger.Info("Found {0} / {1} Entities.", items.Count, allCount);
            }

            return new ListResult<TEntity>(items, allCount, queryModel);
        }

        protected async Task<TEntity> CreateFromAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : CreateEntityViewModel<TEntity>
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.Info("Creating entity from ViewModel.");
            }
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("ViewModel: {0}", JsonConvert.SerializeObject(viewModel));
            }
            CustomValidate(viewModel);
            if (!ModelState.IsValid)
            {
                if (Logger.IsWarnEnabled)
                {
                    Logger.Warn("Validation failed: {0}", JsonConvert.SerializeObject(ModelState));
                }
                return null;
            }

            var entity = new TEntity();

            using (var tx = UnitOfWork.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("Mapping ViewModel to Entity.");
                }
                ViewModelToEntity(viewModel, entity);

                UnitOfWork.Add(entity);
                await UnitOfWork.SaveChangesAsync();

                tx.Commit();

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("New entity created.");
                }
            }

            return entity;
        } 

        protected bool CreateFrom<TViewModel>(TViewModel viewModel, out TEntity entity) where TViewModel : CreateEntityViewModel<TEntity>
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.Info("Creating entity from ViewModel.");
            }
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("ViewModel: {0}", JsonConvert.SerializeObject(viewModel));
            }
            CustomValidate(viewModel);
            if (!ModelState.IsValid)
            {
                if (Logger.IsWarnEnabled)
                {
                    Logger.Warn("Validation failed: {0}", JsonConvert.SerializeObject(ModelState));
                }
                entity = null;
                return false;
            }

            entity = new TEntity();

            using (var tx = UnitOfWork.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("Mapping ViewModel to Entity.");
                }
                ViewModelToEntity(viewModel, entity);

                UnitOfWork.Add(entity);
                UnitOfWork.SaveChanges();

                tx.Commit();

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("New entity created.");
                }
            }

            return true;
        }

        protected async Task<TViewModel> PrepareEditorViewModel<TViewModel>(TKey id) where TViewModel : EditEntityViewModel<TEntity, TKey>, new()
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.Info("Preparing Editor ViewModel for entity with id: {0}.", id);
            }
            var entity = await Repository.GetByKeyAsync(id);
            if (entity == null)
            {
                if (Logger.IsErrorEnabled)
                {
                    Logger.Error("Could not find entity with id: {0}", id);
                }
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var viewModel = new TViewModel();
            viewModel.FillFromEntity(entity);

            return viewModel;
        }

        protected async Task<TEntity> UpdateFromAsync<TViewModel>(TKey entityKey, TViewModel viewModel) where TViewModel : EditEntityViewModel<TEntity, TKey>
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.Info("Updating entity from ViewModel.");
            }
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("ViewModel: {0}", JsonConvert.SerializeObject(viewModel));
            }
            CustomValidate(viewModel);
            if (!ModelState.IsValid)
            {
                if (Logger.IsWarnEnabled)
                {
                    Logger.Warn("Validation failed: {0}", JsonConvert.SerializeObject(ModelState));
                }
                return null;
            }

            using (var tx = UnitOfWork.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("Getting entity from repository.");
                }
                var entity = await Repository.GetByKeyAsync(entityKey);
                if (entity == null)
                {
                    if (Logger.IsWarnEnabled)
                    {
                        Logger.Warn("Entity with key: {0} not found.", entityKey);
                    }
                    ModelState.AddModelError("Entity", "Podana encja nie istnieje w bazie");
                    return null;
                }

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("Mapping ViewModel to Entity.");
                }
                ViewModelToEntity(viewModel, entity);
                await UnitOfWork.SaveChangesAsync();

                tx.Commit();

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("Entity updated.");
                }

                return entity;
            }
        }

        protected async Task RemoveAsync(TKey key)
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.Info("Removing entity with key: {0}", key);
            }
            var entity = await Repository.GetByKeyAsync(key);
            if (entity == null)
            {
                if (Logger.IsWarnEnabled)
                {
                    Logger.Warn("Entity with key: {0} not found.", key);
                }
                return;
            }

            using (var tx = UnitOfWork.BeginTransaction(IsolationLevel.ReadCommitted))
            {

                DisattachReferences(entity);
                UnitOfWork.Delete(entity);
                await UnitOfWork.SaveChangesAsync();

                tx.Commit();

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("Entity with key: {0} removed.", key);
                }
            }
        }

        protected TEntity GetItem(TKey key)
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.Info("Getting Entity with key: {0}", key);
            }
            return Repository.GetByKey(key);
        }

        protected async Task<TEntity> GetItemAsync(TKey key)
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.Info("Getting Entity with key: {0}", key);
            }
            return await Repository.GetByKeyAsync(key);
        }

        protected virtual void DisattachReferences(TEntity entity)
        {

        }

        protected virtual void CustomValidate<TViewModel>(TViewModel viewModel)
        {
            var editableViewModel = viewModel as IValidatableViewModel;
            if (editableViewModel != null)
            {
                editableViewModel.Validate(ModelState);
            }
        }

        protected virtual void ViewModelToEntity<TViewModel>(TViewModel viewModel, TEntity entity) where TViewModel : IFillEntity<TEntity>
        {
            viewModel.FillEntity(entity);
        }

        protected virtual IQueryable<TEntity> PreFilter(IQueryable<TEntity> query)
        {
            return query;
        }

        protected virtual IQueryable<TEntity> Filter(IQueryable<TEntity> query, string filterField, string filterValue, FilterOper filterOper)
        {
            return query;
        }

        protected virtual IQueryable<TEntity> Order(IQueryable<TEntity> query, string orderBy, OrderDir orderDir)
        {
            return query.OrderBy(e => e.Id);
        }

        protected virtual IQueryable<TEntity> Page(IQueryable<TEntity> query, int take, int skip)
        {
            return query.Skip(skip).Take(take);
        }
    }
}