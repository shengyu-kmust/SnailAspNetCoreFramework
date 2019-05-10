using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Entity;
using DAL.Infrastructure;

namespace DAL.Domain
{
    /// <summary>
    /// 用户模型
    /// </summary>
    public class UserModel
    {
        private User _user;//用户的其它信息
        private List<Role> _userRoles;//用户的所有角色
        private HashSet<int> _removeRoleIds;
        private HashSet<int> _addRoleIds;
        private DatabaseContext _db;
        private UserRoleRepository _userRoleRepository;
        private List<int> _userRoleIds;

        public List<int> RemoveRoleIds => _removeRoleIds.ToList();
        public List<int> AddRoleIds => _addRoleIds.ToList();
        public List<int> UserRoleIds
        {
            get
            {
                if (_userRoleIds == null)
                {
                    _userRoleIds = _userRoles.Select(a => a.Id).ToList();
                    return _userRoleIds;
                }
                else
                {
                    return _userRoleIds;
                }
            }
        }
        public UserModel(User user, DatabaseContext db)
        {
            _user = user;
            _db = db;
        }

        public UserModel(int userId)
        {
        }

        public bool HasPermission(Resource resource)
        {
            var myRoles = MyRoles();
            var resourceModel = ResourceModel.CreateResourceModel(resource.Id);
            var resourceRoles = resourceModel.AllPermissionRoles();
            return myRoles.Intersect(resourceRoles).Any();
        }

        /// <summary>
        /// 向用户增加角色的逻辑
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public void AddRole(Role role)
        {
            if (role == null)
            {
                throw new ArgumentNullException();
            }
            if (!_userRoles.Any(a => a.Id == role.Id))
            {
                _addRoleIds.Add(role.Id);
            }
            CommitAddedRole();
        }

        public void AddRoles(List<Role> roles)
        {
            if (roles == null)
            {
                throw new ArgumentNullException();
            }

            if (roles.Count==0)
            {
                return;
            }
            roles.Select(a => a.Id).Except(UserRoleIds).ToList().ForEach(roleId => { _addRoleIds.Add(roleId); });
            CommitAddedRole();
        }

        public void AddRoles(List<int> roleIds)
        {
            if (roleIds == null)
            {
                throw new ArgumentNullException();
            }

            if (roleIds.Count == 0)
            {
                return;
            }
            roleIds.Except(UserRoleIds).ToList().ForEach(roleId => { _addRoleIds.Add(roleId); });
            CommitAddedRole();
        }
        private void CommitAddedRole()
        {
            var userId = _user.Id;
            var roleIds = _addRoleIds.ToList();
            _userRoleRepository.AddUserRoles(_user.Id,roleIds);
            _addRoleIds.Clear();
        }

        public List<Role> MyRoles()
        {
            throw new NotImplementedException();
        }

    }
}
