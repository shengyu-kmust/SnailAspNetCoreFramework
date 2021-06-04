using ApplicationCore.Entities;
using Infrastructure;
using Microsoft.Extensions.Logging;
using Snail.Common;
using Snail.Core;
using Snail.Core.Service;
using Snail.Web.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Service
{
    /// <summary>
    /// 数据库初始化服务，用于初始化权限数据、其它业务数据
    /// </summary>
    public class InitDatabaseService : IService
    {
        private AppDbContext _db;
        private ILogger _logger;

        /// <summary>
        /// InitDatabaseService
        /// </summary>
        /// <param name="db"></param>
        /// <param name="logger"></param>
        public InitDatabaseService(AppDbContext db, ILogger<InitDatabaseService> logger)
        {
            this._db = db;
            this._logger = logger;
        }

        /// <summary>
        /// Invoke
        /// </summary>
        public void Invoke()
        {
            InitPermission();
            InitTableData();
        }

        /// <summary>
        /// 初始化权限数据
        /// </summary>
        private void InitPermission()
        {
            try
            {

                if (!_db.Users.Any() && !_db.Roles.Any() && !_db.UserRoles.Any())
                {
                    var userRoleDatas=buildUserAndRoleData();
                    // 如果权限表不是默认的，下面要修改成自己的表
                    _db.Users.AddRange(userRoleDatas.users);
                    _db.Roles.AddRange(userRoleDatas.roles);
                    _db.UserRoles.AddRange(userRoleDatas.userRoles);
                }
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
            }
        }
        private (List<User> users, List<Role> roles, List<UserRole> userRoles) buildUserAndRoleData()
        {
            var users = new List<User>();
            var roles = new List<Role>();
            var userRoles = new List<UserRole>();
            if (typeof(ITenant<string>).IsAssignableFrom(typeof(User)))
            {
                var tenantUserRoleData1 = buildUserAndRoleDataInternal("tenant1");
                var tenantUserRoleData2=buildUserAndRoleDataInternal("tenant2");
                users.AddRange(tenantUserRoleData1.users);
                users.AddRange(tenantUserRoleData2.users);
                roles.AddRange(tenantUserRoleData1.roles);
                roles.AddRange(tenantUserRoleData2.roles);
                userRoles.AddRange(tenantUserRoleData1.userRoles);
                userRoles.AddRange(tenantUserRoleData2.userRoles);
                return (users, roles, userRoles);

            }
            else
            {
                var userRoleData = buildUserAndRoleDataInternal(null);
                return (userRoleData.users, userRoleData.roles, userRoleData.userRoles);
            }
        }

        private (List<User> users, List<Role> roles, List<UserRole> userRoles) buildUserAndRoleDataInternal(string tenantId)
        {
            var superUserId = IdGenerator.Generate<string>();
            var superRoleId = IdGenerator.Generate<string>();
            var adminUserId = IdGenerator.Generate<string>();
            var adminRoleId = IdGenerator.Generate<string>();
            var pwdHash = BitConverter.ToString(HashAlgorithm.Create(HashAlgorithmName.MD5.Name).ComputeHash(Encoding.UTF8.GetBytes("123456"))).Replace("-", "");
            var now = DateTime.Now;
            var users = new List<User>
            {
                new User { Id = superUserId, Account = "superAdmin", CreateTime = now, IsDeleted = false, Name = "超级管理员", Pwd = pwdHash },
                new User { Id = adminUserId, Account = "admin", CreateTime = now, IsDeleted = false, Name = "管理员", Pwd = pwdHash }
            };
            var roles = new List<Role>
            {
                new Role { Id = superRoleId, Name = "SuperAdmin", CreateTime = now, IsDeleted = false },
                new Role { Id = adminRoleId, Name = "admin", CreateTime = now, IsDeleted = false }
            };
            var userRoles = new List<UserRole> {
                new UserRole { Id = IdGenerator.Generate<string>(), IsDeleted = false, RoleId = superRoleId, UserId = superUserId, CreateTime = now },
                new UserRole { Id = IdGenerator.Generate<string>(), IsDeleted = false, RoleId = adminRoleId, UserId = adminUserId, CreateTime = now }
            };
            SetTenantId(users, tenantId);
            SetTenantId(roles, tenantId);
            SetTenantId(userRoles, tenantId);
            return (users, roles, userRoles);
        }
        private void SetTenantId<T>(List<T> objs,string tenantId)
        {
            foreach (var obj in objs)
            {
                if (obj is ITenant<string> tenantObj)
                {
                    tenantObj.TenantId = tenantId;
                }
            }
        }

        /// <summary>
        /// 初始化业务数据，建议从json文件里读取
        /// </summary>
        private void InitTableData()
        {

        }
    }
}