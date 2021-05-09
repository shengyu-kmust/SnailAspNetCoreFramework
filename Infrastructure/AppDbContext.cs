﻿using ApplicationCore.Entities;
using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore;
using Snail.Core.Default;
using Snail.Core.Interface;
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
        /// <param name="applicationContext"></param>
        public AppDbContext(DbContextOptions<AppDbContext> options, ICapPublisher publisher,IApplicationContext applicationContext) : base(options, publisher,applicationContext)
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

            // todo 可用queryFilter对租户进行实现，参考：https://github.com/dotnet/EntityFramework.Docs/blob/master/samples/core/Querying/QueryFilters/BloggingContext.cs
            //modelBuilder.Entity("").HasQueryFilter()
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