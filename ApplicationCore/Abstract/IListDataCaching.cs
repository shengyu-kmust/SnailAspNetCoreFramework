using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Abstract
{
    /// <summary>
    /// 列表数据缓存，用于缓存应用不常列表数据，如整表数据
    /// </summary>
    public interface IEntityCaching<TKey,TValue> where TValue : class, IEntityId<TKey>
    {
        Dictionary<TKey, TValue> KeyValuePairs { get; }
        List<TValue> Values { get; }

        void Remove(TKey key);
        void AddOrUpdate(TKey key);
        void AddOrUpdate(TKey key, TValue value);
        void Refresh();
    }
}
