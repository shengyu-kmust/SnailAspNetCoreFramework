using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Interceptor
{
    public class LogInterceptor : IInterceptor
    {
        private ILogger _log;
        public LogInterceptor(ILogger log)
        {
            _log = log;
        }
        public void Intercept(IInvocation invocation)
        {
            _log.LogInformation("方法{0}.{1}正在调用，输入参数为：{2}... ",invocation.TargetType.Name,invocation.Method.Name,string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));
            invocation.Proceed();
            _log.LogInformation("方法{0}.{1}正在调用，输出结果为：{2}", invocation.TargetType.Name,invocation.Method.Name,invocation.ReturnValue);
        }
    }
}
