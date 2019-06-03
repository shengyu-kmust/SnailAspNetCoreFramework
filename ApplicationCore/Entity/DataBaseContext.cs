using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ApplicationCore.Entity
{
    public class DatabaseContext : DbContext
    {
        #region 通用权限表
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoleses { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<UserOrg> UserOrgs { get; set; }
        #endregion
        #region 业务表

        #endregion

        #region 示例
        public DbSet<Student> Students { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Card> Cards { get; set; }
        #endregion

        /// <summary>
        /// 要创建此构造函数，配合services.AddDbContext<DatabaseContext>()生成数据库
        /// </summary>
        /// <param name="options"></param>
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetBaseEntity<User>(modelBuilder);
            SetBaseEntity<Role>(modelBuilder);
            SetBaseEntity<UserRole>(modelBuilder);
           
        }

        /// <summary>
        /// 设置公共字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelBuilder"></param>
        private void SetBaseEntity<T>(ModelBuilder modelBuilder) where T : BaseEntity
        {
            modelBuilder.Entity<T>().Property(a => a.CreateTime).HasDefaultValue(DateTime.Now)
                .ValueGeneratedOnAddOrUpdate();
            modelBuilder.Entity<T>().Property(a => a.UpdateTime).HasDefaultValue(DateTime.Now)
                .ValueGeneratedOnAddOrUpdate();
            modelBuilder.Entity<T>().Property(a => a.IsDeleted).HasDefaultValue(false)
                .ValueGeneratedOnAddOrUpdate();
        }

    }
}
