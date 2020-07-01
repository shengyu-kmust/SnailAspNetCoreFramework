using ApplicationCore.IServices;
using AutoMapper;
using DotNetCore.CAP;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Snail.Cache;
using Snail.Core.Entity;
using Snail.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public abstract class ServiceContextBaseService : IService
    {
        protected IEntityCacheManager entityCacheManager => serviceContext.entityCacheManager;
        protected IMapper mapper => serviceContext.mapper;
        protected IApplicationContext applicationContext => serviceContext.applicationContext;
        protected string currentUserId => serviceContext.applicationContext.GetCurrentUserId();
        public AppDbContext db => serviceContext.db;
        public IMemoryCache memoryCache => serviceContext.memoryCache;
        public ICapPublisher publisher => serviceContext.publisher;
        public ISnailCache cache => serviceContext.cache;
        public IServiceProvider serviceProvider => serviceContext.serviceProvider;
        public ServiceContext serviceContext;
        protected ServiceContextBaseService(ServiceContext serviceContext)
        {
            this.serviceContext = serviceContext;
        }

        /// <summary>
        /// 获取entity的缓存，用于TEntity会频繁修改，又需要自己控制cache刷新的情况（EntityCacheManager不适用的情况）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TEntityCache"></typeparam>
        /// <returns></returns>
        public List<TEntityCache> GetEntityCache<TEntity, TEntityCache>()
         where TEntity : class, IBaseEntity
        {
            return cache.GetOrSet<List<TEntityCache>>(GetEntityCacheKey<TEntity, TEntityCache>(), key =>
            {
                return mapper.ProjectTo<TEntityCache>(db.Set<TEntity>().AsNoTracking()).ToList();
            }, null);
        }

        public void ClearEntityCache<TEntity, TEntityCache>()
        {
            cache.Remove(GetEntityCacheKey<TEntity, TEntityCache>());
        }

        public string GetEntityCacheKey<TEntity, TEntityCache>()
        {
            return $"cacheService_{typeof(TEntity).Name}_{typeof(TEntityCache).Name}";
        }
    }
}
