using System.Linq;
using System.Threading.Tasks;
using EvilDuck.Framework.Core.Components;
using EvilDuck.Framework.Entities;
using NLog;

namespace EvilDuck.Framework.Core.DataAccess
{
    public abstract class EntityRepository<TDomainContext, TEntity, TKey> : BaseComponent, IEntityRepository<TEntity, TKey>
        where TEntity : Entity<TKey>
        where TDomainContext : DomainContext
    {
        private readonly TDomainContext _context;

        protected EntityRepository(TDomainContext context, Logger logger)
            : base(logger)
        {
            _context = context;
        }

        public IQueryable<TEntity> AdHocQuery()
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Ad hoc query");
            }
            return _context.Set<TEntity>().AsQueryable();
        }

        public TEntity GetByKey(TKey key)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Getting entity by key: {0}", key);
            }
            return _context.Set<TEntity>().Find(key);
        }

        public Task<TEntity> GetByKeyAsync(TKey key)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Getting entity by key async: {0}", key);
            }
            return _context.Set<TEntity>().FindAsync(key);
        }
    }
}