using Snail.Core.Entity;
using Snail.Core.Permission;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entities
{

    [Table("UserRole")]
    public class UserRole : DefaultBaseEntity, IUserRole
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public string GetRoleKey()
        {
            return this.RoleId;
        }

        public string GetUserKey()
        {
            return this.UserId;
        }

        public void SetRoleKey(string roleKey)
        {
            this.RoleId = roleKey;
        }

        public void SetUserKey(string userKey)
        {
            this.UserId = userKey;
        }
    }
}
