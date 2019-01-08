using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace WebApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController: ControllerBase
    {
        [HttpPost("WebLogin")]
        public ActionResult WebLogin(User user)
        {
            var userEntity = TestData.Users.FirstOrDefault(a => a.Name == user.Name && a.Pwd == user.Pwd);
            if (userEntity == null)
            {
                return Ok("用户名或密码错误");
            }
            HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
            {
                new Claim(ConstValues.NameClaimType,user.Name),
                new Claim(ConstValues.RoleClaimType,userEntity.Role)
            }, CookieAuthenticationDefaults.AuthenticationScheme)));
            return Ok("login success");
        }

        [HttpPost("ApiLogin")]
        public ActionResult ApiLogin(User user)
        {
            var userEntity = TestData.Users.FirstOrDefault(a => a.Name == user.Name && a.Pwd == user.Pwd);
            if (userEntity == null)
            {
                return Ok("用户名或密码错误");
            }
            var claim = new Claim[]{
                new Claim(ConstValues.NameClaimType,user.Name),
                new Claim(ConstValues.RoleClaimType,userEntity.Role)
            };
            //签名证书(秘钥，加密算法)
            var creds = new SigningCredentials(ConstValues.IssuerSigningKey, SecurityAlgorithms.HmacSha256);
            //生成token  [注意]需要nuget添加Microsoft.AspNetCore.Authentication.JwtBearer包，并引用System.IdentityModel.Tokens.Jwt命名空间
            var token = new JwtSecurityToken(ConstValues.Issuer, ConstValues.Audience, claim, DateTime.Now, DateTime.Now.AddMinutes(30), creds);
            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        [HttpGet("ThirdPartLogin")]
        public async Task ApiLogin(string authscheme)
        {
            var authType = HttpContext.Request.Query["authscheme"];
            if (!string.IsNullOrEmpty(authType))
            {
               await HttpContext.ChallengeAsync(authType, new AuthenticationProperties() { RedirectUri = "/api/account/user" });
            }
            else
            {
                HttpContext.Response.WriteAsync("请指定第三方登录应用");
            }
        }


        [HttpGet("logout")]
        public ActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return Ok("Logout success");
        }

        [HttpGet("User")]
        public ActionResult User()
        {
            var result = HttpContext.User.Claims
                .Select(a => new {key = a.Type, value = a.Value}).ToList();
            result.Add(new
                    {key = "authenticateType", value = HttpContext.User.Identity?.AuthenticationType});
            return Ok(JsonConvert.SerializeObject(result));
        }

        [HttpGet("token")]
        public ActionResult Token()
        {
           

            var claim = new Claim[]{
                new Claim("na","shengyu1"),
                new Claim("rl","admin1")
            };

            //对称秘钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("shengyushengyushengyu"));
            //签名证书(秘钥，加密算法)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //生成token  [注意]需要nuget添加Microsoft.AspNetCore.Authentication.JwtBearer包，并引用System.IdentityModel.Tokens.Jwt命名空间
            var token = new JwtSecurityToken("snailServer","snailClient", claim, DateTime.Now, DateTime.Now.AddMinutes(30), creds);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });

        }

    }
}
