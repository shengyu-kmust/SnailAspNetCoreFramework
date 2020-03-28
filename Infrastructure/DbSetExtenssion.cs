using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public static class DbSetExtenssion
    {
        public static void Save<TEntity, TSaveDto>(this DbSet<TEntity> entities, TSaveDto addDto) where TEntity : class
        {
        }

        public static void Remove<TEntity>(this DbContext dbContext, List<string> ids)
        {

        }
    }
}
