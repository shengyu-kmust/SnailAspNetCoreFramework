using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Snail.Cache
{
    public static class SnailMemoryCacheServiceCollectionExtensions
    {
        public static IServiceCollection AddSnailMemoryCache(this IServiceCollection services)
        {
            ServiceCollectionDescriptorExtensions.TryAdd(services, ServiceDescriptor.Singleton<ISnailCache, SnailMemoryCache>());
            services.AddMemoryCache();
            return services;
        }


        public static IServiceCollection AddSnailMemoryCache(this IServiceCollection services, Action<MemoryCacheOptions> setupAction)
        {
            ServiceCollectionDescriptorExtensions.TryAdd(services, ServiceDescriptor.Singleton<ISnailCache, SnailMemoryCache>());
            services.AddMemoryCache(setupAction);
            return services;
        }

    }
}
