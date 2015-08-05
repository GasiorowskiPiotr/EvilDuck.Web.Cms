using System.Data.Entity;
using System.Reflection;
using EvilDuck.Applications.SystemParameters.Entities;
using EvilDuck.Framework.Core.DataAccess;

namespace EvilDuck.Applications.SystemParameters.Core.DataAccess
{
    public class SystemParametersDomainContext : DomainContext
    {
        public SystemParametersDomainContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        protected override Assembly GetMappingsAssembly()
        {
            return typeof (SystemParameter).Assembly;
        }

        public DbSet<SystemParameter> SystemParameters { get; set; } 
    }
}
