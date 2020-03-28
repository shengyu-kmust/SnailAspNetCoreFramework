using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure;

namespace Service
{
    public class DefaultBaseService<TEntity> : BaseService<TEntity> where TEntity : class
    {
        public DefaultBaseService(ServiceContext serviceContext) : base(serviceContext)
        {
        }
    }
}
