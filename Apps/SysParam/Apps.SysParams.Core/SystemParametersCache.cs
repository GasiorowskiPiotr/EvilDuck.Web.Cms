using EvilDuck.Applications.SystemParameters.Core.DataAccess;
using EvilDuck.Applications.SystemParameters.Entities;
using EvilDuck.Framework.Core.Cache;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace EvilDuck.Applications.SystemParameters.Core
{
    public class SystemParametersCache : CustomCache
    {
        public SystemParametersCache(Logger logger) : base(logger)
        {

        }

        public SystemParameter Get(string code)
        {
            return (SystemParameter)base.Get(new SystemParametersCacheKey(code));
        }

        protected override void OnMiss(ICacheKey key, out object value)
        {
            var codeKey = (SystemParametersCacheKey)key;
            var code = codeKey.KeyAsString;

            var domainContext = (SystemParametersDomainContext)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(SystemParametersDomainContext));
            var sp = domainContext.SystemParameters.Find(code);
            if (sp != null)
            {
                base.Add(key, sp);
                value = sp;
            }
            value = null;
            
        }

        private class SystemParametersCacheKey : ICacheKey
        {

            private string _code;

            public SystemParametersCacheKey(string code)
            {
                _code = code;
            }
            

            public string KeyAsString
            {
                get 
                {
                    return _code;
                }
            }
        }
    }
}
