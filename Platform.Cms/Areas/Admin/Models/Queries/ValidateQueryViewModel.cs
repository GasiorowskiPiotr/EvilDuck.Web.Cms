using System.Collections.Generic;
using System.Dynamic;

namespace EvilDuck.Platform.Cms.Areas.Admin.Models.Queries
{
    public class ValidateQueryViewModel
    {
        private readonly ExpandoObject _parameters = new ExpandoObject();

        public IList<string> ParameterNames = new List<string>();

        public dynamic Parameters
        {
            get { return _parameters; }
        }

        public int QueryId { get; set; }

        public void AddParameter(string parameterName)
        {
            ((IDictionary<string, object>) _parameters)[parameterName] = null;
        }
    }
}