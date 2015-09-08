using System;
using EvilDuck.Platform.Core.DataAccess;
using EvilDuck.Platform.Core.DataFramework.Repositories;
using EvilDuck.Platform.Entities.DataFramework;

namespace EvilDuck.Platform.Core.DataFramework.Logic
{
    public class QueryComponentFactory
    {
        private readonly QueriesRepository _repository;
        private readonly PlatformDomainContext _domainContext;

        public QueryComponentFactory(QueriesRepository repository,
            PlatformDomainContext domainContext)
        {
            _repository = repository;
            _domainContext = domainContext;
        }

        public QueryComponent CreateQueryComponent(int queryId)
        {
            var query = _repository.GetByKey(queryId);
            return new QueryComponent(query, _domainContext);
        }

        public QueryComponent CreateQueryComponent(string sql)
        {
            var query = new Query {QueryText = sql, QueryParams = String.Join("|", SqlQueryParamsFinder.Find(sql))};
            return new QueryComponent(query, _domainContext);
        }
    }
}