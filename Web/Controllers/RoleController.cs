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
        [HttpGet]
        public List<RoleResultDto> QueryList([FromQuery]KeyQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<Role>().AndIf(queryDto.KeyWord.HasValue(), a => a.Name.Contains(queryDto.KeyWord));
            return controllerContext.mapper.ProjectTo<RoleResultDto>(_service.QueryList(pred)).ToList();
        }

        [HttpGet]
        public IPageResult<RoleResultDto> QueryPage([FromQuery]KeyQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<Role>().AndIf(queryDto.KeyWord.HasValue(), a => a.Name.Contains(queryDto.KeyWord));
            return controllerContext.mapper.ProjectTo<RoleResultDto>(_service.QueryList(pred)).ToPageList(queryDto);
        }
        [HttpGet]
        public RoleResultDto Find(string id)
        {
            return controllerContext.mapper.Map<RoleResultDto>(_service.QueryList(a => a.Id == id).FirstOrDefault());
        }
        [HttpPost]
        public void Remove(List<string> ids)
        {
            _service.Remove(ids);
            _permissionStore.ReloadPemissionDatas();
        }

        [HttpPost]
        public void Save(RoleSaveDto saveDto)
        {
            _service.Save(saveDto);
            _permissionStore.ReloadPemissionDatas();
        }
    }
}
