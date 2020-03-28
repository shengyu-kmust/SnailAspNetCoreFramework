using AutoMapper;
using Infrastructure;
using Snail.Core.Interface;
namespace Service
{
    public class ServiceContext
    {
        public AppDbContext db;
        public IEntityCacheManager entityCacheManager;
        public IMapper mapper;
        public IApplicationContext applicationContext;
        public ServiceContext(AppDbContext db, IMapper mapper, IApplicationContext applicationContext, IEntityCacheManager entityCacheManager)
        {
            this.mapper = mapper;
            this.db = db;
            this.entityCacheManager = entityCacheManager;
            this.applicationContext = applicationContext;
        }
    }
}
