using ApplicationCore.Entity;
using ApplicationCore.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Snail.Common;
using Snail.Common.Extenssions;
using Snail.Core;
using Snail.Core.Permission;
using System.Collections.Generic;
using System.Linq;
using Web.DTO;
using Web.DTO.Resource;

namespace Web.Controllers
{

    //[Authorize(Policy = PermissionConstant.PermissionAuthorizePolicy)]
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
        [HttpGet]
        public List<ResourceResultDto> QueryList([FromQuery]KeyQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<Resource>().AndIf(queryDto.KeyWord.HasValue(), a => a.Name.Contains(queryDto.KeyWord) || a.Code.Contains(queryDto.KeyWord));
            return controllerContext.mapper.ProjectTo<ResourceResultDto>(_service.QueryList(pred)).ToList();
        }

        [HttpGet]
        public IPageResult<ResourceResultDto> QueryPage([FromQuery]KeyQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<Resource>().AndIf(queryDto.KeyWord.HasValue(), a => a.Name.Contains(queryDto.KeyWord) || a.Code.Contains(queryDto.KeyWord));
            return controllerContext.mapper.ProjectTo<ResourceResultDto>(_service.QueryList(pred)).ToPageList(queryDto);
        }
        [HttpGet]
        public ResourceResultDto Find(string id)
        {
            return controllerContext.mapper.Map<ResourceResultDto>(_service.QueryList(a => a.Id == id).FirstOrDefault());
        }
        [HttpPost]
        public void Remove(List<string> ids)
        {
            _service.Remove(ids);
            _permissionStore.ReloadPemissionDatas();
        }

        [HttpPost]
        public void Save(ResourceSaveDto saveDto)
        {
            _service.Save(saveDto);
            _permissionStore.ReloadPemissionDatas();
        }
    }
}
