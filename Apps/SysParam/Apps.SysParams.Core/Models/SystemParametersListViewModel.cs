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
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public string LastUpdatedOn { get; set; }

        public void FillFromEntity(SystemParameter entity)
        {
            Description = entity.Description;
            SerializedValue = entity.SerializedValue;
            ParameterType = entity.ParameterType.ToString();
            ValueType = entity.ValueType.ToString();
            Code = entity.Id;
            CreatedBy = entity.CreatedBy;
            CreatedOn = entity.CreatedOn.ToString();
            LastUpdatedBy = entity.LastUpdateBy;
            LastUpdatedOn = entity.LastUpdateOn.ToString();
        }
    }
}
