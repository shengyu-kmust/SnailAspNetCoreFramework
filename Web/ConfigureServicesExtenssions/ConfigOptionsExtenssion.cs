using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ConfigureServicesExtenssions
{
    /// <summary>
    /// 配置options
    /// </summary>
    public static class ConfigOptionsExtenssion
    {
        public static IServiceCollection ConfigAllOption(this IServiceCollection services,IConfiguration configuration)
        {
            return services;
        }
    }
}
