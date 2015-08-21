using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EvilDuck.Framework.Core.Web;
using EvilDuck.Platform.Entities.DataFramework;

namespace EvilDuck.Platform.Cms.Areas.Admin.Models.Tables
{
    public class ColumnsListViewModel : EntityListViewModel<Column>
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
        public int ColumnId { get; set; }

        public ColumnsListViewModel(Column entity, int tableId) : base(entity)
        {
            TableId = tableId;
        }

        protected override void FillFieldsFromEntity(Column entity)
        {
            Name = entity.Name;
            Caption = entity.Caption;
            Type = entity.Type;
            Length = entity.Length;
            DefaultValue = entity.DefaultValue;
            IsRelation = entity.IsRelation;
            RelationTable = entity.RelationTable;
            IsKey = entity.IsKey;
            AutoincrementKey = entity.AutoincrementKey;
            ColumnId = entity.Id;
        }
    }
}