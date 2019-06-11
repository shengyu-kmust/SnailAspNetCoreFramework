using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Utility.Page;

namespace ApplicationCore.Abstract
{
    public interface ICRUDService<T> where T : IBaseEntity, new()
    {
        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="saveDto"></param>
        void Add(ISaveDto<T> saveDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValues"></param>
        void Delete(params object[] keyValues);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="saveDto"></param>
        void Update(ISaveDto<T> saveDto);

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        T Find(params object[] keyValues);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        T FirstOrDefault();

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="TResult">查询返回的结果</typeparam>
        /// <param name="query">查询对象</param>
        /// <param name="selector">selector对象</param>
        /// <returns>返回TResult类型的列表</returns>
        List<TResult> Query<TResult>(IQuery<T, TResult> query);
        //List<TResult> Query<TResult>(IQuery<T> query, Func<T, TResult> selector);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="query">分页查询对象</param>
        /// <param name="selector"></param>
        /// <returns></returns>
        PageResult<TResult> QueryPage<TResult>(IQueryPage<T, TResult> query);
        //PageResult<TResult> QueryPage<TResult>(IQueryPage<T> query, Func<T, TResult> selector);
    }
}
