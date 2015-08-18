using System;
using System.Globalization;
using EvilDuck.Framework.Core.Cache;
using EvilDuck.Framework.Core.Testing;
using NLog;
using Xunit;

namespace Framework.Core.Tests
{
    public class CacheTest : IDisposable
    {

        public class SampleTestCache : CustomCache
        {
            public int MissCount = 0;
            public int HitCount = 0;

            public SampleTestCache(Logger logger) : base(logger)
            {
            }

            protected override void OnMiss(ICacheKey key, out object value)
            {
                base.OnMiss(key, out value);
                this.Add(key, key.KeyAsString);
                value = key.KeyAsString;
                MissCount++;
            }

            protected override void OnHit(ICacheKey key, ref object value)
            {
                base.OnHit(key, ref value);
                HitCount++;
            }

            public string Get(int num)
            {
                return (string) Get(new SampleTestCacheKey(num));
            }

            public void Remove(int num)
            {
                Remove(new SampleTestCacheKey(num));
            }

            class SampleTestCacheKey : ICacheKey
            {
                public SampleTestCacheKey(int number)
                {
                    Number = number;
                }

                private int Number { get; set; }

                public string KeyAsString
                {
                    get { return Number.ToString(CultureInfo.InvariantCulture); }
                }
            }
        }

        private Func<SampleTestCache> _cacheFactory;

        public CacheTest()
        {
            _cacheFactory = () => new SampleTestCache(LogManager.CreateNullLogger());
        }

        public void Dispose()
        {
            _cacheFactory = null;
        }

        [Fact]
        public void When_accessing_cached_object_for_the_first_time_it_should_call_OnMiss_and_add_it_to_cache()
        {
            var cache = _cacheFactory();

            var a = cache.Get(111);

            Assert.Equal(a, "111");
            Assert.Equal(0, cache.HitCount);
            Assert.Equal(1, cache.MissCount);

        }

        [Fact]
        public void When_accessing_cached_object_for_the_second_time_it_should_call_OnHit_and_use_cached_one()
        {
            var cache = _cacheFactory();

            var a = cache.Get(222);

            Assert.Equal(0, cache.HitCount);
            Assert.Equal(1, cache.MissCount);

            a = cache.Get(222);

            Assert.Equal(1, cache.HitCount);
            Assert.Equal(1, cache.MissCount);
        }

        [Fact]
        public void When_accessing_cached_object_after_absolute_timeout_it_should_recreate_it()
        {
            var cache = _cacheFactory();
            cache.AbsoluteExpirationSpan = TimeSpan.FromSeconds(5);

            var a = cache.Get(333);

            Assert.Equal(0, cache.HitCount);
            Assert.Equal(1, cache.MissCount);

            Awaiter.Wait(TimeSpan.FromSeconds(6));

            cache.Get(333);

            Assert.Equal(0, cache.HitCount);
            Assert.Equal(2, cache.MissCount);
        }

        [Fact]
        public void When_accessing_cached_object_frequently_but_below_sliding_timeout_it_should_not_recreate()
        {
            var cache = _cacheFactory.Invoke();

            cache.SlidingExpirationSpan = TimeSpan.FromSeconds(3);

            for (var i = 0; i < 10; i++)
            {
                cache.Get(444);
                Awaiter.Wait(TimeSpan.FromSeconds(2));
            }

            Assert.Equal(9, cache.HitCount);
            Assert.Equal(1, cache.MissCount);

        }

        [Fact]
        public void When_accessing_cached_objects_events_should_be_fired()
        {
            var cache = _cacheFactory.Invoke();
            int hitCount = 0;
            int missCount = 0;

            cache.Hit += (sender, args) =>
            {
                hitCount++;
            };

            cache.Miss += (sender, args) =>
            {
                args.Value = args.Key.KeyAsString;
                missCount++;
            };

            cache.Get(555);

            Assert.Equal(1, missCount);

            cache.Get(555);

            Assert.Equal(1, hitCount);
        }

        [Fact]
        public void It_can_remove_and_recreate_value()
        {
            var cache = _cacheFactory.Invoke();
            int hitCount = 0;
            int missCount = 0;

            cache.Hit += (sender, args) =>
            {
                hitCount++;
            };

            cache.Miss += (sender, args) =>
            {
                args.Value = args.Key.KeyAsString;
                missCount++;
            };

            cache.Get(666);

            Assert.Equal(1, missCount);
            Assert.Equal(0, hitCount);

            cache.Remove(666);
            cache.Get(666);

            Assert.Equal(2, missCount);
            Assert.Equal(0, hitCount);
        }
    }
}