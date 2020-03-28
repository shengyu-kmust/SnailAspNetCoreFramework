using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Snail.Common;
using Snail.Core.Default;
using Snail.Core.Interface;
using Snail.Core.Permission;
using SnaiWeb.Permission;
using System;
using System.Text;

namespace Web.Permission
{
    public static class PermissionServiceCollectionExtensions
    {

        /// <summary>
        /// 权限控制核心，即必须的配置
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        public static void AddPermission(this IServiceCollection services, Action<PermissionOptions> action)
        {
            services.TryAddScoped<IPermission, DefaultPermission>();
            services.TryAddScoped<IPermissionStore, DefaultPermissionStore>();
            #region MyRegion
            var permissionOption = new PermissionOptions();
            action(permissionOption);
            //addAuthentication不放到AddPermissionCore方法里，是为了外部可自己配置
            // 当未通过authenticate时（如无token或是token出错时），会返回401，当通过了authenticate但没通过authorize时，会返回403。
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(
                   CookieAuthenticationDefaults.AuthenticationScheme, options =>
                   {
                   //下面的委托方法只会在第一次cookie验证时调用，调用时会用到上面的permissionOption变量，但其实permissionOption变量是在以前已经初始化的，所以在此方法调用之前，permissionOption变量不会被释放
                   options.Cookie.Name = "auth";
                       options.AccessDeniedPath = permissionOption.AccessDeniedPath;
                       options.LoginPath = permissionOption.LoginPath;
                       options.ExpireTimeSpan = permissionOption.ExpireTimeSpan != default ? permissionOption.ExpireTimeSpan : new TimeSpan(12, 0, 0);
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
                   if (permissionOption.IsAsymmetric)
                   {
                       key = new RsaSecurityKey(RSAHelper.GetRSAParametersFromFromPublicPem(permissionOption.RsaPublicKey));
                   }
                   else
                   {
                       key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(permissionOption.SymmetricSecurityKey));
                   }
                   options.TokenValidationParameters = new TokenValidationParameters()
                   {

                       NameClaimType = PermissionConstant.userIdClaim,
                       RoleClaimType = PermissionConstant.roleIdsClaim,
                       ValidIssuer = permissionOption.Issuer,
                       ValidAudience = permissionOption.Audience,
                       IssuerSigningKey = key,
                       ValidateIssuer = false,
                       ValidateAudience = false
                   };
               });
            #endregion
            #region 授权

            //权限控制只要在配置IServiceCollection，不需要额外配置app管道
            //权限控制参考：https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-2.2
            //handler和requirement有几种关系：1 handler对多requirement(此时handler实现IAuthorizationHandler)；1对1（实现AuthorizationHandler<PermissionRequirement>），和多对1
            //所有的handler都要注入到services，用services.AddSingleton<IAuthorizationHandler, xxxHandler>()，而哪个requirement用哪个handler，低层会自动匹配。最后将requirement对到policy里即可
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PermissionConstant.PermissionAuthorizePolicy, policyBuilder =>
                {
                    policyBuilder.AddRequirements(new PermissionRequirement());
                });
            });
            services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();
            services.AddMemoryCache();
            services.TryAddScoped<IApplicationContext, ApplicationContext>();
            services.AddHttpContextAccessor();
            services.Configure(action);
            #endregion
        }

    }
}
