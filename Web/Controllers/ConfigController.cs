using ApplicationCore.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers
{
    /// <summary>
    /// IConfiguration的配置信息存储的是Dictionary<string,string>类型的数据
    /// Options配置的是class类型的数据，并支持从IConfiguration导入配置信息
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ConfigController : ControllerBase
    {
        private IConfiguration _configuration;
        //private IOptionsMonitor<Student> _optionsMonitor;
        //private IOptionsSnapshot<Student> _optionsSnapshot;
        //private IOptionsMonitor<Student> _namedOptionsMonitor;
        //public ConfigController(IConfiguration configuration,IOptionsMonitor<Student> optionsMonitor,IOptionsSnapshot<Student> optionsSnapshot,IOptionsMonitor<Student> namedOptionsMonitor)
        //{
        //    _configuration = configuration;
        //    _optionsMonitor = optionsMonitor;
        //    _optionsSnapshot = optionsSnapshot;
        //    _namedOptionsMonitor = namedOptionsMonitor;
        //}

        //[HttpGet]
        //public JsonResult GetConfig(string configName)
        //{
        //    return new JsonResult(_configuration[configName]);
        //}

        //[HttpGet]
        //public JsonResult GetOption()
        //{
        //    return new JsonResult(new
        //    {
        //        optionsMonitor= _optionsMonitor,
        //        optionsSnapshot = _optionsSnapshot,
        //        namedOptionsMonitor1 = _namedOptionsMonitor.Get("optionBuilderStudent"),
        //        namedOptionsMonitor2 = _namedOptionsMonitor.Get("configBuilderStudent"),
        //    });
        //}
    }
}
