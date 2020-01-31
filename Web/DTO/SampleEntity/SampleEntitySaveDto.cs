using ApplicationCore.Enum;
using Snail.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.DTO.Sample
{
    public class SampleEntitySaveDto : BaseDto
    {
        [Required(ErrorMessage ="姓名必填")]
        public string Name { get; set; }
        [Required(ErrorMessage ="性别必填")]
        public EGender? Gender { get; set; }
        [Required(ErrorMessage ="年龄必填")]
        public int? Age { get; set; }
    }
}
