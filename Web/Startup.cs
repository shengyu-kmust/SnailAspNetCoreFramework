using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using EasyCaching.InMemory;
using Hangfire;
using Hangfire.SqlServer;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Web.Controllers.Example;
using Web.Domain;
using Web.Interceptor;
using Web.Security;
using Web.Services;

namespace Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            #region option配置
            services.AddOptions<Student>("optionBuilderStudent").Configure(a =>
            {
                a.Id = 100;
                a.Name = "optionBuilderStudent name";
            });
            services.Configure<Student>("configBuilderStudent", a => { a.Name = "configBuilderStudent"; a.Id = 101; });
            services.Configure<Student>(Configuration.GetSection("studentData"));
            #endregion


            #region 数据库配置
            services.AddDbContext<DatabaseContext>(optionsAction =>
            {
                optionsAction.UseSqlServer(Configuration.GetConnectionString("DatabaseConnectString"));
            });
            #endregion

            services.AddControllers(options => { options.Filters.Add(new GlobalExceptionFilterAttribute()); });//3.1模板的mvc

            #region 前端界面配置
            // In production, the front end files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
            #endregion
            #region 身份验证
            //约定
            //1、身份验证以支持Jwt和cookie两种为主，先jwt再cookie验证
            //2、支持第三方openid connect登录，但第三方登录成功后，如果是web应用，则同时登录到cookie验证，如果是webapi应用，需在第三方登录成功后从系统获取jwt做后续的api调用
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(
                    CookieAuthenticationDefaults.AuthenticationScheme, options =>
                    {
                        options.AccessDeniedPath = new PathString("/test/AccessDeniedPath");
                        options.LoginPath = new PathString("/test/LoginPath");
                        options.ForwardDefaultSelector = context =>
                        {
                            string authorization = context.Request.Headers["Authorization"];
                            //身份验证的顺序为jwt、cookie
                            if (authorization != null && authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                            {
                                return JwtBearerDefaults.AuthenticationScheme;
                            }
                            else
                            {
                                return CookieAuthenticationDefaults.AuthenticationScheme;
                            }
                        };
                    })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        NameClaimType = ConstValues.NameClaimType,
                        RoleClaimType = ConstValues.RoleClaimType,
                        ValidIssuer = ConstValues.Issuer,
                        ValidAudience = ConstValues.Audience,
                        IssuerSigningKey = ConstValues.IssuerSigningKey
                    };
                })
                .AddOAuth("GitHub", "Github", o =>
                {
                    o.ClientId = "533b5323bfd679470724";
                    o.ClientSecret = "b515a4754fd0597105191cee6003b691adbfa09d";
                    o.CallbackPath = new PathString("/signin-github");
                    o.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
                    o.TokenEndpoint = "https://github.com/login/oauth/access_token";
                    o.UserInformationEndpoint = "https://api.github.com/user";
                    o.ClaimsIssuer = "OAuth2-Github";
                    o.SaveTokens = true;
                    // Retrieving user information is unique to each provider.
                    o.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                    o.ClaimActions.MapJsonKey(ClaimTypes.Name, "login");
                    o.ClaimActions.MapJsonKey("urn:github:name", "name");
                    o.ClaimActions.MapJsonKey(ClaimTypes.Email, "email", ClaimValueTypes.Email);
                    o.ClaimActions.MapJsonKey("urn:github:url", "url");
                    o.Events = new OAuthEvents
                    {
                        OnRemoteFailure = HandleOnRemoteFailure,
                        OnCreatingTicket = async context =>
                        {
                            // Get the GitHub user
                            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                            response.EnsureSuccessStatusCode();

                            var user = JObject.Parse(await response.Content.ReadAsStringAsync());

                            //context.RunClaimActions(user);
                        }
                    };
                });
            #endregion

            #region 权限控制
            //权限控制只要在配置IServiceCollection，不需要额外配置app管道
            //权限控制参考：https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-2.2
            //handler和requirement有几种关系：1 handler对多requirement(此时handler实现IAuthorizationHandler)；1对1（实现AuthorizationHandler<PermissionRequirement>），和多对1
            //所有的handler都要注入到services，用services.AddSingleton<IAuthorizationHandler, xxxHandler>()，而哪个requirement用哪个handler，低层会自动匹配。最后将requirement对到policy里即可
            services.AddAuthorization(options =>
            {
                options.AddPolicy(ConstValues.PermissionPolicy, policy =>
                {
                    policy.Requirements.Add(new PermissionRequirement());
                });
            });
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            #endregion

            #region 依赖注入
            #region 注入swagger
            services.AddSwaggerDocument();
            #endregion
            #region 注入easyCaching
            services.AddEasyCaching(option =>
            {
                //配置方式一：用config配置
                option.UseInMemory(Configuration, "default", "easycaching:inmemory");

                //配置方式一：用代码的方式配置
                option.UseInMemory(config =>
                {
                    config.DBConfig = new InMemoryCachingOptions
                    {
                        // scan time, default value is 60s
                        ExpirationScanFrequency = 60,
                        // total count of cache items, default value is 10000
                        SizeLimit = 100
                    };
                    // the max random second will be added to cache's expiration, default value is 120
                    config.MaxRdSecond = 0;
                    // whether enable logging, default is false
                    config.EnableLogging = false;
                    // mutex key's alive time(ms), default is 5000
                    config.LockMs = 5000;
                    // when mutex key alive, it will sleep some time, default is 300
                    config.SleepMs = 300;
                }, "default");

            });
            #endregion
            #region 注入mediatr
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            #endregion
            #region asp.net core自带的依赖注入，在此用自带的注入写法，注入到serviceCollection里
            services.AddSingleton<PermissionModel>();
            services.AddScoped<ResourceService>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            #endregion

            #region 注入整表缓存
            //services.AddEntityCaching();
            #endregion

            #region 集成autofac
            //参考 https://autofaccn.readthedocs.io/zh/latest/integration/aspnetcore.html
            var builder = new ContainerBuilder();
            builder.Populate(services);//将asp.net core 自带的依赖注入，已经注册的组件，注册到autofac里
            #region autofac组件注入
            //下面写autofac的组件注入
            //用assembly scan的方式批量注入
            var assembly = Assembly.GetExecutingAssembly();
            //builder.RegisterAssemblyTypes(assembly).Where(a => a.Name.EndsWith("Service")).AsImplementedInterfaces().AsSelf().PropertiesAutowired().EnableInterfaceInterceptors();
            //builder.RegisterAssemblyTypes(assembly).Where(a => a.Name.EndsWith("Interceptor"));// interceptor type registration

            builder.RegisterType<AopService>().As<IAopService>().EnableInterfaceInterceptors();
            builder.RegisterType<Aop2Service>().EnableClassInterceptors();
            builder.RegisterType<LogInterceptor>();
            builder.RegisterGeneric(typeof(EntityCaching<,>)).As(typeof(IEntityCaching<,>)).SingleInstance();

            #endregion
            //返回serviceProvider。此方法的默认是不返回的，和autofac集成后，而修改成返回IServiceProvider对象
            this.ApplicationContainer = builder.Build();
      
            #endregion


            #endregion

            #region 定时任务
            services.AddHangfire(configuration => configuration
           .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
           .UseSimpleAssemblyNameTypeSerializer()
           .UseRecommendedSerializerSettings()
           .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
           {
               //也可以换成mysql
               CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
               SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
               QueuePollInterval = TimeSpan.Zero,
               UseRecommendedIsolationLevel = true,
               UsePageLocksOnDequeue = true,
               DisableGlobalLocks = true
           }));
            services.AddHangfireServer();

            GlobalConfiguration.Configuration.UseAutofacActivator(builder.Build());//参考 https://github.com/HangfireIO/Hangfire.Autofac

            BackgroundJob.Enqueue<HangfireService>(a => a.Init());//初始化创建所有定时任务

            #endregion
            return new AutofacServiceProvider(this.ApplicationContainer);
        }
        private async Task HandleOnRemoteFailure(RemoteFailureContext context)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync("<html><body>");
            await context.Response.WriteAsync("A remote failure has occurred: " + UrlEncoder.Default.Encode(context.Failure.Message) + "<br>");

            if (context.Properties != null)
            {
                await context.Response.WriteAsync("Properties:<br>");
                foreach (var pair in context.Properties.Items)
                {
                    await context.Response.WriteAsync($"-{ UrlEncoder.Default.Encode(pair.Key)}={ UrlEncoder.Default.Encode(pair.Value)}<br>");
                }
            }

            await context.Response.WriteAsync("<a href=\"/\">Home</a>");
            await context.Response.WriteAsync("</body></html>");

            // context.Response.Redirect("/error?FailureMessage=" + UrlEncoder.Default.Encode(context.Failure.Message));

            context.HandleResponse();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //开发模式用异常处理程序页
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(errorApp =>
                {

                    errorApp.Run(async context =>
                    {
                        var exceptionHandlerPathFeature =
                            context.Features.Get<IExceptionHandlerPathFeature>();
                        //业务异常
                        if (exceptionHandlerPathFeature?.Error is BusinessException businessException)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            await context.Response.WriteAsync(businessException.Message);
                        }
                        else
                        {

                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            await context.Response.WriteAsync($"服务器异常，异常时间{DateTime.Now}");

                        }
                    });
                });
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            //静态文件
            app.UseStaticFiles();
            //spa前端静态文件
            app.UseSpaStaticFiles();

            // hangfire前端界面的访问控制
            app.UseHangfireDashboard(options: new DashboardOptions
            {
                Authorization = new[] { new HangfireDashboardAuthorizationFilter() }
            });

            app.UseSwagger();
            app.UseSwaggerUi3();
            app.UseMvc();
            #region 3.1模板 的mvc
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #endregion


            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
                //下面是vs模板对spa应用的默认配置，推荐关闭，改用 webpack-dev-server + api proxy 来提高开发速度
                //if (env.IsDevelopment())
                //{
                //    spa.UseReactDevelopmentServer(npmScript: "start");
                //}
            });
        }
    }
}
