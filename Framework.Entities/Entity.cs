using System;

namespace EvilDuck.Framework.Entities
{
    public abstract class Entity<TKey> : Entity
    {
        public TKey Id { get; set; }

        public override bool IsNew()
        {
            return Equals(Id, default(TKey)) || String.IsNullOrEmpty(CreatedBy);
        }
    }

    public abstract class Entity
    {
        public byte[] RowVersion { get; set; }

        public string LastUpdateBy { get; set; }
        public string CreatedBy { get; set; }
        public string DeletedBy { get; set; }

        public DateTime LastUpdateOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }

        public int ReferenceId { get; set; }
        public bool IsActive { get; set; }

        public abstract bool IsNew();

        public void MarkDeleted(string who)
        {
            IsActive = false;
            DeletedOn = DateTime.Now;
            DeletedBy = who;
        }

        public void MarkCreated(string who, DateTime date)
        {
            CreatedBy = who;
            CreatedOn = date;
            LastUpdateBy = who;
            LastUpdateOn = date;
        }

        public void MarkUpdated(string who, DateTime date)
        {
            LastUpdateBy = who;
            LastUpdateOn = date;
        }
    }
}