using System.Web.Mvc;
using EvilDuck.Framework.Entities;

namespace EvilDuck.Framework.Core.Web
{
    public abstract class CreateEntityViewModel<TEntity> : IFillEntity<TEntity>, IValidatableViewModel where TEntity : Entity
    {
        public abstract void FillEntity(TEntity entity);
        public virtual void Validate(ModelStateDictionary modelState) { }
    }
}