using EvilDuck.Framework.Entities;

namespace EvilDuck.Platform.Entities.DataFramework
{
    public class Column : Entity<int>
    {
        public string Name { get; set; }
        public string Caption { get; set; }
        public DbDataType Type { get; set; }
        public int Length { get; set; }
        public string DefaultValue { get; set; }
        public bool IsRelation { get; set; }
        public string RelationTable { get; set; }
        public string RelationColumn { get; set; }
        public bool IsKey { get; set; }
        public bool AutoincrementKey { get; set; }
    }
}