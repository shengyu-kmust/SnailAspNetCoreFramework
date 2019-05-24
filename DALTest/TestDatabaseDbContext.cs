using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace DALTest
{
    public class TestDatabaseDbContext
    {
        public static readonly LoggerFactory MyLoggerFactory
    = new LoggerFactory(new[] { new ConsoleLoggerProvider((_, __) => true, true) });
        public static DatabaseDbContext db { get; set; }
        public static DatabaseDbContext GetDbContext()
        {
            if (db==null)
            {
                Init();
            }
            return db;
        }

        private static void Init()
        {
            var options=new DbContextOptionsBuilder<DatabaseDbContext>().UseInMemoryDatabase("test").UseLoggerFactory(MyLoggerFactory).Options;
            db= new DatabaseDbContext(options);
            db.Database.EnsureCreated();
        }
    }
}
