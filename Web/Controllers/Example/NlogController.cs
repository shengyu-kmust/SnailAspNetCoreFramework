using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.Example
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NlogController : ControllerBase
    {
        private readonly ILogger<NlogController> _logger;
        public NlogController(ILogger<NlogController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Log()
        {
            _logger.LogError("nlog出错测试");
            return "success";
        }
    }
}
