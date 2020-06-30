using ApplicationCore.Entity;
using ApplicationCore.IServices;
using Infrastructure;
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

    //[Authorize(Policy = PermissionConstant.PermissionAuthorizePolicy)]
    public class UserController : DefaultBaseController, ICrudController<User, UserSaveDto, UserResultDto, KeyQueryDto>
    {
        private IUserService _service;
        private IPermissionStore _permissionStore;
        private IPermission _permission;
        public UserController(IUserService service, ControllerContext controllerContext,IPermissionStore permissionStore, IPermission permission) : base(controllerContext)
        {
            this.controllerContext = controllerContext;
            this._service = service;
            this._permissionStore = permissionStore;
            _permission = permission;
        }
        [HttpGet]
        public List<UserResultDto> QueryList([FromQuery]KeyQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<User>().And(a=>!a.IsDeleted).AndIf(queryDto.KeyWord.HasValue(), a => a.Name.Contains(queryDto.KeyWord) || a.Email.Contains(queryDto.KeyWord) || a.Account.Contains(queryDto.KeyWord));
            return controllerContext.mapper.ProjectTo<UserResultDto>(_service.QueryList(pred)).ToList();
        }

        [HttpGet]
        public IPageResult<UserResultDto> QueryPage([FromQuery]KeyQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<User>().And(a => !a.IsDeleted).AndIf(queryDto.KeyWord.HasValue(), a => a.Name.Contains(queryDto.KeyWord) || a.Email.Contains(queryDto.KeyWord) || a.Account.Contains(queryDto.KeyWord));
            return controllerContext.mapper.ProjectTo<UserResultDto>(_service.QueryList(pred)).ToPageList(queryDto);
        }
        [HttpGet]
        public UserResultDto Find(string id)
        {
            return controllerContext.mapper.Map<UserResultDto>(_service.QueryList(a => a.Id == id).FirstOrDefault());
        }
        [HttpPost]
        public void Remove(List<string> ids)
        {
            _service.Remove(ids);
            _permissionStore.ReloadPemissionDatas();
        }

        [HttpPost]
        public void Save(UserSaveDto saveDto)
        {
            var pwd = saveDto.Pwd.HasValue() ? _permission.HashPwd(saveDto.Pwd) : _permission.HashPwd("123456");
            var canUpdatePwd = saveDto.Id.HasValue() && saveDto.Pwd.HasValue();
            db.Users.AddOrUpdate(saveDto, dto =>
            {
                var entityTemp = mapper.Map<User>(dto);
                entityTemp.Pwd = pwd;
                return entityTemp;
            }, (dto, entity) =>
            {
                var pwdBack = entity.Pwd;
                mapper.Map<UserSaveDto, User>(dto, entity);
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
