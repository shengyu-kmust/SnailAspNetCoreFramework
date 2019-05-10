using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DAL.DTO;
using DAL.Entity;
using DAL.Enum;
using DAL.Infrastructure;
using DAL.Services;

namespace DAL.Controllers
{
    /// <summary>
    /// 权限相关的所有接口
    /// </summary>
    public class PermissionManageController : AuthorizeBaseController
    {
        private ResourceRepository _resourceRepository;
        private ResourceService _resourceService;
        private PermissionService _permissionService;
        public PermissionManageController(DatabaseContext db, ResourceRepository resourceRepository, ResourceService resourceService
        , PermissionService permissionService) : base(db)
        {
            _resourceRepository = resourceRepository;
            _resourceService = resourceService;
            _permissionService = permissionService;
        }

        #region 角色-资源
        /// <summary>
        /// 设置角色的权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="resourceIds">资源</param>
        /// <returns></returns>
        public ActionResult AddPermission(int roleId, string resourceIds)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 移除角色的权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="resourceIds">资源ID</param>
        /// <returns></returns>
        public ActionResult RemovePermission(int roleId, string resourceIds)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取角色所有的权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public ActionResult RoleAllPermission(int roleId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 用户-角色
        /// <summary>
        /// 设置用户的角色
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public ActionResult AddRolesToUsers(int userId, string roleIds)
        {
            _permissionService.AddRolesToUser(userId, roleIds.Split(',').Select(a=>int.Parse(a)).ToList());
            return Ok("success");
        }
        /// <summary>
        /// 删除用户的角色
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public ActionResult RemoveRolesFromUsers(int userId, string roleIds)
        {
            _permissionService.RemoveRolesFromUser(userId, roleIds.Split(',').Select(a => int.Parse(a)).ToList());
            return Ok("success");
        }


        #endregion

        #region 资源


        /// <summary>
        /// 资源初始化
        /// </summary>
        /// <remarks>将所有的webapi设置为权限资源</remarks>
        /// <returns></returns>
        [Description("资源初始化")]
        [HttpGet("Init")]
        [AllowAnonymous]
        public ActionResult InitResource()
        {
            _resourceService.InitWebapiResource();
            return Ok("成功初始化");
        }

        /// <summary>
        /// 获取所有的资源
        /// </summary>
        /// <returns></returns>
        public List<ResourceDTO> AllResource()
        {
            return _resourceRepository.AllValid()?.Select(a => new ResourceDTO()
            {
                Id=a.Id,
                Name=a.Key,
                ParentId=a.ParentId,
                ResourceAddress=a.Value,
                ResourceType=a.Category
            }).ToList();
        }

        /// <summary>
        /// 添加受保护资源 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="resourceType"></param>
        /// <param name="resourceAddress"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public ActionResult AddResource(string name,string resourceType,string resourceAddress,int parentId)
        {
            if (!System.Enum.TryParse(resourceAddress, out ResourceCategory category))
            {
                throw  new Exception("无此资源类型");
            }
            _resourceService.AddResource(resourceAddress, name, category);
            return Ok("成功");
        }

        /// <summary>
        /// 删除受保护资源 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteResource(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 角色

        public ActionResult AddRole(string roleName)
        {
            throw  new NotImplementedException();
        }

        public ActionResult DeleteRole(int roleId)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
