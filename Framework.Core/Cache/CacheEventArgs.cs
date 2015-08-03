using System;

namespace EvilDuck.Framework.Core.Cache
{
    public class CacheEventArgs : EventArgs
    {
        public ICacheKey Key;
        public object Value;

        public CacheEventArgs(ICacheKey key, object value)
        {
            Key = key;
            Value = value;
        }

        public CacheEventArgs(ICacheKey key)
        {
            Key = key;
        }
    }
}