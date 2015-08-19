using System.Globalization;
using System.Web.Mvc;
using EvilDuck.Framework.Entities;

namespace EvilDuck.Framework.Core.Web
{
    public abstract class EditEntityViewModel<TEntity, TKey> : IFillEntity<TEntity>, IAmFilledFromEntity<TEntity>, IValidatableViewModel where TEntity : Entity<TKey>
    {
        public string CreatedOn { get; private set; }
        public string CreatedBy { get; private set; }
        public string LastUpdatedOn { get; private set; }
        public string LastUpdatedBy { get; private set; }

        public TKey Id { get; set; }

        public virtual void Validate(ModelStateDictionary modelState)
        {
        
        }

        public void FillFromEntity(TEntity entity)
        {
            Id = entity.Id;
            CreatedBy = entity.CreatedBy;
            CreatedOn = entity.CreatedOn.ToString(CultureInfo.CurrentUICulture);
            LastUpdatedBy = entity.LastUpdateBy;
            LastUpdatedOn = entity.LastUpdateOn.ToString(CultureInfo.CurrentUICulture);
            FillFieldsFromEntity(entity);
        }

        protected abstract void FillFieldsFromEntity(TEntity entity);

        public abstract void FillEntity(TEntity entity);
    }
}