using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Web.Controllers
{
    [ApiController]
    public class OrganizationController : AuthorizeBaseController
    {
        public OrganizationController(AppDbContext db) : base(db)
        {
        }

        //private OrganizationRepository _organizationRepository;
        //public OrganizationController(AppDbContext db, OrganizationRepository organizationRepository) : base(db)
        //{
        //    _organizationRepository = organizationRepository;
        //    _db = db;
        //}

        /// <summary>
        /// 获取所有的组织架构
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllOrgTree()
        {
            //var allOrg = _organizationRepository.All();
            //var tree=from node in allOrg
            //        select new
            //        {
            //            value=node,
            //            childs=allOrg.
            //        }
            throw new NotImplementedException();

        }

        //private object GetChild(List<Organization> dd)
        //{
        //    throw new NotImplementedException();
        //}
        /// <summary>
        /// 获取某个组织下的所有组织架构
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public ActionResult GetOrgChildTree(int orgId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取某个组织架构下的所有人员，包含了它的子架构的人员
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public ActionResult GetOrgAllUser(int orgId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取只属于某个组织架构下的所有人员，但不包含它的子架构的人员
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public ActionResult GetOrgUsersExceptChild(int orgId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将某些用户增加到某个部门
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public ActionResult AddUsers(string userIds,int orgId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将用户从某个部门移除
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public ActionResult RemoveUsers(string userIds, int orgId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将人员从某些部分移动到另一个部门
        /// </summary>
        /// <param name="userOrgIds">要移动的关系id，多个关系以英文逗号隔开</param>
        /// <param name="orgId">要移动到哪个部门</param>
        /// <returns></returns>
        public Action MoveUsers(string userOrgIds, int orgId)
        {
            throw new NotImplementedException();
        }
    }
}