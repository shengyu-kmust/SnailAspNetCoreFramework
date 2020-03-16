using AutoMapper;
using Infrastructure;
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
        public KeyValueController(ControllerContext controllerContext) : base(controllerContext)
        {
        }
        private IEnumKeyValueService _enumKeyValueService;
        private IEntityCacheManager _entityCacheManager;
        public KeyValueController(ControllerContext controllerContext, IEntityCacheManager entityCacheManager, IEnumKeyValueService enumKeyValueService) : base(controllerContext)
        {
            _enumKeyValueService = enumKeyValueService;
            _entityCacheManager = entityCacheManager;
        }
        public List<KeyValueDto> Enum(string enumName)
        {
            return _enumKeyValueService.GetKeyValues(enumName);
        }

    }
}
