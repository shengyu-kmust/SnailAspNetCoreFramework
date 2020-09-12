using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Dtos
{
    public class PermissionRoleResourceInfo
    {
        public string RoleKey { get; set; }
        public List<string> ResourceKeys { get; set; }
    }
}
