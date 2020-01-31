using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Snail.Common;
using System;

namespace Web.Filter
{
    /// <summary>
    /// 缓存过滤器
    /// </summary>
    /// <remarks>
    /// * 经验证，如果在action或是controller上加CacheFilter特性后，CacheFilterAttribute的生命周期是每个action/controller为一个单例。（即有程序里有多少个CacheFilter特性，就会有多少个实例）
    /// </remarks>
    public class CacheFilterAttribute: ActionFilterAttribute
    {

        public int ExpireMinute { get; set; }//
        private string _key { get; set; }//经验证，对于同一个action方法，CacheFilterAttribute是单例，如果多线程用时，_key会出问题。 // todo

        // 下面代码打开时可测试实例的个数
        //private int count;
        //public CacheFilterAttribute()
        //{
        //    count = 1;
        //}
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //count++;
            var cache=context.HttpContext.RequestServices.GetService<IMemoryCache>();
            _key = BuildKey(context);
            if (cache.TryGetValue(_key, out object value))
            {
                context.Result = value as IActionResult;
            }
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var expire = ExpireMinute == 0 ? 30 : ExpireMinute;
            var cache = context.HttpContext.RequestServices.GetService<IMemoryCache>();
            cache.Set(_key, context.Result, DateTimeOffset.Now.AddMinutes(expire));
        }

        private string BuildKey(ActionExecutingContext context)
        {
            var controllerAction = $"{context.ActionDescriptor.RouteValues["Controller"]}_{context.ActionDescriptor.RouteValues["Action"]}";
            if (context.ActionArguments==null || context.ActionArguments.Count==0)
            {
                return controllerAction;
            }
            else
            {
                var argKey = HashHelper.Md5(JsonConvert.SerializeObject(context.ActionArguments));
                return $"{controllerAction}_{argKey}";
            }
        }
      
    }

}
