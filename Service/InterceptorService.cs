using ApplicationCore.Const;
using Autofac.Extras.DynamicProxy;
using DotNetCore.CAP;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Service
{
    [Intercept(typeof(LogInterceptor))]
    [Intercept(typeof(CacheInterceptor))]
    public class InterceptorService : ServiceContextBaseService, IInterceptorService,ICapSubscribe
    {
        public InterceptorService(ServiceContext serviceContext) : base(serviceContext)
        {
        }

        public virtual void SyncVoid(string input)
        {
            //虽然启用了virtual，但不会有缓存
        }

        [CacheInterceptor(1)]
        [LogInerceptor(true,true,true)]
        public virtual string SyncReturn(string input)
        {
            return DateTime.Now.ToString();
        }

        public virtual async Task AsyncVoid(string input)
        {

        }
        [CacheInterceptor(1)]
        public virtual async Task<string> AsyncReturn(string input)
        {
            using (var client = new HttpClient())
            {
                var res = await client.GetAsync("http://www.baidu.com");
                var resS = await res.Content.ReadAsStringAsync();
                return $"{DateTime.Now}{resS}";
            }
        }

        [CacheInterceptor(1)]
        [LogInerceptor(true, true, true)]
        [CapSubscribe(EventNameConst.TestEventName)]
        public virtual string ReceiveEvent(string eventVal)
        {
            return eventVal;
        }

        public void EmitEvent()
        {
            publisher.Publish(EventNameConst.TestEventName, DateTime.Now.ToShortDateString());
        }
    }

    
    public interface IInterceptorService
    {
        [CacheInterceptor(2)]
        Task<string> AsyncReturn(string input);
        Task AsyncVoid(string input);
        void EmitEvent();
        [CacheInterceptor(1), CapSubscribe("TestEventName"), LogInerceptor(true, true, true)]
        string ReceiveEvent(string eventVal);
        [CacheInterceptor(2)]
        string SyncReturn(string input);
        void SyncVoid(string input);
    }
}
