using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Web.Permission
{
    public interface IToken
    {
        List<System.Security.Claims.Claim> ResolveFromToken(string tokenStr);
        string ResolveToken(List<System.Security.Claims.Claim> claims, DateTime expireTime);
    }
}
