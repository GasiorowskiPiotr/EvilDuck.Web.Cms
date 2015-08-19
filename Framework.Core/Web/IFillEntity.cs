using EvilDuck.Framework.Entities;

namespace EvilDuck.Framework.Core.Web
{
    public interface IFillEntity<in TEntity> where TEntity : Entity
    {
        void FillEntity(TEntity entity);
    }
}