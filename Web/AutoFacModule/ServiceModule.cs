using Autofac;
using Autofac.Extras.DynamicProxy;
using Snail.Web.IServices;
using System.Collections.Generic;
using System.Reflection;
using Module = Autofac.Module;
namespace Web.AutoFacModule
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //所有的IService的注册，并启动用属性注册
            var assemblies = new List<Assembly>
            {
                Assembly.Load("ApplicationCore"),
                Assembly.Load("Service"),
                Assembly.Load("Infrastructure"),
                Assembly.Load("Web"),
                Assembly.Load("Snail.Web")
            };
            builder.RegisterAssemblyTypes(assemblies.ToArray()).Where(a => typeof(IService).IsAssignableFrom(a)).AsSelf().AsImplementedInterfaces().PropertiesAutowired().EnableClassInterceptors();
        }
    }
}
