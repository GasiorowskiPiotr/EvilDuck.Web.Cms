using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using EvilDuck.Framework.Core.Cache;
using EvilDuck.Platform.Core.DataAccess;
using EvilDuck.Platform.Core.DataFramework.Repositories;
using EvilDuck.Platform.Entities.DataFramework;
using NLog;

namespace EvilDuck.Platform.Core.DataFramework.Cache
{
    public class TablesCache : CustomCache
    {
        public TablesCache(Logger logger) : base(logger)
        {
            AbsoluteExpirationSpan = TimeSpan.FromHours(24);
        }

        public Table Get(int id)
        {
            return (Table) Get(new TableCacheKey(id));
        }

        protected override void OnMiss(ICacheKey key, out object value)
        {
            var repo = new TablesRepository(new PlatformDomainContext(), Logger);

            var table = repo.AdHocQuery().AsNoTracking().Include(e => e.Columns).SingleOrDefault(e => e.Id == ((TableCacheKey) key).Id);
            if (table != null)
            {
                this.Add(key, table);
            }

            value = table;
        }

        class TableCacheKey : ICacheKey
        {
            private readonly int _tableId;

            public TableCacheKey(int tableId)
            {
                this._tableId = tableId;
            }


            public string KeyAsString
            {
                get { return _tableId.ToString(CultureInfo.InvariantCulture); }
            }

            public int Id
            {
                get { return _tableId; }
            }
        }
    }
}
