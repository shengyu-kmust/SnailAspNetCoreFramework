using System;

namespace Snail.Cache
{
    public interface ISnailCache
    {
        /// <summary>
        /// 获取或是设置缓存，有则返回，无则设置并返回
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <param name="absoluteExpirationRelativeToNow">为null时为永不过期</param>
        /// <returns></returns>
        TItem GetOrSet<TItem>(string key, Func<string, TItem> func,TimeSpan? absoluteExpirationRelativeToNow);
        /// <summary>
        /// 获取缓存，如果不存在，返回TItem的default值
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        TItem Get<TItem>(string key);
        /// <summary>
        /// 设置缓存，有则覆盖
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="absoluteExpirationRelativeToNow">为null时为永不过期</param>
        void Set<TItem>(string key, TItem value, TimeSpan? absoluteExpirationRelativeToNow);

        /// <summary>
        /// 删除缓存，有则删除，无则不操作，不报错
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
    }
}
