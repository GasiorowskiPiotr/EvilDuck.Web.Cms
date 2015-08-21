using EvilDuck.Framework.Core.Web;
using EvilDuck.Platform.Entities.DataFramework;

namespace EvilDuck.Platform.Cms.Areas.Admin.Models.Queries
{
    public class QueryListViewModel : EntityListViewModel<Query>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public string Type { get; set; }

        public QueryListViewModel(Query entity) : base(entity)
        {
        }

        protected override void FillFieldsFromEntity(Query entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Caption = entity.Caption;
            Type = entity.Type.ToString();
        }
    }
}