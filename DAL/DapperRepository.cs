using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
    }
}
