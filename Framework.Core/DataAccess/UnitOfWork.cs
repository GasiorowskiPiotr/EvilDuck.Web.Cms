using System.Data;
using System.Data.Entity;
using System.Security.Principal;
using System.Threading.Tasks;
using EvilDuck.Framework.Entities;

namespace EvilDuck.Framework.Core.DataAccess
{
    public class UnitOfWork<TDomainContext> : IUnitOfWork<TDomainContext> where TDomainContext : DomainContext
    {
        private readonly TDomainContext _context;

        public UnitOfWork(TDomainContext context)
        {
            _context = context;
        }

        public void Add<TEntity>(TEntity entity) where TEntity : Entity
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void Attach<TEntity>(TEntity entity) where TEntity : Entity
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : Entity
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public DbContextTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return _context.Database.BeginTransaction(isolationLevel);
        }

        public void SetUser(IIdentity user)
        {
            _context.SetUser(user);
        }
    }
}