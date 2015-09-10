using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using EvilDuck.Framework.Core.DataAccess;
using EvilDuck.Platform.Core.DataAccess;
using EvilDuck.Platform.Entities.DataFramework;
using NLog;

namespace EvilDuck.Platform.Core.DataFramework.Repositories
{
    public class QueriesRepository : EntityRepository<PlatformDomainContext, Query, int>
    {
        public QueriesRepository(PlatformDomainContext context, Logger logger) : base(context, logger)
        {
        }

        public ICollection<Query> GetManyByKey(IEnumerable<int> selectQueries)
        {
            return AdHocQuery().Where(e => selectQueries.Contains(e.Id)).ToList();
        }

        public Task<Query> GetByNameAndTypes(string method, params QueryType[] types)
        {
            return AdHocQuery().Where(e => e.Name == method && types.Contains(e.Type)).SingleOrDefaultAsync();
        }
    }
}