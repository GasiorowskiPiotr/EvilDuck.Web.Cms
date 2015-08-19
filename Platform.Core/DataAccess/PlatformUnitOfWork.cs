using EvilDuck.Framework.Core.DataAccess;

namespace EvilDuck.Platform.Core.DataAccess
{
    public class PlatformUnitOfWork : UnitOfWork<PlatformDomainContext>
    {
        public PlatformUnitOfWork(PlatformDomainContext context) : base(context)
        {
        }
    }
}