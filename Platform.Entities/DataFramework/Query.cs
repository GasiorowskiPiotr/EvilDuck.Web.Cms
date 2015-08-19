using EvilDuck.Framework.Entities;

namespace EvilDuck.Platform.Entities.DataFramework
{
    public class Query : Entity<int>
    {
        public string Name { get; set; }
        public string Caption { get; set; }

        public string QueryText { get; set; }
        public string QueryParams { get; set; }
        public QueryType Type { get; set; }
    }
}