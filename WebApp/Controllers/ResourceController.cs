using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Services;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourceController : AuthorizeBaseController
    {
        private ResourceService _resourceManager;
        public ResourceController(ResourceService resourceManager)
        {
            _resourceManager = resourceManager;
        }

        [Description("权限初始化")]
        [HttpGet("Init")]
        [AllowAnonymous]
        public ActionResult InitResource()
        {
            _resourceManager.InitWebapiResource();
            return Ok();
        }

    }
}