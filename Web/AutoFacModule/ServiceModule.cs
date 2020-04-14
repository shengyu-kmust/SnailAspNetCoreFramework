using ApplicationCore.IServices;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Infrastructure;
using Module = Autofac.Module;
namespace Web.AutoFacModule
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //所有的IService的注册，并启动用属性注册
            builder.RegisterAssemblyTypes(typeof(IService).Assembly, typeof(AppDbContext).Assembly, typeof(Startup).Assembly).Where(a => typeof(IService).IsAssignableFrom(a)).AsSelf().AsImplementedInterfaces().PropertiesAutowired().EnableClassInterceptors();
        }
    }
}
