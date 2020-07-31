using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Dtos
{
    /// <summary>
    /// dto输入校验
    /// </summary>
    /// <remarks>
    /// 可参考:https://docs.microsoft.com/zh-cn/aspnet/core/mvc/models/validation?view=aspnetcore-3.1
    /// </remarks>
    public class DtoValidateSample : IDto, IValidatableObject
    {
        [Required(ErrorMessage ="必填")]
        public string Reqired { get; set; }
        [StringLength(8, ErrorMessage = "长度不能超过8")]
        public string StringLen { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();
            #region 这里写dto的校验逻辑，如果有问题则加入到errors里
            //errors.Add(new ValidationResult("这是错误内容",new List<string> { "这是核验不通过的字段" }));
            #endregion
            return errors;
        }
    }
}
