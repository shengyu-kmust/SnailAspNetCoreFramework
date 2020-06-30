using AutoMapper;
using DotNetCore.CAP;
using Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Snail.Core.Interface;
using System;

namespace Service
{
    public abstract class ServiceContextBaseService
    {
        protected IEntityCacheManager entityCacheManager => serviceContext.entityCacheManager;
        protected IMapper mapper => serviceContext.mapper;
        protected IApplicationContext applicationContext => serviceContext.applicationContext;
        protected string currentUserId => serviceContext.applicationContext.GetCurrentUserId();
        public AppDbContext db => serviceContext.db;
        public IMemoryCache memoryCache => serviceContext.memoryCache;
        public ICapPublisher publisher => serviceContext.publisher;
        public IServiceProvider serviceProvider => serviceContext.serviceProvider;
        public ServiceContext serviceContext;
        protected ServiceContextBaseService(ServiceContext serviceContext)
        {
            this.serviceContext = serviceContext;
        }
    }
}
