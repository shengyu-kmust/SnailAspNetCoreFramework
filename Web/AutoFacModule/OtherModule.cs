﻿using Autofac;
using Infrastructure;
using Service;
using Snail.Core.Default.Service;
using Snail.Core.Service;
using Snail.Web;
using System;
using System.Collections.Generic;
using Module = Autofac.Module;

namespace Web.AutoFacModule
{
    /// <summary>
    /// 非service的其它类的注册
    /// </summary>
    public class OtherModule: Module
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            var exceptTypes = new List<Type>
            {
            };

            //所有的非IService的注册，并启动用属性注册
            builder.RegisterAssemblyTypes(
                typeof(Startup).Assembly,
                typeof(InitDatabaseService).Assembly, 
                typeof(AppDbContext).Assembly,
                typeof(SnailWebConfigureServicesExtenssions).Assembly,
                typeof(ServiceContext).Assembly
                )
                .Where(a => !typeof(IService).IsAssignableFrom(a) && !exceptTypes.Contains(a))
                .AsSelf()
                .AsImplementedInterfaces()
                .PropertiesAutowired();

        }
    }
}
