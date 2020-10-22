using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Snail.Permission.Entity;
using Snail.Web;
using System.Reflection;

namespace Infrastructure
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public partial class AppDbContext :
        BaseAppDbContext<PermissionDefaultUser, PermissionDefaultRole, PermissionDefaultResource, PermissionDefaultUserRole, PermissionDefaultRoleResource, PermissionDefaultOrg, PermissionDefaultUserOrg>
    {
        /// <summary>
        /// AppDbContext
        /// </summary>
        /// <param name="options"></param>
        /// <param name="publisher"></param>
        public AppDbContext(DbContextOptions<AppDbContext> options, ICapPublisher publisher) : base(options, publisher)
        {
        }

        /// <summary>
        /// AppDbContext
        /// </summary>
        /// <param name="options"></param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// OnModelCreating
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("Infrastructure"));
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// SaveChanges
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}