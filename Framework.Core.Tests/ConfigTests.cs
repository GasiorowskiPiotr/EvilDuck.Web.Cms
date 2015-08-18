using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EvilDuck.Framework.Core.Configuration;
using Xunit;

namespace Framework.Core.Tests
{
    public class ConfigTests
    {
        [Fact]
        public void When_accessing_configuration_that_exists_it_should_return_whole_section()
        {
            var conf = TypedConfiguration.GetSection<SampleConf>(SampleConf.Name);
            Assert.NotNull(conf);

            Assert.NotEmpty(conf.SampleProp);
        }

        [Fact]
        public void When_accessing_configuration_that_doesnt_exist_it_should_throw()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                TypedConfiguration.GetSection<SampleConf>(SampleConf.WrongName);
            });
        }

        
    }

    public class SampleConf : ConfigurationSection
    {
        public const string Name = "Test.SampleConfig";
        public const string WrongName = "Test.WrongConfig";

        [ConfigurationProperty("sampleProp")]
        public string SampleProp
        {
            get { return (string)base["sampleProp"]; }
        }
    }
}
