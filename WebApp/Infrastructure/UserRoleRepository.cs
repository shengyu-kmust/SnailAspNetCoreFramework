using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Entity;
using WebApp.Enum;

namespace WebApp.Infrastructure
{
    /// <summary>
    /// 基础层，不应该有逻辑，只是操作数据
    /// </summary>
    public class UserRoleRepository:BaseRepository<UserRole>
    {

        public UserRoleRepository(DatabaseContext db):base(db)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        public void AddUserRoles(int userId, List<int> roleIds)
        {
            roleIds.ForEach(roleId =>
            {
                db.UserRoleses.Add(new UserRole()
                {
                    RoleId=roleId,
                    UserId = userId,
                    IsValid=(int)ValidOrNot.Valid
                });
            });
            db.SaveChanges();
        }

        public void AddUserRoles(Dictionary<int, List<int>> userRoles)
        {
            foreach (var user in userRoles)
            {
                user.Value.ForEach(roleId =>
                {
                    db.UserRoleses.Add(new UserRole()
                    {
                        UserId = user.Key,
                        RoleId = roleId,
                        IsValid = (int)ValidOrNot.Valid
                    });
                });
            }
            db.SaveChanges();
        }
    }
}
