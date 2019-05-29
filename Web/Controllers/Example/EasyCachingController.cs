using EasyCaching.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
