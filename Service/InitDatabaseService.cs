using ApplicationCore.Entity;
using ApplicationCore.IServices;
using Infrastructure;
using Microsoft.Extensions.Logging;
using Snail.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Service
{
    public class InitDatabaseService:IService
    {
        private AppDbContext _db;
        private ILogger _logger;
        public InitDatabaseService(AppDbContext db, ILogger<InitDatabaseService> logger)
        {
            this._db = db;
            this._logger = logger;
        }

        public void Invoke()
        {
            InitPermission();
        }
        #region 可以从json初始化数据库

        #endregion
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
                    _db.Users.Add(new User { Id = userId, Account = "SuperAdmin", CreateTime = now, IsDeleted = false, Name = "超级管理员", Pwd = pwdHash });
                    _db.Roles.Add(new Role { Id = roleId, Name = "SuperAdmin", CreateTime = now, IsDeleted = false });
                    _db.UserRoles.Add(new UserRole { Id = IdGenerator.Generate<string>(), IsDeleted = false, RoleId = roleId, UserId = userId, CreateTime = now });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
            }
        }
    }
}
