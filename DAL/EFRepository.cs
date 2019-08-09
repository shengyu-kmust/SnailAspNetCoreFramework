using ApplicationCore.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Utility.Page;
using Web.Interface;

namespace DAL
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
            //注意，selector可以是Expression<Func<T,bool>>或是Func<T,bool>，如果是表达式树（前者），则生成的sql会根据需求生成表的部分字段、或关联其它表，ef的projection(即根据select里的字段自动include相关的表)会启作用；如果是委托（后者），ef的projection不起作用，生成的sql只是单表的所有字段。
            return query.AsNoTracking().Select(selector).ToList();
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

            //注意，selector可以是Expression<Func<T,bool>>或是Func<T,bool>，如果是表达式树（前者），则生成的sql会根据需求生成表的部分字段、或关联其它表，ef的projection(即根据select里的字段自动include相关的表)会启作用；如果是委托（后者），ef的projection不起作用，生成的sql只是单表的所有字段。
            var items = query.AsNoTracking().Select(selector).ToList();
            var total = query.AsNoTracking().Count();
            return new PageResult<TResult>
            {
                Items = items,
                PageIndex = pagination.PageIndex,
                PageSize = pagination.PageSize,
                Total = total
            };

        }

        public void Update(TEntity entity, List<string> changeProperties)
        {
            var entityEntry = _dbContext.Entry(entity);
            foreach (var property in changeProperties)
            {
                entityEntry.Property(property).IsModified = true;
            }
            _dbContext.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            _dbContext.Update(entity);
            _dbContext.SaveChanges();
        }


        public void Add(TEntity entity)
        {
            var add=_dbSet.Add(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }
        public void Delete(params object[] keyValues)
        {
            var entity = _dbSet.Find(keyValues);
            if (typeof(TEntity).GetInterface(nameof(IEntitySoftDelete)) == null)
            {
                _dbSet.Remove(entity);
            }
            else
            {
                ((IEntitySoftDelete)entity).IsDeleted = true;
                Update(entity, new List<string> { nameof(IEntitySoftDelete.IsDeleted) });
            }
            Delete(entity);
        }
        #region 查找单个
        public TEntity Find(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);

        }

        public TEntity FirstOrDefault(Expression<Func<TEntity,bool>> predicate)
        {
            return _dbSet.AsNoTracking().FirstOrDefault(predicate);
        }
        public TEntity FirstOrDefault()
        {
            return _dbSet.AsNoTracking().FirstOrDefault();
        }

        public List<TEntity> GetAll()
        {
            return _dbSet.AsNoTracking().ToList();
        }
        #endregion
    }
}

