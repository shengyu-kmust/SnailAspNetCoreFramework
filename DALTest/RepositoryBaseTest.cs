using DAL;
using DAL.Entity;
using DAL.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DALTest
{
    public abstract class RepositoryBaseTest<T> where T:BaseEntity
    {
        public IRepository<T> Repository { get; set; }
        public DbContext DbContext { get; set; }
        public RepositoryBaseTest()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>().UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Test;Trusted_Connection=True;").Options;
            DbContext = new DatabaseContext(options);
            Repository = new EFRepository<T>(DbContext);
        }
    }
}
