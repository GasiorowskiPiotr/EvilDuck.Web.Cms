using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace EvilDuck.Platform.Core.DataFramework.Logic
{
    public class SqlQueryParamsFinder
    {
        public static IEnumerable<string> Find(string sqlQuery)
        {
            return
                Regex.Matches(sqlQuery, @"(?<!@)@\w+")
                    .OfType<Match>()
                    .SelectMany(m => m.Captures.OfType<Capture>().Select(c => c.Value))
                    .ToList();
        }
    }
}