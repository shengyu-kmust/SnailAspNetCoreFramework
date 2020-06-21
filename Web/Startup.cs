using ApplicationCore.Entity;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using DotNetCore.CAP;
using DotNetCore.CAP.Internal;
using Hangfire;
using Hangfire.MySql;
using Hangfire.SqlServer;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NSwag;
using Savorboard.CAP.InMemoryMessageQueue;
using Service;
using Snail.Core;
using Snail.Core.Default;
using Snail.Core.Dto;
using Snail.Core.Interface;
using Snail.Core.Permission;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using Web.ConfigureServicesExtenssions;
using Web.Controllers;
using Web.Filter;
using Web.Hubs;
using Web.Permission;

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
            var dbType = Configuration.GetSection("DbSetting")["DbType"];
            var connectString = Configuration.GetSection("DbSetting")["ConnectionString"];
            var hangfireConnectString = Configuration.GetSection("DbSetting")["Hangfire"];

            #region option配置
            services.ConfigAllOption(Configuration);
            #endregion

            #region 数据库配置
            Action<DbContextOptionsBuilder> optionsAction = options =>
            {
                if (dbType.Equals("MySql", StringComparison.OrdinalIgnoreCase))
                {
                    options.UseMySql(connectString);
                }
                else
                {
                    options.UseSqlServer(connectString);
                }
            };
            services.AddDbContext<DbContext, AppDbContext>(optionsAction);
            services.AddDbContext<AppDbContext>(optionsAction);
            #endregion

            #region 增加通用权限
            services.AddPermission(options =>
            {
                Configuration.GetSection("PermissionOptions").Bind(options);
                options.ResourceAssemblies = new List<Assembly> { Assembly.GetExecutingAssembly() };
            });
            #endregion

            #region MVC
            //3.1模板的mvc
            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilterAttribute>();
                options.Filters.Add<GlobalResultFilterAttribute>();
                options.EnableEndpointRouting = false;
            }).AddNewtonsoftJson(options =>
            {
                //.net core 3.0后，json用了system.text.json，但功能比较单一，可以换成用newtonsoftJson,参考  https://docs.microsoft.com/zh-cn/aspnet/core/web-api/advanced/formatting?view=aspnetcore-3.0
                options.SerializerSettings.Converters.Add(new StringEnumConverter());//配置mvc的action返回格式化对enum类型的处理方式，将enum转成string返回
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    // 配置模型参数校验的返回结果，默认只要contoller上加上ApiController后，会以默认的400状态和错误结构返回，这里进行处理，统一返回BusinessException，并在ExceptionFilter这一层进行拦截处理
                    // 参考:https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-3.1#automatic-http-400-responses
                    var errorContext = string.Join(",", actionContext.ModelState.Values
                        .SelectMany(a => a.Errors.Select(b => b.ErrorMessage))
                        .ToArray());
                    throw new BusinessException($"参数验证失败：{errorContext}");
                };
            });

            #endregion

            #region signalr
            services.AddSignalR();
            #endregion

            #region 前端界面配置
            // In production, the front end files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
            #endregion


            #region swagger
            services.AddOpenApiDocument(conf =>
            {
                conf.Description = "后台接口文档des";
                conf.DocumentName = "后台接口文档name";
                conf.GenerateExamples = true;
                conf.Title = "后台接口文档title";
                conf.PostProcess = document =>
                {
                    document.SecurityDefinitions.Add(
                          "Jwt认证",
                          new OpenApiSecurityScheme
                          {
                              Type = OpenApiSecuritySchemeType.Http,
                              Name = "Authorization",//token会放到header的authorization里
                              In = OpenApiSecurityApiKeyLocation.Header,
                              Description = "请输入 : JWT token",
                              Scheme = "bearer"//定义bearer，不能改
                          });
                    document.Security.Add(new OpenApiSecurityRequirement { { "Jwt认证", new string[0] } });

                };
            }); // add OpenAPI v3 document
            #endregion

            #region 注入easyCaching
            services.AddEasyCaching(option =>
            {
                //配置方式一：用config配置
                option.UseInMemory(Configuration, "default", "easycaching:inmemory");

                //配置方式二：用代码的方式配置
                //option.UseInMemory(config =>
                //{
                //    config.DBConfig = new InMemoryCachingOptions
                //    {
                //        // scan time, default value is 60s
                //        ExpirationScanFrequency = 60,
                //        // total count of cache items, default value is 10000
                //        SizeLimit = 100
                //    };
                //    // the max random second will be added to cache's expiration, default value is 120
                //    config.MaxRdSecond = 0;
                //    // whether enable logging, default is false
                //    config.EnableLogging = false;
                //    // mutex key's alive time(ms), default is 5000
                //    config.LockMs = 5000;
                //    // when mutex key alive, it will sleep some time, default is 300
                //    config.SleepMs = 300;
                //}, "default");
            });
            #endregion

            #region mediatr
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
            #endregion


            #region 定时任务
            //services.AddHangfireServer();
            //services.AddHangfire(configuration =>
            //{
            //    configuration
            //    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            //    .UseSimpleAssemblyNameTypeSerializer()
            //    .UseRecommendedSerializerSettings();
            //    if (dbType.Equals("MySql", StringComparison.OrdinalIgnoreCase))
            //    {
            //        configuration.UseStorage(new MySqlStorage(hangfireConnectString, new MySqlStorageOptions() { 
            //            TablesPrefix = "hangfire_" 
            //        }));
            //    }
            //    else
            //    {
            //        configuration.UseSqlServerStorage(hangfireConnectString, new SqlServerStorageOptions
            //        {
            //            //也可以换成mysql
            //            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            //            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            //            QueuePollInterval = TimeSpan.Zero,
            //            UseRecommendedIsolationLevel = true,
            //            UsePageLocksOnDequeue = true,
            //            DisableGlobalLocks = true
            //        });
            //    }
            //});
            #endregion

            #region 增加enum转keyValue功能
            services.AddEnumKeyValueService(option =>
            {
                option.Assemblies = new List<Assembly> { typeof(BaseEntity).Assembly, typeof(AppDbContext).Assembly, typeof(ServiceContext).Assembly, typeof(Startup).Assembly };
            });
            #endregion

            #region profiler
            //开发环境时，打开分析工具
            if (_environment.IsDevelopment())
            {
                //访问示例：http://localhost:5000/profiler/results
                //profiler/results-index为列表
                // 参考：https://miniprofiler.com/dotnet/HowTo/ProfileEFCore
                services.AddMiniProfiler(options => { options.RouteBasePath = "/profiler"; }).AddEntityFramework();
            }
            #endregion



            #region 增加cap
            services.TryAddSingleton<IConsumerServiceSelector, SnailCapConsumerServiceSelector>();//默认的ConsumerServiceSelector实现不支持和autofac的完美结合，默认的实现的用法，是需用microsoft di进行服务注册后再调用service.AddCap。但用autofac后，所有的服务注册是在autofac里，即在下面的ConfigureContainer里，为了让cap知道事件和事件的处理方法，重写IConsumerServiceSelector的实现，SnailCapConsumerServiceSelector
            services.AddCap(option =>
            {
                option.UseInMemoryStorage();//用内存消息存储和队列 
                option.UseInMemoryMessageQueue();
                option.UseDashboard();//启用dashboard，默认路径为xxx/cap
            });
            #endregion


            #region 依赖注入，asp.net core自带的依赖注入，在此用自带的注入写法，注入到serviceCollection里

            services.AddAllServices(Configuration);
            #endregion

            #region health check
            //可以用：https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks
            services.AddHealthChecks();
            #endregion

            services.AddCors();

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
            // 获取autofac容器
            this.AutofacContainer = app.ApplicationServices.GetAutofacRoot();


            //开发模式用异常处理程序页
            if (env.IsDevelopment())
            {
                app.UseMiniProfiler();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(errorApp =>
                {

                    errorApp.Run(async context =>
                    {
                        var loggerFactory = (ILoggerFactory)context.RequestServices.GetService(typeof(ILoggerFactory));
                        var logger = loggerFactory.CreateLogger("UnKnowException");
                        var exceptionHandlerPathFeature =
                            context.Features.Get<IExceptionHandlerPathFeature>();
                        //业务异常
                        ApiResultDto responseResultModel;
                        if (exceptionHandlerPathFeature?.Error is BusinessException businessException)
                        {
                            responseResultModel = ApiResultDto.BadRequestResult(businessException.Message);
                            if (logger != null)// todo 这里Logger为null
                            {
                                logger.LogError(exceptionHandlerPathFeature?.Error?.ToString());
                            }
                        }
                        else
                        {
                            responseResultModel = ApiResultDto.BadRequestResult($"程序出错，出于安全考虑，出错信息未能返回，请联系IT进行处理，错误时间{DateTime.Now}");
                            if (logger != null)// todo 这里Logger为null
                            {
                                logger.LogError(exceptionHandlerPathFeature?.Error?.ToString());
                            }
                        }
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int)HttpStatusCode.OK;
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(responseResultModel));
                    });
                });


                //HTTP严格传输安全 让网站可以通知浏览器它不应该再使用HTTP加载该网站，而是自动转换该网站的所有的HTTP链接至更安全的HTTPS。它包含在HTTP的协议头 Strict-Transport-Security 中，在服务器返回资源时带上,换句话说，它告诉浏览器将URL协议从HTTP更改为HTTPS（会更安全），并要求浏览器对每个请求执行此操作。
                //正式环境官方建议用UseHsts和UseHttpsRedirection，
                // 如果反方代理服务器，如ngix已经有配置过http重定向https或是设置hsts，则不需要设置这两句
                //参考: https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.1&tabs=visual-studio
                app.UseHsts();
                app.UseHttpsRedirection();//将所有的http重定向https
            }

            

            //静态文件
            app.UseStaticFiles();
            //spa前端静态文件
            app.UseSpaStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/" + serviceProvider.GetService<IOptions<StaticFileUploadOption>>().Value.StaticFilePath,
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, serviceProvider.GetService<IOptions<StaticFileUploadOption>>().Value.StaticFilePath))
            });

            app.UseCors(builder => { builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin(); });

            app.UseAuthentication();

            // hangfire前端界面的访问控制
            //app.UseHangfireDashboard(options: new Hangfire.DashboardOptions
            //{
            //    //Authorization = new[] { new HangfireDashboardAuthorizationFilter() }
            //});
            app.UseApplicationLicensing();

            #region 3.1模板 的mvc
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapHub<DefaultHub>("/defaultHub");
                endpoints.MapControllers();
            });

            #endregion

            #region swag
            //* 如果出现如下错误：Fetch errorundefined / swagger / v1 / swagger.json
            //* 解决：原因是swagger 的api在解析时出错，在chrome f12看具体请求swagger.json的错误，解决
            app.UseOpenApi(config =>
            {
                config.PostProcess = (document, req) =>
                {
                    //下面是向swag怎加https和http的两种方式
                    document.Schemes.Add(OpenApiSchema.Https);
                    document.Schemes.Add(OpenApiSchema.Http);
                };
            });
            app.UseSwaggerUi3();
            //app.UseReDoc();
            #endregion


            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp/dist";
                //下面是vs模板对spa应用的默认配置，推荐关闭，改用 webpack-dev-server + api proxy 来提高开发速度
                //if (env.IsDevelopment())
                //{
                //    spa.UseReactDevelopmentServer(npmScript: "start");
                //}
            });
            using (var scope = AutofacContainer.BeginLifetimeScope())
            {
                //下面两种方法用一种即可
                //scope.Resolve<AppDbContext>().Database.Migrate();//自动migrate，前提是程序集里有add-migration
                scope.Resolve<AppDbContext>().Database.EnsureCreated();//创建数据库
            }

            //BackgroundJob.Enqueue<RunWhenServerStartService>(a => a.Invoke());//启动完成后即执行//这句会出错，可能是hangfire数据库的版本不对，先注释
        }
    }
}
