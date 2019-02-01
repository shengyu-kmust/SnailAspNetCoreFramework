using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Entity
{
    public class UserOrg:BaseEntity
    {
        public int UserId { get; set; }
        public int OrgId { get; set; }
        public User User { get; set; }
        [ForeignKey("OrgId")]
        public Organization Org { get; set; }
    }
}
