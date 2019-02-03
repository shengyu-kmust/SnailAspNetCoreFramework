using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Domain;
using WebApp.Entity;

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
        public PermissionController(PermissionModel permissionModel,DatabaseContext db):base(db)
        {
            _permissionModel = permissionModel;
        }
        [HttpGet("RefreshPermission")]
        [AllowAnonymous]
        public ActionResult RefreshPermission()
        {
            _permissionModel.loadPermissionData();
            return Ok("成功");
        }

        public ActionResult AddPermission(int roleId, string resourceIds)
        {
            throw new NotImplementedException();
        }

        public ActionResult RemovePermission(int roleId, string resourceIds)
        {
            throw new NotImplementedException();
        }

    }
}