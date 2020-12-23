using ApplicationCore.Entities;
using Infrastructure;
using Microsoft.Extensions.Logging;
using Snail.Common;
using Snail.Web.IServices;
using System;
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
                var superUserId = IdGenerator.Generate<string>();
                var superRoleId = IdGenerator.Generate<string>();
                var adminUserId = IdGenerator.Generate<string>();
                var adminRoleId = IdGenerator.Generate<string>();
                var pwdHash = BitConverter.ToString(HashAlgorithm.Create(HashAlgorithmName.MD5.Name).ComputeHash(Encoding.UTF8.GetBytes("123456"))).Replace("-", "");
                var now = DateTime.Now;
                if (!_db.Users.Any() && !_db.Roles.Any() && !_db.UserRoles.Any())
                {
                    // 如果权限表不是默认的，下面要修改成自己的表
                    _db.Users.AddRange(
                        new User { Id = superUserId, Account = "superAdmin", CreateTime = now, IsDeleted = false, Name = "超级管理员", Pwd = pwdHash },
                        new User { Id = adminUserId, Account = "admin", CreateTime = now, IsDeleted = false, Name = "管理员", Pwd = pwdHash });
                    _db.Roles.AddRange(
                        new Role { Id = superRoleId, Name = "SuperAdmin", CreateTime = now, IsDeleted = false },
                        new Role { Id = adminRoleId, Name = "admin", CreateTime = now, IsDeleted = false });
                    _db.UserRoles.AddRange(
                        new UserRole { Id = IdGenerator.Generate<string>(), IsDeleted = false, RoleId = superRoleId, UserId = superUserId, CreateTime = now },
                     new UserRole { Id = IdGenerator.Generate<string>(), IsDeleted = false, RoleId = adminRoleId, UserId = adminUserId, CreateTime = now });
                }
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
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