using Microsoft.AspNetCore.Authorization;
using Snail.Core.Permission;
using System.Threading.Tasks;

namespace Web.Permission
{
    public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
    {
        private IPermission _permission;
        public PermissionRequirementHandler(IPermission permission)
        {
            _permission = permission;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var resourceKey=_permission.GetRequestResourceKey(context.Resource);// 获取资源的key
            var userKey = _permission.GetUserInfo(context.User).UserKey; // 根据用户的claims获取用户的key
            if (_permission.HasPermission(resourceKey,userKey)) // 判断用户是否有权限
            {
                context.Succeed(requirement); // 如果有权限，则获得此Requirement
            }
            return Task.CompletedTask;
        }
    }
}
