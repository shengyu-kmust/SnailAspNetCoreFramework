using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationCore.Abstract
{
    public class EntityCaching<TKey, TValue> : IEntityCaching<TKey, TValue> where TValue:class,IEntityId<TKey>
    {
        private Dictionary<TKey, TValue> _keyValuePairs;
        public Dictionary<TKey, TValue> KeyValuePairs => _keyValuePairs;
        public List<TValue> Values => _keyValuePairs.Values.ToList();//对外只读，这是只读属性
        private IServiceProvider _serviceProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public TValue this[TKey i]=>_keyValuePairs[i];//这是只读索引的快捷写法，参考：https://docs.microsoft.com/zh-cn/dotnet/csharp/programming-guide/indexers/index
        public EntityCaching(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            loadAll();
        }

        private void loadAll()
        {
            _keyValuePairs = new Dictionary<TKey, TValue>();
            using (DbContext db = _serviceProvider.GetService<DbContext>())
            {
                db.Set<TValue>().AsNoTracking().ToList().ForEach(a =>
                {
                    _keyValuePairs.Add(a.Id, a);
                });
            }
        }

        private TValue Find(TKey key)
        {
            using (DbContext db = _serviceProvider.GetService<DbContext>())
            {
                return db.Set<TValue>().Find(key);
            }
        }


        /// <summary>
        /// 刷新
        /// </summary>
        public void Refresh()
        {
            loadAll();
        }

        /// <summary>
        /// 增加或更新对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddOrUpdate(TKey key, TValue value)
        {
            _keyValuePairs.Remove(key);
            _keyValuePairs.Add(key, value);
        }

        /// <summary>
        /// 根据对象的key，增加或更新对象
        /// </summary>
        /// <param name="key"></param>
        public void AddOrUpdate(TKey key)
        {
            _keyValuePairs.Remove(key);
            _keyValuePairs.Add(key, Find(key));
        }

        public void Remove(TKey key)
        {
            _keyValuePairs.Remove(key);
        }
    }

    public static class EntityCachingExtension
    {
        public static IServiceCollection AddEntityCaching(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(typeof(IEntityCaching<,>),typeof(IEntityCaching<,>));
            return serviceCollection;
        }
    }
}
