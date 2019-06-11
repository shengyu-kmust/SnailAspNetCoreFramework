using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Utility.Page;

namespace DAL.Abstract
{
    /// <summary>
    /// 数据仓库接口
    /// <remarks>
    /// 提供单表及多表的通用接口
    /// </remarks>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {

        /// <summary>
        /// 查询并返回指定类型列表
        /// </summary>
        /// <typeparam name="TResult">要返回的结果类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="include">包含条件</param>
        /// <param name="order">排序条件</param>
        /// <param name="selector">selector对象</param>
        /// <returns></returns>
        List<TResult> Query<TResult>(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> include, Func<IQueryable<TEntity>, IQueryable<TEntity>> order, Expression<Func<TEntity, TResult>> selector);

        /// <summary>
        /// 查询并返回指定类型分页结果
        /// </summary>
        /// <typeparam name="TResult">要返回的结果类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="include">包含条件</param>
        /// <param name="order">排序条件</param>
        /// <param name="pagination">分页条件</param>
        /// <param name="selector">selector对象</param>
        /// <returns></returns>
        PageResult<TResult> QueryPage<TResult>(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> include, Func<IQueryable<TEntity>, IQueryable<TEntity>> order, IPagination pagination, Expression<Func<TEntity, TResult>> selector);

        /// <summary>
        /// 修改指定字段
        /// </summary>
        /// <param name="entity">修改的实体</param>
        /// <param name="changeProperties">要修改的字段</param>
        /// <returns></returns>
        void Update(TEntity entity, List<string> changeProperties);

        /// <summary>
        /// 增加实体
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="keyValues"></param>
        void Delete(params object[] keyValues);

        /// <summary>
        /// 查找单个实体
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        TEntity Find(params object[] keyValues);

    }
}
