using ApplicationCore.IServices;
using Microsoft.AspNetCore.Mvc;
using Snail.Common;
using Snail.Common.Extenssions;
using Snail.Core;
using Snail.Core.Attributes;
using Snail.Web.Dtos;
using Snail.Web.Dtos.Config;
using Snail.Web.Entities;
using System.Collections.Generic;
using System.Linq;


namespace Web.Controllers
{

    //[Authorize(Policy = PermissionConstant.PermissionAuthorizePolicy)]
    [Resource(Description = "配置管理")]
    public class ConfigController : DefaultBaseController, ICrudController<Config, ConfigSaveDto, ConfigResultDto, KeyQueryDto>
    {
        private IConfigService _service;
        public ConfigController(IConfigService service, ControllerContext controllerContext) : base(controllerContext)
        {
            this.controllerContext = controllerContext;
            this._service = service;
        }
        [Resource(Description = "查询列表")]
        [HttpGet]
        public List<ConfigResultDto> QueryList([FromQuery]KeyQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<Config>().AndIf(queryDto.KeyWord.HasValue(), a => a.Name.Contains(queryDto.KeyWord) || a.Key.Contains(queryDto.KeyWord) || a.Value.Contains(queryDto.KeyWord));
            return controllerContext.mapper.ProjectTo<ConfigResultDto>(_service.QueryList(pred)).ToList();
        }

        [Resource(Description = "查询配置树")]
        [HttpGet]
        public List<ConfigTreeResultDto> QueryListTree([FromQuery]KeyQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<Config>().AndIf(queryDto.KeyWord.HasValue(), a => a.Name.Contains(queryDto.KeyWord) || a.Key.Contains(queryDto.KeyWord) || a.Value.Contains(queryDto.KeyWord));
            var list=controllerContext.mapper.ProjectTo<ConfigTreeResultDto>(_service.QueryList(pred)).ToList();
            return list.Where(a => !a.ParentId.HasValue()).Select(a => GetChildren(a, list)).ToList();
        }

        private ConfigTreeResultDto GetChildren(ConfigTreeResultDto parent, List<ConfigTreeResultDto> dtos)
        {
            return new ConfigTreeResultDto
            {
                Id = parent.Id,
                ExtraInfo=parent.ExtraInfo,
                Key=parent.Key,
                Value=parent.Value,
                Name = parent.Name,
                ParentId = parent.ParentId,
                Children = dtos.Where(a => a.ParentId == parent.Id).Select(a => GetChildren(a, dtos)).ToList()
            };
        }


        [Resource(Description = "查询分页")]
        [HttpGet]
        public IPageResult<ConfigResultDto> QueryPage([FromQuery]KeyQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<Config>().AndIf(queryDto.KeyWord.HasValue(), a => a.Name.Contains(queryDto.KeyWord) || a.Key.Contains(queryDto.KeyWord) || a.Value.Contains(queryDto.KeyWord));
            return controllerContext.mapper.ProjectTo<ConfigResultDto>(_service.QueryList(pred)).ToPageList(queryDto);
        }

        [Resource(Description = "查询单个")]
        [HttpGet]
        public ConfigResultDto Find(string id)
        {
            return controllerContext.mapper.Map<ConfigResultDto>(_service.QueryList(a => a.Id == id).FirstOrDefault());
        }
        [Resource(Description = "删除")]
        [HttpPost]
        public void Remove(List<string> ids)
        {
            _service.Remove(ids);
        }

        [Resource(Description = "保存")]
        [HttpPost]
        public void Save(ConfigSaveDto saveDto)
        {
            _service.Save(saveDto);
        }
    }
}
