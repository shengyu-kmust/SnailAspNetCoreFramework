using ApplicationCore.Abstract;
using ApplicationCore.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Web.Controllers.Example
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EntityCachingController : ControllerBase
    {
        private IEntityCaching<int, SampleEntity> _userCaching;
        public EntityCachingController(IEntityCaching<int,SampleEntity> userCaching)
        {
            _userCaching = userCaching;
        }

        [HttpGet]
        public List<SampleEntity> GetAllUserByCaching()
        {
            return _userCaching.Values;
        }
    }
}
