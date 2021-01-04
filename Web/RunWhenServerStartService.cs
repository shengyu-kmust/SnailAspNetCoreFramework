using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Service;
using Snail.Core.Default.Service;
using Snail.Core.Interface;
using Snail.Web;
using Snail.Web.Services;
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
        private ILogger _logger;
        private static object _locker = new object();
        public RunWhenServerStartService(IEntityCacheManager entityCacheManager, IServiceProvider serviceProvider, ILogger<RunWhenServerStartService> logger)
        {
            _entityCacheManager = entityCacheManager;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        /// <summary>
        /// 启动后执行的入口
        /// </summary>
        public void Invoke()
        {
            // 当数据库为sqlite时，hangfire会执行多次invoke，暂不清楚是什么原因，故加上锁
            lock (_locker)
            {
                // 增加定时任务
                HangfireHelper.AddHangfire(new Assembly[] { typeof(Startup).Assembly, typeof(ServiceContext).Assembly });

                // 初始化数据库
                InitDatabase();
            }

        }

        private void InitDatabase()
        {
            try
            {
                var initDatabaseService = _serviceProvider.GetService<InitDatabaseService>();
                if (initDatabaseService != null)
                {
                    initDatabaseService.Invoke();
                }
                else
                {
                    _logger.LogError("初始化数据库出错，未注册InitDatabaseService");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化数据库出错，内部异常为：{0}", ex.InnerException);
                throw;
            }
        }
    }
}
