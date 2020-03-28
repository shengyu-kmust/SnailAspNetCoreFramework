using ApplicationCore.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Snail.Common;
using Snail.Common.Extenssions;
using Snail.Core.Interface;
using Snail.Core.Permission;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Permission
{
    public class DefaultPermissionStore :BasePermissionStore<DbContext, User,Role,UserRole,Resource,RoleResource>, IPermissionStore
    {
        private string userCacheKey = $"DefaultPermissionStore_{nameof(userCacheKey)}", roleCacheKey = $"DefaultPermissionStore_{nameof(roleCacheKey)}", userRoleCacheKey = $"DefaultPermissionStore_{nameof(userRoleCacheKey)}", resourceCacheKey = $"DefaultPermissionStore_{nameof(resourceCacheKey)}", roleResourceCacheKey = $"DefaultPermissionStore_{nameof(roleResourceCacheKey)}";

        public DefaultPermissionStore(DbContext db, IMemoryCache memoryCache, IOptionsMonitor<PermissionOptions> permissionOptions, IApplicationContext applicationContext):base(db,memoryCache,permissionOptions,applicationContext)
        {
        }
       


        /// <summary>
        /// 保存资源。会从资源id和资源code两字段考虑是新增还是修改
        /// </summary>
        /// <param name="resource"></param>
        public override void SaveResource(IResource resource)
        {
            var resoureDto = resource as Resource;
            var resourceKey = resource.GetKey();
            var userId = _applicationContext.GetCurrentUserId();
            var resourceEntity = _db.Set<Resource>().FirstOrDefault(a => a.Id == resourceKey || a.Code == resource.GetResourceCode());
            if (resourceEntity == null)
            {
                //add
                var addDto = new Resource
                {
                    Creater = userId,
                    CreateTime = DateTime.Now,
                    IsDeleted = false,
                    Code = resource.GetResourceCode(),
                    Name = resource.GetName(),
                    ParentId = resoureDto.ParentId,
                    Id = string.IsNullOrEmpty(resoureDto.Id) ? IdGenerator.Generate<string>() : resoureDto.Id,
                    Updater = userId,
                    UpdateTime = DateTime.Now
                };
                _db.Add(addDto);
            }
            else
            {
                resourceEntity.Name = resource.GetName();
                resourceEntity.Code = resource.GetResourceCode();
                resourceEntity.ParentId = resoureDto.ParentId;
                resourceEntity.Updater = userId;
                resourceEntity.UpdateTime = DateTime.Now;
            }
            _db.SaveChanges();
            _memoryCache.Remove(resourceCacheKey);
        }

        public override void UpdateRoleEntityByDto(IRole entity, IRole dto, bool isAdd)
        {
            var roleEntity = entity as Role;
            var roleDto = dto as Role;
            var userId= _applicationContext.GetCurrentUserId();
            var now = DateTime.Now;
            if (isAdd)
            {
                roleEntity.Id = IdGenerator.Generate<string>();
                roleEntity.IsDeleted = false;
            }
            roleEntity.CreateTime = now;
            roleEntity.Updater = userId;
            roleEntity.Updater = userId;
            roleEntity.Creater = userId;
            roleEntity.Name = roleDto.Name;
        }

        public override void UpdateUserEntityByDto(IUser entity, IUser dto, bool isAdd)
        {
            var userEntity = entity as User;
            var userDto = dto as User;
            var userId = _applicationContext.GetCurrentUserId();
            var now = DateTime.Now;
            if (isAdd)
            {
                userEntity.Id = IdGenerator.Generate<string>();
                userEntity.IsDeleted = false;
                userEntity.Pwd = userDto.Pwd;
            }

            if (!string.IsNullOrEmpty(userDto.GetName()))
            {
                userEntity.Name = userDto.GetName();
            }
            if (!string.IsNullOrEmpty(userDto.GetAccount()))
            {
                userEntity.Account = userDto.GetAccount();
            }
            if (!string.IsNullOrEmpty(userDto.Email))
            {
                userEntity.Email = userDto.Email;
            }
            if (!string.IsNullOrEmpty(userDto.Phone))
            {
                userEntity.Phone = userDto.Phone;
            }
            if (!string.IsNullOrEmpty(userDto.Pwd))
            {
                userEntity.Pwd = userDto.Pwd;
            }
            userEntity.CreateTime = now;
            userEntity.Updater = userId;
            userEntity.Updater = userId;
            userEntity.Creater = userId;
        }

        public override void SetRoleResources(string roleKey, List<string> resourceKeys)
        {
            var userId = _applicationContext.GetCurrentUserId();
            var allRoleResources = _db.Set<RoleResource>().AsNoTracking().Where(a => a.RoleId == roleKey).ToList();
            var allResource = _db.Set<Resource>().AsNoTracking().ToList();
            allRoleResources.Where(a => !resourceKeys.Contains(a.GetResourceKey())).ToList().ForEach(a =>
            {
                _db.Remove(a);
            });
            resourceKeys.Where(a => !allRoleResources.Select(i => i.GetResourceKey()).Contains(a)).ToList().ForEach(resourceKey =>
            {
                if (allResource.Any(a=>a.Id==resourceKey))
                {
                    _db.Add(new RoleResource
                    {
                        Id = IdGenerator.Generate<string>(),
                        Creater = userId,
                        CreateTime = DateTime.Now,
                        IsDeleted = false,
                        ResourceId = resourceKey,
                        RoleId = roleKey,
                        Updater = userId,
                        UpdateTime = DateTime.Now
                    });
                }
            });
            _db.SaveChanges();
            _memoryCache.Remove(roleResourceCacheKey);
        }

        public override void SetUserRoles(string userKey, List<string> roleKeys)
        {
            var userId = _applicationContext.GetCurrentUserId();
            var allUserRoles = _db.Set<UserRole>().AsNoTracking().Where(a => a.UserId == userKey).ToList();
            var allRole = _db.Set<Role>().AsNoTracking().ToList();
            allUserRoles.Where(a => !roleKeys.Contains(a.GetRoleKey())).ToList().ForEach(a =>
            {
                _db.Remove(a);
            });
            roleKeys.Where(a => !allUserRoles.Select(i => i.RoleId).Contains(a) && a.HasValue()).ToList().ForEach(roleKey =>
            {
                if (allRole.Any(a=>a.Id==roleKey))
                {
                    _db.Add(new UserRole
                    {
                        Id = IdGenerator.Generate<string>(),
                        Creater = userId,
                        CreateTime = DateTime.Now,
                        IsDeleted = false,
                        UserId = userKey,
                        RoleId = roleKey,
                        Updater = userId,
                        UpdateTime = DateTime.Now
                    });
                }
               
            });
            _db.SaveChanges();
            _memoryCache.Remove(userRoleCacheKey);
        }

    }
}
