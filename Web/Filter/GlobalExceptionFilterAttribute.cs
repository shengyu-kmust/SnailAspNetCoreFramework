using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web
{
    public class GlobalExceptionFilterAttribute: ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is BusinessException businessException)
            {
                // 业务类的异常，返回400状态，并返回异常内容。模型校验也会返回400状态和内容
                context.Result=new BadRequestObjectResult(businessException.Message);
            }
            // 非业务类的异常，不做处理，由asp.net core 管道进行异常的统一处理，分开发环境和生产环境进行不同的处理
        }
    }
}
