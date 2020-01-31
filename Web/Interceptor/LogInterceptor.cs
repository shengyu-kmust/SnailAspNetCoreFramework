using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Interceptor
{
    /// <summary>
    /// 日志拦截器
    /// </summary>
    /// <remarks>
    /// 只对同步方法进行拦截，异步的方法未做测试，nlog的name为Web.Interceptor.LogInterceptor
    /// </remarks>
    public class LogInterceptor : IInterceptor
    {

        private readonly ILogger<LogInterceptor> _logger;
       
        public LogInterceptor(ILogger<LogInterceptor> logger)
        {
            _logger = logger;
        }
        public void Intercept(IInvocation invocation)
        {
            _logger.LogInformation("方法{0}.{1}正在调用，输入参数为：{2}... ",invocation.TargetType.Name,invocation.Method.Name,string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));
            invocation.Proceed();
            Type type = invocation.ReturnValue?.GetType();
            if (type != null && type == typeof(Task))
            {
                // Given the method returns a Task, wait for it to complete before performing Step 2
                Func<Task> continuation = async () =>
                {
                    await (Task)invocation.ReturnValue;

                    _logger.LogInformation("方法{0}.{1}正在调用，输出结果为：{2}", invocation.TargetType.Name, invocation.Method.Name, invocation.ReturnValue);
                };

                invocation.ReturnValue = continuation();
                return;
            }

            _logger.LogInformation("方法{0}.{1}调用结束，输出结果为：{2}", invocation.TargetType.Name,invocation.Method.Name,invocation.ReturnValue);
        }
    }

}
