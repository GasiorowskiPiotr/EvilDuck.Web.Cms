using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Reflection;
using System.Security.Principal;
using EvilDuck.Framework.Entities;
using NLog;

namespace EvilDuck.Framework.Core.DataAccess
{
    public abstract class DomainContext : DbContext
    {
        protected Logger Logger { get; private set; }

        protected DomainContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Logger = LogManager.GetLogger(GetType().FullName);
        }

        private IIdentity _user;

        protected override bool ShouldValidateEntity(DbEntityEntry entityEntry)
        {
            return entityEntry.Entity is Entity;
        }

        public void SetUser(IIdentity user)
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.Info("Impersonating DomainContext as: {0}", user.Name);
            }
            _user = user;
        }

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            var entity = entityEntry.Entity as Entity;
            if (entity != null)
            {
                string who = _user.IsAuthenticated ? _user.Name : "UNKNOWN";
                if (entity.IsNew())
                {
                    entity.MarkCreated(who, DateTime.UtcNow);
                }
                else
                {
                    entity.MarkUpdated(who, DateTime.UtcNow);
                }
            }

            return base.ValidateEntity(entityEntry, items);
        }

        protected abstract Assembly GetMappingsAssembly();

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.Info("Creating model.");
            }
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.AddFromAssembly(GetMappingsAssembly());
        }
    }
}