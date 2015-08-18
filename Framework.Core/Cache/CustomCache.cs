using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using EvilDuck.Framework.Core.Components;
using NLog;

namespace EvilDuck.Framework.Core.Cache
{
    public abstract class CustomCache : BaseComponent
    {
        protected CustomCache(Logger logger)
            : base(logger)
        {
        }

        private object Get(string key)
        {
            return MemoryCache.Default.Get(key);
        }

        private void Add(string key, object value, DateTime absoluteExpiration)
        {
            var cacheItemPolicy = new CacheItemPolicy { AbsoluteExpiration = absoluteExpiration };

            if (Logger.IsInfoEnabled)
            {
                Logger.Info("Adding item with key: {0} and absolute expiration: {1}", key, absoluteExpiration);
            }
            MemoryCache.Default.Add(key, value, cacheItemPolicy);
        }

        private void Add(string key, object value, TimeSpan slidingExpiration)
        {

            var cacheItemPolicy = new CacheItemPolicy { SlidingExpiration = slidingExpiration };

            if (Logger.IsInfoEnabled)
            {
                Logger.Info("Adding item with key: {0} and sliding expiration: {1}", key, slidingExpiration);
            }
            MemoryCache.Default.Add(key, value, cacheItemPolicy);
        }

        private object Remove(string key)
        {
            if (Logger.IsInfoEnabled)
            {
                Logger.Info("Removing item with key: {0}", key);
            }
            return MemoryCache.Default.Remove(key);
        }

        private string BuildUniqueStringKey(ICacheKey key)
        {
            return String.Format("{0};{1};{2}", GetType().FullName, key.GetType().FullName, key.KeyAsString);
        }

        private bool _useAbsoluteExpiration = true;
        private bool _useSlidingExpiration;

        private TimeSpan _absoluteExpirationSpan = TimeSpan.FromHours(1);
        public TimeSpan AbsoluteExpirationSpan
        {
            get { return _absoluteExpirationSpan; }
            set
            {
                _useAbsoluteExpiration = true;
                _useSlidingExpiration = false;
                _absoluteExpirationSpan = value;
            }
        }

        private TimeSpan _slidingExpirationSpan = ObjectCache.NoSlidingExpiration;
        public TimeSpan SlidingExpirationSpan
        {
            get { return _slidingExpirationSpan; }
            set
            {
                _useAbsoluteExpiration = false;
                _useSlidingExpiration = true;
                _slidingExpirationSpan = value;
            }
        }

        public DateTime AbsoluteExpiration
        {
            get { return DateTime.UtcNow + AbsoluteExpirationSpan; }
        }

        public event CacheEventHandler Hit;
        public event CacheEventHandler Miss;

        private readonly Dictionary<string, object> _syncItems = new Dictionary<string, object>();
        private readonly object _syncObj = new object();

        protected object Get(ICacheKey key)
        {
            string itemKey = BuildUniqueStringKey(key);
            if (Logger.IsInfoEnabled)
            {
                Logger.Info("Getting item with key: {0}", key);
            }
            object value = Get(itemKey);
            if (value == null)
            {
                if (Logger.IsInfoEnabled)
                {
                    Logger.Info("Item with key: {0} not found in cache.", key);
                }
                //Lock only single (new) item instead of whole cache
                lock (GetSyncObject(itemKey))
                {
                    if (Logger.IsInfoEnabled)
                    {
                        Logger.Info("Trying to get item with key: {0} once more - from memory barrier.", key);
                    }
                    value = Get(itemKey);
                    if (value == null)
                    {
                        if (Logger.IsInfoEnabled)
                        {
                            Logger.Info("Item with key: {0} not found in cache.", key);
                        }
                        OnMiss(key, out value);
                    }
                    else
                    {
                        if (Logger.IsInfoEnabled)
                        {
                            Logger.Info("Item with key: {0} found in cache.", key);
                        }
                        OnHit(key, ref value);
                    }
                    RemoveSyncObject(itemKey);
                }
            }
            else
            {
                if (Logger.IsInfoEnabled)
                {
                    Logger.Info("Item with key: {0} found in cache.", key);
                }
                OnHit(key, ref value);
            }

            return value;
        }

        private object GetSyncObject(string itemKey)
        {
            lock (_syncObj)
            {
                object value;
                if (_syncItems.TryGetValue(itemKey, out value))
                    return value;

                var newValue = new object();
                _syncItems.Add(itemKey, newValue);
                return newValue;
            }
        }

        private void RemoveSyncObject(string itemKey)
        {
            if (_syncItems.ContainsKey(itemKey))
            {
                lock (_syncObj)
                {
                    if (_syncItems.ContainsKey(itemKey))
                    {
                        _syncItems.Remove(itemKey);
                    }
                }
            }
        }

        protected void Add(ICacheKey key, object value)
        {
            if (_useSlidingExpiration)
            {
                Add(BuildUniqueStringKey(key), value, SlidingExpirationSpan);
            }
            else if (_useAbsoluteExpiration)
            {
                Add(BuildUniqueStringKey(key), value, AbsoluteExpiration);
            }
        }

        protected object Remove(ICacheKey key)
        {
            return Remove(BuildUniqueStringKey(key));
        }

        protected virtual void OnHit(ICacheKey key, ref object value)
        {
            if (Hit != null)
            {
                var args = new CacheEventArgs(key, value);
                Hit(this, args);
                value = args.Value;
            }
        }

        protected virtual void OnMiss(ICacheKey key, out object value)
        {
            if (Miss != null)
            {
                var args = new CacheEventArgs(key);
                Miss(this, args);
                value = args.Value;
            }
            else
            {
                value = null;
            }
        }
    }
}