using CommonAbstract;
using Web.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Utility.Page;

namespace Web
{
    /// <summary>
    /// 基于entityframework的数据仓储模式
    /// <remarks>由于entityframework支持多种数据库操作，因此数据仓储只实现一个EFRepository，切换数据库时只要切换DbContext的不同实现即可，代码不需要做更改</remarks>
    /// </summary>
    public class EFRepository<TEntity> : IRepository<TEntity> where TEntity : class,IBaseEntity
    {
        private readonly DbContext _dbContext;
        private DbSet<TEntity> _dbSet;
        public IUnitOfWork UnitOfWork { get; set; }


        public EFRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
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
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (order != null)
            {
                query = order(query);
            }

            return query.Select(selector).ToList();
        }

        
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
            if (pagination != null)
            {
                query = (pagination.PageIndex <= 1) ? query.Take(pagination.PageSize) : query.Skip(pagination.PageSize * (pagination.PageIndex - 1)).Take(pagination.PageSize);
            }
            var items = query.Select(selector).ToList();
            var total = query.Count();
            return new PageResult<TResult>
            {
                Items = items,
                PageIndex = pagination.PageIndex,
                PageSize = pagination.PageSize,
                Total = total
            };

        }


        public void UpdateAsync(TEntity entity, List<string> changeProperties)
        {
            var entityEntry = _dbSet.Attach(entity);
            foreach (var property in changeProperties)
            {
                entityEntry.Property(property).IsModified = true;
            }
            _dbContext.SaveChanges();
        }
    }
}
