using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    /// <summary>
    /// 所有contoller继承此类
    /// </summary>
    [ApiController]
    [Route("api/[Controller]/[Action]")]
    public class DefaultBaseController : ControllerBase
    {
        protected ControllerContext controllerContext;
        public DefaultBaseController(ControllerContext controllerContext)
        {
            this.controllerContext = controllerContext;
        }
    }
}
