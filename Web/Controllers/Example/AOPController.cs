using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            _aopService.TestTask();
            _aopService.TestTaskReturn();
            _aopService.TestAsync();
            _aopService.TestAsyncReturn();
            _aop2Service.Test();
            return "success";
        }

        [HttpGet]
        public async Task TestAsync()
        {
            await _aopService.TestAsync();
        }

        [HttpGet]
        public async Task<string> TestAsyncReturn()
        {
           return await _aopService.TestAsyncReturn();
        }

    }

    [Intercept(typeof(LogInterceptor))]
    public interface IAopService {
        string Test();
        Task TestTask();
        Task<string> TestTaskReturn();
        Task TestAsync();
        Task<string> TestAsyncReturn();
    }
    public class AopService : IAopService
    {
        private ILogger<AopService> _log;
        public AopService(ILogger<AopService> log)
        {
            _log = log;
        }
        public AopService()
        {

        }
        public string Test()
        {
            return "this is aopservice.test result";
        }

        public async Task TestAsync()
        {
            await Task.Run(() =>
            {
                _log.LogInformation("AopService.TestAsync");
            });
        }

        public async Task<string> TestAsyncReturn()
        {
            return await Task.Run(() =>
            {
                _log.LogInformation("AopService.TestAsyncReturn");
                return "AopService.TestAsyncReturn return result";
            });
        }

        public Task TestTask()
        {
            _log.LogInformation("AopService.TestTask");
            return Task.CompletedTask;
        }

        public Task<string> TestTaskReturn()
        {
            return Task.FromResult("this is aopservice.TestTaskReturn result");
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
