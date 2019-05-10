using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Entity;
using DAL.Services;

namespace DAL.Controllers
{
    /// <summary>
    /// 资源管理，系统所有能访问的webapi，菜单等资源
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : AuthorizeBaseController
    {
        private ResourceService _resourceManager;
        public ResourceController(ResourceService resourceManager,DatabaseContext db):base(db)
        {
            _resourceManager = resourceManager;
        }

        /// <summary>
        /// 资源初始化
        /// </summary>
        /// <returns></returns>
        [Description("资源初始化")]
        [HttpGet("Init")]
        [AllowAnonymous]
        public ActionResult InitResource()
        {
            _resourceManager.InitWebapiResource();
            return Ok();
        }

    }
}