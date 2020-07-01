using AutoMapper;
using DotNetCore.CAP;
using Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Snail.Cache;
using Snail.Core.Interface;
using System;

namespace Service
{
    public class ServiceContext
    {
        public AppDbContext db;
        public IEntityCacheManager entityCacheManager;
        public IMapper mapper;
        public IApplicationContext applicationContext;
        public IMemoryCache memoryCache;
        public ICapPublisher publisher;
        public IServiceProvider serviceProvider;
        public ISnailCache cache;
        public ServiceContext(AppDbContext db, IMapper mapper, IApplicationContext applicationContext, IEntityCacheManager entityCacheManager, IMemoryCache memoryCache, ICapPublisher publisher, IServiceProvider serviceProvider,ISnailCache cache)
        {
            this.mapper = mapper;
            this.db = db;
            this.entityCacheManager = entityCacheManager;
            this.applicationContext = applicationContext;
            this.memoryCache = memoryCache;
            this.publisher = publisher;
            this.serviceProvider = serviceProvider;
            this.cache = cache;
        }
    }
}
