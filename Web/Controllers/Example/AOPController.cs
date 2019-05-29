using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Interceptor;

namespace Web.Controllers.Example
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AOPController: ControllerBase
    {
        private IAopService _aopService;
        private Aop2Service _aop2Service;
        public AOPController(IAopService aopService, Aop2Service aop2Service)
        {
            _aopService = aopService;
            _aop2Service = aop2Service;
        }

        [HttpGet]
        public string Test()
        {
            _aopService.Test();
            _aop2Service.Test();
            return "success";
        }
    }

    [Intercept(typeof(LogInterceptor))]
    public interface IAopService {string Test(); }
    public class AopService : IAopService
    {
        public string Test()
        {
            return "this is aopservice.test result";
        }
    }

    /// <summary>
    /// autofac只注册了以service结尾的，命名要注意一下
    /// </summary>
    public class Aop2Service
    {
        public virtual string Test()
        {
            return "this is aop2Service.test return result";
        }
    }


}
