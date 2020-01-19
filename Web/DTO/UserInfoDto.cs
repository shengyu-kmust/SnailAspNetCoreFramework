using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DTO
{
    public class UserInfoDto
    {
        public string Id { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public List<string> RoleIds { get; set; }
        public List<string> RoleNames { get; set; }
        public string Token { get; set; }
    }
}
