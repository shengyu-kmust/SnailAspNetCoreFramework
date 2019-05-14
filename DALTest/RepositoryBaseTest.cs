using CommonAbstract;
using DAL.Entity;
using Microsoft.EntityFrameworkCore;
using Web;

namespace DALTest
{
    public abstract class RepositoryBaseTest<T> where T:DefaultBaseEntity
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
