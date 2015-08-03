using System.Reflection;
using EvilDuck.Framework.Core.DataAccess;

namespace EvilDuck.Platform.Core.DataAccess
{
    public class PlatformDomainContext : DomainContext
    {
        public PlatformDomainContext() : base("")
        {
            
        }

        protected override Assembly GetMappingsAssembly()
        {
            return null;
        }
    }
}
