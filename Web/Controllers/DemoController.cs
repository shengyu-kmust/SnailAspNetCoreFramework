using ApplicationCore.Entity;
using ApplicationCore.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snail.Common;
using Snail.Common.Extenssions;
using Snail.Core;
using Snail.Core.Permission;
using System.Collections.Generic;
using System.Linq;
using Web.DTO;
using Service;
using Snail.Core.Attributes;

namespace Web.Controllers
{
    /// <summary>
    /// 示例接口
    /// </summary>
    [Authorize(Policy = PermissionConstant.PermissionAuthorizePolicy)]
    [Resource(Description ="管理")]
    public class DemoController : DefaultBaseController, ICrudController<Demo, DemoSaveDto, DemoResultDto, DemoQueryDto>
    {
        private DemoService _service;
        public DemoController(DemoService service,ControllerContext controllerContext):base(controllerContext) {
            this.controllerContext = controllerContext;
            this._service = service;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [Resource(Description ="查询列表")]
        [HttpGet]
        public List<DemoResultDto> QueryList([FromQuery]DemoQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<Demo>();
            return controllerContext.mapper.ProjectTo<DemoResultDto>(_service.QueryList(pred)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [Resource(Description ="查询分页")]
        [HttpGet]
        public IPageResult<DemoResultDto> QueryPage([FromQuery]DemoQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<Demo>();
            return controllerContext.mapper.ProjectTo<DemoResultDto>(_service.QueryList(pred)).ToPageList(queryDto);
        }
        /// <summary>
        /// 查找单个
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Resource(Description ="查找单个")]
        [HttpGet]
        public DemoResultDto Find(string id)
        {
            return controllerContext.mapper.Map<DemoResultDto>(_service.QueryList(a => a.Id == id).FirstOrDefault());
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        [Resource(Description ="删除")]
        [HttpPost]
        public void Remove(List<string> ids)
        {
            _service.Remove(ids);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="saveDto"></param>
        [Resource(Description ="保存")]
        [HttpPost]
        public void Save(DemoSaveDto saveDto)
        {
            _service.Save(saveDto);
        }
    }
}
