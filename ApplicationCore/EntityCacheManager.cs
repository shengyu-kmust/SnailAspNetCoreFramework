using ApplicationCore.Abstract;
using AutoMapper;
using EasyCaching.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ApplicationCore
{
    /// <summary>
    /// 约定：一个TEntity只能对应一个TCache
    /// </summary>
    public class EntityCacheManager : IEntityCacheManager
    {
        private readonly IEasyCachingProviderFactory _factory;
        private IMapper _mapper;
        private IEasyCachingProvider _cacheProvider;
        private DbContext _db;
        public const string EntityCacheName = "EntityMemoryCache";
        private TimeSpan DefaultCacheTimeSpan = new TimeSpan(1, 0, 0, 0);
        public EntityCacheManager(IMapper mapper, IEasyCachingProviderFactory factory, DbContext db)
        {
            _factory = factory;
            _mapper = mapper;
            _db = db;
            _cacheProvider = factory.GetCachingProvider(EntityCacheName);
        }

        public List<TCacheItem> GetAll<TEntity, TCacheItem>() where TEntity : class
        {
            var cacheKey = EntityCacheKey<TEntity, TCacheItem>();
            return _cacheProvider.Get<List<TCacheItem>>(cacheKey, GetCacheItemsFromDb<TEntity, TCacheItem>, DefaultCacheTimeSpan).Value;
        }

        

        private List<TCacheItem> GetCacheItemsFromDb<TEntity, TCacheItem>() where TEntity : class
        {
            var allEntities = _db.Set<TEntity>().AsNoTracking().ToList();
            var allCacheItem = _mapper.Map<List<TCacheItem>>(allEntities);
            return allCacheItem;
        }

        public void ReLoadAll<TEntity, TCacheItem>() where TEntity : class
        {
            _cacheProvider.Set(EntityCacheKey<TEntity, TCacheItem>(), GetCacheItemsFromDb<TEntity, TCacheItem>(), DefaultCacheTimeSpan);
        }


        private string EntityCacheKey<TEntity, TCacheItem>()
        {
            return $"{typeof(TEntity).Name}_{typeof(TEntity).Name}";
        }

        private string EntityCacheKey(string entityName,string cacheItemName)
        {
            return $"{entityName}_{cacheItemName}";
        }

        public void UpdateEntityCacheItem<TEntity,TCacheItem,TKey>(TKey key) where TEntity : class where TCacheItem : class, IIdField<TKey>
        {
            var entity = _db.Set<TEntity>().Find(key);
            var cacheKey = EntityCacheKey<TEntity, TCacheItem>();
            var cacheItems = _cacheProvider.Get<List<TCacheItem>>(cacheKey).Value;
            if (cacheItems==null)
            {
                throw new Exception($"{cacheKey}缓存不存在");
            }
            var oldCacheItem = cacheItems.FirstOrDefault(CreateEqualityExpressionForId<TCacheItem, TKey>(key).Compile());
            if (entity!=null)
            {
                // update
                var newCacheItem = _mapper.Map<TCacheItem>(entity);
                if (oldCacheItem==default(TCacheItem))
                {
                    cacheItems.Add(newCacheItem);
                }
                else
                {
                    _mapper.Map(newCacheItem, oldCacheItem);
                }
            }
            else
            {
                //delete 
                if (oldCacheItem != default(TCacheItem))
                {
                    cacheItems.Remove(oldCacheItem);
                }
            }

        }

        private Expression<Func<T, bool>> CreateEqualityExpressionForId<T,TKey>(TKey id) where T : class, IIdField<TKey>
        {
            var lambdaParam = Expression.Parameter(typeof(T));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TKey))
                );

            return Expression.Lambda<Func<T, bool>>(lambdaBody, lambdaParam);
        }

        public List<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return GetAll<TEntity, TEntity>();
        }

        public void ReLoadAll<TEntity>() where TEntity : class
        {
            ReLoadAll<TEntity, TEntity>();
        }

        void IEntityCacheManager.UpdateEntityCacheItem<TEntity, TKey>(TKey key)
        {
            UpdateEntityCacheItem<TEntity, TEntity, TKey>(key);
        }
    }

        public interface IEntityCacheManager
        {
            List<TCacheItem> GetAll<TEntity, TCacheItem>() where TEntity : class;
            List<TEntity> GetAll<TEntity>() where TEntity : class;

            void ReLoadAll<TEntity, TCacheItem>() where TEntity : class;
            void ReLoadAll<TEntity>() where TEntity : class;

            void UpdateEntityCacheItem<TEntity, TCacheItem, TKey>(TKey key) where TEntity : class where TCacheItem : class, IIdField<TKey>;

            void UpdateEntityCacheItem<TEntity, TKey>(TKey key) where TEntity : class, IIdField<TKey>;

        }

    public interface IEntityCacheUpdateEventHandler
    {
    }
}
