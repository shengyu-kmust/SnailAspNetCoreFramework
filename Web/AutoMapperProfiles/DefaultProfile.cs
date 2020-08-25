using ApplicationCore.Dtos;
using AutoMapper;
using Service;
using Snail.Core.Dto;
using Snail.Core.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Web.AutoMapperProfiles
{
    /// <summary>
    /// 默认的automapper配置，只配置entity和dto的相互映射，其它的请用AutoMapAttribute设置
    /// </summary>
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            MapEntityAndDto();
        }

        private void MapEntityAndDto()
        {
            var allEntities = typeof(BaseDto).Assembly.DefinedTypes.ToList().Where(a => a.GetInterfaces().Any(i => i == typeof(IEntityId<string>))).ToList();
            var allDtos =new List<Assembly>() { typeof(BaseDto).Assembly,typeof(Startup).Assembly,typeof(ServiceContext).Assembly }.SelectMany(a=>a.DefinedTypes.ToList()).Where(a => a.GetInterfaces().Any(i => i == typeof(IDto))).ToList();
            
            allEntities.ForEach(entity =>
            {
                allDtos.Where(a => a.Name.StartsWith(entity.Name)).ToList().ForEach(dto =>
                {
                    CreateMap(entity, dto);
                    CreateMap(dto, entity);
                });
            });

            // 自身的映射
            allDtos.ForEach(dto =>
            {
                CreateMap(dto, dto);
            });
            allEntities.ForEach(entity =>
            {
                CreateMap(entity, entity);

            });
        }
    }
}
