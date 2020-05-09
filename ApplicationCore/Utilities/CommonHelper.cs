using Snail.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApplicationCore.Utilities
{
    public static class CommonHelper
    {
        public static void Update<TEntity, Dto>(List<TEntity> entities, List<Dto> dtos, Action<TEntity, Dto> updateFunc, Func<Dto, TEntity> addFunc)
           where TEntity : IIdField<string>
           where Dto : IIdField<string>

        {
            // 删除
            var entityIds = entities.Select(a => a.Id).ToList();
            var dtoIds = dtos.Select(a => a.Id).ToList();
            entities.RemoveAll(a => !dtoIds.Contains(a.Id));

            // 增加
            entities.AddRange(dtos.Where(a => !entityIds.Contains(a.Id)).Select(a => addFunc(a)));

            // 更新
            entities.ForEach(entity =>
            {
                var dto = dtos.FirstOrDefault(a => a.Id == entity.Id);
                if (dto != null)
                {
                    updateFunc(entity, dto);
                }
            });
        }
    }
}
