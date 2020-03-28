using ApplicationCore.Enum;
using AutoMapper;
using Snail.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
