using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Snail.Common;
using Snail.Core.Permission;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Web.Permission
{
    public class Token: IToken
    {
        private IMemoryCache _cache;
        private PermissionOptions _options;
        public Token(IMemoryCache cache,IOptionsMonitor<PermissionOptions> optionsMonitor)
        {
            _cache = cache;
            _options = optionsMonitor?.CurrentValue ?? new PermissionOptions() ;
        }
  
        public List<Claim> ResolveFromToken(string tokenStr)
        {
            _cache.TryGetValue(tokenStr, out List<Claim> claims);
            if (claims==null)
            {
                var handler = new JwtSecurityTokenHandler();
                var key = new RsaSecurityKey(RSAHelper.GetRSAParametersFromFromPublicPem(_options.RsaPublicKey));

                handler.ValidateToken(tokenStr, new TokenValidationParameters
                {
                    IssuerSigningKey = key,
                    ValidateAudience = false,
                    ValidateIssuer = false
                }, out SecurityToken validatedToken);
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    _cache.Set(tokenStr, jwtSecurityToken.Claims, jwtSecurityToken.Payload.ValidTo);
                    return jwtSecurityToken.Claims.ToList();
                }
                else
                {
                    throw new Exception("非法token");
                }
            }
            else
            {
                return claims;
            }
        }

        public string ResolveToken(List<Claim> claims, DateTime expireTime)
        {
            var key = new RsaSecurityKey(RSAHelper.GetRSAParametersFromFromPrivatePem(_options.RsaPrivateKey));
            var jwtSecurityToken = new JwtSecurityToken(null, null, claims, null, expireTime, new SigningCredentials(key, SecurityAlgorithms.RsaSha256));
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            _cache.Set(tokenStr, claims, expireTime);
            return tokenStr;    
        }
    }
}
