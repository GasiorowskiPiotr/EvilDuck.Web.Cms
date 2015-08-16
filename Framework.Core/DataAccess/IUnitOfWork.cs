using System.Data;
using System.Data.Entity;
using System.Security.Principal;
using System.Threading.Tasks;
using EvilDuck.Framework.Entities;

namespace EvilDuck.Framework.Core.DataAccess
{
    public interface IUnitOfWork<TDomainContext> where TDomainContext : DomainContext
    {
        void Add<TEntity>(TEntity entity) where TEntity : Entity;
        void Attach<TEntity>(TEntity entity) where TEntity : Entity;
        void Delete<TEntity>(TEntity entity) where TEntity : Entity;
        void SaveChanges();
        Task SaveChangesAsync();
        DbContextTransaction BeginTransaction(IsolationLevel isolationLevel);

        void SetUser(IIdentity user);
    }
}