using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace Web
{
    public static class HangfireHelper
    {
        // 参考了BackgroundJob和RecurringJob的源代码

        //RecurringJobManager用于管理循环执行的任务
        private static readonly Lazy<RecurringJobManager> RecurringJobManagerInstance = new Lazy<RecurringJobManager>(
          () => new RecurringJobManager(), LazyThreadSafetyMode.PublicationOnly);

        // IBackgroundJobClient用于管理Enqueue和Schedule类型的job
        private static readonly Lazy<IBackgroundJobClient> CachedClient
          = new Lazy<IBackgroundJobClient>(() => new BackgroundJobClient(), LazyThreadSafetyMode.PublicationOnly);

        private static readonly Func<IBackgroundJobClient> DefaultFactory
            = () => CachedClient.Value;

        private static Func<IBackgroundJobClient> _clientFactory;
        private static readonly object ClientFactoryLock = new object();

        internal static Func<IBackgroundJobClient> ClientFactory
        {
            get
            {
                lock (ClientFactoryLock)
                {
                    return _clientFactory ?? DefaultFactory;
                }
            }
            set
            {
                lock (ClientFactoryLock)
                {
                    _clientFactory = value;
                }
            }
        }


        public static void AddHangfire(Assembly[] assemblies)
        {
            assemblies.SelectMany(a => a.GetTypes()).SelectMany(a => a.GetMethods()).Where(a => a.IsDefined(typeof(BackgroundJobAttribute))).ToList().ForEach(methodInfo =>
            {
                var attr = methodInfo.GetCustomAttribute<BackgroundJobAttribute>();
                if (attr != null)
                {
                    if (attr.JobType == EBackgroundJobType.Enqueue)
                    {
                        AddEnqueueJob(methodInfo.DeclaringType, methodInfo);
                    }
                    else if (attr.JobType == EBackgroundJobType.Schedule)
                    {
                        AddScheduleJob(methodInfo.DeclaringType, methodInfo, attr.DelayTimeSpan);
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
            //var invokeMethodInfo = typeof(BackgroundJob).GetMethods().FirstOrDefault(a => a.Name == "Enqueue" && a.IsGenericMethod && a.GetParameters().Length == 1 && a.GetParameters().Any(i => i.ToString().Contains("Action")));
            //var expression = BuildExpression(classType, methodInfo);
            //invokeMethodInfo.MakeGenericMethod(classType).Invoke(typeof(BackgroundJob), new object[] { expression });

            ClientFactory().Create(CreateJob(classType, methodInfo), new EnqueuedState());
        }

        private static Job CreateJob(Type classType, MethodInfo methodInfo)
        {
            return new Job(classType, methodInfo);
        }
        private static void AddScheduleJob(Type classType, MethodInfo methodInfo, string delay)
        {
            if (!TimeSpan.TryParse(delay, out TimeSpan timeSpan))
            {
                timeSpan = new TimeSpan(0, 0, 30);
            }
            //var invokeMethodInfo = typeof(BackgroundJob).GetMethods().FirstOrDefault(a => a.Name == "Schedule" && a.IsGenericMethod && a.GetParameters().Length == 2 && a.GetParameters().Any(i => i.ToString().Contains("Action") && a.GetParameters().Any(i => i.ToString().Contains("TimeSpan"))));
            //var expression = BuildExpression(classType, methodInfo);
            //invokeMethodInfo.MakeGenericMethod(classType).Invoke(typeof(BackgroundJob), new object[] { expression, timeSpan });
            ClientFactory().Create(CreateJob(classType, methodInfo), new ScheduledState(timeSpan));
        }

        private static void AddRecurringJob(Type classType, MethodInfo methodInfo, string cron)
        {
            //var invokeMethodInfo = typeof(RecurringJob).GetMethods().FirstOrDefault(a => a.Name == "AddOrUpdate" && a.IsGenericMethod && a.GetParameters().Length == 4 && a.GetParameters()[0].ToString().Contains("Action") && a.GetParameters()[1].ParameterType == typeof(string));
            //var expression = BuildExpression(classType, methodInfo);
            //invokeMethodInfo.MakeGenericMethod(classType).Invoke(typeof(RecurringJob), new object[] { expression, cron, TimeZoneInfo.Local, "default" });//default不能为null，如果为null，调用的方法就不是上面查询出来的方法了，原因不详
            var job = new Job(classType, methodInfo);
            var id = GetRecurringJobId(job);
            RecurringJobManagerInstance.Value.AddOrUpdate(id, job, cron, TimeZoneInfo.Local, EnqueuedState.DefaultQueue);
        }

        private static string GetRecurringJobId(Job job)
        {
            return $"{job.Type.ToString()}.{job.Method.Name}";
        }


        private static LambdaExpression BuildExpression(Type classType, MethodInfo methodInfo)
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
