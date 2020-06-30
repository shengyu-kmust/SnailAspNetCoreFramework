using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Snail.Common;
using Snail.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure
{
    public static class DbSetExtenssion
    {
        #region addList
        public static void AddList<TEntity, TDto, TKey>(this DbSet<TEntity> entities, List<TDto> dtos, Func<TDto, TEntity> addFunc, TKey userId)
           where TEntity : class, IIdField<TKey>
         where TDto : class, IIdField<TKey>
        {
            dtos.ForEach(dto =>
            {
                Add(entities, dto, addFunc, userId);
            });
        }

        public static void AddList<TEntity, TDto, TKey>(this DbSet<TEntity> entities, List<TDto> dtos, IMapper mapper, TKey userId)
           where TEntity : class, IIdField<TKey>
         where TDto : class, IIdField<TKey>
        {
            dtos.ForEach(dto =>
            {
                Add(entities, dto, mapper, userId);
            });
        }


        #endregion
        #region add
        public static void Add<TEntity, TDto, TKey>(this DbSet<TEntity> entities, TDto dto, Func<TDto, TEntity> addFunc, TKey userId)
          where TEntity : class, IIdField<TKey>
        where TDto : class, IIdField<TKey>
        {
            var now = DateTime.Now;
            var entity = addFunc(dto);
            if (string.IsNullOrEmpty(entity.Id?.ToString()))
            {
                entity.Id = IdGenerator.Generate<TKey>();
            }
            if (entity is IEntityAudit<TKey> auditEntity)
            {
                if (!string.IsNullOrEmpty(userId?.ToString()))
                {
                    auditEntity.Creater = userId;
                    auditEntity.Updater = userId;
                }
                auditEntity.CreateTime = now;
                auditEntity.UpdateTime = now;
            }
            entities.Add(entity);
        }

        public static void Add<TEntity, TDto, TKey>(this DbSet<TEntity> entities, TDto dto, IMapper mapper, TKey userId)
           where TEntity : class, IIdField<TKey>
         where TDto : class, IIdField<TKey>
        {
            Add(entities, dto, (dto) => mapper.Map<TEntity>(dto), userId);
        }
        #endregion

        #region addOrUpdate
        public static void AddOrUpdate<TEntity, TDto, TKey>(this DbSet<TEntity> entities, TDto dto, Func<TDto, TEntity> addFunc, Action<TDto, TEntity> updateFunc, TKey userId)
        where TEntity : class, IIdField<TKey>
        where TDto : class, IIdField<TKey>
        {
            var now = DateTime.Now;
            var entity = entities.Find(dto.Id);
            if (entity == null)
            {
                //add
                entity = addFunc(dto);
                if (string.IsNullOrEmpty(entity.Id?.ToString()))
                {
                    entity.Id = IdGenerator.Generate<TKey>();
                }
                entities.Add(entity);
            }
            else
            {
                //update
                updateFunc(dto, entity);
            }

            if (entity is IEntityAudit<TKey> auditEntity)
            {
                if (!string.IsNullOrEmpty(userId?.ToString()))
                {
                    auditEntity.Creater = userId;
                    auditEntity.Updater = userId;
                }
                auditEntity.CreateTime = now;
                auditEntity.UpdateTime = now;
            }
        }
        public static void AddOrUpdate<TEntity, TDto, TKey>(this DbSet<TEntity> entities, TDto dto, IMapper mapper, TKey userId)
        where TEntity : class, IIdField<TKey>
        where TDto : class, IIdField<TKey>
        {
            AddOrUpdate(entities, dto, dto => mapper.Map<TEntity>(dto), (dto, entity) => mapper.Map<TDto, TEntity>(dto, entity), userId);
        }

        #endregion

        #region addOrUpdateList
        public static void AddOrUpdateList<TEntity, TDto, TKey>(this DbSet<TEntity> entities, List<TEntity> existsEntities, List<TDto> dtos, Func<TDto, TEntity> addFunc, Action<TDto, TEntity> updateFunc, TKey userId)
           where TEntity : class, IIdField<TKey>
           where TDto : class, IIdField<TKey>
        {
            var dtoIds = dtos.Select(a => a.Id).ToList();
            // 删除 
            var removeIds = existsEntities.Select(a => a.Id).Except(dtoIds).ToList();
            RemoveByIds<TEntity, TKey>(entities, removeIds);

            // 增加或 更新
            foreach (var dto in dtos.Where(a => !removeIds.Contains(a.Id)))
            {
                AddOrUpdate(entities, dto, addFunc, updateFunc, userId);
            }
        }


        public static void AddOrUpdateList<TEntity, TDto, TKey>(this DbSet<TEntity> entities, List<TEntity> existsEntities, List<TDto> dtos, IMapper mapper, TKey userId)
           where TEntity : class, IIdField<TKey>
           where TDto : class, IIdField<TKey>
        {
            AddOrUpdateList(entities, existsEntities, dtos, dto => mapper.Map<TEntity>(dto), (dto, entity) => mapper.Map<TDto, TEntity>(dto, entity), userId);
        }

        #endregion


        public static void RemoveByIds<TEntity, TKey>(this DbSet<TEntity> entities, List<TKey> ids)
           where TEntity : class, IIdField<TKey>
        {
            foreach (var id in ids)
            {
                var entity = entities.Find(id);
                if (entity != null)
                {
                    if (entity is IEntitySoftDelete entitySoftDeleteEntity)
                    {
                        entitySoftDeleteEntity.IsDeleted = true;
                    }
                    else
                    {
                        entities.Remove(entity);
                    }
                }
            }
        }
    }
}
