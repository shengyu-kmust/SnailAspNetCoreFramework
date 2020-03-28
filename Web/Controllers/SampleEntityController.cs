using ApplicationCore.Dtos;
using ApplicationCore.Entity;
using ApplicationCore.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snail.Common;
using Snail.Common.Extenssions;
using Snail.Core;
using Snail.Core.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using Web.DTO;

namespace Web.Controllers
{
    [Authorize(Policy = PermissionConstant.PermissionAuthorizePolicy)]
    public class SampleEntityController : DefaultBaseController, ICrudController<SampleEntity, SampleEntitySaveDto, SampleEntityResultDto, SampleEntityQueryDto>
    {
        public ControllerContext controllerContext;
        public ISampleEntityService sampleEntityService;
        public SampleEntityController(ISampleEntityService sampleEntityService,ControllerContext controllerContext):base(controllerContext) {
            this.controllerContext = controllerContext;
            this.sampleEntityService = sampleEntityService;
        } 
        [HttpGet]
        public List<SampleEntityResultDto> QueryList([FromQuery]SampleEntityQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<SampleEntity>().AndIf(queryDto.Name.HasValue(), a => a.Name == queryDto.Name);
            return controllerContext.mapper.ProjectTo<SampleEntityResultDto>(sampleEntityService.QueryList(pred)).ToList();
        }

        [HttpGet]
        public IPageResult<SampleEntityResultDto> QueryPage([FromQuery]SampleEntityQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<SampleEntity>().AndIf(queryDto.Name.HasValue(), a => a.Name == queryDto.Name);
            return controllerContext.mapper.ProjectTo<SampleEntityResultDto>(sampleEntityService.QueryList(pred)).ToPageList(queryDto);
        }
        [HttpGet]
        public SampleEntityResultDto Find(string id)
        {
            return controllerContext.mapper.Map<SampleEntityResultDto>(sampleEntityService.QueryList(a => a.Id == id).FirstOrDefault());
        }
        [HttpPost]
        public void Remove(List<string> ids)
        {
            sampleEntityService.Remove(ids);
        }

        [HttpPost]
        public void Save(SampleEntitySaveDto saveDto)
        {
            sampleEntityService.Save(saveDto);

        }


    }
}
