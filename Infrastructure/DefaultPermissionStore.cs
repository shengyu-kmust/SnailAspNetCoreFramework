using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Snail.Core.Interface;
using Snail.Core.Permission;

namespace Infrastructure
{
    /// <summary>
    /// 权限存储实现
    /// </summary>
    public class DefaultPermissionStore : BasePermissionStore<DbContext, User, Role, UserRole, Resource, RoleResource>
    {
        /// <summary>
        /// DefaultPermissionStore
        /// </summary>
        /// <param name="db"></param>
        /// <param name="memoryCache"></param>
        /// <param name="permissionOptions"></param>
        /// <param name="applicationContext"></param>
        public DefaultPermissionStore(DbContext db, IMemoryCache memoryCache, IOptionsMonitor<PermissionOptions> permissionOptions, IApplicationContext applicationContext) : base(db, memoryCache, permissionOptions, applicationContext)
        {
        }
    }


}
