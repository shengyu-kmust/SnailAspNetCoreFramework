using ApplicationCore.Entity;
using AutoMapper;
using Snail.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.DTO;
using System.Linq;
namespace Web.AutoMapperProfiles
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile()
        {
            //所有entity到dto的映射
            var entities = typeof(BaseEntity).Assembly.GetTypes().Where(a => typeof(IBaseEntity).IsAssignableFrom(a));
            var dtos = typeof(BaseDto).Assembly.GetTypes().Where(a => typeof(IDto).IsAssignableFrom(a));
            entities.ToList().ForEach(entity =>
            {
                var mapDtos = dtos.Where(a => a.Name.StartsWith(entity.Name, StringComparison.OrdinalIgnoreCase));
                mapDtos.ToList().ForEach(dto =>
                {
                    CreateMap(entity, dto).ReverseMap();
                    CreateMap(dto,entity).ReverseMap();
                });
            });
        }
    }
}
