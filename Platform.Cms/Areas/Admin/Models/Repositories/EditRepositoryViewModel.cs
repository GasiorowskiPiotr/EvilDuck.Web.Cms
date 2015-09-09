using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using EvilDuck.Framework.Core.Web;
using EvilDuck.Platform.Core.DataAccess;
using EvilDuck.Platform.Entities.DataFramework;

namespace EvilDuck.Platform.Cms.Areas.Admin.Models.Repositories
{
    public class EditRepositoryViewModel : EditEntityViewModel<Repository, int>, INeedDomainContext<PlatformDomainContext>
    {
        [DisplayName("Nazwa")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Proszê podaæ nazwê repozytorium")]
        public string Name { get; set; }

        [DisplayName("Tytu³ (wyœwietlany)")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Proszê podaæ tytu³ repozytorium")]
        public string Caption { get; set; }

        public IEnumerable<int> SelectQueries { get; set; }
        public int InsertQuery { get; set; }
        public int UpdateQuery { get; set; }
        public int DeleteQuery { get; set; }

        public IDictionary<string, string> AllSelectQueries { get; set; }
        public IDictionary<string, string> AllInsertQueries { get; set; }
        public IDictionary<string, string> AllUpdateQueries { get; set; }
        public IDictionary<string, string> AllDeleteQueries { get; set; } 

        protected override void FillFieldsFromEntity(Repository entity)
        {
            Name = entity.Name;
            Caption = entity.Caption;

            SelectQueries = entity.SelectQueries.Select(q => q.Id).ToList();
            InsertQuery = entity.InsertQuery != null ? entity.InsertQuery.Id : 0;
            UpdateQuery = entity.UpdateQuery != null ? entity.UpdateQuery.Id : 0;
            DeleteQuery = entity.DeleteQuery != null ? entity.DeleteQuery.Id : 0;
        }

        public override void FillEntity(Repository entity)
        {
            entity.Name = Name;
            entity.Caption = Caption;
        }

        public void UseContext(PlatformDomainContext context)
        {
            var allQueries = context.Queries.ToList();

            AllDeleteQueries = allQueries.Where(q => q.Type == QueryType.Delete)
                .ToDictionary(q => q.Id.ToString(), q => q.Name);

            AllInsertQueries = allQueries.Where(q => q.Type == QueryType.Insert)
                .ToDictionary(q => q.Id.ToString(), q => q.Name);

            AllUpdateQueries = allQueries.Where(q => q.Type == QueryType.Update)
                .ToDictionary(q => q.Id.ToString(), q => q.Name);

            AllSelectQueries = allQueries.Where(q => q.Type == QueryType.Select)
                .ToDictionary(q => q.Id.ToString(), q => q.Name);
        }
    }
}