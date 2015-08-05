using EvilDuck.Framework.Entities.Maps;

namespace EvilDuck.Applications.SystemParameters.Entities.Maps
{
    public class SystemParameterMap : BaseEntityWithKeyTypeConfiguration<SystemParameter, string>
    {
        public SystemParameterMap()
        {
            Property(e => e.Description).HasColumnType("TEXT");
            Property(e => e.KeyType);
            Property(e => e.ParameterType);
            Property(e => e.SerializedValue);
            Property(e => e.ValueType);
        }
    }
}
