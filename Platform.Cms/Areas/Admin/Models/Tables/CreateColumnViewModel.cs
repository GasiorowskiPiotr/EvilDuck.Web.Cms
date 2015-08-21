using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using EvilDuck.Framework.Core.Web;
using EvilDuck.Platform.Core.DataAccess;
using EvilDuck.Platform.Entities.DataFramework;

namespace EvilDuck.Platform.Cms.Areas.Admin.Models.Tables
{
    public class CreateColumnViewModel : CreateEntityViewModel<Column>, INeedDomainContext<PlatformDomainContext>
    {
        [DisplayName("Nazwa")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Proszę podać nazwę kolumny")]
        public string Name { get; set; }

        [DisplayName("Tytuł (wyświetlany)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Proszę podać tytuł kolumny")]
        public string Caption { get; set; }

        [DisplayName("Typ danych")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Proszę podać typ danych")]
        public DbDataType Type { get; set; }

        [DisplayName("Długość")]
        public int Length { get; set; }

        [DisplayName("Wartość domyślna")]
        public string DefaultValue { get; set; }

        [DisplayName("Czy jest połączeniem do innej tabeli?")]
        public bool IsRelation { get; set; }

        [DisplayName("Nazwa połączonej tabeli")]
        public string RelationTable { get; set; }

        [DisplayName("Czy jest kluczem głównym?")]
        public bool IsKey { get; set; }

        [DisplayName("Autoinkrementowanie klucza głównego")]
        public bool AutoincrementKey { get; set; }

        public int TableId { get; set; }

        public IDictionary<string, string> AvailableTables { get; set; }

        public CreateColumnViewModel()
        {
        }

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

        public override void Validate(ModelStateDictionary modelState)
        {
            if (IsRelation && String.IsNullOrEmpty(RelationTable))
            {
                modelState.AddModelError(String.Empty, "Nazwa tabeli połączonej jest wymagana.");
            }
        }

        public void UseContext(PlatformDomainContext context)
        {
            AvailableTables = context.Tables.Where(t => t.Columns.Any(c => c.IsKey)).ToList()
                .ToDictionary(d => d.Name, d => String.Format("{0} ({1})", d.Caption, d.Name));
        }
    }
}