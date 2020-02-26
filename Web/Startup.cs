using ApplicationCore.Abstracts;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using DotNetCore.CAP.Internal;
using Hangfire;
using Hangfire.SqlServer;
using Infrastructure;
using Infrastructure.Services;
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
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using NSwag;
using Savorboard.CAP.InMemoryMessageQueue;
using Snail.Common;
using Snail.Core;
using Snail.Core.Interface;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Web.Controllers.Example;
using Web.Hubs;
using Web.Interceptor;
using Web.Security;
using Web.Services;

namespace Web
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment =environment;
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

            #region option配置
            // 示例如下 
            //services.AddOptions<Student>("optionBuilderStudent").Configure(a =>
            //{
            //    a.Id = 100;
            //    a.Name = "optionBuilderStudent name";
            //});
            //services.Configure<Student>("configBuilderStudent", a => { a.Name = "configBuilderStudent"; a.Id = 101; });
            //services.Configure<Student>(Configuration.GetSection("studentData"));
            #endregion


            #region 数据库配置
            services.AddDbContext<AppDbContext>(optionsAction =>
            {
                var dbType = Configuration.GetSection("DbSetting")["DbType"];
                var connectString = Configuration.GetSection("DbSetting")["ConnectionString"];
                if (dbType.Equals("MySql", StringComparison.OrdinalIgnoreCase))
                {
                    optionsAction.UseMySql(connectString);
                }
                else
                {
                    optionsAction.UseSqlServer(connectString);

                }
            });
            #endregion

            #region 身份验证

            var authenticationSetting = new AuthenticationSetting();
            Configuration.Bind("authenticationSetting", authenticationSetting);
            services.Configure<AuthenticationSetting>(Configuration.GetSection("AuthenticationSetting"));
            //约定
            //1、身份验证以支持Jwt和cookie两种为主，优先jwt再cookie验证，只用一种验证
            //2、支持第三方openid connect登录，但第三方登录成功后，如果是web应用，则同时登录到cookie验证，如果是webapi应用，需在第三方登录成功后从系统获取jwt做后续的api调用
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(
                    CookieAuthenticationDefaults.AuthenticationScheme, options =>
                    {
                        options.Cookie.Name = "auth";
                        options.AccessDeniedPath = authenticationSetting.AccessDeniedPath;
                        options.LoginPath = authenticationSetting.LoginPath;
                        options.ExpireTimeSpan = authenticationSetting.ExpireTimeSpan!=default? authenticationSetting.ExpireTimeSpan:new TimeSpan(0,1,0);
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
                    SecurityKey key;
                    if (authenticationSetting.IsAsymmetric)
                    {
                        key = new RsaSecurityKey(RSAHelper.GetRSAParametersFromFromPublicPem(authenticationSetting.RsaPublicKey));
                    }
                    else
                    {
                        key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSetting.SymmetricSecurityKey));
                    }
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {

                        NameClaimType = ConstValues.UserId,
                        RoleClaimType = ConstValues.RoleIds,
                        ValidIssuer = ConstValues.Issuer,
                        ValidAudience = ConstValues.Audience,
                        IssuerSigningKey = key,
                        ValidateIssuer = false,
                        ValidateAudience = false
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

            #region MVC
            //3.1模板的mvc
            services.AddControllers(options => {
                options.Filters.Add(new GlobalExceptionFilterAttribute()); //MVC的异常处理，只会处理业务类异常，其它异常用管道拦截
            }).ConfigureApiBehaviorOptions(options => {
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
                configuration.RootPath = "ClientApp/build";
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

            #region swagger
            services.AddOpenApiDocument(conf => {
                conf.Description = "change the description";
                conf.DocumentName = "change the document name";
                conf.GenerateExamples = true;
                conf.Title = "change the title";
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
            #region 依赖注入，asp.net core自带的依赖注入，在此用自带的注入写法，注入到serviceCollection里

            services.AddScoped<ResourceService>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            #endregion
            services.AddMemoryCache();

            #region 定时任务
            services.AddHangfire(configuration => configuration
           .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
           .UseSimpleAssemblyNameTypeSerializer()
           .UseRecommendedSerializerSettings()
           .UseSqlServerStorage(Configuration.GetValue<string>("DbSetting:ConnectionString"), new SqlServerStorageOptions
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

      
            #endregion

            //开发环境时，打开分析工具
            if (_environment.IsDevelopment())
            {
                //访问示例：http://localhost:5000/profiler/results
                // 参考：https://miniprofiler.com/dotnet/HowTo/ProfileEFCore
                services.AddMiniProfiler(options => { options.RouteBasePath = "/profiler"; }).AddEntityFramework();
            }

            services.AddHttpContextAccessor();//注册，IHttpContextAccessor，在任何地方可以通过此对象获取httpcontext，从而获取单前用户
            
            // automapper
            services.AddAutoMapper(typeof(Startup).Assembly);

            #region 增加cap
            services.TryAddSingleton<IConsumerServiceSelector, SnailCapConsumerServiceSelector>();//默认的ConsumerServiceSelector实现不支持和autofac的完美结合，默认的实现的用法，是需用microsoft di进行服务注册后再调用service.AddCap。但用autofac后，所有的服务注册是在autofac里，即在下面的ConfigureContainer里，为了让cap知道事件和事件的处理方法，重写IConsumerServiceSelector的实现，SnailCapConsumerServiceSelector
            services.AddCap(option =>
            {
                option.UseInMemoryStorage();//用内存消息存储和队列 
                option.UseInMemoryMessageQueue();
                option.UseDashboard();//启用dashboard，默认路径为xxx/cap
            });
            #endregion

            services.AddApplicationLicensing(Configuration.GetSection("ApplicationlicensingOption"));



        }

        /// <summary>
        /// * 用autofac进行注册
        /// * 此方法在ConfigureServices后被调用，并会覆盖之前已经注册的服务
        /// * 此方法不要build contrainer，autofac会自动build
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //下面写autofac的组件注入，推荐用module的方式注册，不要全写在这里。
 
            //BackgroundJob.Enqueue<HangfireService>(a => a.Init());//初始化创建所有定时任务// .net 3.1后不能这么用  // todo
            //GlobalConfiguration.Configuration.UseAutofacActivator(builder.Build());//参考 https://github.com/HangfireIO/Hangfire.Autofac


            //注册所有的service
            builder.RegisterAssemblyModules(typeof(IService).Assembly, typeof(AppDbContext).Assembly, typeof(Startup).Assembly);
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
        // Configure is where you add middleware. This is called after
        // ConfigureContainer. You can use IApplicationBuilder.ApplicationServices
        // here if you need to resolve things from the container.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            // If, for some reason, you need a reference to the built container, you
            // can use the convenience extension method GetAutofacRoot.
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
                

                //HTTP严格传输安全 让网站可以通知浏览器它不应该再使用HTTP加载该网站，而是自动转换该网站的所有的HTTP链接至更安全的HTTPS。它包含在HTTP的协议头 Strict-Transport-Security 中，在服务器返回资源时带上,换句话说，它告诉浏览器将URL协议从HTTP更改为HTTPS（会更安全），并要求浏览器对每个请求执行此操作。
                //正式环境官方建议用UseHsts和UseHttpsRedirection，
                // 如果反方代理服务器，如ngix已经有配置过http重定向https或是设置hsts，则不需要设置这两句
                //参考: https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.1&tabs=visual-studio
                app.UseHsts();
            }




            app.UseHttpsRedirection();//将所有的http重定向https

            //静态文件
            app.UseStaticFiles();
            //spa前端静态文件
            app.UseSpaStaticFiles();

            app.UseAuthentication();

            // hangfire前端界面的访问控制
            app.UseHangfireDashboard(options: new DashboardOptions
            {
                //Authorization = new[] { new HangfireDashboardAuthorizationFilter() }
            });
            app.UseApplicationLicensing();

            #region 3.1模板 的mvc
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<TestHub>("/chat");
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
                spa.Options.SourcePath = "ClientApp";
                //下面是vs模板对spa应用的默认配置，推荐关闭，改用 webpack-dev-server + api proxy 来提高开发速度
                //if (env.IsDevelopment())
                //{
                //    spa.UseReactDevelopmentServer(npmScript: "start");
                //}
            });
            HangfireHelper.AddHangfire(new Assembly[] { typeof(Startup).Assembly });

        }
    }
}
