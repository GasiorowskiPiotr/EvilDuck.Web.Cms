using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EvilDuck.Platform.Entities.DataFramework;

namespace EvilDuck.Platform.Cms.Models
{
    public class QueryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public QueryType QueryType { get; set; }
    }
}