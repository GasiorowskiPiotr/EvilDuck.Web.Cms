using System.Collections.Generic;
using System.Linq;

namespace EvilDuck.Platform.Core.DataFramework.Logic
{
    public class QueryResultRow
    {
        private readonly IDictionary<string, object> _fields = new Dictionary<string, object>();

        public object this[string columnName]
        {
            get { return _fields[columnName]; }
            internal set { _fields[columnName] = value; }
        }

        public IEnumerable<string> ColumnNames
        {
            get { return _fields.Keys; }
        }

    }
}