using AutoMapper;
using Microsoft.AspNetCore.Http;
using Snail.Core.Entity;
using Snail.Core.Interface;
using Snail.DAL;
using System.Linq;
using System.Collections.Concurrent;
using System;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class DefaultCRUDService<TEntity, TSource,TKey> : EFCRUDService<TEntity, TSource, TKey> where TEntity : class, IEntityId<TKey> where TSource:class
    {
        private static ConcurrentDictionary<string, Func<AppDbContext, IQueryable>> queryableDics=new ConcurrentDictionary<string, Func<AppDbContext, IQueryable>>();
        public DefaultCRUDService(AppDbContext db, IMapper mapper, IApplicationContext context) : base(db, mapper, context)
        {
        }

        static DefaultCRUDService()
        {
            #region 这里初始化CRUD的查询源
            //queryableDics.TryAdd("ss", dd);

            #endregion
        }

        public override IQueryable<TSource> GetQueryableSource()
        {
            var key = nameof(TSource);
            if (queryableDics.ContainsKey(key))
            {
                return (IQueryable<TSource>)queryableDics[key](db as AppDbContext);
            }
            else
            {
                return mapper.ProjectTo<TSource>(db.Set<TEntity>().AsNoTracking());
            }
        }

        #region 这里定义通用CRUD的查询源，定义好后加到DefaultCRUDService静态构造函数里
        //public static IQueryable<string> dd(AppDbContext db)
        //{
        //    return null;
        //}
        #endregion

    }
}
