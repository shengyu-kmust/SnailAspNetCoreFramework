using AutoMapper;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snail.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    /// <summary>
    /// 所有contoller继承此类
    /// </summary>
    /// <remarks>
    /// 如果没有AuthorizeAttribute，HttpContext.User不会有用户
    /// </remarks>
    [ApiController]
    [Route("api/[Controller]/[Action]")]
    [Authorize]
    public class DefaultBaseController : ControllerBase
    {
        protected ControllerContext controllerContext;
        protected IMapper mapper => controllerContext.mapper;
        protected IApplicationContext applicationContext => controllerContext.applicationContext;
        protected AppDbContext db => controllerContext.db;
        protected IEntityCacheManager entityCacheManager => controllerContext.entityCacheManager;
        protected string currentUserId => controllerContext.applicationContext.GetCurrentUserId();
        public DefaultBaseController(ControllerContext controllerContext)
        {
            this.controllerContext = controllerContext;
        }
    }
}
