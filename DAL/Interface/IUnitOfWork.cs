using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Remotion.Linq.Clauses;

namespace DAL.Interface
{
    public interface IUnitOfWork:IDisposable
    {
        Task SaveAsync();
        Task<IRepository<TEntity>> GetRepository<TEntity>();
    }
}
