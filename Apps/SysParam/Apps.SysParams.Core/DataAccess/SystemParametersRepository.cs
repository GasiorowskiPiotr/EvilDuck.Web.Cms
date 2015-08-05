using EvilDuck.Applications.SystemParameters.Entities;
using EvilDuck.Framework.Core.DataAccess;
using NLog;

namespace EvilDuck.Applications.SystemParameters.Core.DataAccess
{
    public class SystemParametersRepository : EntityRepository<SystemParametersDomainContext, SystemParameter, string>
    {
        public SystemParametersRepository(SystemParametersDomainContext context, Logger logger) : base(context, logger)
        {
        }
    }
}