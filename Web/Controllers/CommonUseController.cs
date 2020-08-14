using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Service;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[Controller]/[Action]")]
    public class CommonUseController : ControllerBase
    {
        private CommonUseService _service;
        public CommonUseController(CommonUseService commonUseService)
        {
            _service = commonUseService;
        }
        /// <summary>
        /// 文件下载
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public FileStreamResult ExportExcel()
        {
            var stream = _service.ExportExcel();
            stream.Position = 0;
            return File(stream, "application/octet-stream","exportExcel.xls");
        }

        /// <summary>
        /// 文件上传，form-data里的key必须为formFiles才能正确绑定文件到对象
        /// </summary>
        /// <param name="formFiles"></param>
        /// <returns></returns>
        [HttpPost]
        public List<ExcelTestDto> ImportExcel(IFormFileCollection formFiles)
        {
            if (formFiles.Any())
            {
                var stream = formFiles[0].OpenReadStream();
                return _service.ImportExcel(stream);

            }
            else
            {
                return default;
            }
        }
    }
}
