using EvilDuck.Applications.SystemParameters.Entities;
using EvilDuck.Framework.Core.Web;

namespace EvilDuck.Applications.SystemParameters.Core.Models
{
    public class SystemParametersListViewModel : EntityListViewModel<SystemParameter>
    {
        public SystemParametersListViewModel(SystemParameter entity) : base(entity)
        {
        }

        public string Code { get; set; }
        public string Description { get; set; }
        public string SerializedValue { get; set; }
        public string ParameterType { get; set; }
        public string ValueType { get; set; }


        protected override void FillFieldsFromEntity(SystemParameter entity)
        {
            Description = entity.Description;
            SerializedValue = entity.SerializedValue;
            ParameterType = entity.ParameterType.ToString();
            ValueType = entity.ValueType.ToString();
            Code = entity.Id;
        }
    }
}
