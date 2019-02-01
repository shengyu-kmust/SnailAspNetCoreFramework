using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators.Internal;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Entity;

namespace WebApp.Model
{
    public class PermissionModel
    {
        private Hashtable _userRoleMap=new Hashtable();
        private Hashtable _resourceRoleMap = new Hashtable();
        private Hashtable _resourceKeyObjMap = new Hashtable();
        private IServiceScopeFactory _serviceScopeFactory;
        private static object _lock=new object();

        public PermissionModel(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            loadPermissionData();
        }

        public void loadPermissionData()
        {
            // PermissionModel的注入时用的是单例，在单例里不能实例化scope的对象，否则会出错：Cannot consume scoped service 'WebApp.Entity.DatabaseContext' from singleton 'WebApp.Model.PermissionModel'.//
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<DatabaseContext>();
                var userRoles=db.UserRoleses.AsNoTracking().GroupBy(a => a.UserId).Select(a => new {
                    UserId = a.Key,
                    Roles = a.Select(b => b.RoleId).Distinct().ToList()
                }).ToList();

                var permissions = db.Permissions.AsNoTracking().GroupBy(a => a.ResourceId).Select(a => new
                {
                    ResourceId = a.Key,
                    Roles = a.Select(b => b.RoleId).Distinct().ToList()
                }).ToList();

                var resources=db.Resources.AsNoTracking().ToList();
                lock (_lock)
                {
                    _userRoleMap.Clear();
                    _resourceRoleMap.Clear();
                    _resourceKeyObjMap.Clear();
                    userRoles.ForEach(a => { _userRoleMap.Add(a.UserId, a.Roles); });
                    permissions.ForEach(a =>
                    {
                        _resourceRoleMap.Add(a.ResourceId, a.Roles);
                    });
                    resources.ForEach(a => { _resourceKeyObjMap.Add(a.Key, a); });
                }
            }
           

        }
        public bool HasPermission(int userId, string resourceKey)
        {
            var resource = _resourceKeyObjMap[resourceKey] as Resource;
            if (resource==null)
            {
                throw  new Exception($"找不到key为{resourceKey}的资源");
            }
            return HasPermission(userId, resource.Id);
        }

        public bool HasPermission(int userId, int resourceId)
        {
            var userRoles = _userRoleMap[userId] as List<int>;
            var resourceRoles = _resourceRoleMap[resourceId] as List<int>;
            return HasPermission(userRoles,resourceId);
        }

        public bool HasPermission(List<int> roleIds,int resourceId)
        {
            var resourceRoles = _resourceRoleMap[resourceId] as List<int>;
            if (roleIds != null && resourceRoles != null)
            {
                return roleIds.Intersect(resourceRoles).Any();
            }
            else
            {
                return false;
            }
        }

        public bool HasPermission(List<int> roleIds, string resourceKey)
        {
            var resource = _resourceKeyObjMap[resourceKey] as Resource;
            if (resource == null)
            {
                throw new Exception($"找不到key为{resourceKey}的资源");
            }
            return HasPermission(roleIds, resource.Id);
        }

    }
}
