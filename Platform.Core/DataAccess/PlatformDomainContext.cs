using System.Reflection;
using EvilDuck.Framework.Core.DataAccess;
using EvilDuck.Platform.Entities;

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
    }

    public class PlatformUnitOfWork : UnitOfWork<PlatformDomainContext>
    {
        public PlatformUnitOfWork(PlatformDomainContext context) : base(context)
        {
        }
    }
}
