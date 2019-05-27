using Autofac;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Example.autofac;

namespace Web.Controllers.Example
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AutofacController: ControllerBase
    {
        private IAService _aService;
        private IBService _bService;
        private IServiceProvider _serviceProvider;
        private ILifetimeScope _lifetimeScope;
        public AutofacController(IAService aService,IBService bService,IServiceProvider serviceProvider, ILifetimeScope lifetimeScope)
        {
            _aService = aService;
            _bService = bService;
            _serviceProvider = serviceProvider;
            _lifetimeScope = lifetimeScope;
        }

        [HttpGet]
        public string GetAServiceName()
        {
            return _aService.GetServiceName();
        }

        [HttpGet]
        public string GetAServiceNameOfBService()
        {
            return _bService.GetAServiceName();
        }

        [HttpGet]
        public string ResolveAService()
        {
            var aService= _serviceProvider.GetService(typeof(IAService)) as IAService;
            var bService = _serviceProvider.GetService(typeof(IBService)) as IBService ;
            return $"aService:{aService.GetServiceName()},bService.aService:{bService.GetServiceName()}";
        }

        [HttpGet]
        public string ResolveFromScope()
        {
            using (var scope= _lifetimeScope.BeginLifetimeScope())
            {
                var aService = scope.Resolve<IAService>();
                return aService.GetServiceName();
            }
        }

    }
}
