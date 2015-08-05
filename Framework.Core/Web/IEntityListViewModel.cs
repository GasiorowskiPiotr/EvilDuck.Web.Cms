using EvilDuck.Framework.Entities;

namespace EvilDuck.Framework.Core.Web
{
    public interface IEntityListViewModel<in TEntity> where TEntity : Entity
    {
        void FillFromEntity(TEntity entity);
    }
}