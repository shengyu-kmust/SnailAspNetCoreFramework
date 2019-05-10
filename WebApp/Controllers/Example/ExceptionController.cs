using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DAL.Controllers.Example
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExceptionController:ControllerBase
    {
        [HttpGet("BusinessException")]
        public void BusinessException()
        {
            throw new BusinessException("业务异常内容");
        }

        [HttpGet("InnerException")]
        public void InnerException()
        {
            throw new Exception("服务器内部异常");
        }
    }
}
