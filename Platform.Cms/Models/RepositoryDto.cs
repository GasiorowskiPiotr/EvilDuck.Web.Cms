using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EvilDuck.Platform.Cms.Models
{
    public class RepositoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool CanInsert { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public IEnumerable<string> SelectQueries { get; set; }
    }
}