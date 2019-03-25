using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace DAL.Interface
{
    /// <summary>
    /// 数据仓库接口
    /// <remarks>
    /// 提供单表及多表的通用接口
    /// </remarks>
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity>
    {

        #region 单表操作

        #region 查询
        /// <summary>
        /// 根据主键获取数据，异步方法，无则返回Null
        /// </summary>
        /// <param name="keyValues">主键，只可是单一主键或是复合主键</param>
        /// <returns></returns>
        Task<TEntity> FindAsync(params object[] keyValues);

        /// <summary>
        /// 根据查询条件获取第一条数据，无则返回Null
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync();
        /// <summary>
        /// 筛选数据
        /// </summary>
        /// <param name="predicate">筛选条件</param>
        /// <returns></returns>
        Task<List<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 筛选数据，分页
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="predicate">筛选条件</param>
        /// <param name="pageParam">分页参数</param>
        /// <returns></returns>
        Task<(List<TEntity> thisPagEntities, int totalCount)> WhereAsyncPage<TKey>(Expression<Func<TEntity, bool>> predicate, PageParam<TEntity,TKey> pageParam);

        /// <summary>
        /// 返回所有数据
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> AllAsync();
        /// <summary>
        /// 返回所有数据，分页
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageParam"></param>
        /// <returns></returns>
        Task<(List<TEntity> thisPagEntities,int totalCount)> AllAsyncPage<TKey>(PageParam<TEntity, TKey> pageParam);

        #endregion

        #region 增加
        Task<TEntity> AddAsync(TEntity entity);



        #endregion

        #region 修改

        /// <summary>
        /// 修改所有的字段
        /// </summary>
        /// <param name="entity">修改的实体</param>
        /// <returns></returns>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// 修改指定字段
        /// </summary>
        /// <param name="entity">修改的实体</param>
        /// <param name="changeProperties">要修改的字段</param>
        /// <returns></returns>
        Task<TEntity> UpdateAsync(TEntity entity, List<string> changeProperties);

        #endregion

        #region 删除

        Task<TEntity> RemoveAsync (TEntity entity);

        #endregion

        Task SaveAsync();

        #endregion



    }
}
