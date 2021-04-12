using API.Settings;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace API.Cache
{
    public class SimpleCache : ICache
    {
        private IMemoryCache _cache;
        private CacheSettings _cacheSettings;
        public class Wrapper<T>
        {
            public T Value { get; set; }
            public DateTime CacheEndOfLife { get; set; }
        }

        public SimpleCache(IMemoryCache cache, CacheSettings cacheSettings)
        {
            _cache = cache;
            _cacheSettings = cacheSettings;
        }

        public object this[string key] => GetValue(key);

        public void Add(string key, object value)
        {
            _cache.Set(key, new Wrapper<object>
            {
                CacheEndOfLife = DateTime.Now.AddMinutes(_cacheSettings.CacheLifeTimeInMinutes),
                Value = value
            }); ;
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            value = GetValue(key);
            return value != null;
        }

        private object GetValue(object key)
        {
            if (!_cache.TryGetValue(key, out object storedValue))
                return null;
            var wrapper = (Wrapper<object>)storedValue;
            if (wrapper.CacheEndOfLife < DateTime.Now)
            {
                return null;
            }

            return wrapper.Value;
        }
    }
}