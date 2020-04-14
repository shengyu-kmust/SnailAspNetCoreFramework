using Snail.Core.Enum;
using System.ComponentModel.DataAnnotations;

namespace Web.DTO
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
