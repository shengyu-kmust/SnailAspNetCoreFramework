using ApplicationCore.IServices;
using Snail.Core.Permission;
using Snail.Permission.Entity;

namespace Snail.Web.Services
{
    public class RoleService : BaseService<PermissionDefaultRole>
    {
        private IPermissionStore _permissionStore;
        public RoleService(ServiceContext serviceContext,IPermissionStore permissionStore) : base(serviceContext)
        {
            _permissionStore = permissionStore;
        }
    }
}
