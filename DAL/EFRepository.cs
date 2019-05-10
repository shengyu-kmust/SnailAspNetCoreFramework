using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface;
using Microsoft.EntityFrameworkCore;
using Utility.Page;

namespace DAL
{
    /// <summary>
    /// 基于entityframework的数据仓储模式
    /// <remarks>由于entityframework支持多种数据库操作，因此数据仓储只实现一个EFRepository，切换数据库时只要切换DbContext的不同实现即可，代码不需要做更改</remarks>
    /// </summary>
    public class EFRepository<TEntity> : IRepository<TEntity> where TEntity:BaseEntity
    {
        private readonly DbContext _dbContext;
        private DbSet<TEntity> _dbSet;
        public IUnitOfWork UnitOfWork { get; set; }


        public EFRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }



        #region 通用方法
        public List<TEntity> Query(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> include, Func<IQueryable<TEntity>, IQueryable<TEntity>> order)
        {
            return Query(predicate, include, order, a => a);
        }

      

        public List<TResult> Query<TResult>(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> include, Func<IQueryable<TEntity>, IQueryable<TEntity>> order, Expression<Func<TEntity, TResult>> selector)
        {
            var query = _dbSet.AsNoTracking();
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }
            if (include != null)
            {
                query = include(query);
            }
            if (predicate!=null)
            {
                query = query.Where(predicate);
            }
            if (order!=null)
            {
                query = include(query);
            }

            return query.Select(selector).ToList();
        }

        public PageResult<TEntity> QueryPage(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> include, Func<IQueryable<TEntity>, IQueryable<TEntity>> order, IPagination pagination)
        {
            return QueryPage(predicate, include, order, pagination, a => a);
        }

        /// <summary>
        /// 分页查询 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="include"></param>
        /// <param name="order"></param>
        /// <param name="pagination"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public PageResult<TResult> QueryPage<TResult>(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> include, Func<IQueryable<TEntity>, IQueryable<TEntity>> order, IPagination pagination, Expression<Func<TEntity, TResult>> selector)
        {
            var query = _dbSet.AsNoTracking();
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }
            if (include != null)
            {
                query = include(query);
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (order != null)
            {
                query = include(query);
            }
            if (pagination!=null)
            {
                query=(pagination.PageIndex <= 1) ? query.Take(pagination.PageSize) : query.Skip(pagination.PageSize * (pagination.PageIndex - 1)).Take(pagination.PageSize);
            }
            var items=query.Select(selector).ToList();
            var total = query.Count();
            return new PageResult<TResult>
            {
                Items = items,
                PageIndex = pagination.PageIndex,
                PageSize = pagination.PageSize,
                Total = total
            };

        }

        #endregion
        #region 查询 

        public Task<TEntity> FindAsync(params object[] keyValues)
        {
            return _dbSet.FindAsync();
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> FirstOrDefaultAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<(List<TEntity> thisPagEntities, int totalCount)> WhereAsyncPage<TKey>(Expression<Func<TEntity, bool>> predicate, PageParam<TEntity, TKey> pageParam)
        {
            throw new NotImplementedException();
        }

        public Task<List<TEntity>> AllAsync()
        {
            throw new NotImplementedException();
        }

        #endregion

        public Task<(List<TEntity> thisPagEntities, int totalCount)> AllAsyncPage<TKey>(PageParam<TEntity, TKey> pageParam)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> AddAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateAsync(TEntity entity, List<string> changeProperties)
        {
            var entityEntry= _dbSet.Attach(entity);
            foreach (var property in changeProperties)
            {
                entityEntry.Property(property).IsModified = true;
            }
            _dbContext.SaveChanges();
        }

        public Task<TEntity> RemoveAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

      }
}
