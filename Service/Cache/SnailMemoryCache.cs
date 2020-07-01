using Microsoft.Extensions.Caching.Memory;
using System;

namespace Snail.Cache
{
    public class SnailMemoryCache : ISnailCache
    {
        private readonly IMemoryCache memoryCache;
        public SnailMemoryCache (IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="key"></param>
        /// <param name="absoluteExpirationRelativeToNow"></param>
        /// <returns></returns>
        public TItem Get<TItem>(string key)
        {
            return memoryCache.Get<TItem>(key);
        }

        public TItem GetOrSet<TItem>(string key, Func<string, TItem> func, TimeSpan? absoluteExpirationRelativeToNow)
        {
           return memoryCache.GetOrCreate(key, entry =>
            {
                if (absoluteExpirationRelativeToNow==null)
                {
                    entry.AbsoluteExpiration = DateTimeOffset.MaxValue;
                }
                else
                {
                    entry.AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
                }
                return func(key);
            });
        }

        public void Remove(string key)
        {
            memoryCache.Remove(key);
        }

        public void Set<TItem>(string key, TItem value, TimeSpan? absoluteExpirationRelativeToNow)
        {
            if (absoluteExpirationRelativeToNow==null)
            {
                memoryCache.Set<TItem>(key, value,DateTimeOffset.MaxValue);
            }
            else
            {
                memoryCache.Set<TItem>(key, value,absoluteExpirationRelativeToNow.Value);
            }
        }
    }
}
