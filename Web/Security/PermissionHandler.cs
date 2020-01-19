using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Web.Security
{
    /// <summary>
    /// 权限控制处理类
    /// </summary>
    public class PermissionHandler:AuthorizationHandler<PermissionRequirement>
    {
        //private PermissionModel _permissionModel;

        //public PermissionHandler(PermissionModel permissionModel)
        //{
        //    _permissionModel = permissionModel;
        //}
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            //if (!(context.Resource is AuthorizationFilterContext mvcContext))
            //{
            //    return Task.CompletedTask;
            //}

            //if (!(mvcContext.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor))
            //{
            //    return Task.CompletedTask;
            //}
            ////获取请求接口对应的资源KEY
            //var permissionKey = $"{controllerActionDescriptor.ControllerName}Controller_{controllerActionDescriptor.ActionName}";
            //var userIdClaim = mvcContext.HttpContext.User.FindFirst(a => a.Type == ConstValues.UserId)?.Value;
            //var roleIdsClaim = mvcContext.HttpContext.User.FindFirst(a => a.Type == ConstValues.RoleIds)?.Value;
            ////运算是否有权限
            //if (int.TryParse(userIdClaim, out int userId) && _permissionModel.HasPermission(userId, permissionKey))
            //{
            //    context.Succeed(requirement);//如果有权限则设置
            //}
            return Task.CompletedTask;
        }
    }
}
