using ApplicationCore.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Snail.Common.Extenssions;
using Snail.Core.Attributes;
using Snail.Core.Permission;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Web.DTO;
using Web.DTO.Permission;
using Web.Permission;

namespace Web.Controllers
{
    [Authorize(Policy = PermissionConstant.PermissionAuthorizePolicy)]
    [Resource(Description = "权限管理")]
    public class PermissionController : DefaultBaseController
    {
        private IPermission _permission;
        private IPermissionStore _permissionStore;
        private IToken _token;
        public PermissionController(IPermission permission, IPermissionStore permissionStore, ControllerContext controllerContext, IToken token):base(controllerContext)
        {
            _permission = permission;
            _permissionStore = permissionStore;
            _token = token;
        }
        #region 特殊化的
        #endregion

        #region 查询权限数据
        [HttpGet, Resource(Description = "查询所有用户")]
        public List<PermissionUserInfo> GetAllPermissionUserInfo()
        {
            return _permissionStore.GetAllUser().Select(a => new PermissionUserInfo
            {
                Id = a.GetKey(),
                Account = a.GetAccount(),
                Name = a.GetName(),
            }).ToList();
        }
        [HttpGet, Resource(Description = "查询所有角色")]
        public List<PermissionRoleInfo> GetAllRole()
        {
            return _permissionStore.GetAllRole().Select(a => new PermissionRoleInfo
            {
                Id = a.GetKey(),
                Name = a.GetName(),
            }).ToList();
        }

        [HttpGet, Resource(Description = "查询用户的所有角色")]
        public PermissionUserRoleInfo GetUserRoles(string userKey)
        {
            var userRoleKeys = _permissionStore.GetAllUserRole().Where(a => a.GetUserKey() == userKey).Select(a => a.GetRoleKey()).Distinct().ToList();
            return new PermissionUserRoleInfo
            {
                UserKey = userKey,
                RoleKeys = userRoleKeys
            };
        }

        [HttpGet, Resource(Description = "查询角色的所有资源")]
        public PermissionRoleResourceInfo GetRoleResources(string roleKey)
        {
            var roleResourceKeys = _permissionStore.GetAllRoleResource().Where(a => a.GetRoleKey() == roleKey).Select(a => a.GetResourceKey()).Distinct().ToList();
            return new PermissionRoleResourceInfo
            {
                RoleKey = roleKey,
                ResourceKeys = roleResourceKeys
            };
        }
        #endregion

        #region 登录注销
        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        [HttpPost, AllowAnonymous]
        public LoginResult Login(LoginDto loginDto)
        {
            var result = _permission.Login(loginDto);
            if (loginDto.SignIn)
            {
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(_permission.GetClaims(result.UserInfo), CookieAuthenticationDefaults.AuthenticationScheme, PermissionConstant.userIdClaim, PermissionConstant.roleIdsClaim));
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
            }
            return result;
        }

        [HttpPost, AllowAnonymous]
        public void Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        #endregion

        [AllowAnonymous]
        [HttpGet]
        public Snail.Core.Permission.UserInfo GetUserInfo(string token)
        {
            var claims=_token.ResolveFromToken(token);
            return _permission.GetUserInfo(new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, PermissionConstant.userIdClaim, PermissionConstant.roleIdsClaim)));
        }

        /// <summary>
        /// 获取所有的资源以及资源角色的对应关系信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public List<ResourceRoleInfo> GetAllResourceRoles()
        {
            return _permission.GetAllResourceRoles();
        }

        /// <summary>
        /// 初始化权限资源
        /// </summary>
        [HttpGet, Resource(Description = "初始化权限资源")]
        public void InitResource()
        {
            _permission.InitResource();
        }

        [HttpPost, Resource(Description = "保存用户")]
        public void SaveUser(UserSaveDto user)
        {
            // 增加时，设置密码
            if (user.Id.HasNotValue())
            {
                user.Pwd = user.Pwd.HasValue() ? _permission.HashPwd(user.Pwd) : _permission.HashPwd("123456");
            }
            else
            {
                // 修改时，如果密码不为空，则更新密码
                if (user.Id.HasValue() && user.Pwd.HasValue())
                {
                    user.Pwd = _permission.HashPwd(user.Pwd);
                }
            }
            _permissionStore.SaveUser(new User
            {
                Account = user.Account,
                Email = user.Email,
                Gender = user.Gender,
                Id = user.Id,
                Name = user.Name,
                Phone = user.Phone,
                Pwd = user.Pwd,
            });
        }
        [HttpPost, Resource(Description = "删除用户")]
        public void RemoveUser(string userKey)
        {
            _permissionStore.RemoveUser(userKey);
        }

        [HttpPost, Resource(Description = "保存角色")]
        public void SaveRole(PermissionRoleSaveInfo role)
        {
            _permissionStore.SaveRole(new Role
            {
                Id = role.Id,
                Name = role.Name
            });
        }

        [HttpPost, Resource(Description = "删除角色")]
        public void RemoveRole(string roleKey)
        {
            _permissionStore.RemoveRole(roleKey);
        }

        [HttpPost, Resource(Description = "用户授予角色")]
        public void SetUserRoles(PermissionUserRoleInfo dto)
        {
            _permissionStore.SetUserRoles(dto.UserKey, dto.RoleKeys);
        }
        [HttpPost, Resource(Description = "角色授予资源")]
        public void SetRoleResources(PermissionRoleResourceInfo dto)
        {
            _permissionStore.SetRoleResources(dto.RoleKey, dto.ResourceKeys);
        }
    }
}
