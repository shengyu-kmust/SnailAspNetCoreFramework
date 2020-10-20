using ApplicationCore.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snail.Common;
using Snail.Common.Extenssions;
using Snail.Core;
using Snail.Core.Attributes;
using Snail.Core.Permission;
using Snail.EntityFrameworkCore;
using Snail.Permission.Entity;
using Snail.Web.Dtos;
using Snail.Web.Services;
using System.Collections.Generic;
using System.Linq;
namespace Snail.Web.Controllers
{

    [Authorize(Policy = PermissionConstant.PermissionAuthorizePolicy)]
    [Resource(Description = "用户管理")]
    public class UserController : DefaultBaseController, ICrudController<PermissionDefaultUser , UserSaveDto, UserResultDto, KeyQueryDto>
    {
        private UserService _service;
        private IPermissionStore _permissionStore;
        private IPermission _permission;
        public UserController(UserService service, SnailControllerContext controllerContext,IPermissionStore permissionStore, IPermission permission) : base(controllerContext)
        {
            this.controllerContext = controllerContext;
            this._service = service;
            this._permissionStore = permissionStore;
            _permission = permission;
        }

        [Resource(Description ="查询列表")]
        [HttpGet]
        public List<UserResultDto> QueryList([FromQuery]KeyQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<PermissionDefaultUser >().And(a=>!a.IsDeleted).AndIf(queryDto.KeyWord.HasValue(), a => a.Name.Contains(queryDto.KeyWord) || a.Email.Contains(queryDto.KeyWord) || a.Account.Contains(queryDto.KeyWord));
            return controllerContext.mapper.ProjectTo<UserResultDto>(_service.QueryList(pred)).ToList();
        }

        [Resource(Description ="查询分页")]
        [HttpGet]
        public IPageResult<UserResultDto> QueryPage([FromQuery]KeyQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<PermissionDefaultUser >().And(a => !a.IsDeleted).AndIf(queryDto.KeyWord.HasValue(), a => a.Name.Contains(queryDto.KeyWord) || a.Email.Contains(queryDto.KeyWord) || a.Account.Contains(queryDto.KeyWord));
            return controllerContext.mapper.ProjectTo<UserResultDto>(_service.QueryList(pred)).ToPageList(queryDto);
        }

        [Resource(Description ="查找单个")]
        [HttpGet]
        public UserResultDto Find(string id)
        {
            return controllerContext.mapper.Map<UserResultDto>(_service.QueryList(a => a.Id == id).FirstOrDefault());
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
        public void Save(UserSaveDto saveDto)
        {
            var pwd = saveDto.Pwd.HasValue() ? _permission.HashPwd(saveDto.Pwd) : _permission.HashPwd("123456");
            var canUpdatePwd = saveDto.Id.HasValue() && saveDto.Pwd.HasValue();
            db.Set<PermissionDefaultUser>().AddOrUpdate(saveDto, dto =>
            {
                var entityTemp = mapper.Map<PermissionDefaultUser >(dto);
                entityTemp.Pwd = pwd;
                return entityTemp;
            }, (dto, entity) =>
            {
                var pwdBack = entity.Pwd;
                mapper.Map<UserSaveDto, PermissionDefaultUser >(dto, entity);
                if (canUpdatePwd)
                {
                    entity.Pwd = pwd;
                }
                else
                {
                    entity.Pwd = pwdBack;
                }
            }, currentUserId);
            db.SaveChanges();
            _permissionStore.ReloadPemissionDatas();
        }
    }
}
