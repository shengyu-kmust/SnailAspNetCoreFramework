using Autofac;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Web.Controllers.Example
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AutofacController: ControllerBase
    {
        private IServiceProvider _serviceProvider;
        private ILifetimeScope _lifetimeScope;
        public AutofacController(IServiceProvider serviceProvider, ILifetimeScope lifetimeScope)
        {
            _serviceProvider = serviceProvider;
            _lifetimeScope = lifetimeScope;
        }


        [HttpGet]
        public void ResolveAService()
        {
            //var aService= _serviceProvider.GetService(typeof(IAService)) as IAService;
            //var bService = _serviceProvider.GetService(typeof(IBService)) as IBService ;
            //return $"aService:{aService.GetServiceName()},bService.aService:{bService.GetServiceName()}";
        }

        [HttpGet]
        public void ResolveFromScope()
        {
            //using (var scope= _lifetimeScope.BeginLifetimeScope())
            //{
            //    var aService = scope.Resolve<IAService>();
            //    return aService.GetServiceName();
            //}
        }

    }
}
