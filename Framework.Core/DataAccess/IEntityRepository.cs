using System.Linq;
using System.Threading.Tasks;
using EvilDuck.Framework.Entities;

namespace EvilDuck.Framework.Core.DataAccess
{
    public interface IEntityRepository<TEntity, in TKey> where TEntity : Entity<TKey>
    {
        IQueryable<TEntity> AdHocQuery();
        TEntity GetByKey(TKey key);
        Task<TEntity> GetByKeyAsync(TKey key);
    }
}