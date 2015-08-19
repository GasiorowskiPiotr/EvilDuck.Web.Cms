using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using EvilDuck.Framework.Core.Web;
using EvilDuck.Platform.Entities.DataFramework;

namespace EvilDuck.Platform.Cms.Areas.Admin.Models.Tables
{
    public class AddTableViewModel : CreateEntityViewModel<Table>
    {
        [DisplayName("Nazwa")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Proszę podać nazwę tabeli")]
        public string Name { get; set; }

        [DisplayName("Tytuł (wyświetlany)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Proszę podać tytuł tabeli")]
        public string Caption { get; set; }

        public override void FillEntity(Table entity)
        {
            entity.Name = Name;
            entity.Caption = Caption;
            entity.IsExported = false;
        }

        public override void Validate(ModelStateDictionary modelState)
        {
            var match = Regex.Match(Name, "[A-Za-z_]{0,32}");
            if (!match.Success)
            {
                modelState.AddModelError("Name", "Nazwa tabeli zawiera niedozwolone znaki. Należy używać wyłącznie A-Z,a-z i _, oraz mieć długość do 32 znaków.");
            }
        }
    }
}