using ApplicationCore.Abstracts;
using Autofac.Extras.DynamicProxy;
using DotNetCore.CAP;
using Hangfire;
using System;
using System.Diagnostics;
using Web.Interceptor;

namespace Web.Services
{
    [Intercept(typeof(LogInterceptor))]
    public class TestService: ITestService, ICapSubscribe
    {
        [CapSubscribe("service_event_test")]
        public virtual void recieveMsg(string msg)
        {
            Console.WriteLine($"service_event_test:{msg}");
        }
    }

    public interface ITestService:IService
    {
        void recieveMsg(string msg);
    }

    public class HangFireJobRegisterTestClass
    {
        [BackgroundJob(JobType =EBackgroundJobType.Enqueue)]
        public void Enqueue()
        {
            Debug.WriteLine("Enqueue");
        }
        [BackgroundJob(JobType = EBackgroundJobType.Schedule, DelayTimeSpan = "00:00:10")]
        public void Schedule()
        {
            Debug.WriteLine("Schedule");

        }
        [BackgroundJob(JobType =EBackgroundJobType.Recurring,Cron ="*/1 * * * *")]
        public void Recrue()
        {
            Debug.WriteLine("Recrue");
        }
    }


}
