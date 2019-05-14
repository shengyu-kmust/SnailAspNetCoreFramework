using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Web
{
    public static class DbContextExtension
    {
        public static EntityEntry<TEntity> AddOrUpdater<TEntity>(this DbContext dbContext,object entity,Func<object> keyFunc) where TEntity:class
        {
            return null;
        }
    }
}
