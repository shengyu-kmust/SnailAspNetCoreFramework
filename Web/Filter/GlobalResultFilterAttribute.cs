using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Snail.Core.Dto;

namespace Web.Filter
{
    /// <summary>
    /// 只有没有进入到ExceptFilter里的才会进入此过滤器
    /// </summary>
    public class GlobalResultFilterAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult objectResult)//正常返回
            {
                context.Result = new ObjectResult(ApiResultDto.SuccessResult(objectResult.Value));
            }
            else if (context.Result is EmptyResult emptyResult)
            {
                context.Result = new ObjectResult(ApiResultDto.SuccessResult(null));
            }
        }
    }
}
