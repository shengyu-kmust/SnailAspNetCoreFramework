﻿using ApplicationCore.Entity;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Web.Domain;
using Web.Services;

namespace Web.Controllers
{
    /// <summary>
    /// 权限管理，只对角色进行授权
    /// </summary>
    [ApiController]
    public class PermissionController : AuthorizeBaseController
    {
        private PermissionModel _permissionModel;
        private PermissionService _permissionService;
        public PermissionController(PermissionModel permissionModel, AppDbContext db,PermissionService permissionService):base(db)
        {
            _permissionModel = permissionModel;
            _permissionService = permissionService;
        }
        [HttpGet("RefreshPermission")]
        [AllowAnonymous]
        public ActionResult RefreshPermission()
        {
            _permissionModel.loadPermissionData();
            return Ok("成功");
        }

        /// <summary>
        /// 设置角色的权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="resourceIds">资源</param>
        /// <returns></returns>
        public ActionResult AddPermission(int roleId, string resourceIds)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 移除角色的权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="resourceIds">资源ID</param>
        /// <returns></returns>
        public ActionResult RemovePermission(int roleId, string resourceIds)
        {
            throw new NotImplementedException();
        }



    }
}