using ApplicationCore.Entity;
using ApplicationCore.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snail.Common;
using Snail.Common.Extenssions;
using Snail.Core;
using Snail.Core.Attributes;
using Snail.Core.Permission;
using System.Collections.Generic;
using System.Linq;
using Web.DTO;
namespace Web.Controllers
{

    [Authorize(Policy = PermissionConstant.PermissionAuthorizePolicy)]
    [Resource(Description = "角色管理")]
    public class RoleController : DefaultBaseController, ICrudController<Role, RoleSaveDto, RoleResultDto, KeyQueryDto>
    {
        private IRoleService _service;
        private IPermissionStore _permissionStore;
        private IPermission _permission;
        public RoleController(IRoleService service, ControllerContext controllerContext,IPermissionStore permissionStore, IPermission permission) : base(controllerContext)
        {
            this.controllerContext = controllerContext;
            this._service = service;
            this._permissionStore = permissionStore;
            _permission = permission;
        }
        [Resource(Description ="查询列表")]
        [HttpGet]
        public List<RoleResultDto> QueryList([FromQuery]KeyQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<Role>().And(a => !a.IsDeleted).AndIf(queryDto.KeyWord.HasValue(), a => a.Name.Contains(queryDto.KeyWord));
            return controllerContext.mapper.ProjectTo<RoleResultDto>(_service.QueryList(pred)).ToList();
        }

        [Resource(Description ="查询分页")]
        [HttpGet]
        public IPageResult<RoleResultDto> QueryPage([FromQuery]KeyQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<Role>().And(a=>!a.IsDeleted).AndIf(queryDto.KeyWord.HasValue(), a => a.Name.Contains(queryDto.KeyWord));
            return controllerContext.mapper.ProjectTo<RoleResultDto>(_service.QueryList(pred)).ToPageList(queryDto);
        }
        [Resource(Description ="查找单个")]
        [HttpGet]
        public RoleResultDto Find(string id)
        {
            return controllerContext.mapper.Map<RoleResultDto>(_service.QueryList(a => a.Id == id).FirstOrDefault());
        }
        [Resource(Description ="删除")]
        [HttpPost]
        public void Remove(List<string> ids)
        {
            _service.Remove(ids);
            _permissionStore.ReloadPemissionDatas();
        }

        [Resource(Description ="保存")]
        [HttpPost]
        public void Save(RoleSaveDto saveDto)
        {
            _service.Save(saveDto);
            _permissionStore.ReloadPemissionDatas();
        }
    }
}
