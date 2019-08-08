using EasyCaching.Core;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Web.Controllers.Example
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EasyCachingController : ControllerBase
    {
        private IEasyCachingProvider _easyCachingProvider;
        public EasyCachingController(IEasyCachingProvider easyCachingProvider)
        {
            _easyCachingProvider = easyCachingProvider;
        }
        [HttpGet]
        public DateTime GetDateTimeCached()
        {
            if (!_easyCachingProvider.Exists("GetDateTimeCached"))
            {
                _easyCachingProvider.Set("GetDateTimeCached", DateTime.Now, new TimeSpan(0, 0, 5));
            }
            return _easyCachingProvider.Get<DateTime>("GetDateTimeCached").Value;
        }
    }
}
