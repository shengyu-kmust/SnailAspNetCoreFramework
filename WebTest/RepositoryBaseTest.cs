using CommonAbstract;
using DAL.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging.Debug;
using Web;

namespace WebTest
{
    public abstract class RepositoryBaseTest<T> where T : class, IBaseEntity
    {
        public IRepository<T> Repository { get; set; }
        public DbContext DbContext { get; set; }
        public static readonly LoggerFactory MyLoggerFactory
    = new LoggerFactory(new ILoggerProvider[] { new ConsoleLoggerProvider((_, __) => true, true), new DebugLoggerProvider() });
        public RepositoryBaseTest()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>().UseLoggerFactory(MyLoggerFactory).UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Test;Trusted_Connection=True;").Options;
            DbContext = new DatabaseContext(options);
            Repository = new EFRepository<T>(DbContext);
        }
    }
}
