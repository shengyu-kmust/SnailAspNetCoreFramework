using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;

namespace Snail.Cache
{
    public class SnailRedisCache : ISnailCache
    {
        private readonly IDistributedCache cache;
        public SnailRedisCache(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public TItem Get<TItem>(string key)
        {
            var itemStr = cache.GetString(key);
            if (itemStr==null)
            {
                return default(TItem);
            }
            else
            {
                return JsonConvert.DeserializeObject<TItem>(itemStr);
            }
        }

        public TItem GetOrSet<TItem>(string key, Func<string, TItem> func, TimeSpan? absoluteExpirationRelativeToNow)
        {
            var itemStr = cache.GetString(key);
            if (itemStr==null)
            {
                itemStr=JsonConvert.SerializeObject(func(key));
                cache.SetString(key, itemStr, GetDistributedCacheEntryOptions(absoluteExpirationRelativeToNow));
            }
            return JsonConvert.DeserializeObject<TItem>(itemStr);

        }

        public void Remove(string key)
        {
            cache.Remove(key);
        }

        public void Set<TItem>(string key, TItem value, TimeSpan? absoluteExpirationRelativeToNow)
        {
            cache.SetString(key,JsonConvert.SerializeObject(value), GetDistributedCacheEntryOptions(absoluteExpirationRelativeToNow));
        }

        private DistributedCacheEntryOptions GetDistributedCacheEntryOptions(TimeSpan? absoluteExpirationRelativeToNow)
        {
            if (absoluteExpirationRelativeToNow.HasValue)
            {
                return new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow };
            }
            else
            {
                return new DistributedCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.MaxValue };
            }
        }
    }
}
