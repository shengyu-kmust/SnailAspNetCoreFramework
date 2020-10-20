using ApplicationCore.Entity;
using Snail.Core.Permission;

namespace Snail.Web.Services
{
    public class UserService : BaseService<User>
    {
        private IPermissionStore _permissionStore;
        public UserService(ServiceContext serviceContext,IPermissionStore permissionStore) : base(serviceContext)
        {
            _permissionStore = permissionStore;
        }
    }
}
