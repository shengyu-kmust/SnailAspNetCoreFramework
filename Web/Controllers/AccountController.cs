using ApplicationCore.Entity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Web.Controllers
{

    [ApiController]
    public class AccountController:AuthorizeBaseController
    {
        private DatabaseContext _db;

        public AccountController(DatabaseContext db) : base(db)
        {
        }
       
        #region 登录
        //[HttpPost("WebLogin")]
        //public ActionResult WebLogin(User user)
        //{
        //    var userEntity = _db.Users.FirstOrDefault(a => a.Name == user.Name && a.Pwd == user.Pwd);
        //    if (userEntity == null)
        //    {
        //        return Ok("用户名或密码错误");
        //    }
        //    HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
        //    {
        //        new Claim(ConstValues.NameClaimType,user.Name),
        //        new Claim(ConstValues.RoleClaimType,userEntity.Role)
        //    }, CookieAuthenticationDefaults.AuthenticationScheme)));
        //    return Ok("login success");
        //}

        /// <summary>
        /// 用户登录，返回token  //TODO:需考虑用户的权限发生变动时，已经颁发的token要失效
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        [Description("获取登录token")]
        [HttpPost("ApiLogin")]
        [AllowAnonymous]
        public ActionResult ApiLogin(User user)
        {
            var userEntity = _db.Users.FirstOrDefault(a => a.LoginName == user.LoginName && a.Pwd == user.Pwd);
            if (userEntity == null)
            {
                return Ok("用户名或密码错误");
            }

            var userRoles = _db.UserRoleses.Include(a => a.Role).AsNoTracking().Where(a => a.UserId == userEntity.Id)
                .ToList().Select(a => a.Role);
            var roleIds = string.Join(',', userRoles.Select(a => a.Id).ToArray());
            //签名证书(秘钥，加密算法)
            var creds = new SigningCredentials(ConstValues.IssuerSigningKey, SecurityAlgorithms.HmacSha256);
            //生成token  [注意]需要nuget添加Microsoft.AspNetCore.Authentication.JwtBearer包，并引用System.IdentityModel.Tokens.Jwt命名空间
            var jwtSecurityToken = new JwtSecurityTokenHandler();
            //var tokenValidationParameters= new TokenValidationParameters()
            //{
            //    NameClaimType = ConstValues.NameClaimType,
            //    RoleClaimType = ConstValues.RoleClaimType,
            //    ValidIssuer = ConstValues.Issuer,
            //    ValidAudience = ConstValues.Audience,
            //    IssuerSigningKey = ConstValues.IssuerSigningKey
            //};
            var claims = new List<Claim>()
            {
                new Claim(ConstValues.NameClaimType,userEntity.Id.ToString()),
                new Claim(ConstValues.RoleClaimType,roleIds)
            };
            var token = new JwtSecurityToken(ConstValues.Issuer, ConstValues.Audience, claims, DateTime.Now, DateTime.Now.AddMinutes(30), creds);
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



        #endregion


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
