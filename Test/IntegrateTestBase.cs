using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System;
using Web;

namespace Test
{
    /// <summary>
    /// 集成测试的基类
    /// </summary>
    public class IntegrateTestBase
    {
        protected IServiceProvider serviceProvider;
        public IConfiguration Configuration { get; set; }
        public IntegrateTestBase()
        {
            serviceProvider = GetServiceProvider();
        }
        public IServiceProvider GetServiceProvider()
        {
            var env = new TestWebHostEnviroment();
            env.EnvironmentName = "development";
            var services = new ServiceCollection();
            var configBuilder = new ConfigurationBuilder();
            JsonConfigurationExtensions.AddJsonFile(configBuilder, "appsettings.json", true, true);
            Configuration = configBuilder.Build();
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                if (env.EnvironmentName.Contains("develop", StringComparison.OrdinalIgnoreCase))
                {
                    //分环境不同来配置log
                }
                builder.AddDebug();
                builder.AddConsole();
                builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            });
            var startUp = new Startup(Configuration, env);
            startUp.ConfigureServices(services);
            var autofacServiceProviderFactory = new AutofacServiceProviderFactory(startUp.ConfigureContainer);
            var containerBuilder= autofacServiceProviderFactory.CreateBuilder(services);
            return autofacServiceProviderFactory.CreateServiceProvider(containerBuilder);
        }


    }

    public class TestWebHostEnviroment : IWebHostEnvironment
    {
        public IFileProvider WebRootFileProvider { get;set; }
        public string WebRootPath { get;set; }
        public string EnvironmentName { get;set; }
        public string ApplicationName { get;set; }
        public string ContentRootPath { get;set; }
        public IFileProvider ContentRootFileProvider { get;set; }
    }
}
