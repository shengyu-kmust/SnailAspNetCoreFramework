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
        public const string NameClaimType = "name";
        public const string RoleClaimType = "name";
        public const string Issuer = "snailServer";
        public const string Audience = "snailClient";

        public static SecurityKey IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes("shengyushengyushengyu"));
    }
}
