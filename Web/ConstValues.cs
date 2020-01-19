using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Web
{
    public class ConstValues
    {
        public const string UserId = nameof(UserId);//存储的是用户id
        public const string RoleIds = nameof(RoleIds);
        public const string Account = nameof(Account);
        public const string UserName = nameof(UserName);
        public const string Issuer = "snailServer";
        public const string Audience = "snailClient";
        public const string PermissionPolicy = "Permission";

        public static SecurityKey IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes("shengyushengyushengyu"));
    }
}
