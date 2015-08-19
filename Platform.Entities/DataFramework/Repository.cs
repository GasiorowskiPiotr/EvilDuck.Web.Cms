using System.Collections.Generic;
using EvilDuck.Framework.Entities;

namespace EvilDuck.Platform.Entities.DataFramework
{
    public class Repository : Entity<int>
    {
        public string Name { get; set; }
        public string Caption { get; set; }

        public Table Table { get; set; }
        public ICollection<Query> SelectQueries { get; set; }
        public Query InsertQuery { get; set; }
        public Query UpdateQuery { get; set; }
        public Query DeleteQuery { get; set; }
    }
}