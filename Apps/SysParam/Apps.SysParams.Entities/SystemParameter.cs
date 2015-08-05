using EvilDuck.Framework.Entities;

namespace EvilDuck.Applications.SystemParameters.Entities
{
    public class SystemParameter : Entity<string>
    {
        public string Description { get; set; }
        public string SerializedValue { get; set; }
        public SystemParameterType ParameterType { get; set; }
        public Type ValueType { get; set; }
        public Type? KeyType { get; set; }
    }
}