using EvilDuck.Applications.SystemParameters.Core.DataAccess;
using EvilDuck.Applications.SystemParameters.Entities;
using EvilDuck.Framework.Core.Cache;
using NLog;

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

            var domainContext = new SystemParametersDomainContext();
            var sp = domainContext.SystemParameters.Find(code);
            if (sp != null)
            {
                Add(key, sp);
                value = sp;
                return;
            }
            value = null;
        }

        private class SystemParametersCacheKey : ICacheKey
        {

            private readonly string _code;

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
