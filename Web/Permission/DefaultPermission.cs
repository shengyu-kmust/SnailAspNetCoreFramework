using ApplicationCore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Snail.Common;
using Snail.Core.Attributes;
using Snail.Core.Permission;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace SnaiWeb.Permission
{
    /// <summary>
    /// 权限的默认实现类
    /// </summary>
    public class DefaultPermission : BasePermission
    {
        public static readonly string superAdminRoleName = "SuperAdmin";

        protected override PermissionOptions PermissionOptions { get; set; }

        public DefaultPermission(IPermissionStore permissionStore, IOptionsMonitor<PermissionOptions> permissionOptions) : base(permissionStore)
        {
            PermissionOptions = permissionOptions.CurrentValue ?? new PermissionOptions();
        }

        public override bool HasPermission(string resourceKey, string userKey)
        {
            if (IsSuperAdmin(userKey))
            {
                return true;
            }
            return base.HasPermission(resourceKey, userKey);
        }

        public override string GenerateTokenStr(List<Claim> claims)
        {
            var expireTimeSpan = (PermissionOptions.ExpireTimeSpan == null || PermissionOptions.ExpireTimeSpan == TimeSpan.Zero) ? new TimeSpan(6, 0, 0) : PermissionOptions.ExpireTimeSpan;
            SigningCredentials creds;
            if (PermissionOptions.IsAsymmetric)
            {
                var key = new RsaSecurityKey(RSAHelper.GetRSAParametersFromFromPrivatePem(PermissionOptions.RsaPrivateKey));
                creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);
            }
            else
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(PermissionOptions.SymmetricSecurityKey));
                creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            }
            var token = new JwtSecurityToken(PermissionOptions.Issuer, PermissionOptions.Audience, claims, DateTime.Now, DateTime.Now.Add(expireTimeSpan), creds);
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenStr;
        }
        public override string HashPwd(string pwd)
        {
            return HashHelper.Md5($"{pwd}{PermissionOptions.PasswordSalt}");
        }

        /// <summary>
        /// 获取资源对象的code,已经适配如下类型:AuthorizationFilterContext,ControllerActionDescriptor,methodInfo
        /// 默认为className_methodName，或是resourceAttribute里设置的code
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override string GetRequestResourceCode(object obj)
        {
            if (obj is MethodInfo)
            {
                return GetResourceCode((MethodInfo)obj);
            }
            MethodInfo methodInfo;
            if (obj is AuthorizationFilterContext authorizationFilterContext)
            {
                if (authorizationFilterContext.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                {
                    methodInfo = controllerActionDescriptor.MethodInfo;
                    return GetResourceCode(methodInfo);
                    //resourceCode = GetResourceCode(controllerActionDescriptor.ControllerName, controllerActionDescriptor.ActionName);
                }
            }
            if (obj is ControllerActionDescriptor controllerActionDescriptor1)
            {
                methodInfo = controllerActionDescriptor1.MethodInfo;
                return GetResourceCode(methodInfo);
                //resourceCode = GetResourceCode(controllerActionDescriptor1.ControllerName, controllerActionDescriptor1.ActionName);
            }

            if (obj is RouteEndpoint endpoint)
            {
                //.net core 3.1后，AuthorizationHandlerContext.Resource为endpoint
                methodInfo = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>()?.MethodInfo;
                return GetResourceCode(methodInfo);

            }
            return string.Empty;
        }

        /// <summary>
        /// 初始化所有的权限资源。
        /// 所有有定义ResourceAttribute的方法都为权限资源，否则不是。要使方法受权限控制，必须做到如下两点：1、在方法上加ResourceAttribute，2、在controller或是action上加Authorize
        /// </summary>
        public override void InitResource()
        {
            var resources = new List<Resource>();
            if (PermissionOptions.ResourceAssemblies == null)
            {
                PermissionOptions.ResourceAssemblies = new List<Assembly>();
            }
            var existResources = _permissionStore.GetAllResource();
            PermissionOptions.ResourceAssemblies.Add(this.GetType().Assembly);
            PermissionOptions.ResourceAssemblies?.Distinct().ToList().ForEach(assembly =>
            {
                //对所有的controller类进行扫描
                assembly.GetTypes().Where(type => typeof(ControllerBase).IsAssignableFrom(type)).ToList().ForEach(controller =>
                {
                    var controllerIsAdded = false;//父是否增加
                    var parentId = IdGenerator.Generate<string>();
                    var parentResource = controller.GetCustomAttribute<ResourceAttribute>();
                    controller.GetMethods().ToList().ForEach(method =>
                    {
                        if (method.IsDefined(typeof(ResourceAttribute), true))
                        {
                            var methodResource = method.GetCustomAttribute<ResourceAttribute>();
                            if (!controllerIsAdded)
                            {
                                // 增加父
                                resources.Add(new Resource
                                {
                                    Id = parentId,
                                    Code = parentResource?.ResourceCode??controller.Name,
                                    CreateTime = DateTime.Now,
                                    IsDeleted = false,
                                    Name = parentResource?.Description??controller.Name
                                });
                                controllerIsAdded = true;
                            }
                            // 增加子
                            resources.Add(new Resource
                            {
                                Id = IdGenerator.Generate<string>(),
                                Code = GetResourceCode(method),
                                CreateTime = DateTime.Now,
                                IsDeleted = false,
                                ParentId = parentId,
                                Name = methodResource?.Description??method.Name
                            });
                        }
                    });
                });
            });
            resources.ForEach(item =>
            {
                var temp = new Resource
                {
                    Id = item.Id,
                    Code = item.Code,
                    CreateTime = DateTime.Now,
                    IsDeleted = false,
                    Name = item.Name,
                    ParentId = item.ParentId,
                    UpdateTime = DateTime.Now
                };
                // 设置资源的id
                var matchRs = existResources.FirstOrDefault(i => i.GetResourceCode() == temp.Code);
                if (matchRs!=null)
                {
                    temp.Id = matchRs.GetKey();
                }

                // 设置资源的父id
                if (!string.IsNullOrEmpty(temp.ParentId))
                {
                    var pa = resources.FirstOrDefault(a => a.Id == temp.ParentId);
                    var matchPa = existResources.FirstOrDefault(i => i.GetResourceCode() == pa?.Code);
                    if (matchPa!=null)
                    {
                        item.ParentId = matchPa.GetKey();
                    }
                }
                _permissionStore.SaveResource(item);
            });
        }

        private bool IsSuperAdmin(string userKey)
        {
            var superRole = _permissionStore.GetAllRole().FirstOrDefault(a => a.GetName().Equals(DefaultPermission.superAdminRoleName,StringComparison.OrdinalIgnoreCase));
            return _permissionStore.GetAllUserRole().Any(a => a.GetUserKey() == userKey && a.GetRoleKey() == superRole.GetKey());
        }

        /// <summary>
        /// 通过类名和方法名，获取
        /// </summary>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        private string GetResourceCode(MethodInfo methodInfo)
        {
            if (Attribute.IsDefined(methodInfo, typeof(ResourceAttribute)))
            {
                var attr = methodInfo.GetCustomAttribute<ResourceAttribute>();
                if (attr != null && !string.IsNullOrEmpty(attr.ResourceCode))
                {
                    return attr.ResourceCode;
                }
            }
            return $"{methodInfo.DeclaringType.Name.Replace("Controller", "")}_{methodInfo.Name}";
        }

    }
}
