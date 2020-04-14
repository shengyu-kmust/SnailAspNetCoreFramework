using ApplicationCore.Entities;
using ApplicationCore.IServices;
using Snail.Core.Permission;

namespace Service
{
    public class ConfigService : BaseService<Config>, IConfigService
    {
        private IPermissionStore _permissionStore;
        public ConfigService(ServiceContext serviceContext,IPermissionStore permissionStore) : base(serviceContext)
        {
            _permissionStore = permissionStore;
        }
    }
}
