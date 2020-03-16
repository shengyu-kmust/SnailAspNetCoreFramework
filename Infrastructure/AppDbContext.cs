using ApplicationCore;
using ApplicationCore.Entity;
using Autofac;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Snail.Common;
using Snail.Core.Default;
using Snail.Permission;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure
{
    public partial class AppDbContext : DbContext
    {
        #region 通用权限表
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoleses { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<RoleResource> RoleResources { get; set; }
        public DbSet<ApplicationCore.Entity.Org> Orgs { get; set; }
        public DbSet<UserOrg> UserOrgs { get; set; }
        #endregion
        private ICapPublisher _publisher;
        public AppDbContext(DbContextOptions<AppDbContext> options, ICapPublisher publisher)
            : base(options)
        {
            _publisher = publisher;
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
          : base(options)
        {
        }


        public virtual DbSet<SampleEntity> SampleEntity { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 自动应用所有的IEntityTypeConfiguration配置
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var userId = IdGenerator.Generate<string>();
            var roleId = IdGenerator.Generate<string>();
            var pwdHash = BitConverter.ToString(HashAlgorithm.Create(HashAlgorithmName.MD5.Name).ComputeHash(Encoding.UTF8.GetBytes("123456"))).Replace("-", "");
            var now = DateTime.Now;
            modelBuilder.Entity<User>().HasData(new User { Id = userId, Account = "SuperAdmin", CreateTime = now, IsDeleted = false, Name = "超级管理员", Pwd = pwdHash });
            modelBuilder.Entity<Role>().HasData(new Role { Id = roleId, Name = DefaultPermission.superAdminRoleName, CreateTime = now, IsDeleted = false });
            modelBuilder.Entity<UserRole>().HasData(new UserRole { Id = IdGenerator.Generate<string>(), IsDeleted = false, RoleId = roleId, UserId = userId, CreateTime = now });
        }

        public override int SaveChanges()
        {
            //统一在数据库上下文的操作前，触发缓存实体的数据清空。
            if (_publisher != null)
            {
                this.ChangeTracker.Entries().Where(a => Attribute.IsDefined(a.Entity.GetType(), typeof(EnableEntityCacheAttribute))).Select(a => a.Entity.GetType().Name).Distinct().ToList().ForEach(entityName =>
                {
                    _publisher.Publish(EntityCacheManager.EntityCacheEventName, new EntityChangeEvent { EntityName = entityName });
                });
            }

            return base.SaveChanges();
        }
    }
}
