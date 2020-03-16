using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Entity
{
    public class UserOrg:BaseEntity
    {
        public string UserId { get; set; }
        public string OrgId { get; set; }
    }
}
