using ApplicationCore;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Snail.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 测试完后，移入到core里
/// </summary>
namespace Infrastructure.Services
{
    public interface IEntityCacheManager
    {
        List<TEntity> Get<TEntity>() where TEntity : class;
    }

    public class EntityCacheManager: IEntityCacheManager,ICapSubscribe
    {
        private AppDbContext _db;
        private IMemoryCache _memoryCache;
        public EntityCacheManager(AppDbContext db,IMemoryCache memoryCache)
        {
            _db = db;
            _memoryCache = memoryCache;
        }

        public List<TEntity> Get<TEntity>() where TEntity : class
        {
            var key = GenerateEntityCacheKey<TEntity>();
            return  _memoryCache.GetOrCreate<List<TEntity>>(key, a=>LoadEntities<TEntity>());
        }

        private List<TEntity> LoadEntities<TEntity>() where TEntity:class
        {
            EnsureEnableCache<TEntity>();
            return _db.Set<TEntity>().AsNoTracking().ToList();

        }

        private void EnsureEnableCache<TEntity>()
        {
            if (!Attribute.IsDefined(typeof(TEntity),typeof(EnableEntityCacheAttribute)))
            {
                throw new BusinessException($"实体类{typeof(TEntity).Name}未启用缓存");
            }
        }

        private string GenerateEntityCacheKey<TEntity>()
        {
            return GenerateEntityCacheKey(typeof(TEntity).Name);
        }
        private string GenerateEntityCacheKey(string entityName)
        {
            return $"EntityCache_{entityName}".ToLower();
        }

        [CapSubscribe(EventConstant.EntityChange)]
        public void ClearCache(EntityChangeEvent entityChangeEvent)
        {
            _memoryCache.Remove(GenerateEntityCacheKey(entityChangeEvent.EntityName));
        }
        
    }

    public class EntityChangeEvent
    {
        public string EntityName { get; set; }
    }

}
