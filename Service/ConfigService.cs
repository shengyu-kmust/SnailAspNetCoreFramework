using ApplicationCore.Entities;
using ApplicationCore.IServices;
using Snail.Core.Dto;
using Snail.Core.Permission;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class ConfigService : BaseService<Config>, IConfigService
    {
        private IPermissionStore _permissionStore;
        public ConfigService(ServiceContext serviceContext,IPermissionStore permissionStore) : base(serviceContext)
        {
            _permissionStore = permissionStore;
        }

        public List<KeyValueDto> GetConfigKeyValue(string parentKey)
        {
            var allConfig = entityCacheManager.Get<Config>();
            var parent = allConfig.FirstOrDefault(a => a.Key == parentKey);
            return allConfig.Where(a => a.ParentId == parent.Id).Select(a => new KeyValueDto
            {
                Key = a.Key,
                ExtraInfo = a.ExtraInfo,
                Value = a.Value
            }).ToList();
        }
    }
}
