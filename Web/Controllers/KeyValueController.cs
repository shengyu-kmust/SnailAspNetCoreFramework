using ApplicationCore.IServices;
using AutoMapper;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Snail.Core.Dto;
using Snail.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class KeyValueController : DefaultBaseController
    {
        private IEnumKeyValueService _enumKeyValueService;
        private IEntityCacheManager _entityCacheManager;
        private IConfigService _configService;
        public KeyValueController(ControllerContext controllerContext, IEntityCacheManager entityCacheManager, IEnumKeyValueService enumKeyValueService, IConfigService configService) : base(controllerContext)
        {
            _enumKeyValueService = enumKeyValueService;
            _entityCacheManager = entityCacheManager;
            _configService = configService;
        }

        public List<KeyValueDto> Enum(string enumName)
        {
            return _enumKeyValueService.GetKeyValues(enumName);
        }

        /// <summary>
        /// 获取枚举和配置的key-value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public List<KeyValueDto> Get(string key)
        {
            var enumKeyValue = _enumKeyValueService.GetKeyValues(key);
            if (enumKeyValue!=null && enumKeyValue.Any())
            {
                return enumKeyValue;
            }
            else
            {
                return _configService.GetConfigKeyValue(key);
            }
        }
    }
}
