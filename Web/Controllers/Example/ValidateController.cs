using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.Example
{
    /// <summary>
    /// 模型校验，
    /// 参考：
    /// https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-3.1#model-state
    /// https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-3.1#automatic-http-400-responses
    /// </summary>
    /// <remarks>
    /// 有了[apiController]后，就不需要在代码里手动ModelState.IsValid
    /// </remarks>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValidateController
    {
        [HttpPost]
        public IActionResult CheckInput(ValidateModel model)
        {
            return new OkObjectResult("校验通过");
        }
    }


    public class ValidateModel : IValidatableObject
    {
        [Required(ErrorMessage = "年龄必填")]
        public int? Age { get; set; }
        [Required(ErrorMessage = "生日必填")]
        [Display(Name = "生日")]
        public DateTime? Birthday { get; set; }
        [Required(ErrorMessage = "姓名必填")]
        [Display(Name = "姓名")]
        public string Name { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();
            if (Name.Length>5)
            {
                errors.Add(new ValidationResult("姓名不能大于5字符"));
            }
            return errors;
        }
    }
}
