using ApplicationCore.Dtos;
using ApplicationCore.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Snail.Common;
using Snail.Common.Extenssions;
using Snail.Core;
using Snail.Core.Attributes;
using Snail.Core.Permission;
using System.Collections.Generic;
using System.Linq;
namespace Web.Controllers
{

    [Authorize(Policy = PermissionConstant.PermissionAuthorizePolicy)]
    [Resource(Description = "权限资源管理")]
    public class ResourceController : DefaultBaseController, ICrudController<Resource, ResourceSaveDto, ResourceResultDto, KeyQueryDto>
    {
        private ResourceService _service;
        private IPermissionStore _permissionStore;
        public ResourceController(ResourceService service, ControllerContext controllerContext,IPermissionStore permissionStore, IPermission permission) : base(controllerContext)
        {
            this.controllerContext = controllerContext;
            this._service = service;
            this._permissionStore = permissionStore;
        }
        [Resource(Description ="查询列表")]
        [HttpGet]
        public List<ResourceResultDto> QueryList([FromQuery]KeyQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<Resource>().And(a => !a.IsDeleted).AndIf(queryDto.KeyWord.HasValue(), a => a.Name.Contains(queryDto.KeyWord) || a.Code.Contains(queryDto.KeyWord));
            return controllerContext.mapper.ProjectTo<ResourceResultDto>(_service.QueryList(pred)).ToList();
        }

        [Resource(Description ="查询分页")]
        [HttpGet]
        public IPageResult<ResourceResultDto> QueryPage([FromQuery]KeyQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<Resource>().And(a => !a.IsDeleted).AndIf(queryDto.KeyWord.HasValue(), a => a.Name.Contains(queryDto.KeyWord) || a.Code.Contains(queryDto.KeyWord));
            return controllerContext.mapper.ProjectTo<ResourceResultDto>(_service.QueryList(pred)).ToPageList(queryDto);
        }

        [Resource(Description = "查询资源树")]
        [HttpGet]
        public List<ResourceTreeResultDto> QueryListTree([FromQuery]KeyQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<Resource>().And(a=>!a.IsDeleted).AndIf(queryDto.KeyWord.HasValue(), a => a.Name.Contains(queryDto.KeyWord));
            var list = controllerContext.mapper.ProjectTo<ResourceResultDto>(_service.QueryList(pred)).ToList();
            return list.Where(a=>!a.ParentId.HasValue()).Select(a => GetChildren(a, list)).ToList();
        }

        private ResourceTreeResultDto GetChildren(ResourceResultDto parent, List<ResourceResultDto> dtos)
        {
            return new ResourceTreeResultDto
            {
                Id = parent.Id,
                Code = parent.Code,
                Name = parent.Name,
                ParentId=parent.ParentId,
                Children = dtos.Where(a => a.ParentId == parent.Id).Select(a => GetChildren(a, dtos)).ToList()
            };
        }


        [Resource(Description ="查找单个")]
        [HttpGet]
        public ResourceResultDto Find(string id)
        {
            return controllerContext.mapper.Map<ResourceResultDto>(_service.QueryList(a => a.Id == id).FirstOrDefault());
        }


        [Resource(Description = "删除")]
        [HttpPost]
        public void Remove(List<string> ids)
        {
            _service.Remove(ids);
            _permissionStore.ReloadPemissionDatas();
        }

        [Resource(Description = "保存")]
        [HttpPost]
        public void Save(ResourceSaveDto saveDto)
        {
            _service.Save(saveDto);
            _permissionStore.ReloadPemissionDatas();
        }
    }
}
