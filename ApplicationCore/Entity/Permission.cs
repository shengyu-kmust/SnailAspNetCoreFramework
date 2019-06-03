using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Entity
{
    public class Permission : BaseEntity
    {
        public int RoleId { get; set; }
        public int ResourceId { get; set; }
        public Role Role { get; set; }
        public Resource Resource { get; set; }
    }
}
