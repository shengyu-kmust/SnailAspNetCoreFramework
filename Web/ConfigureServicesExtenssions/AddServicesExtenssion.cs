using AutoMapper;
using DotNetCore.CAP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Snail.Core.Default;
using Snail.Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            services.AddAutoMapper(typeof(Startup));
            services.AddApplicationLicensing(configuration.GetSection("ApplicationlicensingOption"));
            services.AddResponseCaching();

            return services;
        }
    }
}
