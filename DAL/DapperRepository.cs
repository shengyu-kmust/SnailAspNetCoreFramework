using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utility.Page;

namespace DAL.Interface
{
    /// <summary>
    /// 用Dapper技术实现仓储，因Dapper要自己写sql，而每种数据库的sql语句写法上有差异，在实际开发中可以要分解成多个DapperRepository，如Sqlserver/Mysql/OracleDapperRepository
    /// </summary>
    public class DapperRepository:IRepository<BaseEntity>
    {
        public Task<BaseEntity> FindAsync(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public Task<BaseEntity> FirstOrDefaultAsync(Expression<Func<BaseEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<BaseEntity> FirstOrDefaultAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<BaseEntity>> WhereAsync(Expression<Func<BaseEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<(List<BaseEntity> thisPagEntities, int totalCount)> WhereAsyncPage<TKey>(Expression<Func<BaseEntity, bool>> predicate, PageParam<BaseEntity, TKey> pageParam)
        {
            throw new NotImplementedException();
        }

        public Task<List<BaseEntity>> AllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<(List<BaseEntity> thisPagEntities, int totalCount)> AllAsyncPage<TKey>(PageParam<BaseEntity, TKey> pageParam)
        {
            throw new NotImplementedException();
        }

        public Task<BaseEntity> AddAsync(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<BaseEntity> UpdateAsync(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<BaseEntity> UpdateAsync(BaseEntity entity, List<string> changeProperties)
        {
            throw new NotImplementedException();
        }

        public Task<BaseEntity> RemoveAsync(BaseEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }

        void IRepository<BaseEntity>.UpdateAsync(BaseEntity entity, List<string> changeProperties)
        {
            throw new NotImplementedException();
        }

        public List<BaseEntity> Query(Expression<Func<BaseEntity, bool>> predicate, Func<IQueryable<BaseEntity>, IQueryable<BaseEntity>> include, Func<IQueryable<BaseEntity>, IQueryable<BaseEntity>> order)
        {
            throw new NotImplementedException();
        }

        public PageResult<BaseEntity> Query(Expression<Func<BaseEntity, bool>> predicate, Func<IQueryable<BaseEntity>, IQueryable<BaseEntity>> include, Func<IQueryable<BaseEntity>, IQueryable<BaseEntity>> order, IPagination pagination)
        {
            throw new NotImplementedException();
        }

        public List<TResult> Query<TResult>(Expression<Func<BaseEntity, bool>> predicate, Func<IQueryable<BaseEntity>, IQueryable<BaseEntity>> include, Func<IQueryable<BaseEntity>, IQueryable<BaseEntity>> order, Expression<Func<BaseEntity, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public PageResult<TResult> Query<TResult>(Expression<Func<BaseEntity, bool>> predicate, Func<IQueryable<BaseEntity>, IQueryable<BaseEntity>> include, Func<IQueryable<BaseEntity>, IQueryable<BaseEntity>> order, IPagination pagination, Expression<Func<BaseEntity, TResult>> selector)
        {
            throw new NotImplementedException();
        }
    }
}
