using EvilDuck.Framework.Entities;

namespace EvilDuck.Framework.Core.Web
{
    public interface IAmFilledFromEntity<in TEntity> where TEntity : Entity
    {
        void FillFromEntity(TEntity entity);
    }
}