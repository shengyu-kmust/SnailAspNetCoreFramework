using ApplicationCore.Entity;
using ApplicationCore.IServices;
using Infrastructure;
using Microsoft.Extensions.Logging;
using Snail.Common;
using Snail.Permission.Entity;
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
                var userId = IdGenerator.Generate<string>();
                var roleId = IdGenerator.Generate<string>();
                var pwdHash = BitConverter.ToString(HashAlgorithm.Create(HashAlgorithmName.MD5.Name).ComputeHash(Encoding.UTF8.GetBytes("123456"))).Replace("-", "");
                var now = DateTime.Now;
                if (!_db.Users.Any() && !_db.Roles.Any() && !_db.UserRoles.Any())
                {
                    // 如果权限表不是默认的，下面要修改成自己的表
                    _db.Users.Add(new User { Id = userId, LoginName = "SuperAdmin", CreateTime = now, IsDeleted = false, Name = "超级管理员", Pwd = pwdHash });
                    _db.Roles.Add(new PermissionDefaultRole { Id = roleId, Name = "SuperAdmin", CreateTime = now, IsDeleted = false });
                    _db.UserRoles.Add(new PermissionDefaultUserRole { Id = IdGenerator.Generate<string>(), IsDeleted = false, RoleId = roleId, UserId = userId, CreateTime = now });
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