using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snail.Core.Attributes;
using Snail.Core.Permission;

namespace Web.Controllers
{
    /// <summary>
    /// 示例表接口
    /// </summary>
    [Authorize(Policy = PermissionConstant.PermissionAuthorizePolicy)]
    [Resource(Description = "管理员")]
    public class ManagerController : DefaultBaseController
    {
        private IPermissionStore _permissionStore;
        public ManagerController(ControllerContext controllerContext, IPermissionStore permissionStore) : base(controllerContext)
        {
            _permissionStore = permissionStore;
        }

        /// <summary>
        /// 刷新所有实体缓存
        /// </summary>
        [HttpGet]
        public void RefreashAllEntityCache()
        {
            entityCacheManager.RefreashAllEntityCache();
        }


        /// <summary>
        /// 刷新所有实体缓存
        /// </summary>
        [HttpGet]
        public void RefreashAllPermissionCache()
        {
            _permissionStore.ReloadPemissionDatas();
        }
    }
}
