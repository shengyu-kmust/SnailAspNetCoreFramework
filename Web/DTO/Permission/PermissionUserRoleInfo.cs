using System;
using System.Collections.Generic;
using System.Text;

namespace Web.DTO.Permission
{
    public class PermissionUserRoleInfo
    {
        public string UserKey { get; set; }
        public List<string> RoleKeys { get; set; }
    }
}
