using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Web
{
    public static class HangfireHelper
    {
        public static void AddHangfire(Assembly[] assemblies)
        {
            assemblies.SelectMany(a => a.GetTypes()).SelectMany(a => a.GetMethods()).Where(a => a.IsDefined(typeof(BackgroundJobAttribute))).ToList().ForEach(methodInfo =>
            {
                var attr = methodInfo.GetCustomAttribute<BackgroundJobAttribute>();
                if (attr!=null)
                {
                    if (attr.JobType==EBackgroundJobType.Enqueue)
                    {
                        AddEnqueueJob(methodInfo.DeclaringType, methodInfo);
                    }
                    else if (attr.JobType == EBackgroundJobType.Schedule)
                    {
                        AddScheduleJob(methodInfo.DeclaringType, methodInfo,attr.DelayTimeSpan);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(attr.Cron))
                        {
                            AddRecurringJob(methodInfo.DeclaringType, methodInfo, attr.Cron);
                        }
                    }
                }
            });
        }

        private static void AddEnqueueJob(Type classType, MethodInfo methodInfo)
        {
            var invokeMethodInfo = typeof(BackgroundJob).GetMethods().FirstOrDefault(a => a.Name == "Enqueue" && a.IsGenericMethod && a.GetParameters().Length == 1 && a.GetParameters().Any(i => i.ToString().Contains("Action")));
            var expression = BuildExpression(classType, methodInfo);
            invokeMethodInfo.MakeGenericMethod(classType).Invoke(typeof(BackgroundJob), new object[] { expression });
        }

        private static void AddScheduleJob(Type classType, MethodInfo methodInfo,string delay)
        {
            if(!TimeSpan.TryParse(delay,out TimeSpan timeSpan))
            {
                timeSpan = new TimeSpan(0, 0, 30);
            }
            var invokeMethodInfo = typeof(BackgroundJob).GetMethods().FirstOrDefault(a => a.Name == "Schedule" && a.IsGenericMethod && a.GetParameters().Length == 2 && a.GetParameters().Any(i => i.ToString().Contains("Action") && a.GetParameters().Any(i => i.ToString().Contains("TimeSpan"))));
            var expression = BuildExpression(classType, methodInfo);
            invokeMethodInfo.MakeGenericMethod(classType).Invoke(typeof(BackgroundJob), new object[] { expression, timeSpan });
        }

        private static void AddRecurringJob(Type classType, MethodInfo methodInfo, string cron)
        {
            var invokeMethodInfo = typeof(RecurringJob).GetMethods().FirstOrDefault(a => a.Name == "AddOrUpdate" && a.IsGenericMethod && a.GetParameters().Length == 4 && a.GetParameters()[0].ToString().Contains("Action") && a.GetParameters()[1].ParameterType == typeof(string));
            var expression = BuildExpression(classType, methodInfo);
            invokeMethodInfo.MakeGenericMethod(classType).Invoke(typeof(RecurringJob), new object[] { expression, cron, TimeZoneInfo.Local,"default" });//default不能为null，如果为null，调用的方法就不是上面查询出来的方法了，原因不详
        }

        private static LambdaExpression BuildExpression(Type classType,MethodInfo methodInfo)
        {
            var pa = Expression.Parameter(classType);
            var body = Expression.Call(pa, methodInfo);
            return Expression.Lambda(body, pa);
        }
    }

    public class BackgroundJobAttribute : Attribute
    {
        public EBackgroundJobType JobType { get; set; }
        public string DelayTimeSpan { get; set; }
        public string Cron { get; set; }
    }


    public enum EBackgroundJobType
    {
        /// <summary>
        /// 立即执行，并只执行一次
        /// </summary>
        Enqueue,
        /// <summary>
        /// 多久后执行，并执行一次
        /// </summary>
        Schedule,
        /// <summary>
        /// 循环执行
        /// </summary>
        Recurring
    }
}
