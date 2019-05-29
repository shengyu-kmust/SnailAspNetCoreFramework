using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.Example
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SwaggerController : ControllerBase
    {
        /// <summary>
        /// swagger测试接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public SwaggerOutputDto TestPost(SwaggerInputDto dto)
        {
            return new SwaggerOutputDto()
            {
                DtValue = DateTime.Now,
                IntValue = 100,
                stringValue = "swagger post test"
            };
        }

        /// <summary>
        /// swagger测试接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public SwaggerOutputDto TestGet()
        {
            return new SwaggerOutputDto()
            {
                DtValue = DateTime.Now,
                IntValue = 100,
                stringValue = "swagger get test"
            };
        }
    }

    /// <summary>
    /// 这是输入参数
    /// </summary>
    public class SwaggerInputDto
    {
        /// <summary>
        /// int类型的值
        /// </summary>
        [Required]
        public int IntValue { get; set; }
        /// <summary>
        /// string类型的值
        /// </summary>
        public string stringValue { get; set; }
        /// <summary>
        /// datetime类型的值
        /// </summary>
        public DateTime DtValue { get; set; }
    }

    /// <summary>
    /// 这是输出参数
    /// </summary>
    public class SwaggerOutputDto
    {
        /// <summary>
        /// int类型的值
        /// </summary>
        [Required]
        public int IntValue { get; set; }
        /// <summary>
        /// string类型的值
        /// </summary>
        public string stringValue { get; set; }
        /// <summary>
        /// datetime类型的值
        /// </summary>
        public DateTime DtValue { get; set; }
    }
}
