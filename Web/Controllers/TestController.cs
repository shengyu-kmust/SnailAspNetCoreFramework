using Microsoft.AspNetCore.Mvc;
using Service;
using System.Threading.Tasks;
using Web.Filter;

namespace Web.Controllers
{
    public class TestController : DefaultBaseController
    {
        private IInterceptorService interceptorService;
        public TestController(ControllerContext controllerContext, IInterceptorService service) : base(controllerContext)
        {
            this.interceptorService = service;

        }

        public string GetCurrentUserId()
        {
            return currentUserId;
        }

        #region interceptor

        [HttpGet]
        public string SyncReturn(string input)
        {
            return interceptorService.SyncReturn(input);
        }


        [HttpGet]
        public void SyncVoid(string input)
        {
            interceptorService.SyncVoid(input);
        }


        [HttpGet]
        public async Task<string> AsyncReturn(string input)
        {
            return await interceptorService.AsyncReturn(input);
        }


        [HttpGet]
        public async Task AsyncVoid(string input)
        {
            await interceptorService.AsyncVoid(input);
        }

        #endregion

        #region event

        [HttpGet]
        public void EventTest()
        {
            interceptorService.EmitEvent();

        }
        #endregion

        #region action cache
        [CacheFilter(ExpireMinute =1)]
        public async Task<string> ActionCache(string input)
        {
            return await interceptorService.AsyncReturn(input);
        }
        
        #endregion
    }
}
