using ApplicationCore.Entities;
using ApplicationCore.Entity;
using ApplicationCore.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snail.Common;
using Snail.Common.Extenssions;
using Snail.Core;
using Snail.Core.Permission;
using System.Collections.Generic;
using System.Linq;
using Web.DTO;
namespace Web.Controllers
{

    [Authorize(Policy = PermissionConstant.PermissionAuthorizePolicy)]
    public class ConfigController : DefaultBaseController, ICrudController<Config, ConfigSaveDto, ConfigResultDto, KeyQueryDto>
    {
        public ControllerContext controllerContext;
        public IConfigService _service;
        public ConfigController(IConfigService service, ControllerContext controllerContext) : base(controllerContext)
        {
            this.controllerContext = controllerContext;
            this._service = service;
        }
        [HttpGet]
        public List<ConfigResultDto> QueryList([FromQuery]KeyQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<Config>().AndIf(queryDto.KeyWord.HasValue(), a => a.Name.Contains(queryDto.KeyWord) || a.Key.Contains(queryDto.KeyWord) || a.Value.Contains(queryDto.KeyWord));
            return controllerContext.mapper.ProjectTo<ConfigResultDto>(_service.QueryList(pred)).ToList();
        }

        [HttpGet]
        public TreeNode<ConfigResultDto> QueryListTree([FromQuery]KeyQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<Config>().AndIf(queryDto.KeyWord.HasValue(), a => a.Name.Contains(queryDto.KeyWord) || a.Key.Contains(queryDto.KeyWord) || a.Value.Contains(queryDto.KeyWord));
            var list=controllerContext.mapper.ProjectTo<ConfigResultDto>(_service.QueryList(pred)).ToList();
            return TreeNodeHelper.GetTree<ConfigResultDto>(list, a => a.Id, a => a.ParentId, "0");
        }

        [HttpGet]
        public IPageResult<ConfigResultDto> QueryPage([FromQuery]KeyQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<Config>().AndIf(queryDto.KeyWord.HasValue(), a => a.Name.Contains(queryDto.KeyWord) || a.Key.Contains(queryDto.KeyWord) || a.Value.Contains(queryDto.KeyWord));
            return controllerContext.mapper.ProjectTo<ConfigResultDto>(_service.QueryList(pred)).ToPageList(queryDto);
        }

        [HttpGet]
        public ConfigResultDto Find(string id)
        {
            return controllerContext.mapper.Map<ConfigResultDto>(_service.QueryList(a => a.Id == id).FirstOrDefault());
        }
        [HttpPost]
        public void Remove(List<string> ids)
        {
            _service.Remove(ids);
        }

        [HttpPost]
        public void Save(ConfigSaveDto saveDto)
        {
            _service.Save(saveDto);
        }
    }
}
