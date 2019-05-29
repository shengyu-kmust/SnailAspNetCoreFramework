using DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using Web.Services;

namespace Web.Controllers
{
    /// <summary>
    /// 资源管理，系统所有能访问的webapi，菜单等资源
    /// </summary>
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