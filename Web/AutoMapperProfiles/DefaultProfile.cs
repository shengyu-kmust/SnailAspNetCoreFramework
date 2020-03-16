using ApplicationCore.Entity;
using AutoMapper;
using Snail.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.DTO;
using System.Linq;
using ApplicationCore.Dtos;

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
            var allEntities = typeof(BaseEntity).Assembly.DefinedTypes.ToList().Where(a => a.GetInterfaces().Any(i => i == typeof(IEntityId<string>))).ToList();
            var allDtos = typeof(IDto).Assembly.DefinedTypes.ToList().Where(a => a.GetInterfaces().Any(i => i == typeof(IDto))).ToList();
            allEntities.ForEach(entity =>
            {
                allDtos.Where(a => a.Name.StartsWith(entity.Name)).ToList().ForEach(dto =>
                {
                    CreateMap(entity, dto);
                    CreateMap(dto, entity);
                });
            });
        }
    }
}
