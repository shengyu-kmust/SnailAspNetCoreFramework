using Castle.DynamicProxy;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Snail.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Interceptor
{
    /// <summary>
    /// 缓存拦截器，未测试
    /// </summary>
    /// <remarks>
    /// 只对同步方法有用
    /// </remarks>
    public class CacheInterceptor : IInterceptor
    {
        private IMemoryCache _memoryCache;
        public CacheInterceptor(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public void Intercept(IInvocation invocation)
        {
            var key = BuildKey(invocation);
            if (_memoryCache.TryGetValue(key,out object value))
            {
                invocation.ReturnValue = value;
            }
            else
            {
                invocation.Proceed();
            }
        }

        private string BuildKey(IInvocation invocation)
        {
            if (invocation.Arguments==null || invocation.Arguments.Length==0)
            {
                return $"{invocation.TargetType.Name}_{invocation.Method.Name}";
            }
            else
            {
                var argKey = HashHelper.Md5(JsonConvert.SerializeObject(invocation.Arguments));
                return $"{invocation.TargetType.Name}_{invocation.Method.Name}_{argKey}";
            }
        }
    }
}
