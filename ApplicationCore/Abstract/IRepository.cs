using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Utility.Page;
namespace ApplicationCore.Abstract
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
        /// <typeparam name="TResult">要返回的结果类型</typeparam>
        /// <param name="predicate">查询条件，ef原生用法，支持单表，关联表的查询条件</param>
        /// <param name="include">包含条件，ef原生用法，用include或是project技术（即在selector对象里取include对象）</param>
        /// <param name="order">排序条件,ef原生用法，</param>
        /// <param name="selector">selector对象,ef原生用法，如果包含了关联表的数据，会自动include关联表</param>
        /// <returns></returns>
        List<TResult> Query<TResult>(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> include, Func<IQueryable<TEntity>, IQueryable<TEntity>> order, Expression<Func<TEntity, TResult>> selector);
        //List<TResult> Query<TResult>(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> include, Func<IQueryable<TEntity>, IQueryable<TEntity>> order, Expression<Func<TEntity, TResult>> selector);


        /// <summary>
        /// 查询并返回指定类型分页结果
        /// </summary>
        /// <typeparam name="TResult">要返回的结果类型</typeparam>
        /// <param name="predicate">查询条件，ef原生用法，支持单表，关联表的查询条件</param>
        /// <param name="include">包含条件,ef原生用法，用include或是project技术（即在selector对象里取include对象）</param>
        /// <param name="order">排序条件，ef原生用法</param>
        /// <param name="pagination">分页条件</param>
        /// <param name="selector">selector对象,ef原生用法，如果包含了关联表的数据，会自动include关联表</param>
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
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        TEntity FirstOrDefault();

    }
}
