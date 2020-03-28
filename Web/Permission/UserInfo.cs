using Snail.Core.Permission;
using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Permission
{
    /// <summary>
    /// 用户信息，用于给前端的数据
    /// </summary>
    public class UserInfo : IUserInfo
    {
        public string UserKey { get;set; }
        public string UserName { get;set; }
        public string Account { get;set; }
        public List<string> RoleKeys { get;set; }
        public List<string> RoleNames { get;set; }
    }
}
