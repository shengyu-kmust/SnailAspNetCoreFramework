using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DAL.Interface
{
    public class UnitOfWork:IUnitOfWork
    {
        private DbContext _dbContext;

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

      
    }
}
