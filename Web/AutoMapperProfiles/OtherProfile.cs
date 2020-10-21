using ApplicationCore.Dtos;
using AutoMapper;
using Snail.Permission.Entity;

namespace Web.AutoMapperProfiles
{

    public class OtherProfile : Profile
    {
        public OtherProfile()
        {
            CreateMap<PermissionDefaultRole, RoleResultDto>().ReverseMap();
            CreateMap<PermissionDefaultRole, RoleSaveDto>().ReverseMap();
            CreateMap<PermissionDefaultResource, ResourceSaveDto>().ReverseMap();
            CreateMap<PermissionDefaultResource, ResourceResultDto>().ReverseMap();
        }
    }
}
