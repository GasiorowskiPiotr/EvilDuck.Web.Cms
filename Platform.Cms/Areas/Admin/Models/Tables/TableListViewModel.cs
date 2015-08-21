using EvilDuck.Framework.Core.Web;
using EvilDuck.Platform.Entities.DataFramework;

namespace EvilDuck.Platform.Cms.Areas.Admin.Models.Tables
{
    public class TableListViewModel : EntityListViewModel<Table>
    {
        public TableListViewModel(Table entity) : base(entity)
        {
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsExported { get; set; }
        public int ColumnCount { get; set; }


        protected override void FillFieldsFromEntity(Table entity)
        {
            Name = entity.Name;
            Description = entity.Caption;
            IsExported = entity.IsExported;
            ColumnCount = entity.Columns.Count;
            Id = entity.Id;
        }
    }
}