using ApplicationCore.Enum;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DTO.Sample
{
    [AutoMap(typeof(SampleEntityResultDto),ReverseMap =true)]
    public class SampleEntitySourceDto : BaseAuditDto
    {
        public string Name { get; set; }
        public EGender Gender { get; set; }
        public int Age { get; set; }
    }
}
