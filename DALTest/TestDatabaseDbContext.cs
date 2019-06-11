using DALTest.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DALTest
{
    public class TestDatabaseDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<IdentityCard> IdentityCards { get; set; }
        public DbSet<BankCard> BankCards { get; set; }
        public TestDatabaseDbContext(DbContextOptions<TestDatabaseDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// 对内丰测试数据库进行初始化
        /// 1、用hasData设置数据
        /// 2、在实例化dbcontext后，调用DbContext.Database.EnsureCreated来初始化数据
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Team>().HasData(
               new Team { Id = 1, Name = "一班" },
               new Team { Id = 2, Name = "二班" },
               new Team { Id = 3, Name = "三班" }
               );

            modelBuilder.Entity<Student>().HasData(
                new Student { Id=1,Name="张三",BirthDay=new DateTime(1901,1,1),TeamId=1},
                new Student { Id=2,Name="李四",BirthDay=new DateTime(1941,1,1),TeamId=2},
                new Student { Id=3,Name="王五",BirthDay=new DateTime(1961,1,1),TeamId=3 }
                );

            modelBuilder.Entity<IdentityCard>().HasData(
               new IdentityCard { Id = 1, CardNo="001",StudentId=1 },
               new IdentityCard { Id = 2, CardNo="002",StudentId= 2 }
               );

            modelBuilder.Entity<BankCard>().HasData(
                          new BankCard { Id = 1, BankCardNo="001", StudentId = 1 },
                          new BankCard { Id = 2, BankCardNo="002", StudentId = 1 }
                          );

        }


        
    }
}
