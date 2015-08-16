using EvilDuck.Framework.Core.DataAccess;

namespace EvilDuck.Applications.SystemParameters.Core.DataAccess
{
    public class SystemParametersUnitOfWork : UnitOfWork<SystemParametersDomainContext>
    {
        public SystemParametersUnitOfWork(SystemParametersDomainContext context) : base(context)
        {
        }
    }
}