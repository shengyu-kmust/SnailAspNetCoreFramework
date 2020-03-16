using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Snail.Core;
using Snail.Core.Dto;

namespace Web.Filter
{
    public class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private ILogger _logger;
        public GlobalExceptionFilterAttribute(ILogger<GlobalExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }
        public override void OnException(ExceptionContext context)
        {

            if (context.Exception is BusinessException businessException)
            {
                // 业务类的异常，返回400状态，并返回异常内容。模型校验也会返回400状态和内容
                //context.Result = new BadRequestObjectResult(businessException.Message);
                context.Result = new ObjectResult(ApiResultDto.BadRequestResult(businessException.Message));
                if (_logger != null)
                {
                    _logger.LogWarning(businessException, "businessException");
                }
            }
            else
            {
                context.Result = new ObjectResult(ApiResultDto.ServerErrorResult("未知异常"));
                if (_logger != null)
                {
                    _logger.LogError("exception:{exception} \n innerException:{innerException}", context.Exception?.ToString(), context.Exception?.InnerException?.ToString());
                }
            }

        }
    }
}
