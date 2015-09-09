using System;
using System.Linq;
using EvilDuck.Framework.Core.Web;
using EvilDuck.Platform.Entities.DataFramework;

namespace EvilDuck.Platform.Cms.Areas.Admin.Models.Repositories
{
    public class RepositoriesListViewModel : EntityListViewModel<Repository>
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Caption { get; set; }

        public string InsertQueryName { get; set; }
        public string UpdateQueryName { get; set; }
        public string DeleteQueryName { get; set; }
        public string SelectQueryNames { get; set; }

        public RepositoriesListViewModel(Repository entity) : base(entity)
        {
        }

        protected override void FillFieldsFromEntity(Repository entity)
        {
            Id = entity.Id;

            Name = entity.Name;
            Caption = entity.Caption;

            InsertQueryName = entity.InsertQuery.Name;
            UpdateQueryName = entity.UpdateQuery.Name;
            DeleteQueryName = entity.DeleteQuery.Name;
            SelectQueryNames = String.Join(", ", entity.SelectQueries.Select(sq => sq.Name));
        }
    }
}