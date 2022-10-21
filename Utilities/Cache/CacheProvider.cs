using System;
using System.Runtime.Caching;

namespace Utilities.Cache
{
    /// <summary>
    /// Class các hàm quản lý cache
    /// </summary>
    public class CacheProvider : ICacheProvider
    {
        private static readonly object _lockCache = new object();

        private ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        public void Set(string key, object data, int cacheTime = 60)
        {
            if (data == null)
            {
                return;
            }
            lock (_lockCache)
            {
                var policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);
                Cache.Add(new CacheItem(key, data), policy);   
            }
        }

        public T Get<T>(string key)
        {
            try
            {
                return (T)Cache[key];
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public bool IsStored(string key)
        {
            return Cache.Contains(key);
        }


        public void RemoveByTerm(string term)
        {
            foreach (var item in Cache)
            {
                if(item.Key.Contains(term))
                {
                    Cache.Remove(item.Key);
                }
            }
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }

        public void Clear()
        {
            foreach (var item in Cache)
            {
                Cache.Remove(item.Key);
            }
        }
    }
}
