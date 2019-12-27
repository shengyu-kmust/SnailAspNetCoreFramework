using ApplicationCore.Dtos;
using ApplicationCore.Dtos.Queue;
using System;
using System.Collections.Concurrent;
using System.Linq;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Snail.Core;
using Microsoft.EntityFrameworkCore;
using Snail.Core.Entity;

namespace Infrastructure.Services
{
    public interface IQueryableSourceService
    {
        IQueryable<TSource> GetQueryable<TSource>(UniversalQueueContext db);
    }
    /// <summary>
    /// 注册成做成单例
    /// </summary>
    public class QueryableSourceService : IQueryableSourceService
    {
        private IServiceProvider _serviceProvider;
        private IMapper _mapper;
        private ConcurrentDictionary<string, Func<UniversalQueueContext, IQueryable>> queryableDics=new ConcurrentDictionary<string, Func<UniversalQueueContext, IQueryable>>();
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
        public IQueryable<TSource> GetQueryable<TSource>(UniversalQueueContext db)
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

        public IQueryable<TEntity> GetEntityQueryable<TEntity>(UniversalQueueContext db) where TEntity: class,IEntityId<string>
        {
            return db.Set<TEntity>().AsNoTracking();
        }

        #region 定义用于CRUD操作的查询功能的IQueryable
        private IQueryable<ConfigSourceDto> QueryableOfConfigSource(UniversalQueueContext db)
        {
            return _mapper.ProjectTo<ConfigSourceDto>(db.Config.AsNoTracking().Where(a => !a.IsDeleted).AsQueryable());
        }
        private IQueryable<QueueResultDto> QueryableOfQueueResult(UniversalQueueContext db)
        {
            return null;
        }

        private IQueryable<TemplateSourceDto> QueryableOfTemplateSource(UniversalQueueContext db)
        {
            return _mapper.ProjectTo<TemplateSourceDto>(db.Template.AsNoTracking().Where(a => !a.IsDeleted).AsQueryable());
        }

        private IQueryable<DeviceSourceDto> QueryableOfDeviceSource(UniversalQueueContext db)
        {
            return _mapper.ProjectTo<DeviceSourceDto>(db.Device.AsNoTracking().Where(a => !a.IsDeleted).AsQueryable());
        }
        #endregion

    }
}
