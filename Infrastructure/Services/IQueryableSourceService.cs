using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Snail.Core;
using Snail.Core.Entity;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Infrastructure.Services
{
    public interface IQueryableSourceService
    {
        IQueryable<TSource> GetQueryable<TSource>(AppDbContext db);
    }
    /// <summary>
    /// 注册成做成单例
    /// </summary>
    public class QueryableSourceService : IQueryableSourceService
    {
        private IServiceProvider _serviceProvider;
        private IMapper _mapper;
        private ConcurrentDictionary<string, Func<AppDbContext, IQueryable>> queryableDics=new ConcurrentDictionary<string, Func<AppDbContext, IQueryable>>();
        public QueryableSourceService(IServiceProvider serviceProvider,IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            Init();
        }
        public void Init()
        {
            queryableDics.GetOrAdd(nameof(QueueResultDto), QueryableOfQueueResult);

        }
        public IQueryable<TSource> GetQueryable<TSource>(AppDbContext db)
        {
            IQueryable queryable;
            switch (typeof(TSource).Name)
            {
                case nameof(QueueResultDto):
                    queryable= QueryableOfQueueResult(db);
                    break;
                case nameof(DeviceSourceDto):
                    queryable = QueryableOfDeviceSource(db);
                    break;
                case nameof(TemplateSourceDto):
                    queryable = QueryableOfTemplateSource(db);
                    break;
                case nameof(ConfigSourceDto):
                    queryable = QueryableOfConfigSource(db);
                    break;
                default:
                    throw new BusinessException($"未找到{typeof(TSource).Name}对应的IQueryable配置");
            }
            return (IQueryable<TSource>)queryable;
        }

        public IQueryable<TEntity> GetEntityQueryable<TEntity>(AppDbContext db) where TEntity: class,IEntityId<string>
        {
            return db.Set<TEntity>().AsNoTracking();
        }

        #region 定义用于CRUD操作的查询功能的IQueryable
        private IQueryable<ConfigSourceDto> QueryableOfConfigSource(AppDbContext db)
        {
            return _mapper.ProjectTo<ConfigSourceDto>(db.Config.AsNoTracking().Where(a => !a.IsDeleted).AsQueryable());
        }
        private IQueryable<QueueResultDto> QueryableOfQueueResult(AppDbContext db)
        {
            return null;
        }

        private IQueryable<TemplateSourceDto> QueryableOfTemplateSource(AppDbContext db)
        {
            return _mapper.ProjectTo<TemplateSourceDto>(db.Template.AsNoTracking().Where(a => !a.IsDeleted).AsQueryable());
        }

        private IQueryable<DeviceSourceDto> QueryableOfDeviceSource(AppDbContext db)
        {
            return _mapper.ProjectTo<DeviceSourceDto>(db.Device.AsNoTracking().Where(a => !a.IsDeleted).AsQueryable());
        }
        #endregion

    }
}
