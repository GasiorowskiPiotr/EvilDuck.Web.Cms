using System;
using System.Collections.Concurrent;
using System.Configuration;

namespace EvilDuck.Framework.Core.Configuration
{
    public class TypedConfiguration
    {
        private static readonly ConcurrentDictionary<string, ConfigurationSection> ConfigurationSections = new ConcurrentDictionary<string, ConfigurationSection>();

        public static TSection GetSection<TSection>(string name) where TSection : ConfigurationSection
        {
            return (TSection)ConfigurationSections.GetOrAdd(name, s =>
            {
                var sec = ConfigurationManager.GetSection(s);
                if (sec == null)
                {
                    throw new ArgumentOutOfRangeException(String.Format("Section: {0} not found in condif file.", s));
                }

                return (ConfigurationSection)sec;
            });
        }
    }
}