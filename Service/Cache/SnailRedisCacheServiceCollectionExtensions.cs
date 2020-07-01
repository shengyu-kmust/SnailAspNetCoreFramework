using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Snail.Cache
{
    public static class SnailRedisCacheServiceCollectionExtensions
    {
        public static IServiceCollection AddSnailRedisMemoryCache(this IServiceCollection services, Action<RedisCacheOptions> setupAction)
        {
            services.AddStackExchangeRedisCache(setupAction);
            ServiceCollectionDescriptorExtensions.TryAdd(services, ServiceDescriptor.Singleton<ISnailCache, SnailRedisCache>());
            return services;
        }
    }
}
