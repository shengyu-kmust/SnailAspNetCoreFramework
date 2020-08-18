using ApplicationCore.Entities;
using ApplicationCore.IServices;
using Snail.Core.Dto;
using Snail.Core.Permission;
using System.Collections.Generic;
using System.Linq;

namespace Service
{
    public class ConfigService : BaseService<Config>,IConfigService
    {
        public static List<(string parentKey, string keyField, string valueField)> ConfigKeyValueFieldSetting = new List<(string parentKey, string keyField, string valueField)>
        {
            ("Unit","Key","Key")
        };

        public ConfigService(ServiceContext serviceContext) : base(serviceContext)
        {
        }

        /// <summary>
        /// 获取父parentKey的key-value对值
        /// </summary>
        /// <param name="parentKey"></param>
        /// <returns></returns>
        /// <remarks>
        /// 预定：key为存储在数据库里的值，value是用于展示给用户看的值
        /// </remarks>
        public List<KeyValueDto> GetConfigKeyValue(string parentKey)
        {
            var allConfig = entityCacheManager.Get<Config>();
            var setting = ConfigKeyValueFieldSetting.FirstOrDefault(a => a.parentKey == parentKey);
            var parent = allConfig.FirstOrDefault(a => a.Key == parentKey);
            if (parent == null)
            {
                return new List<KeyValueDto>();
            }
            return allConfig.Where(a => a.ParentId == parent.Id).Select(a => {
                var settingKey = setting.keyField ?? "Id";
                var settingValue = setting.valueField ?? "Value";
                return new KeyValueDto
                {
                    Key = (settingKey == "Key") ? a.Key : ((settingKey == "Id" ? a.Id : a.Value)),
                    Value = (settingValue == "Value") ? a.Value : ((settingKey == "Name" ? a.Name : a.Key)),
                    ExtraInfo = a.ExtraInfo
                };
            }).ToList();
        }
    }
}
