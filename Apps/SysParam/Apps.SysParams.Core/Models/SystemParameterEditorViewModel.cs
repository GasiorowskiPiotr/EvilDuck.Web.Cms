using System;
using System.Web.Mvc;
using EvilDuck.Applications.SystemParameters.Entities;
using EvilDuck.Framework.Core.Web;
using Type = EvilDuck.Applications.SystemParameters.Entities.Type;

namespace EvilDuck.Applications.SystemParameters.Core.Models
{
    public class SystemParameterEditorViewModel : CreateEntityViewModel<SystemParameter>
    {

        public string Code { get; set; }
        public string Description { get; set; }
        public string ParameterType { get; set; }
        public string ValueType { get; set; }
        public string SerializedValue { get; set; }


        public override void FillEntity(SystemParameter entity)
        {
            entity.Id = Code;
            entity.Description = Description;
            entity.ParameterType = (SystemParameterType) Enum.Parse(typeof(SystemParameterType), ParameterType);
            entity.ValueType = (Type)Enum.Parse(typeof(Type), ValueType);
            entity.SerializedValue = SerializedValue;
        }
    }
}
