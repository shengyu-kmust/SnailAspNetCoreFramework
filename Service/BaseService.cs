using ApplicationCore.IServices;
using Microsoft.EntityFrameworkCore;
using Snail.Common;
using Snail.Common.Extenssions;
using Snail.Core;
using Snail.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Service
{
    public abstract class BaseService<TEntity> : ServiceContextBaseService, IBaseService<TEntity> where TEntity : class
    {
        protected BaseService(ServiceContext serviceContext) : base(serviceContext)
        {
        }

        public virtual IQueryable<TSource> GetQueryable<TSource>()
        {
            if (typeof(TSource) == typeof(TEntity))
            {
                return (IQueryable<TSource>)db.Set<TEntity>().AsNoTracking();
            }
            throw new NotSupportedException();
        }

        public virtual IQueryable<TSource> QueryList<TSource>(Expression<Func<TSource, bool>> pred)
        {
            return GetQueryable<TSource>().Where(pred);
        }
        public virtual IQueryable<TEntity> QueryList(Expression<Func<TEntity, bool>> pred)
        {
            return GetQueryable<TEntity>().Where(pred);
        }

        public virtual void Remove(List<string> ids)
        {
            if (ids == null)
            {
                throw new ArgumentNullException();
            }
            var userId = applicationContext.GetCurrentUserId();
            ids.ForEach(id =>
            {
                var entity = db.Set<TEntity>().Find(id);
                if (entity == null)
                {
                    throw new BusinessException($"您要删除的对象不存在，id为{id}");
                }
                if (entity is IEntityAudit<string> entityAudit)
                {
                    entityAudit.UpdateTime = DateTime.Now;
                    if (userId != null)
                    {
                        entityAudit.Updater = userId;
                    }
                }
                if (entity is IEntitySoftDelete entitySoftDelete)
                {
                    entitySoftDelete.IsDeleted = true;
                }
                else
                {
                    db.Set<TEntity>().Remove(entity);
                }
            });

            db.SaveChanges();
        }

        public virtual void Save<TSaveDto>(TSaveDto saveDto) where TSaveDto : IIdField<string>
        {
            var userId = applicationContext.GetCurrentUserId();
            if (saveDto.Id.HasNotValue())
            {
                saveDto.Id = IdGenerator.Generate<string>();
                var entity = mapper.Map<TEntity>(saveDto);
                if (entity is IEntityAudit<string> entityAudit)
                {
                    entityAudit.UpdateTime = DateTime.Now;
                    entityAudit.CreateTime = DateTime.Now;
                    entityAudit.Creater = userId;
                    entityAudit.Updater = userId;
                }
                db.Set<TEntity>().Add(entity);
            }
            else
            {
                var entity = db.Set<TEntity>().Find(saveDto.Id);
                if (entity == null)
                {
                    throw new Exception("要修改的实体不存在");
                }
                mapper.Map(saveDto, entity, typeof(TSaveDto), typeof(TEntity));
                if (entity is IEntityAudit<string> entityAudit)
                {
                    entityAudit.UpdateTime = DateTime.Now;
                    entityAudit.CreateTime = DateTime.Now;
                    if (userId.HasValue())
                    {
                        entityAudit.Updater = userId;
                    }
                }
            }

            db.SaveChanges();
        }
    }
}
