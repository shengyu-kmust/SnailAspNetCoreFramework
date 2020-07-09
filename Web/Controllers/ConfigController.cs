using ApplicationCore.Entities;
using ApplicationCore.Entity;
using ApplicationCore.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snail.Common;
using Snail.Common.Extenssions;
using Snail.Core;
using Snail.Core.Attributes;
using Snail.Core.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using Web.DTO;
namespace Web.Controllers
{

    public class TreeNodeHelper2
    {
        public static TreeNode<T> GetTree<T>(List<T> list, Func<T, object> IdFunc, Func<T, object> parentIdFunc, object nodeId) where T : class
        {
            T data = (T)list.FirstOrDefault((T a) => IdFunc(a).Equals(nodeId));
            T parent = (data == null) ? null : list.FirstOrDefault((T a) => IdFunc(a).Equals(parentIdFunc((T)data)));
            List<TreeNode<T>> childs = (from a in list
                                        where parentIdFunc(a)!=null && parentIdFunc(a).Equals(nodeId)
                                        select GetTree(list, IdFunc, parentIdFunc, IdFunc(a))).ToList();
            return new TreeNode<T>
            {
                Data = (T)data,
                Parent = parent,
                Childs = childs
            };
        }
    }
    //[Authorize(Policy = PermissionConstant.PermissionAuthorizePolicy)]
    [Resource(Description = "配置管理")]
    public class ConfigController : DefaultBaseController, ICrudController<Config, ConfigSaveDto, ConfigResultDto, KeyQueryDto>
    {
        private IConfigService _service;
        public ConfigController(IConfigService service, ControllerContext controllerContext) : base(controllerContext)
        {
            this.controllerContext = controllerContext;
            this._service = service;
        }
        [Resource(Description = "查询列表")]
        [HttpGet]
        public List<ConfigResultDto> QueryList([FromQuery]KeyQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<Config>().AndIf(queryDto.KeyWord.HasValue(), a => a.Name.Contains(queryDto.KeyWord) || a.Key.Contains(queryDto.KeyWord) || a.Value.Contains(queryDto.KeyWord));
            return controllerContext.mapper.ProjectTo<ConfigResultDto>(_service.QueryList(pred)).ToList();
        }

        [Resource(Description = "查询树")]
        [HttpGet]
        public TreeNode<ConfigResultDto> QueryListTree([FromQuery]KeyQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<Config>().AndIf(queryDto.KeyWord.HasValue(), a => a.Name.Contains(queryDto.KeyWord) || a.Key.Contains(queryDto.KeyWord) || a.Value.Contains(queryDto.KeyWord));
            var list=controllerContext.mapper.ProjectTo<ConfigResultDto>(_service.QueryList(pred)).ToList();
            return TreeNodeHelper2.GetTree<ConfigResultDto>(list??new List<ConfigResultDto>(), a => a.Id, a => a.ParentId, "0");
        }

        [Resource(Description = "查询分页")]
        [HttpGet]
        public IPageResult<ConfigResultDto> QueryPage([FromQuery]KeyQueryDto queryDto)
        {
            var pred = ExpressionExtensions.True<Config>().AndIf(queryDto.KeyWord.HasValue(), a => a.Name.Contains(queryDto.KeyWord) || a.Key.Contains(queryDto.KeyWord) || a.Value.Contains(queryDto.KeyWord));
            return controllerContext.mapper.ProjectTo<ConfigResultDto>(_service.QueryList(pred)).ToPageList(queryDto);
        }

        [Resource(Description = "查询单个")]
        [HttpGet]
        public ConfigResultDto Find(string id)
        {
            return controllerContext.mapper.Map<ConfigResultDto>(_service.QueryList(a => a.Id == id).FirstOrDefault());
        }
        [Resource(Description = "删除")]
        [HttpPost]
        public void Remove(List<string> ids)
        {
            _service.Remove(ids);
        }

        [Resource(Description = "保存")]
        [HttpPost]
        public void Save(ConfigSaveDto saveDto)
        {
            _service.Save(saveDto);
        }
    }
}
