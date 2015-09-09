using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using EvilDuck.Framework.Core;
using EvilDuck.Platform.Core.DataAccess;
using EvilDuck.Platform.Entities.DataFramework;
using NLog;

namespace EvilDuck.Platform.Core.DataFramework.Logic
{
    public class QueryComponent
    {
        private readonly Query _query;
        private readonly PlatformDomainContext _domainContext;
        private readonly Logger _logger;

        private bool _testMode = false;

        public QueryComponent(Query query, PlatformDomainContext domainContext)
        {
            _query = query;
            _domainContext = domainContext;
            _logger = LogManager.GetLogger(typeof (QueryComponent).FullName);
        }

        public Result Test(out QueryResult result, IDictionary<string, object> parameters = null)
        {
            _testMode = true;
            Result res;
            if (_query.Type == QueryType.Select)
            {
                res = ExecuteQuery(out result, parameters);
            }
            else
            {
                result = null;
                res = ExecuteAsNonQuery(parameters);
            }
            _testMode = false;

            return res;
        }

        public Result ExecuteAsNonQuery(IDictionary<string, object> parameters = null)
        {
            try
            {
                using (var tx = _domainContext.Database.Connection.BeginTransaction())
                {
                    if (_domainContext.Database.Connection.State != ConnectionState.Open)
                    {
                        _domainContext.Database.Connection.Open();
                    }
                    var cmd = _domainContext.Database.Connection.CreateCommand();
                    cmd.Transaction = tx;
                    cmd.CommandText = _query.QueryText;

                    AddParameters(parameters, cmd);

                    cmd.ExecuteNonQuery();

                    if (!_testMode)
                    {
                        tx.Commit();
                    }
                    else
                    {
                        tx.Rollback();
                    }

                    return Result.Success("Zapytanie wykonano poprawnie");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception while executing query: {0} ({1})", _query.Name, _query.QueryText);
                return Result.Failure("B³¹d w czasie wykonywania zapytania", ex);
            }
        }

        private void AddParameters(IDictionary<string, object> parameters, DbCommand cmd)
        {
            if (parameters != null)
            {
                foreach (var @param in _query.QueryParams.Split('|').Select(c => c.Replace("@", String.Empty)))
                {
                    if (parameters.ContainsKey(@param))
                    {
                        var parameter = cmd.CreateParameter();
                        parameter.ParameterName = @param;
                        parameter.Value = parameters[@param];

                        cmd.Parameters.Add(parameter);
                    }
                }
            }
        }

        public Result ExecuteQuery(out QueryResult results, IDictionary<string, object> parameters = null)
        {
            Func<DbDataReader, QueryResultRow> mappingFunc = reader =>
            {
                var queryResultRow = new QueryResultRow();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var columnName = reader.GetName(i);
                    var value = reader[i];

                    queryResultRow[columnName] = value;
                }

                return queryResultRow;
            };

            IEnumerable<QueryResultRow> queryResultRows;
            var result = ExecuteAsMapper(mappingFunc, out queryResultRows , parameters);
            if (!result.IsSuccess)
            {
                results = QueryResult.Empty;
                return result;
            }

            results = new QueryResult(queryResultRows);
            return Result.Success("Zapytanie zakoñczone sukcesem.");
        }

        public Result ExecuteAsMapper<TResult>(Func<DbDataReader, TResult> mapper, out IEnumerable<TResult> results,
            IDictionary<string, object> parameters = null)
        {
            try
            {
                if (_domainContext.Database.Connection.State != ConnectionState.Open)
                {
                    _domainContext.Database.Connection.Open();
                }
                var cmd = _domainContext.Database.Connection.CreateCommand();
                cmd.CommandText = _query.QueryText;
                AddParameters(parameters, cmd);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        results = Enumerable.Empty<TResult>();
                        return Result.Success("Zapytanie wykonane poprawne");
                    }

                    
                    var resultList = new List<TResult>();
                    while (reader.Read())
                    {
                        var mappedItem = mapper(reader);
                        resultList.Add(mappedItem);
                    }

                    results = resultList;
                    return Result.Success("Zapytanie wykonane poprawne"); ;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception while executing query: {0} ({1})", _query.Name, _query.QueryText);
                results = Enumerable.Empty<TResult>(); ;
                return Result.Failure("Wykonanie zapytania zakoñczone b³êdem. ", ex);
            }
        }
    }
}