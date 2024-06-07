/***************************************************************************
 * COGNIZANT CONFIDENTIAL AND/OR TRADE SECRET
 * Copyright [2018] – [2021] Cognizant. All rights reserved.
 * NOTICE: This unpublished material is proprietary to Cognizant and
 * its suppliers, if any. The methods, techniques and technical
 * concepts herein are considered Cognizant confidential and/or trade secret information.
 * This material may be covered by U.S. and/or foreign patents or patent applications.
 * Use, distribution or copying, in whole or in part, is forbidden, except by express written permission of Cognizant.
 ***************************************************************************/

using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Runtime.Caching;

namespace CTS.Applens.Framework
{
    /// <summary>
    /// Class for Cache Manager
    /// </summary>
    public class CacheManager : ICacheService
    {
        private IDatabase _cache;

        /// <param name="cacheKey"></param>
        /// <param name="getItemCallback"></param>
        /// <param name="cacheDuration"></param>
        /// <returns></returns>
        public T GetOrCreate<T>(string cacheKey, Func<T> getItemCallback, CacheDuration cacheDuration) where T : class
        {
            T item = MemoryCache.Default.Get(cacheKey) as T;
            if (item == null)
            {
                item = getItemCallback();
                MemoryCache.Default.Add(cacheKey, item, DateTime.Now.AddMinutes((int)cacheDuration));
            }
            return item;
        }

        /// <summary>
        /// Method to clear the cache item
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method to check key is exits in caching
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public bool IsExists(string cacheKey)
        {
            if (MemoryCache.Default.Get(cacheKey) != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method to get or create item in cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objRedis"></param>
        /// <param name="getItemCallback"></param>
        /// <param name="cacheDuration"></param>
        /// <returns></returns>
        public T GetOrCreate<T>(RedisCacheModel objRedis, Func<T> getItemCallback, CacheDuration cacheDuration) where T : class
        {
            if (objRedis != null && !String.IsNullOrEmpty(objRedis.CacheKey))
            {
                if (objRedis.EnabledRedis)
                {
                    string RedisJsonPath = Environment.GetEnvironmentVariable("RedisConfigPath");
                    if (Environment.OSVersion.Platform == PlatformID.Win32NT && RedisJsonPath != null)
                    {
                        var redisJsonDetails = System.IO.File.ReadAllText(RedisJsonPath);
                        var jsonObj = JsonConvert.DeserializeObject<RedisServer>(redisJsonDetails);
                        var connectionMultiplexer = ConnectionMultiplexer.Connect($"{jsonObj.IP}:{jsonObj.Port}," +
                       $"resolvedns=1,abortConnect=False");
                        _cache = connectionMultiplexer.GetDatabase();

                        T cacheDetail;
                        var item = _cache.StringGetAsync(objRedis.CacheKey);
                        if (item.Result.HasValue)
                        {
                            cacheDetail = JsonConvert.DeserializeObject<T>(item.Result);
                        }
                        else
                        {
                            cacheDetail = getItemCallback();
                            _cache.StringSetAsync(objRedis.CacheKey, JsonConvert.SerializeObject(cacheDetail),
                                new TimeSpan(0,(int)cacheDuration, 0));
                        }
                        return cacheDetail;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    T item = MemoryCache.Default.Get(objRedis.CacheKey) as T;
                    if (item == null)
                    {
                        item = getItemCallback();
                        MemoryCache.Default.Add(objRedis.CacheKey, item, DateTime.Now.AddMinutes((int)cacheDuration));
                    }
                    return item;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///  Method to clear the cache item
        /// </summary>
        /// <param name="objRedis"></param>
        /// <returns></returns>
        public bool Clear(RedisCacheModel objRedis)
        {
            if (objRedis != null && !String.IsNullOrEmpty(objRedis.CacheKey))
            {
                if (objRedis.EnabledRedis)
                {
                    string RedisJsonPath = Environment.GetEnvironmentVariable("RedisConfigPath");
                    if (Environment.OSVersion.Platform == PlatformID.Win32NT && RedisJsonPath != null)
                    {
                        var redisJsonDetails = System.IO.File.ReadAllText(RedisJsonPath);
                        var jsonObj = JsonConvert.DeserializeObject<RedisServer>(redisJsonDetails);
                        var connectionMultiplexer = ConnectionMultiplexer.Connect($"{jsonObj.IP}:{jsonObj.Port}," +
                       $"resolvedns=1,abortConnect=False");
                        _cache = connectionMultiplexer.GetDatabase();

                        if (!_cache.StringGet(objRedis.CacheKey).IsNull)
                        {
                            _cache.KeyDelete(objRedis.CacheKey);
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (MemoryCache.Default.Get(objRedis.CacheKey) != null)
                    {
                        MemoryCache.Default.Remove(objRedis.CacheKey);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Method to check key is exits in caching
        /// </summary>
        /// <param name="objRedis"></param>
        /// <returns></returns>
        public bool IsExists(RedisCacheModel objRedis)
        {
            if (objRedis != null && !String.IsNullOrEmpty(objRedis.CacheKey))
            {
                if (objRedis.EnabledRedis)
                {
                    string RedisJsonPath = Environment.GetEnvironmentVariable("RedisConfigPath");
                    if (Environment.OSVersion.Platform == PlatformID.Win32NT && RedisJsonPath != null)
                    {
                        var redisJsonDetails = System.IO.File.ReadAllText(RedisJsonPath);
                        var jsonObj = JsonConvert.DeserializeObject<RedisServer>(redisJsonDetails);
                        var connectionMultiplexer = ConnectionMultiplexer.Connect($"{jsonObj.IP}:{jsonObj.Port}," +
                       $"resolvedns=1,abortConnect=False");
                        _cache = connectionMultiplexer.GetDatabase();
                        if (!_cache.StringGet(objRedis.CacheKey).IsNull)
                        {
                            return true;
                        }
                        return false;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (MemoryCache.Default.Get(objRedis.CacheKey) != null)
                    {
                        return true;
                    }
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
    /// <summary>
    /// Caching Interface 
    /// </summary>
    internal interface ICacheService
    {
       T GetOrCreate<T>(string cacheKey, Func<T> getItemCallback, CacheDuration cacheDuration) where T : class;
        bool Clear(string cacheKey);
        bool IsExists(string cacheKey);
        T GetOrCreate<T>(RedisCacheModel objRedis, Func<T> getItemCallback, CacheDuration cacheDuration) where T : class;
        bool Clear(RedisCacheModel objRedis);
        bool IsExists(RedisCacheModel objRedis);
    }

    /// <summary>
    /// Enum to cache duration
    /// </summary>
    public enum CacheDuration
    {
        Short = 15,
        Medium = 20,
        Long = 30
    }

    /// <summary>
    /// RedisCache Model 
    /// </summary>
    public class RedisCacheModel
    {
        public string CacheKey { get; set; }
        public bool EnabledRedis { get; set; }
    }

    /// <summary>
    /// Redis Server Details
    /// </summary>
    public class RedisServer
    {
        public string IP { get; set; }
        public int Port { get; set; }
    }
}