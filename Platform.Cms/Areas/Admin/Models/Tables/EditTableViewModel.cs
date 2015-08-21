using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using EvilDuck.Framework.Core.Web;
using EvilDuck.Platform.Entities.DataFramework;

namespace EvilDuck.Platform.Cms.Areas.Admin.Models.Tables
{
    public class EditTableViewModel : EditEntityViewModel<Table, int>
    {

        [DisplayName("Tytuł (wyświetlany)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Proszę podać tytuł tabeli")]
        public string Caption { get; set; }

        [DisplayName("Nazwa")]
        public string Name { get; set; }

        public bool IsExported { get; set; }

        protected override void FillFieldsFromEntity(Table entity)
        {
            Caption = entity.Caption;
            Name = entity.Name;
            IsExported = entity.IsExported;
        }

        public override void FillEntity(Table entity)
        {
            entity.Caption = Caption;
        }

        public override void Validate(ModelStateDictionary modelState)
        {
            if (Id == 0)
            {
                modelState.AddModelError(String.Empty, "Nie można odnaleźć wybranej tabeli");
            }
        }
    }
}