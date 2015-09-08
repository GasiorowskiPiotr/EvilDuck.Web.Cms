using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EvilDuck.Framework.Core.Web;
using EvilDuck.Platform.Core.DataFramework.Logic;
using EvilDuck.Platform.Entities.DataFramework;

namespace EvilDuck.Platform.Cms.Areas.Admin.Models.Queries
{
    public class AddQueryViewModel : CreateEntityViewModel<Query>
    {
        [DisplayName("Nazwa")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Proszę podać nazwę zapytania")]
        public string Name { get; set; }

        [DisplayName("Tytuł")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Proszę podać tytuł zapytania")]
        public string Caption { get; set; }

        [DisplayName("Zapytanie SQL")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Proszę podać treść zapytania")]
        public string QueryText { get; set; }

        public QueryType Type { get; set; }

        public override void FillEntity(Query entity)
        {
            entity.Caption = Caption;
            entity.Name = Name;
            entity.QueryText = QueryText;
            entity.Type = Type;
            entity.QueryParams = String.Join("|", SqlQueryParamsFinder.Find(entity.QueryText));
        }
    }
}