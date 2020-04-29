using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Web
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("应用正在启动...");
                CreateHostBuilder(args).Build().Run();

            }
            catch (Exception ex)
            {
                logger.Error(ex, "应用启动失败，已经停止。");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <remarks>
        /// * autofac对.net core 3.1的集成和2.2是有区别的，用本项目里的autofac配置方法，参考https://autofaccn.readthedocs.io/zh/latest/integration/aspnetcore.html#asp-net-core-3-0-and-generic-hosting
        /// 
        /// </remarks>

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).UseWindowsService()
            //// ASP.NET Core 3.0+:
            // The UseServiceProviderFactory call attaches the
            // Autofac provider to the generic hosting mechanism.
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            }).ConfigureAppConfiguration(config => {
                // 下面配置IConfiguration的来源，IConfiguration其实就是从一个Dictionary<string,string>里取值
                // 由于从数据库里取配置时，不好对数据库变更后要更新Config，所以约定如果配置是从数据库里取的，统一从数据库里取
                // 约定不会变更的配置从json文件里配置，否则从数据库里配置
                //config.SetBasePath(Directory.GetCurrentDirectory());
                //config.AddInMemoryCollection(new List<KeyValuePair<string,string>> { });
                //config.AddJsonFile(
                //    "json_array.json", optional: false, reloadOnChange: false);
                //config.AddJsonFile(
                //    "starship.json", optional: false, reloadOnChange: false);
                //config.AddXmlFile(
                //    "tvshow.xml", optional: false, reloadOnChange: false);
                //config.AddEFConfiguration(
                //    options => options.UseInMemoryDatabase("InMemoryDb"));
                //config.AddCommandLine(args);
            }).ConfigureLogging((hostBuilderContext,logging) =>
                {
                    logging.ClearProviders();
                    if (hostBuilderContext.HostingEnvironment.EnvironmentName.Contains("develop",StringComparison.OrdinalIgnoreCase))
                    {
                        //分环境不同来配置log
                    }
                    logging.AddDebug();
                    logging.AddConsole();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
        .UseNLog();  // NLog: setup NLog for Dependency injection;
    }
}

