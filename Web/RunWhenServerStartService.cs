using Hangfire;
using Snail.Core.Interface;
using System;
using System.Reflection;

namespace Web
{
    /// <summary>
    /// 服务启动后，会运行
    /// </summary>
    public class RunWhenServerStartService
    {
        private IEntityCacheManager _entityCacheManager;
        private IServiceProvider _serviceProvider;
        public RunWhenServerStartService(IEntityCacheManager entityCacheManager, IServiceProvider serviceProvider)
        {
            _entityCacheManager = entityCacheManager;
            _serviceProvider = serviceProvider;
        }


        public void Invoke()
        {
            HangfireHelper.AddHangfire(new Assembly[] { typeof(Startup).Assembly });
        }
    }
}
