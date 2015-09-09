using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EvilDuck.Platform.Core.DataFramework.Logic;

namespace EvilDuck.Platform.Cms.Areas.Admin.Models.Queries
{
    public class QueryValidationResultViewModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public QueryResult Data { get; set; }
    }
}