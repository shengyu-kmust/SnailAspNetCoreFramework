using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace WebApp
{
    public class ConstValues
    {
        public const string NameClaimType = "userId";//存储的是用户id
        public const string RoleClaimType = "roleIds";
        public const string Issuer = "snailServer";
        public const string Audience = "snailClient";
        public const string PermissionPolicy = "Permission";

        public static SecurityKey IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes("shengyushengyushengyu"));
    }
}
