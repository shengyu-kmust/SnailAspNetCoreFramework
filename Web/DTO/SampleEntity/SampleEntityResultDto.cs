using AutoMapper;
using Snail.Core.Enum;

namespace Web.DTO
{
    [AutoMap(typeof(SampleEntityQueryDto),ReverseMap =true)]
    public class SampleEntityResultDto : BaseAuditDto
    {
        public string Name { get; set; }
        public EGender Gender { get; set; }
        public int Age { get; set; }
    }
}
