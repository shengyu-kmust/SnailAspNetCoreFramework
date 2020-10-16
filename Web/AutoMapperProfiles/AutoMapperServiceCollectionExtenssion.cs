using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Reflection;

namespace Web.AutoMapperProfiles
{
    public static class AutoMapperServiceCollectionExtenssion
    {
        public static void ConfigAutoMapper(this IServiceCollection services)
        {
            var assemblies = new List<Assembly>
            {
                Assembly.Load("Snail.Web"),
                Assembly.Load("Web")
            };
            services.AddAutoMapper(assemblies);
        }
    }
}
