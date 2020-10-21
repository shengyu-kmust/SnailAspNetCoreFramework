using Microsoft.AspNetCore.Mvc;
using Snail.Web.Controllers;
using Snail.Web.Filter;
using Snail.Web.Services;
using System.Threading.Tasks;

namespace Web.Controllers
{
    /// <summary>
    /// 测试用
    /// </summary>
    public class TestController : DefaultBaseController
    {
        private IInterceptorService interceptorService;
        public TestController(SnailControllerContext controllerContext, IInterceptorService service) : base(controllerContext)
        {
            this.interceptorService = service;

        }

        /// <summary>
        /// 获取当前登录人
        /// </summary>
        /// <returns>返回登录人的id</returns>
        public string GetCurrentUserId()
        {
            return currentUserId;
        }

        #region interceptor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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
