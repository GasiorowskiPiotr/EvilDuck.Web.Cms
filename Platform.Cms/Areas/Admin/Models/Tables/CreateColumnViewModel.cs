using EvilDuck.Framework.Core.Web;
using EvilDuck.Platform.Entities.DataFramework;

namespace EvilDuck.Platform.Cms.Areas.Admin.Models.Tables
{
    public class CreateColumnViewModel : CreateEntityViewModel<Column>
    {

        public string Name { get; set; }
        public string Caption { get; set; }
        public DbDataType Type { get; set; }
        public int Length { get; set; }
        public string DefaultValue { get; set; }
        public bool IsRelation { get; set; }
        public string RelationTable { get; set; }
        public bool IsKey { get; set; }
        public bool AutoincrementKey { get; set; }

        public int TableId { get; set; }

        public CreateColumnViewModel(int tableId)
        {
            TableId = tableId;
        }

        public override void FillEntity(Column entity)
        {
            entity.Name = Name;
            entity.Caption = Caption;
            entity.Type = Type;
            entity.Length = Length;
            entity.DefaultValue = DefaultValue;
            entity.IsRelation = IsRelation;
            entity.RelationTable = RelationTable;
            entity.IsKey = IsKey;
            entity.AutoincrementKey = AutoincrementKey;
        }
    }
}