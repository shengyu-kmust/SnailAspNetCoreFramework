using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Snail.Common;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web.DTO;
using Web.Security;

namespace Web.Controllers.Example
{
    /// <summary>
    /// 用户登录授权
    /// </summary>

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountTestController: ControllerBase
    {
        private AuthenticationSetting _authenticationSetting;
        public AccountTestController(IOptionsSnapshot<AuthenticationSetting> authenticationSetting)
        {
            _authenticationSetting = authenticationSetting.Value;
        }
        [HttpPost]
        public async Task<UserInfoDto> Login(LoginDto loginDto)
        {
            var userId = Guid.NewGuid().ToString();
            var userName = "测试用户";
            var roleIds = string.Join(",", Enumerable.Range(1, 2).Select(a => Guid.NewGuid().ToString()).ToList());
            var claims = new List<Claim>() { new Claim(ConstValues.UserId, userId), new Claim(ConstValues.UserName,userName), new Claim(ConstValues.RoleIds, roleIds), new Claim(ConstValues.Account, loginDto.Account) };
            SigningCredentials creds;
            if (_authenticationSetting.IsAsymmetric)
            {
               var key = new RsaSecurityKey(RSAHelper.GetRSAParametersFromFromPrivatePem(_authenticationSetting.RsaPrivateKey));
               creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);
            }
            else
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSetting.SymmetricSecurityKey));
                creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            }
            var token = new JwtSecurityToken(ConstValues.Issuer, ConstValues.Audience, claims, DateTime.Now, DateTime.Now.AddMinutes(30), creds);
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims, "authenticationTypeName")));
            return  new UserInfoDto
            {
                Account = loginDto.Account,
                Id = userId,
                Name = userName,
                RoleIds = roleIds.Split(",").ToList(),
                RoleNames = new List<string>() { "管理员", "普通用户" },
                Token = tokenStr
            };
        }

        [HttpGet]
        public ActionResult GetLoginUserInfo()
        {
            return new JsonResult(HttpContext.User.Claims.Select(a=>new { a.Type,a.Value}));
        }
    }
}
