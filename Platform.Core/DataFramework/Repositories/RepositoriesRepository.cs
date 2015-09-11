using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvilDuck.Framework.Core.DataAccess;
using EvilDuck.Platform.Core.DataAccess;
using EvilDuck.Platform.Entities.DataFramework;
using NLog;

namespace EvilDuck.Platform.Core.DataFramework.Repositories
{
    public class RepositoriesRepository : EntityRepository<PlatformDomainContext, Repository, int>
    {
        public RepositoriesRepository(PlatformDomainContext context, Logger logger) : base(context, logger)
        {
        }

        public async Task<Repository> GetByNameAsync(string name)
        {
            return await AdHocQuery().Include(e => e.DeleteQuery).Include(e => e.InsertQuery).Include(e => e.SelectQueries).Include(e => e.UpdateQuery).Where(e => e.Name == name).SingleOrDefaultAsync();
        }

        public Repository GetByName(string name)
        {
            return AdHocQuery().Include(e => e.DeleteQuery).Include(e => e.InsertQuery).Include(e => e.SelectQueries).Include(e => e.UpdateQuery).SingleOrDefault(e => e.Name == name);
        }
    }
}
