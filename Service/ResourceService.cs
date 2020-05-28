using ApplicationCore.Entity;
using ApplicationCore.IServices;
using Snail.Core.Permission;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public class ResourceService : BaseService<Resource>
    {
        public ResourceService(ServiceContext serviceContext) : base(serviceContext)
        {
        }
    }
}
