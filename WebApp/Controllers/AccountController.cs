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
        [HttpGet("login")]
        public ActionResult Login(string name,string role)
        {
            HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
            {
                new Claim("name",name),
                new Claim("role",role)
            }, CookieAuthenticationDefaults.AuthenticationScheme)));
            return Ok("login success");
        }
        [HttpGet("logout")]
        public ActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return Ok("Logout success");
        }

        [HttpGet("user")]
        public ActionResult User()
        {
            var jwt = HttpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme).Result;
            var cook = HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme).Result;
            var obj = new
            {
                result1=jwt.Principal?.Claims.ToList().Select(a=>new{key=a.Type,value=a.Value}),
                result2=cook.Principal?.Claims.ToList().Select(a => new { key = a.Type, value = a.Value })
            };
            return Ok(JsonConvert.SerializeObject(obj));
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
