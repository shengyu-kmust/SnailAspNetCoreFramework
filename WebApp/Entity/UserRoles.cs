using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Entity
{

    [Table("UserRole")]
    public class UserRoles:BaseEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public User User { get; set; }
    }
}
