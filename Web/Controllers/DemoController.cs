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

namespace Web.Controllers
{
    [Authorize(Policy = PermissionConstant.PermissionAuthorizePolicy)]
    public class DemoController : DefaultBaseController, ICrudController<Demo, DemoSaveDto, DemoResultDto, DemoQueryDto>
    {
        private DemoService _service;
        public DemoController(DemoService service,ControllerContext controllerContext):base(controllerContext) {
            this.controllerContext = controllerContext;
            this._service = service;
        } 
        [HttpGet]
        public List<DemoResultDto> QueryList([FromQuery]DemoQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<Demo>();
            return controllerContext.mapper.ProjectTo<DemoResultDto>(_service.QueryList(pred)).ToList();
        }

        [HttpGet]
        public IPageResult<DemoResultDto> QueryPage([FromQuery]DemoQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<Demo>();
            return controllerContext.mapper.ProjectTo<DemoResultDto>(_service.QueryList(pred)).ToPageList(queryDto);
        }
        [HttpGet]
        public DemoResultDto Find(string id)
        {
            return controllerContext.mapper.Map<DemoResultDto>(_service.QueryList(a => a.Id == id).FirstOrDefault());
        }
        [HttpPost]
        public void Remove(List<string> ids)
        {
            _service.Remove(ids);
        }

        [HttpPost]
        public void Save(DemoSaveDto saveDto)
        {
            _service.Save(saveDto);
        }
    }
}
