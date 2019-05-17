using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Utility.Page;
namespace CommonAbstract
{
    /// <summary>
    /// 数据仓库接口
    /// <remarks>
    /// 提供单表及多表的通用接口
    /// </remarks>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : IBaseEntity
    {

        /// <summary>
        /// 查询并返回指定类型列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="include"></param>
        /// <param name="order"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        List<TResult> Query<TResult>(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> include, Func<IQueryable<TEntity>, IQueryable<TEntity>> order, Expression<Func<TEntity, TResult>> selector);

        /// <summary>
        /// 查询并返回指定类型分页结果
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="include"></param>
        /// <param name="order"></param>
        /// <param name="pagination"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        PageResult<TResult> QueryPage<TResult>(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> include, Func<IQueryable<TEntity>, IQueryable<TEntity>> order, IPagination pagination, Expression<Func<TEntity, TResult>> selector);

        /// <summary>
        /// 修改指定字段
        /// </summary>
        /// <param name="entity">修改的实体</param>
        /// <param name="changeProperties">要修改的字段</param>
        /// <returns></returns>
        void Update(TEntity entity, List<string> changeProperties);

        void Add(TEntity entity);

        void Delete(TEntity entity);
        void Delete(params object[] keyValues);
        TEntity Find(params object[] keyValues);

    }
}
