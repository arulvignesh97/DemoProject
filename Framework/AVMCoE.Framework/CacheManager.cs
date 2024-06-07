using System;
using System.Runtime.Caching;

namespace AVMCoE.Framework
{
    public class CacheManger : ICacheService
    {
        public T ExecuteCache<T>(string cacheKey, Func<T> getItemCallback, CacheDuration cacheDuration) where T : class
        {
            T item = MemoryCache.Default.Get(cacheKey) as T;
            if (item == null)
            {
                item = getItemCallback();
                MemoryCache.Default.Add(cacheKey, item, DateTime.Now.AddMinutes((int)cacheDuration));
            }
            return item;
        }

        public bool Clear(string cacheKey)
        {
            if (MemoryCache.Default.Get(cacheKey) != null)
            {
                MemoryCache.Default.Remove(cacheKey);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsExists(string cacheKey)
        {
            if (MemoryCache.Default.Get(cacheKey) != null)
            {
                return true;
            }
            return false;
        }
    }

    interface ICacheService
    {
        T ExecuteCache<T>(string cacheKey, Func<T> getItemCallback, CacheDuration cacheDuration) where T : class;

        bool Clear(string cacheKey);

        bool IsExists(string cacheKey);
    }

    public enum CacheDuration
    {
        Short = 15,
        Medium = 20,
        Long = 30
    }
}