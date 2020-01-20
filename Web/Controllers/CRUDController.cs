using Microsoft.AspNetCore.Mvc;
using Snail.Core;
using Snail.Core.Entity;
using Snail.Core.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[Controller]/[Action]")]
    public class CRUDController<TEntity,TSource,TResult,TSaveDto,TQueryDto>: ControllerBase where TEntity: class,IEntityId<string> where TSaveDto:IIdField<string> where TQueryDto:IIdField<string>,IPagination,new() where TResult:class
    {
        private ICRUDService<TEntity, string> _CRUDService;
        public CRUDController(ICRUDService<TEntity, string> CRUDService)
        {
            _CRUDService = CRUDService;
        }
        [HttpPost]
        public TResult Save(TSaveDto saveDto)
        {
            TEntity device;
            if (string.IsNullOrEmpty(saveDto.Id))
            {
                device = _CRUDService.Add(saveDto);
            }
            else
            {
                device = _CRUDService.Update(saveDto);
            }
            return _CRUDService.Query<TResult, TQueryDto>(new TQueryDto { Id = device.Id }).FirstOrDefault();
        }

        [HttpGet]
        public IPageResult<TResult> QueryPage([FromQuery]TQueryDto queryDto)
        {
            return _CRUDService.QueryPage<TResult, TQueryDto>(queryDto);
        }

        [HttpGet]
        public List<TResult> Query([FromQuery]TQueryDto queryDto)
        {
            return _CRUDService.Query<TResult, TQueryDto>(queryDto);
        }


        [HttpPost]
        public ActionResult Delete(string id)
        {
            _CRUDService.Delete(id);
            return new OkResult();
        }
    }
}
