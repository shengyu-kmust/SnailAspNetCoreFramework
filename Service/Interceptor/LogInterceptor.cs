using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class LogInterceptor:SnailBaseInterceptor
    {
        private ILogger _logger;
        private bool _logInput=true;
        private bool _logOutput=false;
        private bool _logTime=true;
        private Stopwatch _stopwatch;
        private StringBuilder _sb;
        private string _methodName;
        private IOptionsMonitor<LogInterceptorOption> options;
        public LogInterceptor(ILogger<LogInterceptor> logger, IOptionsMonitor<LogInterceptorOption> options)
        {
            this._logger = logger;
            this.options = options;
        }
        public override bool ExecuteBefore(IInvocation invocation)
        {
            _sb = new StringBuilder();
            _methodName = $" { invocation.TargetType?.Name}.{ invocation.MethodInvocationTarget?.Name}";
            _sb.AppendLine($"开始执行方法：{_methodName}");
            var attr=(LogInerceptorAttribute)(Attribute.GetCustomAttribute(invocation.Method, typeof(LogInerceptorAttribute), true));
            if (attr!=null)
            {
                _logInput = attr.LogInput;
                _logOutput = attr.LogOutput;
                _logTime = attr.LogTime;
            }
            if (_logInput)
            {
                _sb.AppendLine($"输入参数为:{JsonConvert.SerializeObject(invocation.Arguments)}");
            }
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
            return base.ExecuteBefore(invocation);
        }
        public override void ExcecuteAfter(IInvocation invocation)
        {
            base.ExcecuteAfter(invocation);
            _sb.AppendLine($"结束执行方法：{_methodName}");
            if (_logOutput && HasReturnValue)
            {
                _sb.AppendLine($"输出结束为：{GetReturnValue(invocation)}");
            }
            if (_logTime)
            {
                _sb.AppendLine($"方法{_methodName}耗时为：{_stopwatch.ElapsedMilliseconds}ms");
            }
            _logger.LogInformation(_sb.ToString());
            if (_stopwatch.ElapsedMilliseconds>(options.CurrentValue?.WarnMilliseconds ?? 2000))
            {
                _logger.LogWarning(_sb.ToString());
            }
        }

        private object GetReturnValue(IInvocation invocation)
        {
            if (HasReturnValue)
            {
                if (methodType == MethodType.Synchronous)
                {
                    return invocation.ReturnValue;
                }
                if (methodType == MethodType.AsyncFunction)
                {
                    return ((Task<object>)invocation.ReturnValue).Result;
                }
            }
            return null;
        }
    }

    public class LogInerceptorAttribute:Attribute
    {
        public bool LogInput { get; set; }
        public bool LogOutput { get; set; }
        public bool LogTime { get; set; }
        public LogInerceptorAttribute(bool logInput=true,bool logOutput=false,bool logTime=true)
        {
            this.LogInput = logInput;
            this.LogOutput = logOutput;
            this.LogTime = logTime;
        }
    }

    public class LogInterceptorOption
    {
        public int WarnMilliseconds { get; set; } = 2000;
    }
}
