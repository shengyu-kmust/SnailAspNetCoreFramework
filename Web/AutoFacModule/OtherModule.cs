using ApplicationCore.Abstracts;
using Autofac;
using Infrastructure;
using Infrastructure.Services;
using Snail.Core;
using Snail.Core.Interface;
using Web.Interceptor;
using Module=Autofac.Module;

namespace Web.AutoFacModule
{
    /// <summary>
    /// 非service的其它类的注册
    /// </summary>
    public class OtherModule: Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            //所有的非IService的注册，并启动用属性注册
            builder.RegisterAssemblyTypes(typeof(IService).Assembly, typeof(AppDbContext).Assembly, typeof(Startup).Assembly).Where(a => !typeof(IService).IsAssignableFrom(a)).AsSelf().AsImplementedInterfaces().PropertiesAutowired();

            //
            builder.RegisterGeneric(typeof(DefaultCRUDService<,,>)).As(typeof(ICRUDService<,,>)).InstancePerLifetimeScope();

            builder.RegisterType<LogInterceptor>();//日志拦截器

        }
    }
}
