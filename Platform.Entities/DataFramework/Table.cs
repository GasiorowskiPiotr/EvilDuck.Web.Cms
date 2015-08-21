using System.Collections.Generic;
using System.Collections.ObjectModel;
using EvilDuck.Framework.Entities;

namespace EvilDuck.Platform.Entities.DataFramework
{
    public class Table : Entity<int>
    {
        public Table()
        {
            Columns = new Collection<Column>();
        }

        public string Name { get; set; }
        public string Caption { get; set; }
        public virtual ICollection<Column> Columns { get; set; }
        public bool IsExported { get; set; }
    }
}