using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Collections.Generic;
using System.Text;

namespace DALTest
{
    public class DatabaseDbContextHelper
    {
        public static readonly LoggerFactory MyLoggerFactory
    = new LoggerFactory(new ILoggerProvider[] { new ConsoleLoggerProvider((_, __) => true, true) ,new DebugLoggerProvider()});
        public static TestDatabaseDbContext db { get; set; }
        public static TestDatabaseDbContext GetInMemoryDbContext()
        {
            if (db==null)
            {
                InitInMemoryDatabase();
            }
            return db;
        }

        public static TestDatabaseDbContext GetSqlServerDbContext()
        {
            if (db == null)
            {
                InitSqlServerDatabase();
            }
            return db;
        }

        private static void InitInMemoryDatabase()
        {
            var options=new DbContextOptionsBuilder<TestDatabaseDbContext>().UseInMemoryDatabase("test").UseLoggerFactory(MyLoggerFactory).Options;
            db= new TestDatabaseDbContext(options);
            db.Database.EnsureCreated();
        }

        private static void InitSqlServerDatabase()
        {
            var options = new DbContextOptionsBuilder<TestDatabaseDbContext>().UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SnailAspNetCoreFrameworkTest;Trusted_Connection=True;").UseLoggerFactory(MyLoggerFactory).Options;
            db = new TestDatabaseDbContext(options);
            //用EnsureCreated，创建数据库并初始化数据，比较简单。用的不是和migrate的技术。缺点是后面如果表结构如果有变动，则不会再更新数据库
            db.Database.EnsureCreated();
        }
    }
}
