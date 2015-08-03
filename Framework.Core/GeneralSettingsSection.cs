using System.Configuration;

namespace EvilDuck.Framework.Core
{
    public class GeneralSettingsSection : ConfigurationSection
    {
        [ConfigurationProperty("environmentType", DefaultValue = EnvironmentType.Development)]
        public EnvironmentType EnvironmentType
        {
            get { return (EnvironmentType) base["environmentType"]; }
        }

        [ConfigurationProperty("pluginAssemblies")]
        public string PluginAssemblies
        {
            get { return (string) base["pluginAssemblies"]; }
        }

        public const string SectionName = "EvilDuck.Framework.GeneralSettings";
    }
}