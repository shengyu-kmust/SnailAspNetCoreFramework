using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Snail.Core.Entity;
using Snail.Core.Interface;
using Snail.Core.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Permission
{
    public abstract class BasePermissionStore<TDbContext, TUser, TRole, TUserRole, TResource, TRoleResource> : IPermissionStore
        where TDbContext : DbContext
        where TUser : class, IUser, new()
        where TRole : class, IRole,new()
        where TUserRole : class, IUserRole, new()
        where TResource : class,IResource, new()
        where TRoleResource : class, IRoleResource, new()
    {
        protected TDbContext _db;
        protected IMemoryCache _memoryCache;
        protected IApplicationContext _applicationContext;
        private string userCacheKey = $"DefaultPermissionStore_{nameof(userCacheKey)}";
        private string roleCacheKey = $"DefaultPermissionStore_{nameof(roleCacheKey)}";
        private string userRoleCacheKey = $"DefaultPermissionStore_{nameof(userRoleCacheKey)}";
        private string resourceCacheKey = $"DefaultPermissionStore_{nameof(resourceCacheKey)}";
        private string roleResourceCacheKey = $"DefaultPermissionStore_{nameof(roleResourceCacheKey)}";
        protected IOptionsMonitor<PermissionOptions> _permissionOptions;

        public BasePermissionStore(TDbContext db, IMemoryCache memoryCache, IOptionsMonitor<PermissionOptions> permissionOptions, IApplicationContext applicationContext)
        {
            _db = db;
            _memoryCache = memoryCache;
            _permissionOptions = permissionOptions;
            _applicationContext = applicationContext;
        }

        #region 查询数据
        public virtual List<IResource> GetAllResource()
        {
            return _memoryCache.GetOrCreate(resourceCacheKey, a => _db.Set<TResource>().AsNoTracking().Select(i => (IResource)i).ToList());
        }

        public virtual List<IRole> GetAllRole()
        {
            return _memoryCache.GetOrCreate(roleCacheKey, a => _db.Set<TRole>().AsNoTracking().Select(i => (IRole)i).ToList());

        }

        public virtual List<IRoleResource> GetAllRoleResource()
        {
            return _memoryCache.GetOrCreate(roleResourceCacheKey, a => _db.Set<TRoleResource>().AsNoTracking().Select(i => (IRoleResource)i).ToList());

        }

        public virtual List<IUser> GetAllUser()
        {
            return _memoryCache.GetOrCreate(userCacheKey, a => _db.Set<TUser>().AsNoTracking().Select(i => (IUser)i).ToList());

        }

        public virtual List<IUserRole> GetAllUserRole()
        {
            return _memoryCache.GetOrCreate(userRoleCacheKey, a => _db.Set<TUserRole>().AsNoTracking().Select(i => (IUserRole)i).ToList());

        }


        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// 经验证，cache里的值在remove后，之前已经从cache里获取的值不会删除
        /// </remarks>
        public virtual void ReloadPemissionDatas()
        {
            _memoryCache.Remove(userCacheKey);
            _memoryCache.Remove(roleCacheKey);
            _memoryCache.Remove(userRoleCacheKey);
            _memoryCache.Remove(resourceCacheKey);
            _memoryCache.Remove(roleResourceCacheKey);
        }


        #endregion


        public virtual void RemoveRole(string roleKey)
        {
            var roleEntity = GetAllRole().FirstOrDefault(a => a.GetKey() == roleKey) as TRole;
            if (roleEntity!=null)
            {
                _db.Set<TRole>().Remove(roleEntity);
            }
            _db.SaveChanges();
            _memoryCache.Remove(roleCacheKey);

        }


        public virtual void RemoveUser(string userKey)
        {
            var userEntity = _db.Set<TUser>().Find(userKey);
            if (userEntity is IEntitySoftDelete entitySoftDeleteEntity)
            {
                entitySoftDeleteEntity.IsDeleted = true;
            }
            else
            {
                _db.Set<TUser>().Remove(userEntity);
            }
            _db.SaveChanges();
            _memoryCache.Remove(userCacheKey);
        }

        public virtual void RemoveResource(string resourceKey)
        {
            var resourceEntity = _db.Set<TResource>().Find(resourceKey);
            if (resourceEntity != null)
            {
                _db.Remove(resourceEntity);//资源为真删
                _db.SaveChanges();
            }
            _memoryCache.Remove(resourceCacheKey);
        }

        /// <summary>
        /// 保存资源。会从资源id和资源code两字段考虑是新增还是修改
        /// </summary>
        /// <param name="resource"></param>
        public abstract void SaveResource(IResource resource);

        public abstract void UpdateRoleEntityByDto(IRole entity, IRole dto, bool isAdd);
        public abstract void UpdateUserEntityByDto(IUser entity, IUser dto, bool isAdd);

        public virtual void SaveRole(IRole role)
        {
            var roleKey = role.GetKey();
            if (string.IsNullOrEmpty(roleKey))
            {
                var addRole = new TRole();
                UpdateRoleEntityByDto(addRole, role, true);
                _db.Add(addRole);
            }
            else
            {
                var editRole = _db.Set<TRole>().Find(role.GetKey());
                UpdateRoleEntityByDto(editRole, role, false);
            }
            _db.SaveChanges();
            _memoryCache.Remove(roleCacheKey);

        }

        public virtual void SaveUser(IUser user)
        {
            var userKey = user.GetKey();
            if (string.IsNullOrEmpty(userKey))
            {
                var addUser = new TUser();
                UpdateUserEntityByDto(addUser, user, true);
                _db.Add(addUser);
            }
            else
            {
                var editRole = _db.Set<TUser>().Find(user.GetKey());
                UpdateUserEntityByDto(editRole, user, false);
            }
            _db.SaveChanges();
            _memoryCache.Remove(userCacheKey);
        }

        public abstract void SetRoleResources(string roleKey, List<string> resourceKeys);
        public abstract void SetUserRoles(string userKey, List<string> roleKeys);
    }
}
