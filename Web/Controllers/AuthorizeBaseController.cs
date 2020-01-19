using ApplicationCore.Entity;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize(Policy = ConstValues.PermissionPolicy)]
    [Route("api/[controller]/[action]")]
    public class AuthorizeBaseController : ControllerBase
    {
        public AppDbContext _db;

        public AuthorizeBaseController(AppDbContext db)
        {
            _db = db;
        }
    }
}