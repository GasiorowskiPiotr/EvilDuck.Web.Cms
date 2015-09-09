using System.Collections.Generic;
using EvilDuck.Framework.Entities;

namespace EvilDuck.Platform.Entities.DataFramework
{
    public class Repository : Entity<int>
    {
        public string Name { get; set; }
        public string Caption { get; set; }

        public virtual Table Table { get; set; }
        public virtual ICollection<Query> SelectQueries { get; set; }
        public virtual Query InsertQuery { get; set; }
        public virtual Query UpdateQuery { get; set; }
        public virtual Query DeleteQuery { get; set; }
    }
}