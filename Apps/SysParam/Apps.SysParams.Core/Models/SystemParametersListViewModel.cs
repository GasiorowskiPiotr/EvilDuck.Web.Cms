using EvilDuck.Applications.SystemParameters.Entities;
using EvilDuck.Framework.Core.Web;

namespace EvilDuck.Applications.SystemParameters.Core.Models
{
    public class SystemParametersListViewModel : IEntityListViewModel<SystemParameter>
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string SerializedValue { get; set; }
        public string ParameterType { get; set; }
        public string ValueType { get; set; }
        public string KeyType { get; set; }

        public void FillFromEntity(SystemParameter entity)
        {
            Description = entity.Description;
            SerializedValue = entity.SerializedValue;
            ParameterType = entity.ParameterType.ToString();
            ValueType = entity.ValueType.ToString();
            KeyType = entity.KeyType.ToString();
            Code = entity.Id;
        }
    }
}
