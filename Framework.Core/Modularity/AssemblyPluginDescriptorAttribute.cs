using System;

namespace EvilDuck.Framework.Core.Modularity
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class AssemblyPluginDescriptorAttribute : Attribute
    {
        private readonly Type _pluginType;

        public AssemblyPluginDescriptorAttribute(Type pluginType)
        {
            _pluginType = pluginType;
        }

        public Type PluginType
        {
            get { return _pluginType; }
        }
    }
}