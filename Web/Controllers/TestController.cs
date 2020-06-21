using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    public class TestController : DefaultBaseController
    {
        public TestController(ControllerContext controllerContext) : base(controllerContext)
        {
        }

        public string GetCurrentUserId()
        {
            return currentUserId;
        }
     
    }
}
