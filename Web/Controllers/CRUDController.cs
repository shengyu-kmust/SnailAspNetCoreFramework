using Microsoft.AspNetCore.Mvc;
using Snail.Core;
using Snail.Core.Entity;
using Snail.Core.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Web.Controllers
{
    /// <summary>
    /// 通用CRUD接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TSaveDto"></typeparam>
    /// <typeparam name="TQueryDto"></typeparam>
    [ApiController]
    [Route("api/[Controller]/[Action]")]
    public abstract class CRUDController<TEntity,TSource,TResult,TSaveDto,TQueryDto>: ControllerBase 
        where TEntity : class, IEntityId<string> 
        where TSource : class
        where TSaveDto : IIdField<string>
        where TResult : class
        where TQueryDto : IPagination,new()
    {
        private ICRUDService<TEntity, TSource,string> _CRUDService;
        public CRUDController(ICRUDService<TEntity, TSource, string> CRUDService)
        {
            _CRUDService = CRUDService;
        }
        [HttpPost]
        public virtual void Save(TSaveDto saveDto) 
        {
            TEntity entity;
            if (string.IsNullOrEmpty(saveDto.Id))
            {
                entity = _CRUDService.Add(saveDto);
            }
            else
            {
                entity = _CRUDService.Update(saveDto);
            }
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
