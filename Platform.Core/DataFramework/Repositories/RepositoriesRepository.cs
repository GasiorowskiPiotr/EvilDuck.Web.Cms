using System;
using System.Collections.Generic;
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
    }
}
