using ApplicationCore.Entities;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Snail.Web;
using System.Reflection;

namespace Infrastructure
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public partial class AppDbContext :
        BaseAppDbContext<User, Role, Resource, UserRole, RoleResource, ApplicationCore.Entities.Org, UserOrg>
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
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("Infrastructure"));//应用infrastructure里的EntityTypeConfiguration
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