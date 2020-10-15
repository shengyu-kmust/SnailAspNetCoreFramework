using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snail.Web.Interceptor;
using Web.Controllers;

namespace Web.ConfigureServicesExtenssions
{
    /// <summary>
    /// 配置options
    /// </summary>
    public static class ConfigOptionsExtenssion
    {
        public static IServiceCollection ConfigAllOption(this IServiceCollection services,IConfiguration configuration)
        {
            services.Configure<StaticFileUploadOption>(configuration.GetSection("StaticFileUploadOption"));
            services.Configure<LogInterceptorOption>(configuration.GetSection("LogInterceptorOption"));
            return services;
        }
    }
}
