using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        }
    
        public IQueryable<TSource> GetQueryable<TSource>(AppDbContext db)
        {
            //IQueryable queryable;
            //switch (typeof(TSource).Name)
            //{
            //    case nameof(QueueResultDto):
            //        queryable= QueryableOfQueueResult(db);
            //        break;
            //    default:
            //        throw new BusinessException($"未找到{typeof(TSource).Name}对应的IQueryable配置");
            //}
            //return (IQueryable<TSource>)queryable;
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> GetEntityQueryable<TEntity>(AppDbContext db) where TEntity: class,IEntityId<string>
        {
            return db.Set<TEntity>().AsNoTracking();
        }

        #region 定义用于CRUD操作的查询功能的IQueryable
        //private IQueryable<ConfigSourceDto> QueryableOfConfigSource(AppDbContext db)
        //{
        //    return _mapper.ProjectTo<ConfigSourceDto>(db.Config.AsNoTracking().Where(a => !a.IsDeleted).AsQueryable());
        //}
      
        #endregion

    }
}
