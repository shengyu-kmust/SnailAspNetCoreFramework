using ApplicationCore.Entity;
using ApplicationCore.IServices;
using Snail.Core.Permission;

namespace Service
{
    public class RoleService : BaseService<Role>, IRoleService
    {
        private IPermissionStore _permissionStore;
        public RoleService(ServiceContext serviceContext,IPermissionStore permissionStore) : base(serviceContext)
        {
            _permissionStore = permissionStore;
        }
    }
}
