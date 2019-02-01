using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Model;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : AuthorizeBaseController
    {
        private PermissionModel _permissionModel;
        public PermissionController(PermissionModel permissionModel)
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
    }
}