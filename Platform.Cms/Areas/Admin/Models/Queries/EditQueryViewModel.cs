using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EvilDuck.Framework.Core.Web;
using EvilDuck.Platform.Core.DataFramework.Logic;
using EvilDuck.Platform.Entities.DataFramework;

namespace EvilDuck.Platform.Cms.Areas.Admin.Models.Queries
{
    public class EditQueryViewModel : EditEntityViewModel<Query, int>
    {
        [DisplayName("Tytuł")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Proszę podać tytuł zapytania")]
        public string Caption { get; set; }

        [DisplayName("Zapytanie SQL")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Proszę podać treść zapytania")]
        public string QueryText { get; set; }

        public QueryType Type { get; set; }

        protected override void FillFieldsFromEntity(Query entity)
        {
            Caption = entity.Caption;
            QueryText = entity.QueryText;
            Type = entity.Type;
        }

        public override void FillEntity(Query entity)
        {
            entity.Caption = Caption;
            entity.Type = Type;
            entity.QueryText = QueryText;
            entity.QueryParams = String.Join("|", SqlQueryParamsFinder.Find(entity.QueryText));
        }
    }
}