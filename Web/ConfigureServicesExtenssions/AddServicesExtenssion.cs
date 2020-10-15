using AutoMapper;
using DotNetCore.CAP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Snail.Core.Default;
using Snail.Core.Interface;
using Snail.FileStore;
using Snail.Office;
using Snail.Web;

namespace Web.ConfigureServicesExtenssions
{
    public static class AddServicesExtenssion
    {
        public static IServiceCollection AddAllServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddMemoryCache();
            services.TryAddScoped<IApplicationContext, ApplicationContext>();
            services.TryAddScoped<IEntityCacheManager, EntityCacheManager>();
            services.AddScoped<ICapSubscribe, EntityCacheManager>();//将EntityCacheManager注册为ICapSubscribe,使SnailCapConsumerServiceSelector能注册监听方法
            services.AddHttpContextAccessor();//注册，IHttpContextAccessor，在任何地方可以通过此对象获取httpcontext，从而获取单前用户
            services.AddAutoMapper(typeof(Startup));//AddAutoMapper只能用一次，否则后面的会不走作用
            services.AddApplicationLicensing(configuration.GetSection("ApplicationlicensingOption"));
            services.AddResponseCaching();
            services.AddTransient<Snail.Office.IExcelHelper,ExcelNPOIHelper>();

            #region 增加文件存储

            // 本地存储
            services.AddScoped<IFileProvider, DictoryFileProvider>();
            services.AddOptions<DictoryFileProviderOption>().Configure(option => { option.BasePath = "./staticFile"; option.MaxLength = 60000; });
            services.AddScoped<IFileStore, EFFileStore>();


            // 数据库存储
            //services.AddScoped<IFileProvider, DatabaseFileProvider>();
            //services.AddScoped<IFileStore, EFFileStore>();


            // mongodb存储
            //services.AddScoped<IFileProvider, MongodbFileProvider>();
            //services.AddScoped<IFileStore, EFFileStore>();
            //services.AddOptions<MongodbFileProviderOption>().Configure(option => { option.ConnectString = "mongodb://localhost/admin"; option.DatabaseName = "admin"; });

            #endregion
            return services;
        }
    }
}
