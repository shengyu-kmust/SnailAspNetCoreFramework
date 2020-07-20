using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Service;
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
        private ILogger _logger;
        public RunWhenServerStartService(IEntityCacheManager entityCacheManager, IServiceProvider serviceProvider,ILogger<RunWhenServerStartService> logger)
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
            // 增加定时任务
            HangfireHelper.AddHangfire(new Assembly[] { typeof(Startup).Assembly,typeof(ServiceContext).Assembly });

            // 初始化数据库
            InitDatabase();
        }

        private void InitDatabase()
        {
            try
            {
                _serviceProvider.GetService<InitDatabaseService>().Invoke();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化数据库出错");
            }
        }
    }
}
