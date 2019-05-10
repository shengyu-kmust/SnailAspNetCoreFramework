using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Entity;

namespace DAL.Controllers
{

    /// <summary>
    /// 用户角色授权，授予某些用户某些角色
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : AuthorizeBaseController
    {
        public UserRoleController(DatabaseContext db) : base(db) { }

        /// <summary>
        /// 设置用户的角色
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public ActionResult AddRolesToUsers(string userIds,string roleIds)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 删除用户的角色
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public ActionResult RemoveRolesFromUsers(string userIds, string roleIds)
        {
            throw new NotImplementedException();
        }
        
    }
}