using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Domain;
using WebApp.Infrastructure;

namespace WebApp.Services
{
    /// <summary>
    /// 权限服务，提供所有权限相关的服务，最终为应用层的接口服务
    /// </summary>
    public class PermissionService
    {
        private UserRoleRepository _userRoleRepository;
        private PermissionRepository _permissionRepository;

        public PermissionService(UserRoleRepository userRoleRepository, PermissionRepository permissionRepository)
        {
            _userRoleRepository = userRoleRepository;
            _permissionRepository = permissionRepository;
        }
        public void AddPermissions(int roleId, string resourceIds)
        {
            //var rolePermission=_permissionRepository.Where(a=>a.)
            throw new NotImplementedException();
        }

        public void RemovePermission(int roleId, string resourceIds)
        {

        }

        public void AddRolesToUser(int userId, List<int> roleIds)
        {
            var user = new UserModel(userId);
            user.AddRoles(roleIds);
            _userRoleRepository.AddUserRoles(userId,user.AddRoleIds);
        }

        public void RemoveRolesFromUser(int userId, List<int> roleIds)
        {

        }

        public bool HasPermission(string userId, string resourceId)
        {
            throw new NotImplementedException();
        }
    }
}
