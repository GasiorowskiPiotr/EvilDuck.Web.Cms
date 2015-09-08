using System;
using System.Collections.Generic;
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

        public QueryComponent(Query query, PlatformDomainContext domainContext)
        {
            _query = query;
            _domainContext = domainContext;
            _logger = LogManager.GetLogger(typeof (QueryComponent).FullName);
        }

        public Result ExecuteAsNonQuery(IDictionary<string, object> parameters = null)
        {
            try
            {
                using (var tx = _domainContext.Database.Connection.BeginTransaction())
                {
                    var cmd = _domainContext.Database.Connection.CreateCommand();
                    cmd.Transaction = tx;
                    cmd.CommandText = _query.QueryText;

                    AddParameters(parameters, cmd);

                    cmd.ExecuteNonQuery();
                    tx.Commit();

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

        public QueryResult ExecuteQuery(IDictionary<string, object> parameters = null)
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

            var result = ExecuteAsMapper(mappingFunc, parameters);

            return new QueryResult(result);
        }

        public IEnumerable<TResult> ExecuteAsMapper<TResult>(Func<DbDataReader, TResult> mapper,
            IDictionary<string, object> parameters = null)
        {
            try
            {
                var cmd = _domainContext.Database.Connection.CreateCommand();
                cmd.CommandText = _query.QueryText;
                AddParameters(parameters, cmd);

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows) 
                        return Enumerable.Empty<TResult>();
                    
                    var resultList = new List<TResult>();
                    while (reader.Read())
                    {
                        var mappedItem = mapper(reader);
                        resultList.Add(mappedItem);
                    }

                    return resultList;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception while executing query: {0} ({1})", _query.Name, _query.QueryText);
                return null;
            }
        }


    }
}