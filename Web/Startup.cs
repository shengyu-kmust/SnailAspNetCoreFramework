using ApplicationCore.Entities;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snail.Core.Default;
using Snail.Web;
using System;
using System.Linq;
using Web.AutoMapperProfiles;

namespace Web
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        public IConfiguration Configuration { get; }
        public ILifetimeScope AutofacContainer { get; private set; }

        /// <summary>
        /// * 这个方法用于依赖注入，会由系统自动调用，并在ConfigureContainer方法前调用
        /// * 约定，由于用了autofac，所有的依赖注入优先用autofac（即在ConfigureContainer里进行注册），这个方法里只注册非自己写的服务，如授权，MVC，signalr等，自己写的service类，统一用autofac注册
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigSnailWebServices(Configuration, _environment);
            services.ConfigSnailWebDbAndPermission<AppDbContext,User, Role, UserRole, Resource, RoleResource > (Configuration);
            services.ConfigAutoMapper();//AddAutoMapper只能用一次，否则后面的会不走作用

            #region 增加enum转keyValue功能
            services.AddEnumKeyValueService(option =>
            {
                option.Assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            });
            #endregion


        }

        /// <summary>
        /// * 用autofac进行注册
        /// * 此方法在ConfigureServices后被调用，并会覆盖之前已经注册的服务
        /// * 此方法不要build contrainer，autofac会自动build
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // 所有的autofac注册通过module方式，请写在AutoFacModule里
            builder.RegisterAssemblyModules(typeof(Startup).Assembly);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Configure is where you add middleware. This is called after
        // ConfigureContainer. You can use IApplicationBuilder.ApplicationServices
        // here if you need to resolve things from the container.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            app.ConfigSnailWebApplicationBuilder(env, serviceProvider, Configuration);
            using (var scope = AutofacContainer.BeginLifetimeScope())
            {
                //下面两种方法用一种即可 // todo 下面配置成可切换
                //scope.Resolve<AppDbContext>().Database.Migrate();//自动migrate，前提是程序集里有add-migration
                scope.Resolve<AppDbContext>().Database.EnsureCreated();//创建数据库
            }

            BackgroundJob.Enqueue<RunWhenServerStartService>(a => a.Invoke());//启动完成后即执行//这句会出错，可能是hangfire数据库的版本不对，先注释
        }
    }
}
