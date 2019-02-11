using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Domain;
using WebApp.Entity;
using WebApp.Services;

namespace WebApp.Controllers
{
    /// <summary>
    /// 权限管理，只对角色进行授权
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : AuthorizeBaseController
    {
        private PermissionModel _permissionModel;
        private PermissionService _permissionService;
        public PermissionController(PermissionModel permissionModel,DatabaseContext db,PermissionService permissionService):base(db)
        {
            _permissionModel = permissionModel;
            _permissionService = permissionService;
        }
        [HttpGet("RefreshPermission")]
        [AllowAnonymous]
        public ActionResult RefreshPermission()
        {
            _permissionModel.loadPermissionData();
            return Ok("成功");
        }

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



    }
}