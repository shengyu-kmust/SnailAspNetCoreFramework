using AutoMapper;
using Infrastructure;
using Snail.Core.Interface;
namespace Service
{
    public class ServiceContext
    {
        private AppDbContext _db;
        private IEntityCacheManager _entityCacheManager;
        private IMapper _mapper;
        public ServiceContext(AppDbContext db, IMapper mapper, IApplicationContext applicationContext, IEntityCacheManager entityCacheManager)
        {
            _mapper = mapper;
            _db = db;
            _entityCacheManager = entityCacheManager;
        }
    }
}
