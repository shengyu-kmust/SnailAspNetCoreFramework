using ApplicationCore.Abstracts;
using Autofac.Extras.DynamicProxy;
using DotNetCore.CAP;
using System;
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
}
