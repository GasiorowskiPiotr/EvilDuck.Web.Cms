using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvilDuck.Framework.Entities.Maps
{
    public abstract class BaseEntityTypeConfiguration<TEntity> : EntityTypeConfiguration<TEntity> where TEntity : Entity
    {
        protected BaseEntityTypeConfiguration()
        {
            Property(e => e.RowVersion).IsRowVersion();

            Property(e => e.CreatedBy).IsRequired();
            Property(e => e.CreatedOn).IsRequired();

            Property(e => e.DeletedOn);
            Property(e => e.DeletedBy);

            Property(e => e.IsActive).IsRequired();
            Property(e => e.ReferenceId);

            Property(e => e.LastUpdateBy).IsRequired();
            Property(e => e.LastUpdateOn).IsRequired();
        }
    }

    public abstract class BaseEntityWithKeyTypeConfiguration<TEntity, TKey> : BaseEntityTypeConfiguration<TEntity>
        where TEntity : Entity<TKey>
    {
        protected BaseEntityWithKeyTypeConfiguration()
        {
            HasKey(e => e.Id);
        }
    }
}
