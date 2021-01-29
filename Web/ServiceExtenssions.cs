using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snail.Core.Permission;
using System.Collections.Generic;
using System.Reflection;

namespace Web
{
    /// <summary>
    /// config和Option的奴化会注册
    /// </summary>
    public static class ServiceExtenssions
    {
        public static IServiceCollection ConfigAndOptionSetting(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }


    }
}
