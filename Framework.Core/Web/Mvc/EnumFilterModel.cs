using System;

namespace EvilDuck.Framework.Core.Web.Mvc
{
    public class EnumFilterModel
    {
        public string Property { get; private set; }
        public Type EnumType { get; private set; }

        private EnumFilterModel(string property, Type enumType)
        {
            Property = property;
            EnumType = enumType;
        }

        public static EnumFilterModel Init(string property, Type enumType)
        {
            return new EnumFilterModel(property, enumType);
        }
    }
}