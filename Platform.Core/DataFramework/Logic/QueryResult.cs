using System.Collections.Generic;
using System.Linq;

namespace EvilDuck.Platform.Core.DataFramework.Logic
{
    public class QueryResult
    {
        private readonly IList<string> _columnNames = new List<string>();
        private readonly IList<QueryResultRow> _rows = new List<QueryResultRow>();

        public QueryResult(IEnumerable<QueryResultRow> results)
        {
            var resultsAsList = results.ToList();
            if (resultsAsList.Any())
            {
                foreach (var columnName in resultsAsList.ElementAt(0).ColumnNames)
                {
                   _columnNames.Add(columnName);
                }
            }

            _rows = resultsAsList;
            
        }

        public IEnumerable<string> ColumnNames
        {
            get { return _columnNames; }
        }

        public IEnumerable<QueryResultRow> Rowses
        {
            get { return _rows; }
        }

        public static QueryResult Empty
        {
            get
            {
                return new QueryResult(Enumerable.Empty<QueryResultRow>());
            }
        }
    }
}