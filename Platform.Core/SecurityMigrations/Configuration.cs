using EvilDuck.Platform.Core.Security;
using EvilDuck.Platform.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EvilDuck.Platform.Core.SecurityMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EvilDuck.Platform.Core.Security.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"SecurityMigrations";
        }

        protected override void Seed(EvilDuck.Platform.Core.Security.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            var roleExists = context.Roles.Any(r => r.Name == "Administrator");
            if (!roleExists)
            {
                new RoleManager<IdentityRole, string>(new RolesStore(context)).Create(new IdentityRole("Administrator"));
            }

            var adminExists = context.Users.Any(u => u.Email == "admin@evilduck.org");
            if (!adminExists)
            {
                var appUser = new ApplicationUser()
                {
                    Email = "admin@evilduck.org",
                    UserName = "admin@evilduck.org"
                };

                new ApplicationUserManager(new UsersStore(context)).Create(appUser, "!QAZxsw2#");
                new ApplicationUserManager(new UsersStore(context)).AddToRole(appUser.Id, "Administrator");
            }

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
