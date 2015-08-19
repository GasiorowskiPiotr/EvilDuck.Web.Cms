using System.Globalization;
using EvilDuck.Framework.Entities;

namespace EvilDuck.Framework.Core.Web
{
    public abstract class EntityListViewModel<TEntity> : IAmFilledFromEntity<TEntity> where TEntity : Entity
    {
        public string CreatedOn { get; private set; }
        public string CreatedBy { get; private set; }
        public string LastUpdatedOn { get; private set; }
        public string LastUpdatedBy { get; private set; }

        protected EntityListViewModel(TEntity entity)
        {
            FillFromEntity(entity);
        } 

        protected abstract void FillFieldsFromEntity(TEntity entity);

        public void FillFromEntity(TEntity entity)
        {
            CreatedBy = entity.CreatedBy;
            CreatedOn = entity.CreatedOn.ToString(CultureInfo.CurrentUICulture);
            LastUpdatedBy = entity.LastUpdateBy;
            LastUpdatedOn = entity.LastUpdateOn.ToString(CultureInfo.CurrentUICulture);
            FillFieldsFromEntity(entity);
        }
    }
}