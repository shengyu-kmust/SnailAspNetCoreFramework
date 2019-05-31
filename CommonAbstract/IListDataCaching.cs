using System;
using System.Collections.Generic;
using System.Text;

namespace CommonAbstract
{
    /// <summary>
    /// 列表数据缓存，用于缓存应用不常列表数据，如整表数据
    /// </summary>
    public interface IListDataCaching<TKey,TValue>
    {
        
        ///// <summary>
        ///// 返回所有的列表
        ///// </summary>
        ///// <returns></returns>
        //List<T> Values();
        //void Update(T value, object key);
        
    }
}
