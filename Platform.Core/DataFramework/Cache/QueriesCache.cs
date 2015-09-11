using System.Globalization;
using EvilDuck.Framework.Core.Cache;
using EvilDuck.Platform.Core.DataAccess;
using EvilDuck.Platform.Core.DataFramework.Repositories;
using EvilDuck.Platform.Entities.DataFramework;
using NLog;

namespace EvilDuck.Platform.Core.DataFramework.Cache
{
    public class QueriesCache : CustomCache
    {
        public QueriesCache(Logger logger) : base(logger)
        {
        }

        public Query Get(string queryName)
        {
            return (Query) Get(new QueriesCacheStringKey(queryName));
        }

        public Query Get(int id)
        {
            return (Query) Get(new QueriesCacheIntKey(id));
        }

        protected override void OnMiss(ICacheKey key, out object value)
        {
            var repository = new QueriesRepository(new PlatformDomainContext(), Logger);

            var byNameCacheKey = key as QueriesCacheStringKey;
            if (byNameCacheKey != null)
            {
                var query = repository.GetByName(byNameCacheKey.Name);
                if (query != null)
                {
                    Add(key, query);
                    Add(new QueriesCacheIntKey(query.Id), query);
                }

                value = query;
                return;
            }
            var byIdCacheCacheKey = key as QueriesCacheIntKey;
            if (byIdCacheCacheKey != null)
            {
                var query = repository.GetByKey(byIdCacheCacheKey.QueryId);
                if (query != null)
                {
                    Add(key, query);
                    Add(new QueriesCacheStringKey(query.Name), query);
                }

                value = query;
                return;
            }

            value = null;

        }

        public class QueriesCacheStringKey : ICacheKey
        {
            public QueriesCacheStringKey(string name)
            {
                KeyAsString = name;
            }

            public string KeyAsString { get; private set; }

            public string Name
            {
                get { return KeyAsString; }
            }
        }

        public class QueriesCacheIntKey : ICacheKey
        {
            private readonly int _queryId;
            public string KeyAsString { get; private set; }

            public QueriesCacheIntKey(int queryId)
            {
                _queryId = queryId;
                KeyAsString = queryId.ToString(CultureInfo.InvariantCulture);
            }

            public int QueryId
            {
                get { return _queryId; }

            }
        }
    }
}