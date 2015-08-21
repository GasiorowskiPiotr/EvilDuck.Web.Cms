using System.Linq;
using EvilDuck.Framework.Core.DataAccess;
using EvilDuck.Platform.Core.DataAccess;
using EvilDuck.Platform.Entities.DataFramework;
using NLog;

namespace EvilDuck.Platform.Core.DataFramework.Repositories
{
    public class TablesRepository : EntityRepository<PlatformDomainContext, Table, int>
    {
        public TablesRepository(PlatformDomainContext context, Logger logger) : base(context, logger)
        {
        }

        public Table GetByName(string tableName)
        {
            return AdHocQuery().SingleOrDefault(t => t.Name == tableName);
        }
    }
}
