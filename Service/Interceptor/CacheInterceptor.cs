using Castle.DynamicProxy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Service
{
    public class CacheInterceptor : SnailBaseInterceptor
    {
        private IMemoryCache cache;
        protected static readonly MethodInfo SetReturnValueMethodInfo = typeof(CacheInterceptor)
         .GetMethod(nameof(SetReturnValue));
        private int CacheMin = 5;//默认为5分钟
        private ILogger logger;
        private string cacheKey;
        public CacheInterceptor(IMemoryCache cache,ILogger<CacheInterceptor> logger)
        {
            this.cache = cache;
            this.logger = logger;
        }
        public override bool ExecuteBefore(IInvocation invocation)
        {
            var cacheAttri = (CacheInterceptorAttribute)Attribute.GetCustomAttribute(invocation.MethodInvocationTarget, typeof(CacheInterceptorAttribute));
            if (cacheAttri!=null)
            {
                CacheMin = cacheAttri.Min;
            }
            if (!HasReturnValue)
            {
                return false;
            }
            cacheKey = GetCacheKey(invocation);
            if (cache.TryGetValue(cacheKey, out object value))
            {
                logger.LogInformation($"命中缓存:{cacheKey}");
                SetInvocationReturnValue(value, invocation);
                //SetInvocationReturnValue(value, invocation);
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void ExcecuteAfter(IInvocation invocation)
        {
            SetInvocationReturnValueToCache(invocation);
        }


        private void SetInvocationReturnValue(object val, IInvocation invocation)
        {
            if (HasReturnValue)
            {
                if (methodType == MethodType.Synchronous)
                {
                    Type returnType = invocation.Method.ReturnType.GetType();
                    MethodInfo method = SetReturnValueMethodInfo.MakeGenericMethod(returnType);
                    method.Invoke(this, new object[] { val,invocation });

                }
                if (methodType == MethodType.AsyncFunction)
                {
                    Type returnType = invocation.Method.ReturnType.GetGenericArguments()[0];
                    MethodInfo method = SetReturnValueMethodInfo.MakeGenericMethod(returnType);
                    method.Invoke(this, new object[] { val,invocation });
                }
            }
        }

        private void SetInvocationReturnValueToCache(IInvocation invocation)
        {
            if (HasReturnValue)
            {
                if (methodType == MethodType.Synchronous)
                {
                    logger.LogInformation($"设置缓存:{cacheKey}，缓存时间{CacheMin}min");
                    cache.Set(GetCacheKey(invocation), invocation.ReturnValue,TimeSpan.FromMinutes(CacheMin));
                }
                if (methodType == MethodType.AsyncFunction)
                {
                    var resultValue = (object)((dynamic)invocation.ReturnValue).Result;
                    //var t = (Task)invocation.ReturnValue;
                    //var resultValue = t.GetType().GetProperty("Result").GetValue(t);
                    logger.LogInformation($"设置缓存:{cacheKey}，缓存时间{CacheMin}min");
                    cache.Set(GetCacheKey(invocation), resultValue, TimeSpan.FromMinutes(CacheMin));
                }
            }
        }
        public void SetReturnValue<TResult>(object result, IInvocation invocation)
        {
            if (HasReturnValue)
            {
                if (methodType == MethodType.AsyncFunction)
                {
                    invocation.ReturnValue = Task.FromResult<TResult>((TResult)result);
                }
                if (methodType == MethodType.Synchronous)
                {
                    invocation.ReturnValue = result;
                }
            }
        }

        private string GetCacheKey(IInvocation invocation)
        {
            var targetClassName = invocation.TargetType?.Name;
            var targetMethodName = invocation.MethodInvocationTarget?.Name;
            var args = JsonConvert.SerializeObject(invocation.Arguments)?.GetHashCode();
            return $"{targetClassName}_{targetMethodName}_{args}";
        }
    }

    public class CacheInterceptorAttribute : Attribute
    {
        public int Min { get; set; }
        public CacheInterceptorAttribute(int min)
        {
            this.Min = min;
        }
    }
}
