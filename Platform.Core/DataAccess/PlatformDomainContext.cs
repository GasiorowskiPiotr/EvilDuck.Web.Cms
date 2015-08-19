using System.Data.Entity;
using System.Reflection;
using EvilDuck.Framework.Core.DataAccess;
using EvilDuck.Platform.Entities;
using EvilDuck.Platform.Entities.DataFramework;

namespace EvilDuck.Platform.Core.DataAccess
{
    public class PlatformDomainContext : DomainContext
    {
        public PlatformDomainContext()
            : base("DefaultConnection")
        {
            
        }

        protected override Assembly GetMappingsAssembly()
        {
            return typeof (ApplicationUser).Assembly;
        }

        public DbSet<Column> Columns { get; set; }
        public DbSet<Query> Queries { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Repository> Repositories { get; set; }
    }
}
