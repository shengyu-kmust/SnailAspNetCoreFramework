using ApplicationCore.Entity;
using ApplicationCore.IServices;
using Snail.Core.Permission;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public class UserService : BaseService<User>, IUserService
    {
        private IPermissionStore _permissionStore;
        public UserService(ServiceContext serviceContext,IPermissionStore permissionStore) : base(serviceContext)
        {
            _permissionStore = permissionStore;
        }
    }
}
